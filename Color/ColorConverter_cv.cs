using System;
using System.Drawing;

//using Sano.PersonalProjects.ColorPicker.Controls;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorConverter
	static class ColorConverter
	{
//		ColorConverter()
//		{}


		#region Methods (static)
		internal static RGB ColorToRgb(Color color)
		{
			return new RGB(color.R, color.G, color.B);
		}

		internal static HSB ColorToHsb(Color color)
		{
			return RgbToHsb(ColorToRgb(color));
		}

		internal static Color RgbToColor(RGB rgb)
		{
			int red = Math.Max(0, Math.Min(rgb.Red, 255));
			int green = Math.Max(0, Math.Min(rgb.Green, 255));
			int blue = Math.Max(0, Math.Min(rgb.Blue, 255));
			return Color.FromArgb(red, green, blue);
		}

		internal static Color RgbToColor(RGB rgb, int iAlpha)
		{
			int red = Math.Max(0, Math.Min(rgb.Red, 255));
			int green = Math.Max(0, Math.Min(rgb.Green, 255));
			int blue = Math.Max(0, Math.Min(rgb.Blue, 255));
			iAlpha = Math.Max(0, Math.Min(iAlpha, 255));
			return Color.FromArgb(iAlpha, red, green, blue);
		}

		internal static Color HsbToColor(HSB hsb)
		{
			return RgbToColor(HsbToRgb(hsb));
		}

		internal static HSB RgbToHsb(RGB rgb)
		{
			double num = (double)rgb.Red / 255.0;
			double num2 = (double)rgb.Green / 255.0;
			double num3 = (double)rgb.Blue / 255.0;
			double minimumValue = GetMinimumValue(num, num2, num3);
			double maximumValue = GetMaximumValue(num, num2, num3);
			double num4 = maximumValue - minimumValue;
			double num5 = 0.0;
			double num6 = 0.0;
			double a = maximumValue * 100.0;

			if (maximumValue == 0.0 || num4 == 0.0)
			{
				num5 = 0.0;
				num6 = 0.0;
			}
			else
			{
				num6 = ((minimumValue != 0.0) ? (num4 / maximumValue * 100.0) : 100.0);
				if (Math.Abs(num - maximumValue) < double.Epsilon)
				{
					num5 = (num2 - num3) / num4;
				}
				else if (Math.Abs(num2 - maximumValue) < double.Epsilon)
				{
					num5 = 2.0 + (num3 - num) / num4;
				}
				else if (Math.Abs(num3 - maximumValue) < double.Epsilon)
				{
					num5 = 4.0 + (num - num2) / num4;
				}
			}

			num5 *= 60.0;
			if (num5 < 0.0)
				num5 += 360.0;

			return new HSB((int)Math.Round(num5), (int)Math.Round(num6), (int)Math.Round(a));
		}

		internal static RGB HsbToRgb(HSB hsb)
		{
			double num1 = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = hsb.Hue;
			double num5 = (double)hsb.Saturation / 100.0;
			double num6 = (double)hsb.Brightness / 100.0;

			if (num5 == 0.0)
			{
				num1 = num6;
				num2 = num6;
				num3 = num6;
			}
			else
			{
				double num7 = num4 / 60.0;
				int num8 = (int)Math.Floor(num7);
				double num9 = num7 - (double)num8;
				double num10 = num6 * (1.0 - num5);
				double num11 = num6 * (1.0 - num5 * num9);
				double num12 = num6 * (1.0 - num5 * (1.0 - num9));

				switch (num8)
				{
					case 0:
						num1 = num6;
						num2 = num12;
						num3 = num10;
						break;

					case 1:
						num1 = num11;
						num2 = num6;
						num3 = num10;
						break;

					case 2:
						num1 = num10;
						num2 = num6;
						num3 = num12;
						break;

					case 3:
						num1 = num10;
						num2 = num11;
						num3 = num6;
						break;

					case 4:
						num1 = num12;
						num2 = num10;
						num3 = num6;
						break;

					case 5:
						num1 = num6;
						num2 = num10;
						num3 = num11;
						break;
				}
			}

			int r = (int)Math.Round(num1 * 255.0);
			int g = (int)Math.Round(num2 * 255.0);
			int b = (int)Math.Round(num3 * 255.0);
			return new RGB(r, g, b);
		}

		internal static double GetMaximumValue(params double[] values)
		{
			double num = values[0];

			if (values.Length >= 2)
			{
				for (int i = 1; i != values.Length; ++i)
				{
					double val = values[i];
					num = Math.Max(num, val);
				}
			}
			return num;
		}

		internal static double GetMinimumValue(params double[] values)
		{
			double num = values[0];

			if (values.Length >= 2)
			{
				for (int i = 1; i != values.Length; ++i)
				{
					double val = values[i];
					num = Math.Min(num, val);
				}
			}
			return num;
		}

		internal static Color HexToColor(string val)
		{
			if (String.IsNullOrEmpty(val))
				val = "0";

//			val = "0x" + val;
			int num = Convert.ToInt32(val, 16);

			return Color.FromArgb((num & 0xFF0000) >> 16, (num & 0x00FF00) >> 8, num & 0x0000FF);
		}

		internal static RGB HexToRgb(string val)
		{
			Color color = HexToColor(val);
			return new RGB(color.R, color.G, color.B);
		}
		#endregion Methods (static)
	}
}
