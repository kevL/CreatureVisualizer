using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSelectedEventArgs
	public class ColorEventArgs
		: EventArgs
	{
		#region Properties
		internal Color Color
		{ get; private set; }
		#endregion Properties


		#region cTor
		internal ColorEventArgs(Color color)
		{
			Color = color;
		}
		#endregion cTor
	}
}
