using System;


namespace creaturevisualizer
{
	[Serializable]
	public class CreatureVisualizerPreferences
	{
		#region Fields
		const int Tp_Controls  = 0;
		const int Tp_Character = 1;
		#endregion Fields


		#region Properties (static)
		static CreatureVisualizerPreferences _that;
		public static CreatureVisualizerPreferences that
		{
			get
			{
				if (_that == null)
					_that = new CreatureVisualizerPreferences();

				return _that;
			}
			set { _that = value; }
		}
		#endregion Properties (static)


		#region Properties
		public int x
		{ get; set; }
		public int y
		{ get; set; }
		public int w
		{ get; set; }
		public int h
		{ get; set; }

		public bool StayOnTop
		{ get; set; }

		public bool RefreshOnFocus
		{ get; set; }

		public int ControlPanelDirection
		{ get; set; }

		public bool ShowControls
		{ get; set; }

		public bool ShowMinipanel
		{ get; set; }

		public int TabPageCurrent
		{ get; set; }


		public float LightIntensity
		{ get; set; }

		public float CameraBaseHeight
		{ get; set; }


		public int x_ColorDialog
		{ get; set; }
		public int y_ColorDialog
		{ get; set; }


//		[Browsable(true)]
//		[Description("true to show female gender if available")]
//		[Category("Creature Visualizer")]
		public bool char_Female
		{ get; set; }
		#endregion Properties


		#region cTor
		CreatureVisualizerPreferences()
		{
			x = y =
			w = h = Int32.MinValue;

			StayOnTop = true;
			RefreshOnFocus = true;
			ControlPanelDirection = (int)CpDir.e;
			ShowControls = false;
			ShowMinipanel = true;
			TabPageCurrent = Tp_Controls;

			LightIntensity = 0.75F;
			CameraBaseHeight = 1.10F;

			x_ColorDialog =
			y_ColorDialog = Int32.MinValue;

			char_Female = false;
		}
		#endregion cTor
	}
}
