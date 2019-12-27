using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSlider
	sealed class ColorSlider
		: UserControl
	{
		#region Events
		public event SliderChangedEventHandler SliderChanged;
		#endregion Events


		#region Fields (static)
		internal const int width  =  18; // w of the gradient rectangle
		internal const int height = 256; // h of the gradient rectangle
		#endregion Fields (static)


		#region Fields
		ColorSpaceControl _csc;

		readonly Rectangle _rect;

		Rectangle _l; // for invalidating/redrawing the left  tri
		Rectangle _r; // for invalidating/redrawing the right tri

		bool _track;
		#endregion Fields


		#region Properties
		int _y;
		public int Value
		{
			get
			{
				return 255 - _y + _rect.Y;
			}
			set
			{
				_y = _rect.Y + 255 - value;
				InvalidateTris(_y);
			}
		}
		#endregion Properties


		#region cTor
		public ColorSlider()
		{
			InitializeComponent();
			_rect = new Rectangle((Width  - width)  / 2,
								  (Height - height) / 2,
								   width,
								   height);
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics graphics = e.Graphics;

			DrawTris(graphics, _y);
			DrawGradient(graphics);

			graphics.DrawRectangle(Pens.Black,
								   _rect.X     - 1, _rect.Y      - 1,
								   _rect.Width + 1, _rect.Height + 1);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_track = true;
				ChangeValue(e.Y);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_track = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (_track) ChangeValue(e.Y);
		}
		#endregion Handlers (override)


		#region Methods
		public void ChangeColorspace(ColorSpaceControl colorspace)
		{
			_csc = colorspace;
			Invalidate(_rect);
		}

		void ChangeValue(int y)
		{
			if (y < _rect.Y)
				y = _rect.Y;
			else if (y >= _rect.Y + _rect.Height)
					 y  = _rect.Y + _rect.Height - 1;

			InvalidateTris(_y = y);

			if (SliderChanged != null)
				SliderChanged(new SliderChangedEventArgs(255 - _y + _rect.Y));
		}

		void InvalidateTris(int y)
		{
			Invalidate(_l);
			Invalidate(_r);

			_l = GetRectangleTriL(y);
			_r = GetRectangleTriR(y);

			Invalidate(_l);
			Invalidate(_r);

			Update(); // quick refresh. Just say no to sticky tris.
		}

		Rectangle GetRectangleTriL(int y)
		{
			int x = _rect.X - 9;
			y -= 5;
			return new Rectangle(x,y, 6,11);
		}

		Rectangle GetRectangleTriR(int y)
		{
			int x = _rect.X + _rect.Width + 3;
			y -= 5;
			return new Rectangle(x,y, 6,11);
		}

		void DrawTris(Graphics graphics, int y)
		{
			Point[] tri;

			tri = new Point[3]
			{
				new Point(_rect.X - 9, y - 5),
				new Point(_rect.X - 4, y),
				new Point(_rect.X - 9, y + 5)
			};
			graphics.DrawPolygon(Pens.Black, tri);

			tri = new Point[3]
			{
				new Point(_rect.X + _rect.Width + 8, y - 5),
				new Point(_rect.X + _rect.Width + 3, y),
				new Point(_rect.X + _rect.Width + 8, y + 5)
			};
			graphics.DrawPolygon(Pens.Black, tri);
		}

		void DrawGradient(Graphics graphics)
		{
			if (_csc != null)
			{
				if ((_csc as ColorSpaceHsb) != null)
				{
					var hsb = (_csc as ColorSpaceHsb).Structure as HSB;

					switch (_csc.Co.DisplayCharacter)
					{
						case 'H':
							using (var linearGradientBrush = new LinearGradientBrush(_rect,
																					 Color.Blue,
																					 Color.Red,
																					 90f,
																					 false))
							{
								var blend = new ColorBlend();
								blend.Colors    = GradientService._colors;
								blend.Positions = GradientService._positions;
								linearGradientBrush.InterpolationColors = blend;

								graphics.FillRectangle(linearGradientBrush, _rect);
							}
							break;

						case 'S':
						{
							RGB rgb1 = ColorConverter.HsbToRgb(new HSB(hsb.Hue, 100, hsb.Brightness));
							RGB rgb2 = ColorConverter.HsbToRgb(new HSB(hsb.Hue,   0, hsb.Brightness));
							Color color1 = Color.FromArgb(rgb1.Red, rgb1.Green, rgb1.Blue);
							Color color2 = Color.FromArgb(rgb2.Red, rgb2.Green, rgb2.Blue);

							using (var brush = new LinearGradientBrush(_rect, color1, color2, 90f))
								graphics.FillRectangle(brush, _rect);
							break;
						}

						case 'B':
						{
							RGB rgb1 = ColorConverter.HsbToRgb(new HSB(hsb.Hue, hsb.Saturation, 100));
							RGB rgb2 = ColorConverter.HsbToRgb(new HSB(hsb.Hue, hsb.Saturation,   0));
							Color color1 = Color.FromArgb(rgb1.Red, rgb1.Green, rgb1.Blue);
							Color color2 = Color.FromArgb(rgb2.Red, rgb2.Green, rgb2.Blue);

							using (var brush = new LinearGradientBrush(_rect, color1, color2, 90f))
								graphics.FillRectangle(brush, _rect);
							break;
						}
					}
				}
				else // ColorSpaceRgb
				{
					var rgb = (_csc as ColorSpaceRgb).Structure as RGB;

					Color color1, color2;
					switch (_csc.Co.DisplayCharacter)
					{
						case 'R':
							color1 = Color.FromArgb(  0, rgb.Green, rgb.Blue);
							color2 = Color.FromArgb(255, rgb.Green, rgb.Blue);
							break;

						case 'G':
							color1 = Color.FromArgb(rgb.Red,   0, rgb.Blue);
							color2 = Color.FromArgb(rgb.Red, 255, rgb.Blue);
							break;

						default: // case 'B':
							color1 = Color.FromArgb(rgb.Red, rgb.Green,   0);
							color2 = Color.FromArgb(rgb.Red, rgb.Green, 255);
							break;
					}

					using (var brush = new LinearGradientBrush(_rect, color1, color2, 270f))
						graphics.FillRectangle(brush, _rect);
				}
			}
		}
		#endregion Methods



		#region Designer
		void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ColorSlider
			// 
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSlider";
			this.Size = new System.Drawing.Size(36, 267);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}



	sealed class SliderChangedEventArgs
		: EventArgs
	{
		internal int Value
		{ get; private set; }

		internal SliderChangedEventArgs(int val)
		{
			Value = val;
		}
	}


	internal delegate void SliderChangedEventHandler(SliderChangedEventArgs e);
}
