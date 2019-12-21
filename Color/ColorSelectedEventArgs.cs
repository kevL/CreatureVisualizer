using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSelectedEventArgs
	public class ColorSelectedEventArgs
		: EventArgs
	{
		#region Fields
		Color m_color;
		#endregion Fields


		#region Properties
		internal Color Color
		{
			get { return m_color; }
		}
		#endregion Properties


		#region cTor
		internal ColorSelectedEventArgs(Color color)
		{
			m_color = color;
		}
		#endregion cTor
	}
}
