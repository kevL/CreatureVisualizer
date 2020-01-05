using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
		internal event ColorChangedEvent ColorChanged;
		#endregion Events


		#region Fields (static)
		internal static bool _bypassCisco;
		internal static bool _bypassAlpha;
		internal static bool _bypassHecate;
		#endregion Fields (static)


		#region Fields
		ColorSpaceControl _csc;

		Bitmap _slider   = new Bitmap(ColorSlider.width, ColorSlider.height);
		Image  _checkers = new ResourceManager("CreatureVisualizer.Properties.Resources",
											   typeof(Resources).Assembly).GetObject("checkers") as Image;
		#endregion Fields


		#region cTor
		public ColorControl()
		{
			InitializeComponent();

			GradientService.InstantiateConstantObjects();

			// this has nothing to do with the Slider ...
			using (Graphics graphics = Graphics.FromImage(_slider))
			{
				graphics.PixelOffsetMode = PixelOffsetMode.Half;
				GradientService.DrawField_base(graphics, new Rectangle(0,0, 1,256));
			}
		}

		/// <summary>
		/// This must be called after the Color form is instantiated and before
		/// the form is shown. It sets up the form's initial display.
		/// </summary>
		/// <param name="color"></param>
		internal void InitialColor(Color color)
		{
			swatches.UpdateSelector(color);

			colortop.Activated = true;
			colortop.BackColor =
			colorbot.BackColor = color;

			_bypassCisco = true;
			cscRgb.rgb = ColorConverter.ColorToRgb(color);
			cscHsl.hsl = ColorConverter.ColorToHsl(color);
			_bypassCisco = false;

			_bypassAlpha = true;
			tb_Alpha.Text = color.A.ToString();
			_bypassAlpha = false;

			_bypassHecate = true;
			tb_Hecate.Text = cscRgb.GetHecate();
			_bypassHecate = false;


			cscHsl.SelectCisco(cscHsl.cH); // this shall start things -> 'ciscoselected(cscHsl)'
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
//			base.OnPaint(e);

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
		void ciscoselected(ColorSpaceControl sender)
		{
			if (sender is ColorSpaceControlRGB)
			{
				cscHsl.DeselectCiscos();
			}
			else
				cscRgb.DeselectCiscos();

			colorslider.Configurate(_csc = sender);
			UpdateField();
		}

		void ciscovaluechanged(ColorSpaceControl sender)
		{
			_bypassCisco = true;
			if (sender is ColorSpaceControlRGB)
			{
				cscHsl.hsl = ColorConverter.RgbToHsl(cscRgb.rgb);
			}
			else
				cscRgb.rgb = ColorConverter.HslToRgb(cscHsl.hsl);
			_bypassCisco = false;

			_bypassHecate = true;
			tb_Hecate.Text = cscRgb.GetHecate();
			_bypassHecate = false;

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(Byte.Parse(tb_Alpha.Text),
										  ColorConverter.RgbToColor(cscRgb.rgb));

			colorslider.Configurate(_csc);
			UpdateField();

			if (ColorChanged != null)
				ColorChanged();
		}

		void sliderchanged(SliderChangedEventArgs e)
		{
			_bypassCisco = true;
			if (_csc is ColorSpaceControlRGB)
			{
				cscHsl.hsl = ColorConverter.RgbToHsl(cscRgb.rgb);
				cscHsl.Refresh();
			}
			else
			{
				cscRgb.rgb = ColorConverter.HslToRgb(cscHsl.hsl);
				cscRgb.Refresh();
			}
			_bypassCisco = false;

			_bypassHecate = true;
			tb_Hecate.Text = cscRgb.GetHecate();
			tb_Hecate.Refresh();
			_bypassHecate = false;

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(Byte.Parse(tb_Alpha.Text), cscRgb.GetColor());
			bo.Refresh();

			UpdateField(false);

			if (ColorChanged != null)
				ColorChanged();
		}

		void pointselected(ColorEventArgs e)
		{
			_bypassCisco = true;
			cscRgb.rgb = ColorConverter.ColorToRgb(e.Color);
			cscHsl.hsl = ColorConverter.ColorToHsl(e.Color);

			cscHsl.Refresh();
			cscRgb.Refresh();
			_bypassCisco = false;

			_bypassHecate = true;
			tb_Hecate.Text = cscRgb.GetHecate();
			tb_Hecate.Refresh();
			_bypassHecate = false;

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = Color.FromArgb(Byte.Parse(tb_Alpha.Text), e.Color);
			bo.Refresh();

			colorslider.UpdateSlider();

			if (ColorChanged != null)
				ColorChanged();
		}

		void textchanged_alpha(object sender, EventArgs e)
		{
			if (!_bypassAlpha)
			{
				string alpha;
				if (!String.IsNullOrEmpty(tb_Alpha.Text)) alpha = tb_Alpha.Text;
				else                                      alpha = "0";

				ColorBox bo = GetActiveColorbox();
				bo.BackColor = Color.FromArgb(Byte.Parse(alpha), bo.BackColor);

				if (ColorChanged != null)
					ColorChanged();
			}
		}

		void textchanged_hecate(object sender, EventArgs e)
		{
			if (!_bypassHecate)
			{
				_bypassCisco = true;
				cscRgb.rgb = ColorConverter.HecateToRgb(tb_Hecate.Text);
				cscHsl.hsl = ColorConverter.RgbToHsl(cscRgb.rgb);
				_bypassCisco = false;

				GetActiveColorbox().BackColor = Color.FromArgb(Byte.Parse(tb_Alpha.Text),
															   ColorConverter.RgbToColor(cscRgb.rgb));

				colorslider.Configurate(_csc);
				UpdateField();

				if (ColorChanged != null)
					ColorChanged();
			}
		}

		void swatchselected(ColorEventArgs e)
		{
			_bypassCisco = true;
			cscRgb.rgb = ColorConverter.ColorToRgb(e.Color);
			cscHsl.hsl = ColorConverter.ColorToHsl(e.Color);
			_bypassCisco = false;

			_bypassAlpha = true;
			tb_Alpha.Text = e.Color.A.ToString();
			_bypassAlpha = false;

			_bypassHecate = true;
			tb_Hecate.Text = cscRgb.GetHecate();
			_bypassHecate = false;

			ColorBox bo = GetActiveColorbox();
			bo.BackColor = e.Color;

			colorslider.Configurate(_csc);
			UpdateField();

			if (ColorChanged != null)
				ColorChanged();
		}

		internal void mouseup_colorbox(object sender, MouseEventArgs e)
		{
			if (sender == null														// -> fired by ColorF.OnKeyDown()
				|| (e.Button == MouseButtons.Right
					&& ((sender as ColorBox).ClientRectangle.Contains(e.X, e.Y))))	// -> don't fire if user moves cursor out of the box.
			{
				colortop.Activated = !colortop.Activated;
				colorbot.Activated = !colorbot.Activated;
	
				ColorBox bo = GetActiveColorbox();
				swatches.UpdateSelector(bo.BackColor);

				_bypassCisco = true;
				cscRgb.rgb = ColorConverter.ColorToRgb(bo.BackColor);
				cscHsl.hsl = ColorConverter.ColorToHsl(bo.BackColor);
				_bypassCisco = false;

				_bypassAlpha = true;
				tb_Alpha.Text = bo.BackColor.A.ToString();
				_bypassAlpha = false;

				_bypassHecate = true;
				tb_Hecate.Text = cscRgb.GetHecate();
				_bypassHecate = false;

				colorslider.Configurate(_csc);
				UpdateField();

				if (ColorChanged != null)
					ColorChanged();
			}
		}
		#endregion Handlers


		#region Methods
		void UpdateField(bool setPoint = true)
		{
			switch (_csc.Cisco.DisplayCharacter)
			{
				// _csc is ColorSpaceControlHSL
				case 'H':
				{
					Color slidecol = _slider.GetPixel(0, 255 - colorslider.Val);
					colorfield.ChangeField(slidecol, _csc, setPoint);
					break;
				}
				case 'S':
				case 'L':

				// _csc is ColorSpaceControlRGB
				case 'R':
				case 'G':
				case 'B':
					colorfield.ChangeField(_csc.Cisco.Val, _csc, setPoint);
					break;
			}
		}

		internal Color GetColor()
		{
			return GetActiveColorbox().BackColor;
		}

		internal ColorBox GetActiveColorbox()
		{
			if (colortop.Activated)
				return colortop;

			return colorbot;
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
		internal ColorField colorfield;
		internal ColorSlider colorslider;
		ColorBox colortop;
		ColorBox colorbot;
		ColorSpaceControlHSL cscHsl;
		ColorSpaceControlRGB cscRgb;
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
			this.cscRgb = new creaturevisualizer.ColorSpaceControlRGB();
			this.cscHsl = new creaturevisualizer.ColorSpaceControlHSL();
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
			this.colorfield.PointSelected += new creaturevisualizer.PointSelectedEvent(this.pointselected);
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
			this.colorslider.SliderChanged += new creaturevisualizer.SliderChangedEvent(this.sliderchanged);
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
			this.swatches.SwatchSelected += new creaturevisualizer.SwatchSelectedEvent(this.swatchselected);
			// 
			// cscRgb
			// 
			this.cscRgb.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscRgb.Location = new System.Drawing.Point(310, 150);
			this.cscRgb.Margin = new System.Windows.Forms.Padding(0);
			this.cscRgb.Name = "cscRgb";
			this.cscRgb.Size = new System.Drawing.Size(75, 60);
			this.cscRgb.TabIndex = 5;
			this.cscRgb.CiscoSelected_hi += new creaturevisualizer.ColorSpaceEvent(this.ciscoselected);
			this.cscRgb.CiscoValueChanged_hi += new creaturevisualizer.ColorSpaceEvent(this.ciscovaluechanged);
			// 
			// cscHsl
			// 
			this.cscHsl.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscHsl.Location = new System.Drawing.Point(310, 80);
			this.cscHsl.Margin = new System.Windows.Forms.Padding(0);
			this.cscHsl.Name = "cscHsl";
			this.cscHsl.Size = new System.Drawing.Size(75, 60);
			this.cscHsl.TabIndex = 4;
			this.cscHsl.CiscoSelected_hi += new creaturevisualizer.ColorSpaceEvent(this.ciscoselected);
			this.cscHsl.CiscoValueChanged_hi += new creaturevisualizer.ColorSpaceEvent(this.ciscovaluechanged);
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
			// 
			// ColorControl
			// 
			this.Controls.Add(this.colorfield);
			this.Controls.Add(this.colorslider);
			this.Controls.Add(this.colortop);
			this.Controls.Add(this.colorbot);
			this.Controls.Add(this.cscHsl);
			this.Controls.Add(this.cscRgb);
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

					foreach (ColorSpaceControlCisco co in rgbColorSpace.ColorSpaceControls)
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
						foreach (ColorSpaceControlCisco co in hsbColorSpace.ColorSpaceControls)
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


	internal delegate void ColorChangedEvent();
}
