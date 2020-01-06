using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorRenderingHelper
	static class GradientService
	{
		#region Fields (static)
		static Bitmap _underlay = new Bitmap(256,256);

		internal static Color[] _colors;
		internal static float[] _positions;
		#endregion Fields (static)


		#region Methods (static)
		internal static void InstantiateConstantObjects()
		{
			_colors = new Color[7]
			{
				Color.Red,
				Color.Magenta,
				Color.Blue,
				Color.Cyan,
				Color.Lime,
				Color.Yellow,
				Color.Red
			};

			_positions = new float[7]
			{
				0.0000f,
				0.1667f,
				0.3333f, //0.3372f,
				0.5000f, //0.5020f,
				0.6667f, //0.6686f,
				0.8333f, //0.8313f,
				1.0000f
			};

			CreateUnderlay();
		}

		static void CreateUnderlay()
		{
			using (Graphics graphics = Graphics.FromImage(_underlay))
			{
				Color[] colors;

				var rect = new Rectangle(0,0, 256,1);
				for (int y = 0; y != 256; ++y)
				{
					rect.Y = y;
					using (var brush = new LinearGradientBrush(rect,
															   Color.Transparent,
															   Color.Transparent,
															   LinearGradientMode.Horizontal))
					{
						colors = new Color[7]
						{
							Color.FromArgb(255,   y,   y),
							Color.FromArgb(255, 255,   y),
							Color.FromArgb(  y, 255,   y),
							Color.FromArgb(  y, 255, 255),
							Color.FromArgb(  y,   y, 255),
							Color.FromArgb(255,   y, 255),
							Color.FromArgb(255,   y,   y)
						};

						var blend = new ColorBlend();
						blend.Colors    = colors;
						blend.Positions = _positions;
						brush.InterpolationColors = blend;

						graphics.FillRectangle(brush, rect);
					}
				}
			}
		}

		internal static void DrawField_hue(Graphics graphics, Color slidercolor)
		{
			double r = 255.0;
			double g = 255.0;
			double b = 255.0;

			double rd = (255 - slidercolor.R) / 255.0;
			double gd = (255 - slidercolor.G) / 255.0;
			double bd = (255 - slidercolor.B) / 255.0;

			Rectangle rect; Color color;
			for (int x = 0; x != 256; ++x)
			{
				rect = new Rectangle(x,0, 1,256);
				color = Color.FromArgb((int)Math.Round(r, MidpointRounding.AwayFromZero),
									   (int)Math.Round(g, MidpointRounding.AwayFromZero),
									   (int)Math.Round(b, MidpointRounding.AwayFromZero));

				using (var brush = new LinearGradientBrush(rect,
														   color,
														   Color.Black,
														   LinearGradientMode.Vertical))
				{
					graphics.FillRectangle(brush, rect);
				}
				r -= rd;
				g -= gd;
				b -= bd;
			}
		}

		internal static void DrawField_sat(Graphics graphics, int sat)
		{
			graphics.DrawImage(_underlay, 0,0);

// draw white to transparent gradient overlay ->
			int a = (int)(255 - Math.Round(255 * (sat / 100.0), MidpointRounding.AwayFromZero));
			Color color = Color.FromArgb(a, 255,255,255);

			var rect = new Rectangle(0,0, 256,256);
			using (var brush = new LinearGradientBrush(rect,
													   color,
													   Color.Black,
													   LinearGradientMode.Vertical))
			{
				graphics.FillRectangle(brush, rect);
			}
		}

		internal static void DrawField_lit(Graphics graphics, int lit)
		{
			graphics.DrawImage(_underlay, 0,0);

// draw black to transparent solid overlay ->
			int a = (int)(255 - Math.Round(lit * 2.55, MidpointRounding.AwayFromZero));
			using (var brush = new SolidBrush(Color.FromArgb(a, 0,0,0)))
			{
				graphics.FillRectangle(brush, 0,0, 256,256);
			}
		}

		internal static void DrawField_r(Graphics graphics, int red)
		{
			var rect = new Rectangle(0,0, 256,1);

			int g = 255;
			for (int y = 0; y != 256; ++y)
			{
				rect.Y = y;
				using (var brush = new LinearGradientBrush(rect,
														   Color.FromArgb(red, g,   0),
														   Color.FromArgb(red, g, 255),
														   LinearGradientMode.Horizontal))
				{
					graphics.FillRectangle(brush, rect);
				}
				--g;
			}
		}

		internal static void DrawField_g(Graphics graphics, int green)
		{
			var rect = new Rectangle(0,0, 256,1);

			int r = 255;
			for (int y = 0; y != 256; ++y)
			{
				rect.Y = y;
				using (var brush = new LinearGradientBrush(rect,
														   Color.FromArgb(r, green,   0),
														   Color.FromArgb(r, green, 255),
														   LinearGradientMode.Horizontal))
				{
					graphics.FillRectangle(brush, rect);
				}
				--r;
			}
		}

		internal static void DrawField_b(Graphics graphics, int blue)
		{
			var rect = new Rectangle(0,0, 256,1);

			int g = 255;
			for (int y = 0; y != 256; ++y)
			{
				rect.Y = y;
				using (var brush = new LinearGradientBrush(rect,
														   Color.FromArgb(  0, g, blue),
														   Color.FromArgb(255, g, blue),
														   LinearGradientMode.Horizontal))
				{
					graphics.FillRectangle(brush, rect);
				}
				--g;
			}
		}

		internal static bool IsBright(Color color)
		{
			return color.R > 230
				|| color.G > 223;
//				|| color.B > 253;
		}
		#endregion Methods (static)
	}
}
