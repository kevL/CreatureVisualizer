using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

using CreatureVisualizer.Properties;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorPanel
	sealed class ColorControl
		: UserControl
	{
		#region Events
		internal event ColorChangedEventHandler ColorChanged;
		#endregion Events


		#region Fields
		ColorSpaceControl _csCurrent;

		Bitmap _slider   = new Bitmap(ColorSlider.width, ColorSlider.height);
		Image  _checkers = new ResourceManager("CreatureVisualizer.Properties.Resources",
											   typeof(Resources).Assembly).GetObject("checkers") as Image;
		#endregion Fields


		#region Properties
		// Good god, those bastards go about things in a cockamamie way ...
		// TODO: Consolidate firing the ColorChanged event in a central function
		// such as SelectColor() or Satisfy() or similar.

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Color Color
		{
			get
			{
				return GetActiveColorbox().BackColor;
			}
			set
			{
				SetColor(value, true);
				tb_Alpha.Text = value.A.ToString();
			}
		}

//		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
//		[Browsable(false)]
//		public byte Alpha // IMPORTANT: Set alpha by its textbox.
//		{ get; set; }
		#endregion Properties


		#region cTor
		public ColorControl()
		{
			InitializeComponent();

			GradientService.InstantiateConstantObjects();
			using (Graphics graphics = Graphics.FromImage(_slider))
				GradientService.DrawSlider(graphics, new Rectangle(0,0, ColorSlider.width, ColorSlider.height));

			colortop.Activated = true;

			hsbColorSpace.SelectCo(hsbColorSpace.coHue);
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

// draw border around colorfield ->
			e.Graphics.DrawRectangle(Pens.Black,
									 colorfield.Left  - 1, colorfield.Top    - 1,
									 colorfield.Width + 1, colorfield.Height + 1);


// draw checkers under colorboxes ->
			e.Graphics.DrawImage(_checkers, colortop.Location.X, colortop.Location.Y);

// draw borders top/bot for colorboxes -> (left/right borders is handled by the colorboxes' OnPaint())
			e.Graphics.DrawLine(Pens.Black,
								colortop.Location.X                  + 2, colortop.Top - 1,
								colortop.Location.X + colortop.Width - 3, colortop.Top - 1);
			e.Graphics.DrawLine(Pens.Black,
								colorbot.Location.X                  + 2, colorbot.Bottom,
								colorbot.Location.X + colorbot.Width - 3, colorbot.Bottom);


// draw border around swatches ->
			e.Graphics.DrawRectangle(Pens.Black,
									 swatches.Left  - 1, swatches.Top    - 1,
									 swatches.Width + 1, swatches.Height + 1);
		}
		#endregion Handlers (override)


		#region Handlers
		internal void mouseup_colorbox(object sender, MouseEventArgs e)
		{
			if (sender == null														// -> fired by ColorF.OnKeyDown()
				|| (e.Button == MouseButtons.Right
					&& ((sender as ColorBox).ClientRectangle.Contains(e.X, e.Y))))	// -> don't fire if user moves cursor out of the box.
			{
				ColorBox bo = SwapActiveColorbox();

				rgbColorSpace.Structure = ColorConverter.ColorToRgb(bo.BackColor);
				hsbColorSpace.Structure = ColorConverter.ColorToHsb(bo.BackColor);

				if (tb_Alpha.Text != bo.Alpha.ToString())
					tb_Alpha.Text  = bo.Alpha.ToString();

				SetSlider();
				Satisfy(true, true, true);

				if (ColorChanged != null)
					ColorChanged();
			}
		}

		void sliderchanged_slider(SliderChangedEventArgs e)
		{
			int val = e.Val;

			switch (_csCurrent.Co.Units)
			{
				case ColorSpaceControlCo.Unit.Degree:
					val = (int)Math.Ceiling(val * 24.0 / 17.0);
					if (val == 360)
						val  = 0;
					break;

				case ColorSpaceControlCo.Unit.Percent:
					val = (int)Math.Ceiling(val / 2.55);
					break;
			}

			_csCurrent.Co.Val = val;

			if (_csCurrent is ColorSpaceRgb)
			{
				hsbColorSpace.Structure = ColorConverter.RgbToHsb((RGB)rgbColorSpace.Structure);
			}
			else if (_csCurrent is ColorSpaceHsb)
			{
				rgbColorSpace.Structure = ColorConverter.HsbToRgb((HSB)hsbColorSpace.Structure);
			}

			Satisfy(false, false, true);

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(bo.Alpha, ColorConverter.RgbToColor((RGB)rgbColorSpace.Structure));

			hsbColorSpace.Refresh(); // fast updates ->
			rgbColorSpace.Refresh();
			tb_Hecate    .Refresh();
			bo           .Refresh();

			if (ColorChanged != null)
				ColorChanged();
		}

		void selectedcochanged_csc(ColorSpaceControl sender)
		{
			if (sender is ColorSpaceRgb)
			{
				hsbColorSpace.DeselectComponents();
			}
			else if (sender is ColorSpaceHsb)
			{
				rgbColorSpace.DeselectComponents();
			}

			_csCurrent = sender;

			SetSlider();
			Satisfy(true, true, true);
		}

		void covaluechanged_csc(ColorSpaceControl sender)
		{
			if (sender is ColorSpaceRgb)
			{
				hsbColorSpace.Structure = ColorConverter.RgbToHsb((RGB)rgbColorSpace.Structure);
			}
			else if (sender is ColorSpaceHsb)
			{
				rgbColorSpace.Structure = ColorConverter.HsbToRgb((HSB)hsbColorSpace.Structure);
			}

			SetSlider();
			Satisfy(true, true, true);

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(bo.Alpha, ColorConverter.RgbToColor((RGB)rgbColorSpace.Structure));

			if (ColorChanged != null)
				ColorChanged();
		}

		void swatchselected_swatches(ColorEventArgs e)
		{
			SetColor(e.Color, true);
		}

		void pointselected_colorfield(ColorEventArgs e)
		{
			RGB rgb = ColorConverter.ColorToRgb(e.Color);
			rgbColorSpace.Structure = rgb;

			HSB hsb = ColorConverter.RgbToHsb(rgb);

			switch (_csCurrent.Co.DisplayCharacter)
			{
				case 'H':
				{
					int hue = ((HSB)hsbColorSpace.Structure).H;
					hsbColorSpace.Structure = new HSB(hue, hsb.S, hsb.B);
					break;
				}

				case 'S':
				{
					int sat = ((HSB)hsbColorSpace.Structure).S;
					hsbColorSpace.Structure = new HSB(hsb.H, sat, hsb.B);
					break;
				}

				default:
					hsbColorSpace.Structure = hsb;
					break;
			}

			tb_Hecate.Text = rgbColorSpace.GetHecate();

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(bo.Alpha, ColorConverter.RgbToColor(rgb));

			hsbColorSpace.Refresh(); // fast updates ->
			rgbColorSpace.Refresh();
			tb_Hecate    .Refresh();
			bo           .Refresh();

			if (ColorChanged != null)
				ColorChanged();
		}


		void textchanged_hecate(object sender, EventArgs e)
		{
			RGB rgb = ColorConverter.HexToRgb(tb_Hecate.Text);
			rgbColorSpace.Structure = rgb;
			hsbColorSpace.Structure = ColorConverter.RgbToHsb(rgb);

			SetSlider();
			Satisfy(true, true, false);
		}

		void textchanged_alpha(object sender, EventArgs e)
		{
			string alpha;
			if (!String.IsNullOrEmpty(tb_Alpha.Text)) alpha = tb_Alpha.Text;
			else                                      alpha = "0";

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(Byte.Parse(alpha), bo.BackColor);
			bo.Invalidate();

			SetColor(bo.BackColor, false, false);
		}

		void mousehover_label(object sender, EventArgs e)
		{
			if ((sender as Label) == la_Alpha)
			{
				ColorF.That.Print("Alpha 0..255 byte");
			}
			else //if ((sender as Label) == la_Hex)
			{
				ColorF.That.Print("RGB 000000..FFFFFF");
			}
		}
		#endregion Handlers


		#region Methods
		void SetColor(Color color, bool setSlider, bool setHecateText = true)
		{
			if (!ColorConverter.ColorToRgb(color).Equals(rgbColorSpace.Structure)	// TODO: store alpha in the Structure(s)
				|| !setHecateText)													// TODO: remove 'setHecateText' shenanigans
			{
				RGB rgb = ColorConverter.ColorToRgb(color);
				rgbColorSpace.Structure = rgb;
				hsbColorSpace.Structure = ColorConverter.RgbToHsb(rgb);

				if (setSlider) SetSlider();
				Satisfy(false, true, setHecateText);

				if (ColorChanged != null)
					ColorChanged();
			}
		}

		void SetSlider()
		{
			int val = _csCurrent.Co.Val;

			switch (_csCurrent.Co.Units)
			{
				case ColorSpaceControlCo.Unit.Degree:
					val = (int)Math.Ceiling(val * 17.0 / 24.0);
					break;

				case ColorSpaceControlCo.Unit.Percent:
					val = (int)Math.Ceiling(val * 2.55);
					break;
			}
			colorslider.Value = val;
		}

		void Satisfy(bool setSliderColorspace, bool setPoint, bool setHecateText)
		{
			string alpha;
			if (!String.IsNullOrEmpty(tb_Alpha.Text)) alpha = tb_Alpha.Text;
			else                                      alpha = "0";

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(bo.Alpha, ColorConverter.RgbToColor((RGB)rgbColorSpace.Structure));
			bo.Invalidate();


			if (setSliderColorspace)
				colorslider.ChangeColorspace(_csCurrent);


			if (_csCurrent is ColorSpaceHsb)
			{
				if (_csCurrent.Co.DisplayCharacter == 'H')
				{
					Color color = _slider.GetPixel(0, 255 - colorslider.Value);
					colorfield.ChangeColor(color, _csCurrent, setPoint);
				}
				else // 'S','B'
					colorfield.ChangeColor(_csCurrent.Co.Val, _csCurrent, setPoint);
			}
			else if (_csCurrent is ColorSpaceRgb) // 'R','G','B'
			{
				colorfield.ChangeColor(_csCurrent.Co.Val, _csCurrent, setPoint);
			}


			if (setHecateText)
				tb_Hecate.Text = rgbColorSpace.GetHecate();
		}


		ColorBox SwapActiveColorbox()
		{
			ColorBox act;

			if (GetActiveColorbox() == colorbot)
			{
				colortop.Activated = true;
				colorbot.Activated = false;
				act = colortop;
			}
			else
			{
				colortop.Activated = false;
				colorbot.Activated = true;
				act = colorbot;
			}

			swatches.SelectSwatch(act.BackColor);
			return act;
		}

		internal ColorBox GetActiveColorbox()
		{
			if (colortop.Activated)
				return colortop;

			return colorbot;
		}

		internal void InitializeColor(Color color)
		{
			colorbot.BackColor = color;
			swatches.SelectSwatch(color);
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
		ColorBox colortop;
		ColorBox colorbot;
		ColorSpaceHsb hsbColorSpace;
		ColorSpaceRgb rgbColorSpace;
		Label la_Hecate;
		TextboxRestrictive tb_Hecate;
		Label la_Alpha;
		TextboxRestrictive tb_Alpha;
		SwatchControl swatches;


		void InitializeComponent()
		{
			this.colorfield = new creaturevisualizer.ColorField();
			this.colortop = new creaturevisualizer.ColorBox();
			this.colorbot = new creaturevisualizer.ColorBox();
			this.colorslider = new creaturevisualizer.ColorSlider();
			this.swatches = new creaturevisualizer.SwatchControl();
			this.rgbColorSpace = new creaturevisualizer.ColorSpaceRgb();
			this.hsbColorSpace = new creaturevisualizer.ColorSpaceHsb();
			this.tb_Hecate = new creaturevisualizer.TextboxRestrictive();
			this.tb_Alpha = new creaturevisualizer.TextboxRestrictive();
			this.la_Hecate = new System.Windows.Forms.Label();
			this.la_Alpha = new System.Windows.Forms.Label();
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
			this.colorfield.TabStop = false;
			this.colorfield.PointSelected += new creaturevisualizer.PointSelectedEventHandler(this.pointselected_colorfield);
			// 
			// colortop
			// 
			this.colortop.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colortop.Location = new System.Drawing.Point(305, 8);
			this.colortop.Margin = new System.Windows.Forms.Padding(0);
			this.colortop.Name = "colortop";
			this.colortop.Size = new System.Drawing.Size(80, 30);
			this.colortop.TabIndex = 2;
			this.colortop.TabStop = false;
			this.colortop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_colorbox);
			// 
			// colorbot
			// 
			this.colorbot.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorbot.Location = new System.Drawing.Point(305, 38);
			this.colorbot.Margin = new System.Windows.Forms.Padding(0);
			this.colorbot.Name = "colorbot";
			this.colorbot.Size = new System.Drawing.Size(80, 30);
			this.colorbot.TabIndex = 3;
			this.colorbot.TabStop = false;
			this.colorbot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_colorbox);
			// 
			// colorslider
			// 
			this.colorslider.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorslider.Location = new System.Drawing.Point(265, 3);
			this.colorslider.Margin = new System.Windows.Forms.Padding(0);
			this.colorslider.Name = "colorslider";
			this.colorslider.Size = new System.Drawing.Size(36, 267);
			this.colorslider.TabIndex = 1;
			this.colorslider.TabStop = false;
			this.colorslider.Value = 0;
			this.colorslider.SliderChanged += new creaturevisualizer.SliderChangedEventHandler(this.sliderchanged_slider);
			// 
			// swatches
			// 
			this.swatches.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.swatches.Location = new System.Drawing.Point(395, 8);
			this.swatches.Margin = new System.Windows.Forms.Padding(0);
			this.swatches.Name = "swatches";
			this.swatches.Size = new System.Drawing.Size(93, 309);
			this.swatches.TabIndex = 10;
			this.swatches.TabStop = false;
			this.swatches.SwatchSelected += new creaturevisualizer.SwatchSelectedEventHandler(this.swatchselected_swatches);
			// 
			// rgbColorSpace
			// 
			this.rgbColorSpace.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rgbColorSpace.Location = new System.Drawing.Point(310, 150);
			this.rgbColorSpace.Margin = new System.Windows.Forms.Padding(0);
			this.rgbColorSpace.Name = "rgbColorSpace";
			this.rgbColorSpace.Size = new System.Drawing.Size(75, 60);
			this.rgbColorSpace.TabIndex = 5;
			this.rgbColorSpace.SelectedCoChanged += new creaturevisualizer.ColorSpaceEventHandler(this.selectedcochanged_csc);
			this.rgbColorSpace.CoValueChanged += new creaturevisualizer.ColorSpaceEventHandler(this.covaluechanged_csc);
			// 
			// hsbColorSpace
			// 
			this.hsbColorSpace.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hsbColorSpace.Location = new System.Drawing.Point(310, 80);
			this.hsbColorSpace.Margin = new System.Windows.Forms.Padding(0);
			this.hsbColorSpace.Name = "hsbColorSpace";
			this.hsbColorSpace.Size = new System.Drawing.Size(75, 60);
			this.hsbColorSpace.TabIndex = 4;
			this.hsbColorSpace.SelectedCoChanged += new creaturevisualizer.ColorSpaceEventHandler(this.selectedcochanged_csc);
			this.hsbColorSpace.CoValueChanged += new creaturevisualizer.ColorSpaceEventHandler(this.covaluechanged_csc);
			// 
			// tb_Hecate
			// 
			this.tb_Hecate.BackColor = System.Drawing.Color.White;
			this.tb_Hecate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Hecate.Location = new System.Drawing.Point(344, 220);
			this.tb_Hecate.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Hecate.MaxLength = 6;
			this.tb_Hecate.Name = "tb_Hecate";
			this.tb_Hecate.Restrict = creaturevisualizer.TextboxRestrictive.Type.Hecate;
			this.tb_Hecate.Size = new System.Drawing.Size(45, 20);
			this.tb_Hecate.TabIndex = 7;
			this.tb_Hecate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_Hecate.TextChanged += new System.EventHandler(this.textchanged_hecate);
			// 
			// tb_Alpha
			// 
			this.tb_Alpha.BackColor = System.Drawing.Color.White;
			this.tb_Alpha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Alpha.Location = new System.Drawing.Point(344, 245);
			this.tb_Alpha.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Alpha.MaxLength = 3;
			this.tb_Alpha.Name = "tb_Alpha";
			this.tb_Alpha.Restrict = creaturevisualizer.TextboxRestrictive.Type.Byte;
			this.tb_Alpha.Size = new System.Drawing.Size(45, 20);
			this.tb_Alpha.TabIndex = 9;
			this.tb_Alpha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_Alpha.TextChanged += new System.EventHandler(this.textchanged_alpha);
			// 
			// la_Hecate
			// 
			this.la_Hecate.Location = new System.Drawing.Point(305, 220);
			this.la_Hecate.Margin = new System.Windows.Forms.Padding(0);
			this.la_Hecate.Name = "la_Hecate";
			this.la_Hecate.Size = new System.Drawing.Size(39, 20);
			this.la_Hecate.TabIndex = 6;
			this.la_Hecate.Text = "hex";
			this.la_Hecate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_Hecate.MouseHover += new System.EventHandler(this.mousehover_label);
			// 
			// la_Alpha
			// 
			this.la_Alpha.Location = new System.Drawing.Point(305, 245);
			this.la_Alpha.Margin = new System.Windows.Forms.Padding(0);
			this.la_Alpha.Name = "la_Alpha";
			this.la_Alpha.Size = new System.Drawing.Size(39, 20);
			this.la_Alpha.TabIndex = 8;
			this.la_Alpha.Text = "alpha";
			this.la_Alpha.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.la_Alpha.MouseHover += new System.EventHandler(this.mousehover_label);
			// 
			// ColorControl
			// 
			this.Controls.Add(this.colorfield);
			this.Controls.Add(this.colorslider);
			this.Controls.Add(this.colortop);
			this.Controls.Add(this.colorbot);
			this.Controls.Add(this.hsbColorSpace);
			this.Controls.Add(this.rgbColorSpace);
			this.Controls.Add(this.la_Hecate);
			this.Controls.Add(this.tb_Hecate);
			this.Controls.Add(this.la_Alpha);
			this.Controls.Add(this.tb_Alpha);
			this.Controls.Add(this.swatches);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorControl";
			this.Size = new System.Drawing.Size(490, 317);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
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

					foreach (ColorSpaceControlCo co in rgbColorSpace.ColorSpaceControls)
					{
						if (co.Name.Equals(_settings.SelectedColorSpaceComponent))
						{
							rgbColorSpace.ChangeCurrentlySelectedComponent(co);
							flag = true;
							break;
						}
					}

					if (!flag)
					{
						foreach (ColorSpaceControlCo co in hsbColorSpace.ColorSpaceControls)
						{
							if (co.Name.Equals(_settings.SelectedColorSpaceComponent))
							{
								hsbColorSpace.ChangeCurrentlySelectedComponent(co);
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


	internal delegate void ColorChangedEventHandler();
}
