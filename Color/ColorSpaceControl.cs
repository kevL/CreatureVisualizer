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
		public event ColorSpaceEventHandler ValueChanged;
		#endregion Events


		#region Properties
		readonly List<ColorSpaceControlCo> _cos = new List<ColorSpaceControlCo>();
		internal List<ColorSpaceControlCo> ColorSpaceControls
		{
			get { return _cos; }
		}

		internal ColorSpaceControlCo Co
		{ get; private set; }
		#endregion Properties


		#region Handlers
		internal void OnCoSelected(ColorSpaceControlCo sender)
		{
			SelectCo(sender);
		}

		internal void OnValueChanged() // TODO: implement that
		{
			if (ValueChanged != null)
				ValueChanged(this);
		}
		#endregion Handlers


		#region Methods
		internal void SelectCo(ColorSpaceControlCo co)
		{
			DeselectComponents();

			co.Selected = true;
			Co = co;

			if (SelectedCoChanged != null)
				SelectedCoChanged(this);
		}

		internal void DeselectComponents()
		{
			foreach (ColorSpaceControlCo co in ColorSpaceControls)
				co.Selected = false;
		}
		#endregion Methods
	}


	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceEventHandler
	internal delegate void ColorSpaceEventHandler(ColorSpaceControl sender);
}
