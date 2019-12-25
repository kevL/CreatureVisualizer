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
		public event ColorSpaceEventHandler ComponentValueChanged;
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
		void OnSelectedComponentChanged(EventArgs e)
		{
			if (SelectedComponentChanged != null)
				SelectedComponentChanged(this, e);
		}

		internal void OnComponentValueChanged(EventArgs e)
		{
			if (ComponentValueChanged != null)
				ComponentValueChanged(this, e);
		}
		#endregion Handlers


		#region Methods
		internal void SelectComponent(ColorSpaceControl csc)
		{
			ResetComponents();

			csc.Selected = true;
			Selected = csc;
			OnSelectedComponentChanged(EventArgs.Empty);
		}

		internal void ResetComponents()
		{
			foreach (ColorSpaceControl csc in _cscs)
				csc.Selected = false;
		}
		#endregion Methods
	}



	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceEventHandler
	internal delegate void ColorSpaceEventHandler(ColorSpace sender, EventArgs e);
}
