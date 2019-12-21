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
		public event ValueChangedEventHandler ValueChanged;
		#endregion Events


		#region Fields (static)
		internal const int width  =  18;
		internal const int height = 256;
		#endregion Fields (static)


		#region Fields
		bool _lb;
		bool _track;

		int _y;

		Rectangle _l;
		Rectangle _r;

		readonly Rectangle _rectOuter = new Rectangle(10,4, 26,264);
		readonly Rectangle _rectInner = new Rectangle(13,7, width, height);

		ColorSpace _colorspace;
		#endregion Fields


		#region Properties
		public int Value
		{
			get { return 255 - _y + _rectInner.Y; }
			set
			{
				_y = _rectInner.Y + (255 - value);
				InvalidateTris(_y);
			}
		}
		#endregion Properties


		#region cTor
		public ColorSlider()
		{
			InitializeComponent();

			_y = _rectInner.Top;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics graphics = e.Graphics;

			DrawTriL(graphics, _y);
			DrawTriR(graphics, _y);

			DrawGradient(graphics);

			graphics.DrawRectangle(Pens.Black,
								   _rectInner.X     - 1, _rectInner.Y      - 1,
								   _rectInner.Width + 1, _rectInner.Height + 1);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_lb = true;
				ChangeValue(e.Y);
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_lb = _track = false;

			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (_lb)
			{
				_track = true;
				ChangeValue(e.Y);
			}

			base.OnMouseMove(e);
		}
		#endregion Handlers (override)


		#region Methods
		public void ChangeColorspace(ColorSpace colorspace)
		{
			_colorspace = colorspace;
			Invalidate(_rectInner);
		}

		void ChangeValue(int y)
		{
			if (_lb && !_track)
			{
				if (y < _rectInner.Top || y > _rectInner.Bottom)
					return;
			}
			else
			{
				if (!_track)
					return;

				if      (y <  _rectInner.Top)    y = _rectInner.Top;
				else if (y >= _rectInner.Bottom) y = _rectInner.Bottom - 1;
			}

			InvalidateTris(_y = y);

			if (ValueChanged != null)
				ValueChanged(this, new ValueChangedEventArgs(255 - (_y - _rectInner.Y)));
		}

		void InvalidateTris(int y)
		{
			Invalidate(_l);
			Invalidate(_r);

			_l = GetRegionTriL(y);
			_r = GetRegionTriR(y);

			Invalidate(_l);
			Invalidate(_r);

			Update(); // quick refresh. Just say no to sticky tris.
		}

		Rectangle GetRegionTriL(int y)
		{
			int x = _rectOuter.Left - 8;
			y -= 6;
			return new Rectangle(x,y, 8,13);
		}

		Rectangle GetRegionTriR(int y)
		{
			int x = _rectOuter.Right - 1;
			y -= 6;
			return new Rectangle(x,y, 8,13);
		}

		void DrawTriL(Graphics graphics, int y)
		{
			var tri = new Point[3]
			{
				new Point(_rectOuter.Left - 7, y - 5),
				new Point(_rectOuter.Left - 2, y),
				new Point(_rectOuter.Left - 7, y + 5)
			};
			graphics.DrawPolygon(Pens.Black, tri);
		}

		void DrawTriR(Graphics graphics, int y)
		{
			var tri = new Point[3]
			{
				new Point(_rectOuter.Right + 4, y - 5),
				new Point(_rectOuter.Right - 1, y),
				new Point(_rectOuter.Right + 4, y + 5)
			};
			graphics.DrawPolygon(Pens.Black, tri);
		}

		void DrawGradient(Graphics graphics)
		{
			if (_colorspace != null)
			{
				if ((_colorspace as HsbColorSpace) != null)
				{
					var hsb = (_colorspace as HsbColorSpace).Structure as HSB;

					switch (_colorspace.Selected.DisplayCharacter)
					{
						case 'H':
							using (var linearGradientBrush = new LinearGradientBrush(_rectInner,
																					 Color.Blue,
																					 Color.Red,
																					 90f,
																					 false))
							{
								var colorBlend = new ColorBlend();
								colorBlend.Colors    = GradientService._colors;
								colorBlend.Positions = GradientService._positions;
								linearGradientBrush.InterpolationColors = colorBlend;

								graphics.FillRectangle(linearGradientBrush, _rectInner);
							}
							break;

						case 'S':
						{
							RGB rgb1 = ColorConverter.HsbToRgb(new HSB(hsb.Hue, 100, hsb.Brightness));
							RGB rgb2 = ColorConverter.HsbToRgb(new HSB(hsb.Hue,   0, hsb.Brightness));
							Color color1 = Color.FromArgb(rgb1.Red, rgb1.Green, rgb1.Blue);
							Color color2 = Color.FromArgb(rgb2.Red, rgb2.Green, rgb2.Blue);

							using (var brush = new LinearGradientBrush(_rectInner, color1, color2, 90f))
							{
								graphics.FillRectangle(brush, _rectInner);
							}
							break;
						}

						case 'B':
						{
							RGB rgb1 = ColorConverter.HsbToRgb(new HSB(hsb.Hue, hsb.Saturation, 100));
							RGB rgb2 = ColorConverter.HsbToRgb(new HSB(hsb.Hue, hsb.Saturation,   0));
							Color color1 = Color.FromArgb(rgb1.Red, rgb1.Green, rgb1.Blue);
							Color color2 = Color.FromArgb(rgb2.Red, rgb2.Green, rgb2.Blue);

							using (var brush = new LinearGradientBrush(_rectInner, color1, color2, 90f))
							{
								graphics.FillRectangle(brush, _rectInner);
							}
							break;
						}
					}
				}
				else // RgbColorSpace
				{
					var rgb = (_colorspace as RgbColorSpace).Structure as RGB;

					Color color1, color2;
					switch (_colorspace.Selected.DisplayCharacter)
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

					using (var brush = new LinearGradientBrush(_rectInner, color1, color2, 270f))
						graphics.FillRectangle(brush, _rectInner);
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
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSlider";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}



	sealed class ValueChangedEventArgs
		: EventArgs
	{
		readonly int m_value;

		internal int Value
		{
			get { return m_value; }
		}

		internal ValueChangedEventArgs(int newValue)
		{
			m_value = newValue;
		}
	}


	internal delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);
}
