using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.RGB
	sealed class RGB
	{
		#region Properties
		internal int Red
		{ get; set; }

		internal int Green
		{ get; set; }

		internal int Blue
		{ get; set; }
		#endregion Properties


		#region cTor
		internal RGB(int red, int green, int blue)
		{
			Red   = red;
			Green = green;
			Blue  = blue;
		}
		#endregion cTor


		#region Methods (override)
		public override string ToString()
		{
			return "r: " + Red + " g: " + Green + " b: " + Blue;
		}

		public override bool Equals(object obj)
		{
			var rgb = obj as RGB;
			return rgb != null
				&& rgb.Red == Red
				&& rgb.Green == Green
				&& rgb.Blue == Blue;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode(); // are you a fucking moron - no, actually the designers of c#/.Net were fucking morons.
		}
		#endregion Methods (override)
	}
}
