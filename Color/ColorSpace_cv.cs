using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpace
	class ColorSpace
		: UserControl
	{
		internal enum ColorSpaceType // TODO: This can be done by checking the child against the type of the ColorSpace parent.
		{
			Hsb,
			Rgb
		}

		// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaces
		internal enum ColorSpaces
		{
			HueSaturationBrightness,
			RedGreenBlue
		}


		#region Events
		public event ColorSpaceEventHandler ComponentValueChanged;
		public event ColorSpaceEventHandler SelectedComponentChanged;
		#endregion Events


		#region Fields
		protected internal ColorSpaceType space;
		#endregion Fields


		#region Properties
		readonly List<ColorSpaceComponent> _cscs = new List<ColorSpaceComponent>();
		internal List<ColorSpaceComponent> ColorSpaceComponents
		{
			get { return _cscs; }
		}

		internal ColorSpaceComponent Selected
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
		internal void SelectComponent(ColorSpaceComponent csc)
		{
			ResetComponents();

			csc.Selected = true;
			Selected = csc;
			OnSelectedComponentChanged(EventArgs.Empty);
		}

		internal void ResetComponents()
		{
			foreach (ColorSpaceComponent csc in _cscs)
				csc.Selected = false;
		}
		#endregion Methods
	}



	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceEventHandler
	internal delegate void ColorSpaceEventHandler(ColorSpace sender, EventArgs e);
}
