using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HSB
	sealed class HSB
	{
		#region Properties
		internal int Hue
		{ get; set; }

		internal int Saturation
		{ get; set; }

		internal int Brightness
		{ get; set; }
		#endregion Properties


		#region cTor
		internal HSB(int hue, int saturation, int brightness)
		{
			Hue        = hue;
			Saturation = saturation;
			Brightness = brightness;
		}
		#endregion cTor


		#region Methods (override)
		public override string ToString()
		{
			return "h: " + Hue + " s: " + Saturation + " b: " + Brightness;
		}

		public override bool Equals(object obj)
		{
			var hsb = obj as HSB;
			return hsb != null
				&& hsb.Hue == Hue
				&& hsb.Saturation == Saturation
				&& hsb.Brightness == Brightness;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion Methods (override)
	}
}
