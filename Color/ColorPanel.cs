﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

using CreatureVisualizer.Properties;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorPanel
	/// <summary>
	/// ColorPanel is not a panel.
	/// </summary>
	sealed class ColorPanel
		: UserControl
	{
		#region Events
		internal event EventHandler ColorValueChanged;
		#endregion Events


		#region Fields
		ColorSpace _csCurrent;

		Bitmap _hueSlider = new Bitmap(ColorSlider.width, ColorSlider.height);

//		bool m_isLeftMouseButtonDown;
		#endregion Fields


		#region Properties
		// Good god, those bastards go about things in a cockamamie way ...
		// TODO: Consolidate firing the ColorValueChanged event in a central
		// function like SelectColor() or Satisfy() or similar.

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Color Color
		{
			get { return GetActiveColorbox().BackColor; }
			set
			{
				SelectColor(value, true);
				tb_Alpha.Text = value.A.ToString();
			}
		}

//		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//		[Browsable(false)]
//		public byte Alpha // IMPORTANT: Set alpha by its textbox.
//		{ get; set; }
		#endregion Properties


		#region cTor
		public ColorPanel()
		{
			InitializeComponent();

			colorfield.ColorSelected += colorselected_field;
			swatches.ColorSwatchSelected += colorswatchselected_swatches;

			GradientService.InstantiateConstantObjects();

			using (Graphics graphics = Graphics.FromImage(_hueSlider))
				GradientService.DrawSlider(graphics, new Rectangle(new Point(0,0), _hueSlider.Size));

			colorbox1.IsActive = true;

			hsbColorSpace.SetDefaultSelection();
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

// draw checkers under colorboxes ->
			var checkers = new ResourceManager("CreatureVisualizer.Properties.Resources",
											   typeof(Resources).Assembly).GetObject("checkers") as Image;
			e.Graphics.DrawImage(checkers, colorbox1.Location.X, colorbox1.Location.Y);

// draw border around colorfield ->
			e.Graphics.DrawRectangle(Pens.Black,
									 colorfield.Left  - 1, colorfield.Top    - 1,
									 colorfield.Width + 1, colorfield.Height + 1);

// draw border around colorboxes ->
//			e.Graphics.DrawRectangle(Pens.Black,
//									 colorbox1.Left  - 1, colorbox1.Top                       - 1,
//									 colorbox1.Width + 1, colorbox1.Height + colorbox0.Height + 1);

// draw borders top/bot for colorboxes -> (left/right borders is handled by the colorboxes' OnPaint())
			e.Graphics.DrawLine(Pens.Black,
								colorbox1.Location.X                   + 1, colorbox1.Top - 1,
								colorbox1.Location.X + colorbox1.Width - 2, colorbox1.Top - 1);
			e.Graphics.DrawLine(Pens.Black,
								colorbox0.Location.X                   + 1, colorbox0.Bottom,
								colorbox0.Location.X + colorbox0.Width - 2, colorbox0.Bottom);

// draw border around swatches ->
			e.Graphics.DrawRectangle(Pens.Black,
									 swatches.Left  - 1, swatches.Top    - 1,
									 swatches.Width + 1, swatches.Height + 1);
		}


		// what ...
/*		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				m_isLeftMouseButtonDown = true;

				UpdateColorPanels(false, false, true);

				if (m_currentColorSpace.SelectedComponent.DisplayCharacter == 'H')
				{
					activeColorBox.BackColor = colorFieldPanel.GetCurrentColor();
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (m_isLeftMouseButtonDown)
				UpdateColorField(false);

			m_isLeftMouseButtonDown = false;
		} */
		#endregion Handlers (override)


		#region Handlers
		internal void mousedown_colorbox(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ColorBox bo = SwapActiveColorbox();

				rgbColorSpace.Structure = ColorConverter.ColorToRgb(bo.BackColor);
				hsbColorSpace.Structure = ColorConverter.ColorToHsb(bo.BackColor);

				colorslider.Value = CalculateSliderPosition(_csCurrent.Selected);

				Satisfy(true, true, true);

				if (ColorValueChanged != null)
					ColorValueChanged(this, EventArgs.Empty);
			}
		}

		void valuechanged_slider(object sender, ValueChangedEventArgs e)
		{
			int val = e.Value;

			switch (_csCurrent.Selected.Unit)
			{
				case ColorSpaceControl.Units.Percent:
					val = (int)Math.Ceiling((double)val / 2.55);
					break;

				case ColorSpaceControl.Units.Degree:
					val = (int)Math.Ceiling((double)val / (17.0 / 24.0));
					if (val == 360)
						val = 0;

					break;
			}

			_csCurrent.Selected.Value = val;

			if (_csCurrent is RgbColorSpace)
			{
				hsbColorSpace.Structure = ColorConverter.RgbToHsb((RGB)rgbColorSpace.Structure);
			}
			else if (_csCurrent is HsbColorSpace)
			{
				rgbColorSpace.Structure = ColorConverter.HsbToRgb((HSB)hsbColorSpace.Structure);
			}

			Satisfy(false, false, true);

			GetActiveColorbox().BackColor = ColorConverter.RgbToColor((RGB)rgbColorSpace.Structure);

			if (ColorValueChanged != null)
				ColorValueChanged(this, EventArgs.Empty);
		}

		void dragdrop_colorbox(object sender, DragEventArgs e)
		{
			if (((ColorBox)sender).IsActive)
			{
				object data = e.Data.GetData(typeof(Color));
				if (data != null)
				{
					var color = (Color)data;
					if (color != Color.Empty)
					{
						rgbColorSpace.Structure = ColorConverter.ColorToRgb(color);
						hsbColorSpace.Structure = ColorConverter.ColorToHsb(color);

						colorslider.Value = CalculateSliderPosition(_csCurrent.Selected);

						Satisfy(true, true, true);
					}
				}
			}
		}

		void selectedcomponentchanged_colorspace(ColorSpace sender, EventArgs e)
		{
			if (sender is RgbColorSpace)
			{
				hsbColorSpace.ResetComponents();
			}
			else if (sender is HsbColorSpace)
			{
				rgbColorSpace.ResetComponents();
			}

			_csCurrent = sender;

			SetSliderValue();
			Satisfy(true, true, true);
		}

		void componentvaluechanged_colorspace(ColorSpace sender, EventArgs e)
		{
			if (sender is RgbColorSpace)
			{
				hsbColorSpace.Structure = ColorConverter.RgbToHsb((RGB)rgbColorSpace.Structure);
			}
			else if (sender is HsbColorSpace)
			{
				rgbColorSpace.Structure = ColorConverter.HsbToRgb((HSB)hsbColorSpace.Structure);
			}

			SetSliderValue();
			Satisfy(true, true, true);

			GetActiveColorbox().BackColor = ColorConverter.RgbToColor((RGB)rgbColorSpace.Structure);

			if (ColorValueChanged != null)
				ColorValueChanged(this, EventArgs.Empty);
		}

		void colorswatchselected_swatches(object sender, ColorSelectedEventArgs e)
		{
			SelectColor(e.Color, true);
		}

		void colorselected_field(object sender, ColorSelectedEventArgs e)
		{
			RGB rgb = ColorConverter.ColorToRgb(e.Color);
			rgbColorSpace.Structure = rgb;

			HSB hsb = ColorConverter.RgbToHsb(rgb);

			switch (_csCurrent.Selected.DisplayCharacter)
			{
				case 'H':
				{
					int hue = ((HSB)hsbColorSpace.Structure).Hue;
					hsbColorSpace.Structure = new HSB(hue, hsb.Saturation, hsb.Brightness);
					break;
				}

				case 'S':
				{
					int sat = ((HSB)hsbColorSpace.Structure).Saturation;
					hsbColorSpace.Structure = new HSB(hsb.Hue, sat, hsb.Brightness);
					break;
				}

				default:
					hsbColorSpace.Structure = hsb;
					break;
			}

			tb_Hex.Text = rgbColorSpace.ConvertToHex();
			GetActiveColorbox().BackColor = ColorConverter.RgbToColor(rgb);

			if (ColorValueChanged != null)
				ColorValueChanged(this, EventArgs.Empty);
		}


		void textchanged_hecate(object sender, EventArgs e)
		{
			RGB rgb = ColorConverter.HexToRgb(tb_Hex.Text);
			rgbColorSpace.Structure = rgb;
			hsbColorSpace.Structure = ColorConverter.RgbToHsb(rgb);

			SetSliderValue();
			Satisfy(true, true, false);
		}

		void textchanged_alpha(object sender, EventArgs e)
		{
			string alpha;
			if (!String.IsNullOrEmpty(tb_Alpha.Text))
				alpha = tb_Alpha.Text;
			else
				alpha = "0";

			SelectColor(GetActiveColorbox().BackColor, false, false);
		}
		#endregion Handlers


		#region Methods
		void SelectColor(Color color, bool resetslider, bool setHexText = true)
		{
			if (!ColorConverter.ColorToRgb(color).Equals(rgbColorSpace.Structure)	// TODO: store alpha in the Structure(s)
				|| !setHexText)														// and remove 'setHextText' shenanigans
			{
				RGB rgb = ColorConverter.ColorToRgb(color);
				rgbColorSpace.Structure = rgb;
				hsbColorSpace.Structure = ColorConverter.RgbToHsb(rgb);

				if (resetslider) SetSliderValue();

				Satisfy(false, true, setHexText);

				if (ColorValueChanged != null)
					ColorValueChanged(this, EventArgs.Empty);
			}
		}

		void SetSliderValue()
		{
			colorslider.Value = CalculateSliderPosition(_csCurrent.Selected);
		}

		int CalculateSliderPosition(ColorSpaceControl csc)
		{
			int val = csc.Value;
			switch (csc.Unit)
			{
				case ColorSpaceControl.Units.Degree:
					val = (int)Math.Ceiling(17.0 / 24.0 * (double)val);
					break;

				case ColorSpaceControl.Units.Percent:
					val = (int)Math.Ceiling(2.55 * (double)val);
					break;
			}
			return val;
		}

		void Satisfy(bool setSliderColorspace, bool updatePoint, bool setHexText)
		{
			string text;
			if (!String.IsNullOrEmpty(tb_Alpha.Text))
				text = tb_Alpha.Text;
			else
				text = "0";

			GetActiveColorbox().BackColor = Color.FromArgb(Byte.Parse(text),
														   ColorConverter.RgbToColor((RGB)rgbColorSpace.Structure));


			if (setSliderColorspace)
				colorslider.ChangeColorspace(_csCurrent);


			if (_csCurrent is HsbColorSpace)
			{
				if (_csCurrent.Selected.DisplayCharacter == 'H')
				{
					Color color = _hueSlider.GetPixel(0, 255 - colorslider.Value);
					colorfield.ChangeColor(color, _csCurrent, updatePoint);
				}
				else // 'S','B'
					colorfield.ChangeColor(_csCurrent.Selected.Value, _csCurrent, updatePoint);
			}
			else if (_csCurrent is RgbColorSpace) // 'R','G','B'
			{
				colorfield.ChangeColor(_csCurrent.Selected.Value, _csCurrent, updatePoint);
			}


			if (setHexText)
				tb_Hex.Text = rgbColorSpace.ConvertToHex();
		}


