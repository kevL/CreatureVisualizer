using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HSB
	sealed class HSB
	{
		#region Properties
		internal int H
		{ get; set; }

		internal int S
		{ get; set; }

		internal int B
		{ get; set; }
		#endregion Properties


		#region cTor
		internal HSB(int h, int s, int b)
		{
			H = h;
			S = s;
			B = b;
		}
		#endregion cTor


		#region Methods (override)
		public override string ToString()
		{
			return "h: " + H + " s: " + S + " b: " + B;
		}

		public override bool Equals(object obj)
		{
			var hsb = obj as HSB;
			return hsb != null
				&& hsb.H == H
				&& hsb.S == S
				&& hsb.B == B;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion Methods (override)
	}
}
