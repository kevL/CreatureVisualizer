using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpace
	class ColorSpaceControl
		: UserControl
	{
		#region Events
		public event ColorSpaceEvent CiscoSelected;
		public event ColorSpaceEvent CiscoValueChanged;
		#endregion Events


		#region Properties
		readonly List<ColorSpaceControlCisco> _ciscos = new List<ColorSpaceControlCisco>();
		internal protected List<ColorSpaceControlCisco> SpaceControls
		{
			get { return _ciscos; }
		}

		internal ColorSpaceControlCisco Cisco
		{ get; private set; }
		#endregion Properties


		#region Handlers
		internal protected void OnCiscoSelected(ColorSpaceControlCisco sender)
		{
			SelectCisco(sender);
		}

		internal protected void OnCiscoValueChanged(ColorSpaceControlCisco sender)
		{
			if (CiscoValueChanged != null)
				CiscoValueChanged(this);
		}
		#endregion Handlers


		#region Methods
		internal void SelectCisco(ColorSpaceControlCisco cisco)
		{
			DeselectCiscos();

			(Cisco = cisco).Selected = true;

			if (CiscoSelected != null)
				CiscoSelected(this);
		}

		internal void DeselectCiscos()
		{
			foreach (ColorSpaceControlCisco cisco in SpaceControls)
				cisco.Selected = false;
		}
		#endregion Methods
	}


	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceEventHandler
	internal delegate void ColorSpaceEvent(ColorSpaceControl sender);
}