/*		bool SetActiveColorbox(ColorBox bo)
		{
			if (!bo.IsActive)
			{
				if (bo == colorbox1)
				{
					colorbox1.IsActive = true;
					colorbox0.IsActive = false;
				}
				else //if (bo == colorbox0)
				{
					colorbox1.IsActive = false;
					colorbox0.IsActive = true;
				}
				return true;
			}
			return false;
		} */
		ColorBox SwapActiveColorbox()
		{
			if (GetActiveColorbox() == colorbox0)
			{
				colorbox1.IsActive = true;
				colorbox0.IsActive = false;
				return colorbox1;
			}

			colorbox1.IsActive = false;
			colorbox0.IsActive = true;
			return colorbox0;
		}

		ColorBox GetActiveColorbox()
		{
			if (colorbox1.IsActive)
				return colorbox1;

			return colorbox0;
		}

		internal void InitInactiveColorbox(Color color)
		{
			colorbox0.BackColor = color;
		}


		internal bool IsTextboxFocused(ControlCollection controls)
		{
			foreach (Control control in controls)
			{
				if ((control as TextBoxBase) != null && control.Focused)
					return true;

				if (IsTextboxFocused(control.Controls))
					return true;
			}
			return false;
		}
		#endregion Methods



		#region Designer
		ColorField colorfield;
		ColorSlider colorslider;
		ColorBox colorbox1;
		ColorBox colorbox0;
		HsbColorSpace hsbColorSpace;
		RgbColorSpace rgbColorSpace;
		Label la_Hex;
		TextboxRestrictive tb_Hex;
		Label la_Alpha;
		TextboxRestrictive tb_Alpha;
		ColorSwatchPanel swatches;


		void InitializeComponent()
		{
			this.colorfield = new creaturevisualizer.ColorField();
			this.tb_Hex = new creaturevisualizer.TextboxRestrictive();
			this.colorbox1 = new creaturevisualizer.ColorBox();
			this.colorbox0 = new creaturevisualizer.ColorBox();
			this.colorslider = new creaturevisualizer.ColorSlider();
			this.swatches = new creaturevisualizer.ColorSwatchPanel();
			this.rgbColorSpace = new creaturevisualizer.RgbColorSpace();
			this.hsbColorSpace = new creaturevisualizer.HsbColorSpace();
			this.la_Hex = new System.Windows.Forms.Label();
			this.la_Alpha = new System.Windows.Forms.Label();
			this.tb_Alpha = new creaturevisualizer.TextboxRestrictive();
			this.SuspendLayout();
			// 
			// colorfield
			// 
			this.colorfield.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorfield.Location = new System.Drawing.Point(5, 8);
			this.colorfield.Margin = new System.Windows.Forms.Padding(0);
			this.colorfield.Name = "colorfield";
			this.colorfield.Size = new System.Drawing.Size(256, 256);
			this.colorfield.TabIndex = 0;
			// 
			// tb_Hex
			// 
			this.tb_Hex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Hex.Location = new System.Drawing.Point(345, 220);
			this.tb_Hex.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Hex.MaxLength = 6;
			this.tb_Hex.Name = "tb_Hex";
			this.tb_Hex.Restrict = creaturevisualizer.TextboxRestrictive.Type.Hecate;
			this.tb_Hex.Size = new System.Drawing.Size(45, 20);
			this.tb_Hex.TabIndex = 7;
			this.tb_Hex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_Hex.TextChanged += new System.EventHandler(this.textchanged_hecate);
			// 
			// colorbox1
			// 
			this.colorbox1.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorbox1.Location = new System.Drawing.Point(305, 8);
			this.colorbox1.Margin = new System.Windows.Forms.Padding(0);
			this.colorbox1.Name = "colorbox1";
			this.colorbox1.Size = new System.Drawing.Size(80, 30);
			this.colorbox1.TabIndex = 2;
			this.colorbox1.TabStop = false;
			this.colorbox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dragdrop_colorbox);
			this.colorbox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_colorbox);
			// 
			// colorbox0
			// 
			this.colorbox0.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorbox0.Location = new System.Drawing.Point(305, 38);
			this.colorbox0.Margin = new System.Windows.Forms.Padding(0);
			this.colorbox0.Name = "colorbox0";
			this.colorbox0.Size = new System.Drawing.Size(80, 30);
			this.colorbox0.TabIndex = 3;
			this.colorbox0.TabStop = false;
			this.colorbox0.DragDrop += new System.Windows.Forms.DragEventHandler(this.dragdrop_colorbox);
			this.colorbox0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_colorbox);
			// 
			// colorslider
			// 
			this.colorslider.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorslider.Location = new System.Drawing.Point(265, 3);
			this.colorslider.Margin = new System.Windows.Forms.Padding(0);
			this.colorslider.Name = "colorslider";
			this.colorslider.Size = new System.Drawing.Size(36, 267);
			this.colorslider.TabIndex = 1;
			this.colorslider.Value = 0;
			this.colorslider.ValueChanged += new creaturevisualizer.ValueChangedEventHandler(this.valuechanged_slider);
			// 
			// swatches
			// 
			this.swatches.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.swatches.Location = new System.Drawing.Point(395, 8);
			this.swatches.Margin = new System.Windows.Forms.Padding(0);
			this.swatches.Name = "swatches";
			this.swatches.Size = new System.Drawing.Size(100, 222);
			this.swatches.TabIndex = 10;
			// 
			// rgbColorSpace
			// 
			this.rgbColorSpace.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rgbColorSpace.Location = new System.Drawing.Point(310, 150);
			this.rgbColorSpace.Margin = new System.Windows.Forms.Padding(0);
			this.rgbColorSpace.Name = "rgbColorSpace";
			this.rgbColorSpace.Size = new System.Drawing.Size(75, 60);
			this.rgbColorSpace.TabIndex = 5;
			this.rgbColorSpace.ComponentValueChanged += new creaturevisualizer.ColorSpaceEventHandler(this.componentvaluechanged_colorspace);
			this.rgbColorSpace.SelectedComponentChanged += new creaturevisualizer.ColorSpaceEventHandler(this.selectedcomponentchanged_colorspace);
			// 
			// hsbColorSpace
			// 
			this.hsbColorSpace.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hsbColorSpace.Location = new System.Drawing.Point(310, 80);
			this.hsbColorSpace.Margin = new System.Windows.Forms.Padding(0);
			this.hsbColorSpace.Name = "hsbColorSpace";
			this.hsbColorSpace.Size = new System.Drawing.Size(75, 60);
			this.hsbColorSpace.TabIndex = 4;
			this.hsbColorSpace.ComponentValueChanged += new creaturevisualizer.ColorSpaceEventHandler(this.componentvaluechanged_colorspace);
			this.hsbColorSpace.SelectedComponentChanged += new creaturevisualizer.ColorSpaceEventHandler(this.selectedcomponentchanged_colorspace);
			// 
			// la_Hex
			// 
			this.la_Hex.Location = new System.Drawing.Point(305, 220);
			this.la_Hex.Margin = new System.Windows.Forms.Padding(0);
			this.la_Hex.Name = "la_Hex";
			this.la_Hex.Size = new System.Drawing.Size(40, 20);
			this.la_Hex.TabIndex = 6;
			this.la_Hex.Text = "hex";
			this.la_Hex.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_Alpha
			// 
			this.la_Alpha.Location = new System.Drawing.Point(305, 245);
			this.la_Alpha.Margin = new System.Windows.Forms.Padding(0);
			this.la_Alpha.Name = "la_Alpha";
			this.la_Alpha.Size = new System.Drawing.Size(40, 20);
			this.la_Alpha.TabIndex = 8;
			this.la_Alpha.Text = "alpha";
			this.la_Alpha.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_Alpha
			// 
			this.tb_Alpha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Alpha.Location = new System.Drawing.Point(345, 245);
			this.tb_Alpha.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Alpha.MaxLength = 3;
			this.tb_Alpha.Name = "tb_Alpha";
			this.tb_Alpha.Restrict = creaturevisualizer.TextboxRestrictive.Type.Byte;
			this.tb_Alpha.Size = new System.Drawing.Size(45, 20);
			this.tb_Alpha.TabIndex = 9;
			this.tb_Alpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_Alpha.TextChanged += new System.EventHandler(this.textchanged_alpha);
			// 
			// ColorPanel
			// 
			this.Controls.Add(this.colorfield);
			this.Controls.Add(this.colorslider);
			this.Controls.Add(this.colorbox1);
			this.Controls.Add(this.colorbox0);
			this.Controls.Add(this.hsbColorSpace);
			this.Controls.Add(this.rgbColorSpace);
			this.Controls.Add(this.la_Hex);
			this.Controls.Add(this.tb_Hex);
			this.Controls.Add(this.la_Alpha);
			this.Controls.Add(this.tb_Alpha);
			this.Controls.Add(this.swatches);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorPanel";
			this.Size = new System.Drawing.Size(500, 270);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
