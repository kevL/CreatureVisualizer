using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorConverter
	static class ColorConverter
	{
		// NOTE: HSB is usually called HSL.
		// but in this model sat and bri range 0..100 instead of 0..1
		//
		// hue: 360 deg. 0=red, 120=green, 240=blue, 360=0
		// sat: 0=fully desaturated (bri=black(0) to gray(50) to white(100))
		//      100=fully saturated (bri=black(0) to hue (50) to white(100))
		// bri: 0=black, 50=hue/gray*, 100=white
		//
		// *if sat=0 50=gray.

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
			return Color.FromArgb(Math.Max(0, Math.Min(rgb.R, 255)),
								  Math.Max(0, Math.Min(rgb.G, 255)),
								  Math.Max(0, Math.Min(rgb.B, 255)));
		}

//		internal static Color RgbToColor(RGB rgb, int a)
//		{
//			return Color.FromArgb(Math.Max(0, Math.Min(a,     255)),
//								  Math.Max(0, Math.Min(rgb.R, 255)),
//								  Math.Max(0, Math.Min(rgb.G, 255)),
//								  Math.Max(0, Math.Min(rgb.B, 255)));
//		}

		internal static Color HsbToColor(HSB hsb)
		{
			return RgbToColor(HsbToRgb(hsb));
		}


		internal static HSB RgbToHsb(RGB rgb)
		{
			double r = rgb.R / 255.0;
			double g = rgb.G / 255.0;
			double b = rgb.B / 255.0;

			double min = getmindouble(r,g,b);
			double max = getmaxdouble(r,g,b);

			double d = max - min;
			double hue, sat;

			if (max < Single.Epsilon || d < Single.Epsilon)
			{
				hue = sat = 0;
			}
			else
			{
				if      (Math.Abs(max - r) < Single.Epsilon) hue = (g - b) / d;
				else if (Math.Abs(max - g) < Single.Epsilon) hue = (b - r) / d + 2;
				else if (Math.Abs(max - b) < Single.Epsilon) hue = (r - g) / d + 4;
//				if      (max - r < Single.Epsilon) hue = (g - b) / delta;
//				else if (max - g < Single.Epsilon) hue = (b - r) / delta + 2;
//				else if (max - b < Single.Epsilon) hue = (r - g) / delta + 4;
				else                               hue = 0;

				if (min > Single.Epsilon)
					sat = d / max * 100;
				else
					sat = 100;
			}

			if ((hue *= 60) < 0) hue += 360;

			return new HSB((int)Math.Round(hue),
						   (int)Math.Round(sat),
						   (int)Math.Round(max * 100));
		}
		// https://en.wikipedia.org/wiki/HSL_and_HSV
/*		internal static HSB RgbToHsb(RGB rgb)
		{
			int r = rgb.Red;
			int g = rgb.Green;
			int b = rgb.Blue;

			double dr = r / 255.0;
			double dg = g / 255.0;
			double db = b / 255.0;

			double min = getmindouble(dr,dg,db);
			double max = getmaxdouble(dr,dg,db);


			double d_hue;

			if (r > g && r > b)
			{
				d_hue = 60 * ((dg - db) / (max - min));
			}
			else if (g > r && g > b)
			{
				d_hue = 60 * ((db - dr) / (max - min) + 2);
			}
			else if (b > r && b > g)
			{
				d_hue = 60 * ((dr - dg) / (max - min) + 4);
			}
			else //if (r == g && r == b)
			{
				d_hue = 0;
			}

			if (d_hue < 0) d_hue += 360;


			double d_sat;

			if (   (r ==   0 && g ==   0 && b ==   0)
				|| (r == 255 && g == 255 && b == 255))
			{
				d_sat = 0;
			}
			else
				d_sat = (max - min) / (1 - Math.Abs(max + min - 1));


			double d_bri = (max + min) / 2;


			return new HSB((int)Math.Round(d_hue),
						   (int)Math.Round(d_sat * 100),
						   (int)Math.Round(d_bri * 100));
		} */

		internal static RGB HsbToRgb(HSB hsb)
		{
			double r,g,b;
			double bri = hsb.B / 100.0;

			if (hsb.S == 0)
			{
				r = g = b = bri;
			}
			else
			{
				double sat = hsb.S / 100.0;

				double d1 = hsb.H / 60.0;
				int    d2 = hsb.H / 60; //(int)Math.Floor(d1); // NOTE: Do not allow a hue of 360.
				double d  = d1 - d2;

				switch (d2)
				{
					case 0:
						r = bri;
						g = bri * (1 - sat * (1 - d));
						b = bri * (1 - sat);
						break;

					case 1:
						r = bri * (1 - sat * d);
						g = bri;
						b = bri * (1 - sat);
						break;

					case 2:
						r = bri * (1 - sat);
						g = bri;
						b = bri * (1 - sat * (1 - d));
						break;

					case 3:
						r = bri * (1 - sat);
						g = bri * (1 - sat * d);
						b = bri;
						break;

					case 4:
						r = bri * (1 - sat * (1 - d));
						g = bri * (1 - sat);
						b = bri;
						break;

					case 5:
						r = bri;
						g = bri * (1 - sat);
						b = bri * (1 - sat * d);
						break;

					default:
						r = g = b = 0;
						break;
				}
			}

			return new RGB((int)Math.Round(r * 255),
						   (int)Math.Round(g * 255),
						   (int)Math.Round(b * 255));
		}
		// https://en.wikipedia.org/wiki/HSL_and_HSV
