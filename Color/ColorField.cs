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
		internal enum DirPoint // left,right,up,down, etc
		{
			nul, l,r,u,d, lu,ld,ru,rd
		}


		#region Events
		public event PointSelectedEvent PointSelected;
		#endregion Events


		#region Fields
		ColorSpaceControl _csc;

		Color _slidecol;
		int   _ciscoval;

		Color _col;
		RGB   _rgb = new RGB();
		HSL   _hsl = new HSL();
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
				if (_rgb.IsBright()) pen = Pens.Black;
				else                 pen = Pens.White;

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
			ColorF.That.ColorControl.GetActiveColorbox().Select(); // take focus off tb if applicable

			if (e.Button == MouseButtons.Left)
			{
				_track = true;
				ChangePoint(new Point(e.X, e.Y));
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_track = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (_track)
			{
				const int leeway = 5;
				if (   (e.X > -1 - leeway && e.X < 256 + leeway)
					|| (e.Y > -1 - leeway && e.Y < 256 + leeway))
				{
					var pt = new Point(Math.Max(0, Math.Min(e.X, 255)),
									   Math.Max(0, Math.Min(e.Y, 255)));
					ChangePoint(pt);
				}
			}
		}

		internal void ChangePoint_key(DirPoint dir)
		{
			int x = _pt.X;
			int y = _pt.Y;

			switch (dir)
			{
				case DirPoint.l: if (x !=   0) --x; break;
				case DirPoint.r: if (x != 255) ++x; break;
				case DirPoint.u: if (y !=   0) --y; break;
				case DirPoint.d: if (y != 255) ++y; break;

				case DirPoint.lu: if (x !=   0 && y !=   0) { --x; --y; } break;
				case DirPoint.ld: if (x !=   0 && y != 255) { --x; ++y; } break;
				case DirPoint.ru: if (x != 255 && y !=   0) { ++x; --y; } break;
				case DirPoint.rd: if (x != 255 && y != 255) { ++x; ++y; } break;
			}

			if (x != _pt.X || y != _pt.Y)
				ChangePoint(new Point(x,y));
		}

		void ChangePoint(Point pt)
		{
			Invalidate(new Rectangle(_pt.X - 4, _pt.Y - 4, 9,9));
			Invalidate(new Rectangle( pt.X - 4,  pt.Y - 4, 9,9));

			_pt = pt;

			SelectColor();
			Update();

			if (PointSelected != null)
				PointSelected(new ColorEventArgs(_col, _rgb, _hsl)); // ColorControl.pointselected()
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
				SelectColor();
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
						x =       (int)Math.Round(hsl.S * 2.55, MidpointRounding.AwayFromZero);
						y = 255 - (int)Math.Round(hsl.L * 2.55, MidpointRounding.AwayFromZero);
						break;

					case 'S':
						x =       (int)Math.Round(hsl.H * 17.0 / 24.0, MidpointRounding.AwayFromZero);
						y = 255 - (int)Math.Round(hsl.L * 2.55,        MidpointRounding.AwayFromZero);
						break;

					case 'L':
						x =       (int)Math.Round(hsl.H * 17.0 / 24.0, MidpointRounding.AwayFromZero);
						y = 255 - (int)Math.Round(hsl.S * 2.55,        MidpointRounding.AwayFromZero);
						break;
				}
			}
			_pt = new Point(x,y);
		}

		void SelectColor()
		{
			var csc = _csc as ColorSpaceControlRGB;
			if (csc != null)
			{
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'R': _rgb.R = csc.rgb.R;   _rgb.G = 255 - _pt.Y; _rgb.B = _pt.X;     break;
					case 'G': _rgb.R = 255 - _pt.Y; _rgb.G = csc.rgb.G;   _rgb.B = _pt.X;     break;
					case 'B': _rgb.R =       _pt.X; _rgb.G = 255 - _pt.Y; _rgb.B = csc.rgb.B; break;
				}
				_hsl = ColorConverter.RgbToHsl(_rgb);
				_col = ColorConverter.RgbToCol(_rgb);
			}
			else
			{
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'H':
					{
						int lit = (int)Math.Round((255 - _pt.Y) / 2.55, MidpointRounding.AwayFromZero);
						int sat = (int)Math.Round(       _pt.X  / 2.55, MidpointRounding.AwayFromZero);

						_hsl.H = (_csc as ColorSpaceControlHSL).hsl.H;
						_hsl.L = Math.Max(0, Math.Min(lit, 100));
						_hsl.S = Math.Max(0, Math.Min(sat, 100));
						break;
					}

					case 'S':
					{
						int hue = (int)Math.Round(_pt.X * 24.0 / 17.0,  MidpointRounding.AwayFromZero);
						int lit = (int)Math.Round((255 - _pt.Y) / 2.55, MidpointRounding.AwayFromZero);

						_hsl.H = Math.Max(0, Math.Min(hue, 359));
						_hsl.S = (_csc as ColorSpaceControlHSL).hsl.S;
						_hsl.L = Math.Max(0, Math.Min(lit, 100));
						break;
					}

					case 'L':
					{
						int hue = (int)Math.Round(_pt.X * 24.0 / 17.0,  MidpointRounding.AwayFromZero);
						int sat = (int)Math.Round((255 - _pt.Y) / 2.55, MidpointRounding.AwayFromZero);

						_hsl.H = Math.Max(0, Math.Min(hue, 359));
						_hsl.S = Math.Max(0, Math.Min(sat, 100));
						_hsl.L = (_csc as ColorSpaceControlHSL).hsl.L;
						break;
					}
				}
				_rgb = ColorConverter.HslToRgb(_hsl);
				_col = ColorConverter.RgbToCol(_rgb);
			}
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
