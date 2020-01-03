using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorFieldPanel
	sealed class ColorField
		: UserControl
	{
		#region Events
		public event PointSelectedEvent PointSelected;
		#endregion Events


		#region Fields
		ColorSpaceControl _csc;

		Color _slidecol;
		int   _ciscoval;

		Color _color;
		Point _pt;

		bool _track;
		#endregion Fields


		#region cTor
		public ColorField()
		{
			InitializeComponent();
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!ColorF.reallyDesignMode)
			{
				e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
				DrawField(e.Graphics);
				e.Graphics.PixelOffsetMode = PixelOffsetMode.None;

				Pen pen;
				if (GradientService.IsBright(_color))
					pen = Pens.Black;
				else
					pen = Pens.White;

				e.Graphics.DrawEllipse(pen, _pt.X - 4, _pt.Y - 4, 8,8);
			}
		}

		void DrawField(Graphics graphics)
		{
			switch (_csc.Cisco.DisplayCharacter)
			{
				// _csc is ColorSpaceControlHSL
				case 'H': GradientService.DrawField_hue(graphics, _slidecol); break;
				case 'S': GradientService.DrawField_sat(graphics, _ciscoval); break;
				case 'L': GradientService.DrawField_lit(graphics, _ciscoval); break;

				// _csc is ColorSpaceControlRGB
				case 'R': GradientService.DrawField_r(graphics, _ciscoval); break;
				case 'G': GradientService.DrawField_g(graphics, _ciscoval); break;
				case 'B': GradientService.DrawField_b(graphics, _ciscoval); break;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_track = true;
				UpdatePoint(new Point(e.X, e.Y));
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_track = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
//			Color color = GetCurrentColor();
//			ColorF.That.Print("r= " + color.R + " g= " + color.G + " b= " + color.B + " a= " + color.A);

			if (_track)
			{
				const int leeway = 5;
				if (   (e.X > -1 - leeway && e.X < 256 + leeway)
					|| (e.Y > -1 - leeway && e.Y < 256 + leeway))
				{
					var pt = new Point(Math.Max(0, Math.Min(e.X, 255)),
									   Math.Max(0, Math.Min(e.Y, 255)));
					UpdatePoint(pt);
				}
			}
		}

		void UpdatePoint(Point pt)
		{
			Invalidate(new Rectangle(_pt.X - 4, _pt.Y - 4, 9,9));
			Invalidate(new Rectangle( pt.X - 4,  pt.Y - 4, 9,9));

			_pt = pt;
			SetPointColor();
			Update();

			if (PointSelected != null)
				PointSelected(new ColorEventArgs(_color));
		}

		void SetPointColor()
		{
			var csc = _csc as ColorSpaceControlRGB;
			if (csc != null)
			{
				var rgb = csc.rgb;
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'R': rgb = new RGB(rgb.R,       255 - _pt.Y, _pt.X); break;
					case 'G': rgb = new RGB(255 - _pt.Y, rgb.G,       _pt.X); break;
					case 'B': rgb = new RGB(_pt.X,       255 - _pt.Y, rgb.B); break;
				}
				_color = ColorConverter.RgbToColor(rgb);
			}
			else
			{
				var hsl = (_csc as ColorSpaceControlHSL).hsl;
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'H':
					{
						int lit = (int)((255f - _pt.Y) / 2.55f);
						int sat = (int)(        _pt.X  / 2.55f);
						lit = Math.Max(0, Math.Min(lit, 100));
						sat = Math.Max(0, Math.Min(sat, 100));

						hsl = new HSL(hsl.H, sat, lit);
						break;
					}

					case 'S':
					{
						int hue = (int)(_pt.X * 24.0f / 17.0f);
						int lit = (int)((255f - _pt.Y) / 2.55f);
						hue = Math.Max(0, Math.Min(hue, 359));
						lit = Math.Max(0, Math.Min(lit, 100));

						hsl = new HSL(hue, hsl.S, lit);
						break;
					}

					case 'L':
					{
						int hue = (int)(_pt.X * 24.0f / 17.0f);
						int sat = (int)((255f - _pt.Y) / 2.55f);
						hue = Math.Max(0, Math.Min(hue, 359));
						sat = Math.Max(0, Math.Min(sat, 100));

						hsl = new HSL(hue, sat, hsl.L);
						break;
					}
				}
				_color = ColorConverter.HslToColor(hsl);
			}
		}
		#endregion Handlers (override)


		#region Methods
		internal void ChangeField(Color slidecol, ColorSpaceControl csc, bool setPoint)
		{
			_slidecol = slidecol;
			ChangeColorspace(csc, setPoint);
		}

		internal void ChangeField(int ciscoval, ColorSpaceControl csc, bool setPoint)
		{
			_ciscoval = ciscoval;
			ChangeColorspace(csc, setPoint);
		}

		void ChangeColorspace(ColorSpaceControl csc, bool setPoint)
		{
			_csc = csc;
			if (setPoint)
			{
				CalculatePoint();
				SetPointColor();
			}

			Refresh();
		}

		void CalculatePoint()
		{
			int x = 0;
			int y = 0;

			var csc = _csc as ColorSpaceControlRGB;
			if (csc != null)
			{
				Color color = csc.GetColor();
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'R': x = color.B; y = 255 - color.G; break;
					case 'G': x = color.B; y = 255 - color.R; break;
					case 'B': x = color.R; y = 255 - color.G; break;
				}
			}
			else
			{
				var hsl = (_csc as ColorSpaceControlHSL).hsl;
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'H':
						x =       (int)Math.Round(hsl.S * 2.55);
						y = 255 - (int)Math.Round(hsl.L * 2.55);
						break;

					case 'S':
						x = (int)Math.Ceiling(hsl.H * 17.0 / 24.0);
						y = (int)(      255 - hsl.L * 2.55);
						break;

					case 'L':
						x =       (int)Math.Ceiling(hsl.H * 17.0 / 24.0);
						y = 255 - (int)Math.Round(  hsl.S * 2.55);
						break;
				}
			}
			_pt = new Point(x,y);
		}
		#endregion Methods



		#region Designer
		void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ColorField
			// 
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorField";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}


	internal delegate void PointSelectedEvent(ColorEventArgs e);
}
