using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSelectedEventArgs
	public class ColorEventArgs
		: EventArgs
	{
		#region Properties
		internal Color Col
		{ get; private set; }

		internal RGB RGB
		{ get; private set; }

		internal HSL HSL
		{ get; private set; }
		#endregion Properties


		#region cTor
		internal ColorEventArgs(Color col, RGB rgb = null, HSL hsl = null)
		{
			Col = col;
			RGB = rgb;
			HSL = hsl;
		}
		#endregion cTor
	}
}
