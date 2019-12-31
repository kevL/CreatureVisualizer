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
		public event ColorSpaceEventHandler SelectedCoChanged;
		public event ColorSpaceEventHandler CoValueChanged;
		#endregion Events


		#region Properties
		readonly List<ColorSpaceControlCo> _cos = new List<ColorSpaceControlCo>();
		internal protected List<ColorSpaceControlCo> SpaceControls
		{
			get { return _cos; }
		}

		internal ColorSpaceControlCo Co
		{ get; private set; }
		#endregion Properties


		#region Handlers
		internal protected void OnCoSelected(ColorSpaceControlCo sender)
		{
			SelectCo(sender);
		}

		internal protected void OnValueChanged(ColorSpaceControlCo sender)
		{
			if (CoValueChanged != null)
				CoValueChanged(this);
		}
		#endregion Handlers


		#region Methods
		internal void SelectCo(ColorSpaceControlCo co)
		{
			DeselectComponents();

			(Co = co).Selected = true;

			if (SelectedCoChanged != null)
				SelectedCoChanged(this);
		}

		internal void DeselectComponents()
		{
			foreach (ColorSpaceControlCo co in SpaceControls)
				co.Selected = false;
		}
		#endregion Methods
	}


	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceEventHandler
	internal delegate void ColorSpaceEventHandler(ColorSpaceControl sender);
}
