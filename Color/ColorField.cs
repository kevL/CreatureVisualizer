﻿using System;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorFieldPanel
	sealed class ColorField
		: UserControl
	{
		#region Events
		public event ColorSelectedEventHandler ColorSelected;
		#endregion Events


		#region Fields
		ColorSpaceControl _csc;
		Color _color;
		int _val;

		Point _pt;
		bool _track;
		#endregion Fields


		#region cTor
		public ColorField()
		{
			InitializeComponent();

			_pt = CalculatePoint();
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!DesignMode)
			{
				DrawField(e.Graphics);

				Pen pen;
				if (GradientService.IsBright(GetCurrentColor())) pen = Pens.Black;
				else                                             pen = Pens.White;

				e.Graphics.DrawEllipse(pen,
									   _pt.X - 4,
									   _pt.Y - 4,
									   8,8);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_track = true;

				var point = new Point(e.X, e.Y);
				InvalidatePoints(_pt, point);

				_pt = point;
				SelectColor(new ColorEventArgs(GetCurrentColor()));
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
					var point = new Point(Math.Max(0, Math.Min(e.X, 255)),
										  Math.Max(0, Math.Min(e.Y, 255)));

					InvalidatePoints(_pt, point);

					_pt = point;
					SelectColor(new ColorEventArgs(GetCurrentColor()));
//					SelectColor(new ColorEventArgs(color));
				}
			}
		}

//		protected override void OnMouseEnter(EventArgs e)
//		{
//			Cursor = new Cursor(SanoResources.GetFileResource("ColorFieldPanelCursor.cur")); // TODO ->>
//		}

//		protected override void OnMouseLeave(EventArgs e)
//		{
//			Cursor.Show();
//			ParentForm.Cursor = Cursors.Default;
//		}
		#endregion Handlers (override)


		#region Handlers
		void SelectColor(ColorEventArgs e)
		{
			if (ColorSelected != null)
				ColorSelected(e);
		}
		#endregion Handlers


		#region Methods
		void DrawField(Graphics graphics)
		{
			if (_csc is ColorSpaceHsb)
			{
				switch (_csc.Co.DisplayCharacter)
				{
					case 'H':
					{
						Color color = _color;

						if (color.Equals(Color.FromArgb(0,0,0,0)) || color.Equals(Color.FromArgb(0,0,0))) // TODO: wtf
							color = Color.FromArgb(255,0,0);

						GradientService.DrawFieldHue(graphics, color);
						break;
					}

					case 'S':
						GradientService.DrawFieldSaturation(graphics, _val);
						break;

					case 'B':
						GradientService.DrawFieldBrightness(graphics, _val);
						break;
				}
			}
			else if (_csc is ColorSpaceRgb)
			{
				switch (_csc.Co.DisplayCharacter)
				{
					case 'R':
						GradientService.DrawFieldRed(graphics, _val);
						break;

					case 'G':
						GradientService.DrawFieldGreen(graphics, _val);
						break;

					case 'B':
						GradientService.DrawFieldBlue(graphics, _val);
						break;
				}
			}
		}

		void InvalidatePoints(Point pt0, Point pt1)
		{
			Rectangle rect;

			rect = new Rectangle(pt0.X - 4, pt0.Y - 4, 9,9);
			Invalidate(rect);

			rect = new Rectangle(pt1.X - 4, pt1.Y - 4, 9,9);
			Invalidate(rect);
		}

		Point CalculatePoint()
		{
			int x = 0;
			int y = 0;

			if ((_csc as ColorSpaceHsb) != null)
			{
				var hsb = (HSB)((ColorSpaceHsb)_csc).Structure;

				switch (_csc.Co.DisplayCharacter)
				{
					case 'H':
						x =       (int)Math.Round((double)hsb.Saturation * 2.55);
						y = 255 - (int)Math.Round((double)hsb.Brightness * 2.55);
						break;

					case 'S':
						x = (int)Math.Ceiling((double)hsb.Hue        * 17.0 / 24.0);
						y = (int)(    255.0 - (double)hsb.Brightness * 2.55);
						break;

					case 'B':
						x =       (int)Math.Ceiling((double)hsb.Hue        * 17.0 / 24.0);
						y = 255 - (int)Math.Round(  (double)hsb.Saturation * 2.55);
						break;
				}
			}
			else if ((_csc as ColorSpaceRgb) != null)
			{
				Color color = ((ColorSpaceRgb)_csc).GetColor();

				switch (_csc.Co.DisplayCharacter)
				{
					case 'R': x = color.B; y = 255 - color.G; break;
					case 'G': x = color.B; y = 255 - color.R; break;
					case 'B': x = color.R; y = 255 - color.G; break;
				}
			}
			return new Point(x,y);
		}

		internal void ChangeColor(Color color, ColorSpaceControl csc, bool updateFieldPoint)
		{
			_color = color;
			ChangeColorspace(csc, updateFieldPoint);
		}

		internal void ChangeColor(int val, ColorSpaceControl csc, bool updateFieldPoint)
		{
			_val = val;
			_color = Color.Empty;
			ChangeColorspace(csc, updateFieldPoint);
		}

		void ChangeColorspace(ColorSpaceControl csc, bool updateFieldPoint)
		{
			_csc = csc;

			if (updateFieldPoint)
				_pt = CalculatePoint();

			Refresh();
		}

		Color GetCurrentColor()
		{
			if ((_csc as ColorSpaceHsb) != null)
			{
				var hsb = (HSB)((ColorSpaceHsb)_csc).Structure;

				switch (_csc.Co.DisplayCharacter)
				{
					case 'H':
					{
						int brightness = (int)((255f - _pt.Y) / 2.55f);
						int saturation = (int)(        _pt.X  / 2.55f);
						hsb = new HSB(hsb.Hue, saturation, brightness);
						break;
					}

					case 'S':
					{
						int hue        = (int)(_pt.X * 24.0f / 17.0f);
						int brightness = (int)((255f - _pt.Y) / 2.55f);
						hsb = new HSB(hue % 360, hsb.Saturation, brightness);
						break;
					}

					case 'B':
					{
						int hue        = (int)(_pt.X * 24.0f / 17.0f);
						int saturation = (int)((255f - _pt.Y) / 2.55f);
						hsb = new HSB(hue % 360, saturation, hsb.Brightness);
						break;
					}
				}
				return ColorConverter.HsbToColor(hsb);
			}
			else //if ((_csc as ColorSpaceRgb) != null)
			{
				var rgb = (RGB)((ColorSpaceRgb)_csc).Structure;

				switch (_csc.Co.DisplayCharacter)
				{
					case 'R': rgb = new RGB(rgb.Red,     255 - _pt.Y, _pt.X);    break;
					case 'G': rgb = new RGB(255 - _pt.Y, rgb.Green,   _pt.X);    break;
					case 'B': rgb = new RGB(_pt.X,       255 - _pt.Y, rgb.Blue); break;
				}
				return ColorConverter.RgbToColor(rgb);
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


	internal delegate void ColorSelectedEventHandler(ColorEventArgs e);
}