/*		ColorPanelSettings _settings;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ColorPanelSettings Settings
		{
			get
			{
				if (_settings == null)
					_settings = new ColorPanelSettings();

				_settings.TopColorBoxColor    = colorbox1.BackColor;
				_settings.BottomColorBoxColor = colorbox0.BackColor;

				_settings.ActiveColorBox = GetActiveColorbox().Name;

				_settings.SelectedColorSpaceComponent = _colorspace.Selected.Name;

				return _settings;
			}
			set
			{
				if ((_settings = value) != null)
				{
					bool flag = false;

					foreach (ColorSpaceControl csc in rgbColorSpace.ColorSpaceControls)
					{
						if (csc.Name.Equals(_settings.SelectedColorSpaceComponent))
						{
							rgbColorSpace.ChangeCurrentlySelectedComponent(csc);
							flag = true;
							break;
						}
					}

					if (!flag)
					{
						foreach (ColorSpaceControl csc in hsbColorSpace.ColorSpaceControls)
						{
							if (csc.Name.Equals(_settings.SelectedColorSpaceComponent))
							{
								hsbColorSpace.ChangeCurrentlySelectedComponent(csc);
								break;
							}
						}
					}

					colorbox1.BackColor = _settings.TopColorBoxColor;
					colorbox0.BackColor = _settings.BottomColorBoxColor;

					ColorBox bo;

					if (_settings.ActiveColorBox.Equals(colorbox1.Name))
					{
						bo = colorbox1;
					}
					else //if (_settings.ActiveColorBox.Equals(colorbox0.Name))
					{
						bo = colorbox0;
					}

					rgbColorSpace.Structure = ColorConverter.ColorToRgb(bo.BackColor);
					hsbColorSpace.Structure = ColorConverter.ColorToHsb(bo.BackColor);

					SetActiveColorbox(bo);
					SetSliderValue();
					UpdateColorPanels(true, true, true);
				}
			}
		} */
