using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorRenderingHelper
	static class GradientService
	{
		#region Fields (static)
		static Bitmap _gradient;

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
		}


		internal static void DrawFieldRed(Graphics graphics, int red)
		{
			Rectangle rect;

			int g = 255;
			for (int y = 0; y != 256; ++y)
			{
				rect = new Rectangle(0,y, 256,1);
				using (var brush = new LinearGradientBrush(rect,
														   Color.FromArgb(red, g,   0),
														   Color.FromArgb(red, g, 255),
														   0f,
														   false))
				{
					graphics.FillRectangle(brush, rect);
				}
				--g;
			}
		}

		internal static void DrawFieldGreen(Graphics graphics, int green)
		{
			Rectangle rect;

			int r = 255;
			for (int y = 0; y != 256; ++y)
			{
				rect = new Rectangle(0,y, 256,1);
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

		internal static void DrawFieldBlue(Graphics graphics, int blue)
		{
			Rectangle rect;

			int g = 255;
			for (int y = 0; y != 256; ++y)
			{
				rect = new Rectangle(0,y, 256,1);
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


		internal static void DrawFieldHue(Graphics graphics, Color color)
		{
			double d1 = (double)(255 - color.R) / 255.0;
			double d2 = (double)(255 - color.G) / 255.0;
			double d3 = (double)(255 - color.B) / 255.0;
			double d4 = 255.0;
			double d5 = 255.0;
			double d6 = 255.0;

			Rectangle rect;
			for (int x = 0; x != 256; ++x)
			{
				rect = new Rectangle(x,0, 1,256);

				using (var brush = new LinearGradientBrush(rect,
														   Color.FromArgb((int)Math.Round(d4),
																		  (int)Math.Round(d5),
																		  (int)Math.Round(d6)),
														   Color.FromArgb(0,0,0),
														   90f,
														   false))
				{
					graphics.FillRectangle(brush, rect);
				}

				d4 -= d1;
				d5 -= d2;
				d6 -= d3;
			}
		}

		internal static void DrawFieldSaturation(Graphics graphics, int saturation)
		{
			var rect = new Rectangle(0,0, 256,256);
			int alpha = (int)(255.0 - Math.Round(255.0 * ((double)saturation / 100.0)));

			if (_gradient == null)
				_gradient = DrawGradient();

			graphics.DrawImage(_gradient, 0,0);

			Color color = Color.FromArgb(alpha, 255,255,255);
			using (var brush = new LinearGradientBrush(rect, color, Color.Black, 90f))
			{
				graphics.FillRectangle(brush, rect);
			}
		}

		internal static void DrawFieldBrightness(Graphics graphics, int brightness)
		{
			int alpha = (int)(255.0 - Math.Round((double)brightness * 2.55));

			if (_gradient == null)
				_gradient = DrawGradient();

			graphics.DrawImage(_gradient, 0,0);

			using (var brush = new SolidBrush(Color.FromArgb(alpha, 0,0,0)))
			{
				graphics.FillRectangle(brush, 0,0, 256,256);
			}
		}

		static Bitmap DrawGradient()
		{
			var b = new Bitmap(256,256);
			using (Graphics graphics = Graphics.FromImage(b))
			{
				for (int y = 0; y != 256; ++y)
				{
					var rect = new Rectangle(0,y, 256,1);
					using (var brush = new LinearGradientBrush(rect,
															   Color.Blue,
															   Color.Red,
															   0f,
															   false))
					{
						var colors = new Color[7]
						{
							Color.FromArgb(255,   y,   y),
							Color.FromArgb(255, 255,   y),
							Color.FromArgb(  y, 255,   y),
							Color.FromArgb(  y, 255, 255),
							Color.FromArgb(  y,   y, 255),
							Color.FromArgb(255,   y, 255),
							Color.FromArgb(255,   y,   y)
						};

						var colorBlend = new ColorBlend();
						colorBlend.Colors    = colors;
						colorBlend.Positions = _positions;
						brush.InterpolationColors = colorBlend;

						graphics.FillRectangle(brush, rect);
					}
				}
				return b;
			}
		}


		internal static void DrawSlider(Graphics graphics, Rectangle rect)
		{
			using (var brush = new LinearGradientBrush(rect,
													   Color.Blue,
													   Color.Red,
													   90f,
													   false))
			{
				var colorBlend = new ColorBlend();
				colorBlend.Colors    = _colors;
				colorBlend.Positions = _positions;
				brush.InterpolationColors = colorBlend;

				graphics.FillRectangle(brush, rect);
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
