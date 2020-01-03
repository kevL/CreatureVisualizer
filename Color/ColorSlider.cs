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
		public event SliderChangedEvent SliderChanged;
		#endregion Events


		#region Fields (static)
		internal const int width  =  18; // w of the gradient rectangle
		internal const int height = 256; // h of the gradient rectangle
		#endregion Fields (static)


		#region Fields
		ColorSpaceControl _csc;

		readonly Rectangle _grad;

		Rectangle _l; // for invalidating/redrawing the left  tri
		Rectangle _r; // for invalidating/redrawing the right tri

		bool _track;
		#endregion Fields


		#region Properties
		int _val;
		/// <summary>
		/// The colorslider's value always ranges [0..255] regardless of which
		/// cisco is currently selected.
		/// </summary>
		internal int Val
		{
			get { return _val; }
			set
			{
				_val = value;
				UpdateTris();
			}
		}
		#endregion Properties


		#region cTor
		public ColorSlider()
		{
			InitializeComponent();
			_grad = new Rectangle((Width  - width)  / 2,
								  (Height - height) / 2,
								   width,
								   height);
		}
		#endregion cTor


		#region Handlers (override)
		Graphics _graphics;
		protected override void OnPaint(PaintEventArgs e)
		{
//			base.OnPaint(e);

			_graphics = e.Graphics;

			DrawTris();
			_graphics.DrawRectangle(Pens.Black,
								   _grad.X     - 1, _grad.Y      - 1,
								   _grad.Width + 1, _grad.Height + 1);
			DrawGradient();
		}

		void DrawTris()
		{
			int y = _grad.Y + 255 - Val;

			Point[] tri;

			tri = new Point[3]
			{
				new Point(_grad.X - 9, y - 5),
				new Point(_grad.X - 4, y),
				new Point(_grad.X - 9, y + 5)
			};
			_graphics.DrawPolygon(Pens.Black, tri);

			tri = new Point[3]
			{
				new Point(_grad.X + _grad.Width + 8, y - 5),
				new Point(_grad.X + _grad.Width + 3, y),
				new Point(_grad.X + _grad.Width + 8, y + 5)
			};
			_graphics.DrawPolygon(Pens.Black, tri);
		}

		void DrawGradient()
		{
			_graphics.PixelOffsetMode = PixelOffsetMode.Half;

			var csc = _csc as ColorSpaceControlRGB;
			if (csc != null)
			{
				var rgb = csc.rgb;
				Color color1, color2;
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'R':
						color1 = Color.FromArgb(  0, rgb.G, rgb.B);
						color2 = Color.FromArgb(255, rgb.G, rgb.B);
						break;

					case 'G':
						color1 = Color.FromArgb(rgb.R,   0, rgb.B);
						color2 = Color.FromArgb(rgb.R, 255, rgb.B);
						break;

					default:
//					case 'B':
						color1 = Color.FromArgb(rgb.R, rgb.G,   0);
						color2 = Color.FromArgb(rgb.R, rgb.G, 255);
						break;
				}

				using (var brush = new LinearGradientBrush(_grad, color1, color2, 270f))
					_graphics.FillRectangle(brush, _grad);
			}
			else
			{
				var hsl = (_csc as ColorSpaceControlHSL).hsl;
				switch (_csc.Cisco.DisplayCharacter)
				{
					case 'H':
						using (var linearGradientBrush = new LinearGradientBrush(_grad,
																				 Color.Blue,
																				 Color.Red,
																				 90f,
																				 false))
						{
							var blend = new ColorBlend();
							blend.Colors    = GradientService._colors;
							blend.Positions = GradientService._positions;
							linearGradientBrush.InterpolationColors = blend;

							_graphics.FillRectangle(linearGradientBrush, _grad);
						}
						break;

					case 'S':
					{
						RGB rgb1 = ColorConverter.HslToRgb(new HSL(hsl.H, 100, hsl.L));
						RGB rgb2 = ColorConverter.HslToRgb(new HSL(hsl.H,   0, hsl.L));
						Color color1 = Color.FromArgb(rgb1.R, rgb1.G, rgb1.B);
						Color color2 = Color.FromArgb(rgb2.R, rgb2.G, rgb2.B);

						using (var brush = new LinearGradientBrush(_grad, color1, color2, 90f))
							_graphics.FillRectangle(brush, _grad);
						break;
					}

					case 'L':
					{
						RGB rgb1 = ColorConverter.HslToRgb(new HSL(hsl.H, hsl.S, 100));
						RGB rgb2 = ColorConverter.HslToRgb(new HSL(hsl.H, hsl.S,   0));
						Color color1 = Color.FromArgb(rgb1.R, rgb1.G, rgb1.B);
						Color color2 = Color.FromArgb(rgb2.R, rgb2.G, rgb2.B);

						using (var brush = new LinearGradientBrush(_grad, color1, color2, 90f))
							_graphics.FillRectangle(brush, _grad);
						break;
					}
				}
			}
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

		void ChangeValue(int y)
		{
			y = Math.Max(_grad.Y, Math.Min(y, _grad.Y + 255));

			Val = 255 - y + _grad.Y;

			if (SliderChanged != null)
				SliderChanged(new SliderChangedEventArgs(Val));
		}

		void UpdateTris()
		{
			Invalidate(_l);
			Invalidate(_r);

			SetTriRects();

			Invalidate(_l);
			Invalidate(_r);

			Update(); // quick refresh. Just say no to sticky tris.
		}

		void SetTriRects()
		{
			int y = _grad.Y + 255 - _val - 5;

			int x = _grad.X - 9;
			_l = new Rectangle(x,y, 6,11);

			x = _grad.X + _grad.Width + 3;
			_r = new Rectangle(x,y, 6,11);
		}
		#endregion Handlers (override)


		#region Methods
		internal void Configurate(ColorSpaceControl csc)
		{
			_csc = csc;
			Invalidate(_grad);

			int val;
			switch (_csc.Cisco.Units)
			{
				case ColorSpaceControlCisco.Unit.Degree:
					val = (int)Math.Ceiling(_csc.Cisco.Val * 17.0 / 24.0);
					val = Math.Max(0, Math.Min(val, 255));
					break;

				case ColorSpaceControlCisco.Unit.Percent:
					val = (int)Math.Ceiling(_csc.Cisco.Val * 2.55);
					val = Math.Max(0, Math.Min(val, 255));
					break;

				default:
//				case ColorSpaceControlCisco.Unit.Byte:
					val = _csc.Cisco.Val;
					break;
			}
			Val = val;
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
		internal int Val
		{ get; private set; }

		internal SliderChangedEventArgs(int val)
		{
			Val = val;
		}
	}


	internal delegate void SliderChangedEvent(SliderChangedEventArgs e);
}
