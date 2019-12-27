using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorPanelSettings
	[Serializable]
	public class ColorPanelSettings
	{
		public string ActiveColorBox
		{ get; set; }

		public Color TopColorBoxColor
		{ get; set; }

		public Color BottomColorBoxColor
		{ get; set; }

		public string SelectedColorSpaceComponent
		{ get; set; }
	}
}
