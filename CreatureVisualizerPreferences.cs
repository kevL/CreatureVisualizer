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

			char_Female = false;

/*			m_CRPercent = 100;
			m_lowercaseTag = false;
			m_showCRWarning = true;
			m_prefix = "";
			m_varSet = "c_TypicalCreatureVars.xml";
			m_scriptSet = "c_StandardScripts.xml";
//			m_hpOption = HitPointOption.Percentage;
			m_hitPointPercent = 67; */
		}
		#endregion cTor



/*		private string m_prefix;
		private bool m_lowercaseTag;
		private string m_varSet;
		private string m_scriptSet;
		private bool m_showCRWarning;
		private int m_hitPointPercent;
//		private HitPointOption m_hpOption;
		private int m_CRPercent;


		[Description("When creating new creatures, this setting forces all tags entered to be in lowercase.")]
		[Browsable(true)]
		[Category("Creature Creation")]
		public bool ForceTagToLowercase
		{
			get { return m_lowercaseTag; }
			set { m_lowercaseTag = value; }
		}

		[Description("If set, all created creature tags and resrefs are prefixed with this value.")]
		[Category("Creature Creation")]
		[Browsable(true)]
		public string BuilderTagAndResrefPrefix
		{
			get { return m_prefix; }
			set { m_prefix = value; }
		}

		[Description("If provided, new creatures will default to using the scriptset that is specified in this file.  The scriptset files are located in your NWN2 installation directory, under NWN2Toolset\\ScriptSets")]
		[Category("Scripting")]
		[Browsable(true)]
		public string DefaultScriptSet
		{
			get { return m_scriptSet; }
			set { m_scriptSet = value; }
		}

		[Category("Scripting")]
		[Description("If provided, new creatures will default to using the variable set that is specified in this file. The variableset files are located in your NWN2 installation directry, under NWN2Toolset\\VariableSets")]
		[Browsable(true)]
		public string DefaultVariableSet
		{
			get { return m_varSet; }
			set { m_varSet = value; }
		}

		[Browsable(true)]
		[Description("The auto-calculated challenge rating is multiplied by this amount.")]
		[Category("Challenge Rating")]
		public int ChallengeRatingModifier
		{
			get { return m_CRPercent; }
			set { m_CRPercent = value; }
		}

		[Category("Challenge Rating")]
		[Description("If set to True, you are asked before changing a selected creature's challenge rating through the menu option.")]
		[Browsable(true)]
		public bool ShowSaveWarningOnCRAdjust
		{
			get { return m_showCRWarning; }
			set { m_showCRWarning = value; }
		}


//		public HitPointOption HitPointOption
//		{
//			get { return m_hpOption; }
//			set { m_hpOption = value; }
//		}

		public int HitPointPercentOfMax
		{
			get { return m_hitPointPercent; }
			set
			{
				if (value < 0)
					m_hitPointPercent = 0;
				else if (value > 100)
					m_hitPointPercent = 100;
				else
					m_hitPointPercent = value;
			}
		} */
	}
}
