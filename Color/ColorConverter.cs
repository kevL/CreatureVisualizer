using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorConverter
	static class ColorConverter
	{
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
			return Color.FromArgb(Math.Max(0, Math.Min(rgb.Red,   255)),
								  Math.Max(0, Math.Min(rgb.Green, 255)),
								  Math.Max(0, Math.Min(rgb.Blue,  255)));
		}

//		internal static Color RgbToColor(RGB rgb, int a)
//		{
//			return Color.FromArgb(Math.Max(0, Math.Min(a,         255)),
//								  Math.Max(0, Math.Min(rgb.Red,   255)),
//								  Math.Max(0, Math.Min(rgb.Green, 255)),
//								  Math.Max(0, Math.Min(rgb.Blue,  255)));
//		}

		internal static Color HsbToColor(HSB hsb)
		{
			return RgbToColor(HsbToRgb(hsb));
		}

/*		internal static HSB RgbToHsb(RGB rgb)
		{
			Color color = RgbToColor(rgb);

			//var trace = new System.Diagnostics.StackTrace();
			//System.Windows.Forms.MessageBox.Show("h= " + (int)Math.Round(color.GetHue()) + "\ns= "
			//+ ((int)Math.Round(color.GetSaturation()) * 100) + "\nb= " + ((int)Math.Round(color.GetBrightness()) * 100)
			//+ "\n" + trace);

			return new HSB((int)Math.Round(color.GetHue()),
						   (int)Math.Round(color.GetSaturation()) * 100,
						   (int)Math.Round(color.GetBrightness()) * 100);


//			int r = rgb.Red;
//			int g = rgb.Green;
//			int b = rgb.Blue;
//
//			int bri = getmaxint(r,g,b);
//			if (bri != 0)
//			{
//				int darkest = getminint(r,g,b);
//
//				double delta = (bri - darkest) / 255.0;
//
//				int hue;
//				if      (bri == r) hue = (int)Math.Round(      (g - b) / 255.0 / delta);
//				else if (bri == g) hue = (int)Math.Round(2.0 + (b - r) / 255.0 / delta);
//				else if (bri == b) hue = (int)Math.Round(4.0 + (r - g) / 255.0 / delta);
//				else               hue = 0;
//
//				int sat;
//				if (darkest != 0) sat = (int)Math.Round(delta / bri * 2.55);
//				else              sat = 100;
//
//				while ((hue *= 60) < 0) hue += 360;
//
//				return new HSB(hue,
//							   sat,
//							   (int)Math.Round(bri * 100.0));
//			}
//			return new HSB(0,0,0);
		} */
		internal static HSB RgbToHsb(RGB rgb)
		{
			double d1 = (double)rgb.Red   / 255.0;
			double d2 = (double)rgb.Green / 255.0;
			double d3 = (double)rgb.Blue  / 255.0;

			double min = getmindouble(d1,d2,d3);
			double max = getmaxdouble(d1,d2,d3);

			double d4 = max - min;
			double d5;
			double d6;

//			if (max == 0.0 || d4 == 0.0)
			if (max < Single.Epsilon || d4 < Single.Epsilon)
			{
				d5 = 0.0;
				d6 = 0.0;
			}
			else
			{
				if      (Math.Abs(d1 - max) < Single.Epsilon) d5 =       (d2 - d3) / d4;
				else if (Math.Abs(d2 - max) < Single.Epsilon) d5 = 2.0 + (d3 - d1) / d4; 
				else if (Math.Abs(d3 - max) < Single.Epsilon) d5 = 4.0 + (d1 - d2) / d4;
				else                                          d5 = 0.0;

				d6 = ((min > Single.Epsilon) ? (d4 / max * 100.0) : 100.0);
//				d6 = ((min != 0.0) ? (d4 / max * 100.0) : 100.0);
			}

			while ((d5 *= 60.0) < 0.0) d5 += 360.0;

			//System.Windows.Forms.MessageBox.Show("h= " + (int)Math.Round(d5) + "\ns= " + (int)Math.Round(d6) + "\nb= " + (int)Math.Round(max * 100.0));

			return new HSB((int)Math.Round(d5),
						   (int)Math.Round(d6),
						   (int)Math.Round(max * 100.0));
		}

		internal static RGB HsbToRgb(HSB hsb)
		{
			double d1 = 0.0;
			double d2 = 0.0;
			double d3 = 0.0;

			double d4 = hsb.Hue;
			double d5 = (double)hsb.Saturation / 100.0;
			double d6 = (double)hsb.Brightness / 100.0;

			if (d5 == 0.0)
			{
				d1 = d6;
				d2 = d6;
				d3 = d6;
			}
			else
			{
				double d7 = d4 / 60.0;
				double d8 = (int)Math.Floor(d7);
				double d9 = d7 - (double)d8;

				switch ((int)d8)
				{
					case 0:
						d1 = d6;
						d2 = d6 * (1.0 - d5 * (1.0 - d9));
						d3 = d6 * (1.0 - d5);
						break;

					case 1:
						d1 = d6 * (1.0 - d5 * d9);
						d2 = d6;
						d3 = d6 * (1.0 - d5);
						break;

					case 2:
						d1 = d6 * (1.0 - d5);
						d2 = d6;
						d3 = d6 * (1.0 - d5 * (1.0 - d9));
						break;

					case 3:
						d1 = d6 * (1.0 - d5);
						d2 = d6 * (1.0 - d5 * d9);
						d3 = d6;
						break;

					case 4:
						d1 = d6 * (1.0 - d5 * (1.0 - d9));
						d2 = d6 * (1.0 - d5);
						d3 = d6;
						break;

					case 5:
						d1 = d6;
						d2 = d6 * (1.0 - d5);
						d3 = d6 * (1.0 - d5 * d9);
						break;
				}
			}

			return new RGB((int)Math.Round(d1 * 255.0),
						   (int)Math.Round(d2 * 255.0),
						   (int)Math.Round(d3 * 255.0));
		}

		internal static RGB HexToRgb(string hextext)
		{
			Color color = HexToColor(hextext);
			return new RGB(color.R,
						   color.G,
						   color.B);
		}

		static Color HexToColor(string hextext)
		{
			if (String.IsNullOrEmpty(hextext))
				hextext = "0";

			int val = Convert.ToInt32(hextext, 16);

			return Color.FromArgb((val & 0xFF0000) >> 16,
								  (val & 0x00FF00) >>  8,
								   val & 0x0000FF);
		}


		static int getminint(params int[] vals)
		{
			int val = vals[0];
			for (int i = 1; i != vals.Length; ++i)
				val = Math.Min(val, vals[i]);

			return val;
		}

		static int getmaxint(params int[] vals)
		{
			int val = vals[0];
			for (int i = 1; i != vals.Length; ++i)
				val = Math.Max(val, vals[i]);

			return val;
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
		#endregion Methods (static)
	}
}
