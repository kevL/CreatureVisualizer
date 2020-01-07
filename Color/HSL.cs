using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HSB
	sealed class HSL
	{
		#region Properties
		internal int H
		{ get; set; }

		internal int S
		{ get; set; }

		internal int L
		{ get; set; }
		#endregion Properties


		#region cTor
		internal HSL(int h = 0, int s = 0, int l = 0)
		{
			H = h;
			S = s;
			L = l;
		}
		#endregion cTor


		#region Methods (override)
		public override string ToString()
		{
			return "h: " + H + " s: " + S + " l: " + L;
		}

		public override bool Equals(object obj)
		{
			var hsb = obj as HSL;
			return hsb != null
				&& hsb.H == H
				&& hsb.S == S
				&& hsb.L == L;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion Methods (override)
	}
}
