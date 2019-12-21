using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorFieldPanel
	sealed class ColorField
		: UserControl
	{
		#region Events
		[EditorBrowsable(EditorBrowsableState.Always)]
		internal event ColorSelectedEventHandler ColorSelected;
		#endregion Events


		#region Fields
		Color _color;
		ColorSpace _colorspace;

		bool _track;
		int _selectedComponentValue;

		Point _pt;
		#endregion Fields


		#region cTor
		public ColorField()
		{
			InitializeComponent();

			_pt = CalculatePoint();
		}
		#endregion cTor


		#region Handlers (override)
//		protected override void OnLoad(EventArgs e)
//		{
//			_pt = CalculatePoint();
//		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (!DesignMode)
			{
				Graphics graphics = e.Graphics;

				UpdateCurrentColor(graphics);

				Pen pen;
				Color color = GetCurrentColor();
				if (color.R + color.G + color.B > 450)
				{
					pen = Pens.Black;
				}
				else
					pen = Pens.White;

				var location = new Point(_pt.X - 4, _pt.Y - 4);
				graphics.DrawEllipse(pen, new Rectangle(location, new Size(8,8)));
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left
				&& (   (e.X > -1 && e.X < 256)
					|| (e.Y > -1 && e.Y < 256)))
			{
				_track = true;

				var point = new Point(e.X, e.Y);
				InvalidateRegions(_pt, point);

				OnColorSelected(new ColorSelectedEventArgs(CalculateSelectedColor(_pt = point)));
			}
			else
				_track = false;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_track = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			const int leeway = 5;

			if (_track
				&& (   (e.X > -1 - leeway && e.X < 256 + leeway)
					|| (e.Y > -1 - leeway && e.Y < 256 + leeway)))
			{
				int x = Math.Max(Math.Min(e.X, 255), 0);
				int y = Math.Max(Math.Min(e.Y, 255), 0);
				var point = new Point(x, y);

				InvalidateRegions(_pt, point);

				OnColorSelected(new ColorSelectedEventArgs(CalculateSelectedColor(_pt)));
				_pt = point;
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
		void OnColorSelected(ColorSelectedEventArgs e)
		{
			if (ColorSelected != null)
				ColorSelected(this, e);
		}
		#endregion Handlers


		#region Methods
		void InvalidateRegions(Point pt0, Point pt1)
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

			if ((_colorspace as HsbColorSpace) != null)
			{
				var hsb = (HSB)((HsbColorSpace)_colorspace).Structure;

				switch (_colorspace.Selected.DisplayCharacter)
				{
					case 'H':
						x = (int)Math.Round((double)hsb.Saturation * 2.55);
						y = 255 - (int)Math.Round((double)hsb.Brightness * 2.55);
						break;

					case 'S':
						x = (int)Math.Ceiling((double)hsb.Hue * (17.0 / 24.0));
						y = (int)(255.0 - (double)hsb.Brightness * 2.55);
						break;

					case 'B':
						x = (int)Math.Ceiling((double)hsb.Hue * (17.0 / 24.0));
						y = 255 - (int)Math.Round((double)hsb.Saturation * 2.55);
						break;
				}
			}
			else if ((_colorspace as RgbColorSpace) != null)
			{
				Color color = ((RgbColorSpace)_colorspace).GetColor();

				switch (_colorspace.Selected.DisplayCharacter)
				{
					case 'R':
						x = color.B;
						y = 255 - color.G;
						break;

					case 'G':
						x = color.B;
						y = 255 - color.R;
						break;

					case 'B':
						x = color.R;
						y = 255 - color.G;
						break;
				}
			}
			return new Point(x,y);
		}

		void UpdateCurrentColor(Graphics graphics)
		{
			if (_colorspace is HsbColorSpace)
			{
				switch (_colorspace.Selected.DisplayCharacter)
				{
					case 'H':
					{
						Color color = _color;

						if (color.Equals(Color.FromArgb(0,0,0,0)) || color.Equals(Color.FromArgb(0,0,0)))
							color = Color.FromArgb(255,0,0);

						GradientService.DrawFieldHue(graphics, color);
						break;
					}

					case 'S':
						GradientService.DrawFieldSaturation(graphics, _selectedComponentValue);
						break;

					case 'B':
						GradientService.DrawFieldBrightness(graphics, _selectedComponentValue);
						break;
				}
			}
			else if (_colorspace is RgbColorSpace)
			{
				switch (_colorspace.Selected.DisplayCharacter)
				{
					case 'R':
						GradientService.DrawFieldRed(graphics, _selectedComponentValue);
						break;

					case 'G':
						GradientService.DrawFieldGreen(graphics, _selectedComponentValue);
						break;

					case 'B':
						GradientService.DrawFieldBlue(graphics, _selectedComponentValue);
						break;
				}
			}
		}

		internal void UpdateColor(Color color, ColorSpace colorspace, bool updatePoint)
		{
			_color = color;
			UpdateColor(colorspace, updatePoint);
		}

		internal void UpdateColor(int val, ColorSpace colorspace, bool updatePoint)
		{
			_selectedComponentValue = val;
			_color = Color.Empty;
			UpdateColor(colorspace, updatePoint);
		}

		void UpdateColor(ColorSpace colorspace, bool updatePoint)
		{
			_colorspace = colorspace;

			if (updatePoint)
				_pt = CalculatePoint();

			Invalidate();
		}

		Color CalculateSelectedColor(Point pt)
		{
			object o = null;

			if ((_colorspace as HsbColorSpace) != null)
			{
				var hsb = (HSB)((HsbColorSpace)_colorspace).Structure;

				switch (_colorspace.Selected.DisplayCharacter)
				{
					case 'H':
					{
						int brightness = (int)((255.0 - (double)pt.Y) / 2.55);
						int saturation = (int)((double)pt.X / 2.55);
						o = new HSB(hsb.Hue, saturation, brightness);
						break;
					}

					case 'S':
					{
						int val = (int)((double)pt.X * 1.411764705882353);
						int brightness = (int)((255.0 - (double)pt.Y) / 2.55);
						if (val == 360) val = 0;
						o = new HSB(val, hsb.Saturation, brightness);
						break;
					}

					case 'B':
					{
						int val = (int)((double)pt.X * 1.411764705882353);
						int saturation = (int)((255.0 - (double)pt.Y) / 2.55);
						if (val == 360) val = 0;
						o = new HSB(val, saturation, hsb.Brightness);
						break;
					}
				}
			}
			else if ((_colorspace as RgbColorSpace) != null)
			{
				var rgb = (RGB)((RgbColorSpace)_colorspace).Structure;

				switch (_colorspace.Selected.DisplayCharacter)
				{
					case 'R':
						o = new RGB(rgb.Red, 255 - pt.Y, pt.X);
						break;

					case 'G':
						o = new RGB(255 - pt.Y, rgb.Green, pt.X);
						break;

					case 'B':
						o = new RGB(pt.X, 255 - pt.Y, rgb.Blue);
						break;
				}
			}

			if ((o as HSB) != null)
				return ColorConverter.HsbToColor(o as HSB);

			return ColorConverter.RgbToColor(o as RGB);
		}

		internal Color GetCurrentColor()
		{
			if (_pt != Point.Empty)
				return CalculateSelectedColor(_pt);

			return Color.White;
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



	internal delegate void ColorSelectedEventHandler(object sender, ColorSelectedEventArgs e);
}
