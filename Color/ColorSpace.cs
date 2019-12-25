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
		public event ColorSpaceEventHandler SelectedCscChanged;
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
		internal void OnCscSelected(ColorSpaceControl sender)
		{
			SelectCsc(sender);
		}

		internal void OnValueChanged() // TODO: implement that
		{
			if (ValueChanged != null)
				ValueChanged(this);
		}
		#endregion Handlers


		#region Methods
		internal void SelectCsc(ColorSpaceControl csc)
		{
			ResetComponents();

			csc.Selected = true;
			Selected = csc;

			if (SelectedCscChanged != null)
				SelectedCscChanged(this);
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
