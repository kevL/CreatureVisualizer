using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpace
	class ColorSpace
		: UserControl
	{
		#region Events
		public event ColorSpaceEventHandler SelectedComponentChanged;
		public event ColorSpaceEventHandler ValueChanged;
		#endregion Events


		#region Properties
		readonly List<ColorSpaceControl> _cscs = new List<ColorSpaceControl>();
		internal List<ColorSpaceControl> ColorSpaceControls
		{
			get { return _cscs; }
		}

		internal ColorSpaceControl Selected
		{ get; set; }
		#endregion Properties


		#region Handlers
		internal void OnComponentSelected(ColorSpaceControl sender)
		{
			SelectComponent(sender);
		}

		internal void OnValueChanged()
		{
			if (ValueChanged != null)
				ValueChanged(this);
		}
		#endregion Handlers


		#region Methods
		internal void SelectComponent(ColorSpaceControl csc)
		{
			ResetComponents();

			csc.Selected = true;
			Selected = csc;

			if (SelectedComponentChanged != null)
				SelectedComponentChanged(this);
		}

		internal void ResetComponents()
		{
			foreach (ColorSpaceControl csc in _cscs)
				csc.Selected = false;
		}
		#endregion Methods
	}



	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceEventHandler
	internal delegate void ColorSpaceEventHandler(ColorSpace sender);
}
