using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.RGB
	sealed class RGB
	{
		#region Properties
		internal int R
		{ get; set; }

		internal int G
		{ get; set; }

		internal int B
		{ get; set; }
		#endregion Properties


		#region cTor
		internal RGB(int r, int g, int b)
		{
			R = r;
			G = g;
			B = b;
		}
		#endregion cTor


		#region Methods (override)
		public override string ToString()
		{
			return "r: " + R + " g: " + G + " b: " + B;
		}

		public override bool Equals(object obj)
		{
			var rgb = obj as RGB;
			return rgb != null
				&& rgb.R == R
				&& rgb.G == G
				&& rgb.B == B;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion Methods (override)
	}
}
