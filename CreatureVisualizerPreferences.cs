using System;


namespace creaturevisualizer
{
	/// <summary>
	/// Preferences per Electron toolset plugin interface.
	/// @note Prefs are stored at
	/// C:\Users\User\AppData\Local\NWN2 Toolset\Plugins\CreatureVisualizer.xml
	/// </summary>
	[Serializable]
	public class CreatureVisualizerPreferences
	{
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

		public int RefreshProtocol
		{ get; set; }

		public int ControlPanelDirection
		{ get; set; }

		public bool ShowControlPanel
		{ get; set; }

		public bool ShowMiniPanel
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


		public string LastSaveDirectory
		{ get; set; }
		#endregion Properties


		public bool ProcessEquipped_body
		{ get; set; }

		public bool ProcessEquipped_held
		{ get; set; }

		public bool ProcessInventory
		{ get; set; }


		#region cTor
		/// <summary>
		/// cTor. Sets default values for the CreatureVisualizer's preferences.
		/// @note CreVisF..cTor will quickly override these values with values
		/// in the user's prefs-file (if it exists).
		/// </summary>
		CreatureVisualizerPreferences()
		{
			x = y =
			w = h = Int32.MinValue;

			StayOnTop             = true;
			RefreshProtocol       = (int)RefreshType.foc | (int)RefreshType.oac;
			ControlPanelDirection = (int)CpDir.e;
			ShowControlPanel      = false;
			ShowMiniPanel         = true;
			TabPageCurrent        = 0;

			LightIntensity   = 0.75F;
			CameraBaseHeight = 1.00F;

			x_ColorDialog =
			y_ColorDialog = Int32.MinValue;

			LastSaveDirectory = String.Empty;

			ProcessEquipped_body =
			ProcessEquipped_held =
			ProcessInventory     = true;
		}
		#endregion cTor
	}
}