/*		internal static RGB HsbToRgb(HSB hsb)
		{
			int      hue = hsb.Hue;
			double f_sat = hsb.Saturation / 100.0;
			double f_bri = hsb.Brightness / 100.0;

			double c = (1 - Math.Abs(f_bri * 2 - 1)) * f_sat;

			hue /= 60;

			double x = c * (1 - Math.Abs(hue % 2 - 1));

			double dr,dg,db;
			switch (hue)
			{
				default:
				case 0: dr = c; dg = x; db = 0; break;
				case 1: dr = x; dg = c; db = 0; break;
				case 2: dr = 0; dg = c; db = x; break;
				case 3: dr = 0; dg = x; db = c; break;
				case 4: dr = x; dg = 0; db = c; break;
				case 5: dr = c; dg = 0; db = x; break;
			}

			double delta = f_bri - c / 2;
			dr += delta;
			dg += delta;
			db += delta;

//			string text;
//			text = "hue= " + hue + " d_sat= " + d_sat + " d_bri= " + d_bri;
//			text += "\nc= " + c + " x= " + x + " delta= " + delta;
//			text += "\ndr= " + dr + " dg= " + dg + " db= " + db;
//			System.Windows.Forms.MessageBox.Show(text);

			return new RGB((int)Math.Round(dr * 255),
						   (int)Math.Round(dg * 255),
						   (int)Math.Round(db * 255));
		} */


		internal static RGB HexToRgb(string hecate)
		{
			Color color = HexToColor(hecate);
			return new RGB(color.R,
						   color.G,
						   color.B);
		}

		static Color HexToColor(string hecate)
		{
			if (String.IsNullOrEmpty(hecate))
				hecate = "0";

			int val = Convert.ToInt32(hecate, 16);

			return Color.FromArgb((val & 0xFF0000) >> 16,
								  (val & 0x00FF00) >>  8,
								   val & 0x0000FF);
		}


		static double getmindouble(params double[] vals)
		{
			double val = vals[0];
			for (int i = 1; i != vals.Length; ++i)
				val = Math.Min(val, vals[i]);

			return val;
		}

		static double getmaxdouble(params double[] vals)
		{
			double val = vals[0];
			for (int i = 1; i != vals.Length; ++i)
				val = Math.Max(val, vals[i]);

			return val;
		}

//		static int getminint(params int[] vals)
//		{
//			int val = vals[0];
//			for (int i = 1; i != vals.Length; ++i)
//				val = Math.Min(val, vals[i]);
//
//			return val;
//		}
//		static int getmaxint(params int[] vals)
//		{
//			int val = vals[0];
//			for (int i = 1; i != vals.Length; ++i)
//				val = Math.Max(val, vals[i]);
//
//			return val;
//		}
		#endregion Methods (static)
	}
}
