using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;

using NWN2Toolset;
using NWN2Toolset.NWN2.Data.Blueprints;
using NWN2Toolset.NWN2.Data.Instances;
using NWN2Toolset.NWN2.Data.Templates;
using NWN2Toolset.NWN2.Data.TypedCollections;
using NWN2Toolset.NWN2.NetDisplay;
using NWN2Toolset.NWN2.Views;

using OEIShared.IO;
using OEIShared.NetDisplay;
using OEIShared.OEIMath;
using OEIShared.UI;
using OEIShared.UI.Input;
using OEIShared.Utils;


namespace creaturevisualizer
{
	/// <summary>
	/// Reset types for the Model object.
	/// </summary>
	enum ResetType
	{
		RESET_non,	// 0
		RESET_z,	// 1
		RESET_xy,	// 2
		RESET_rot,	// 3
		RESET_scale	// 4
	}


	/// <summary>
	/// Credit: The Grinning Fool's Creature Creation Wizard
	/// https://neverwintervault.org/project/nwn2/other/grinning-fools-creature-creation-wizard
	/// and the NwN2 toolset's Appearance Wizard, etc.
	/// </summary>
	sealed class ElectronPanel_
		: ElectronPanel
	{
		#region Fields (static)
// Camera ->
		internal const float CAM_START_TET = (float)Math.PI /  2F;
		internal const float CAM_START_PHI = (float)Math.PI / 32F;

		internal const float CAM_START_DIST = 5F;

		internal static Vector3 CAM_START_POS;

		internal static Vector3 CAM_BASEHEIGHT = new Vector3(0F,0F, CreatureVisualizerPreferences.that.CameraBaseHeight);

// Model ->
		const float MODEL_START_ROT = (float)Math.PI * 7F / 8F;

		static Vector3 ScaInitial;

// Light ->
		internal static Vector3 LIGHT_START_POS = new Vector3(-0.5F, -4F, 2F);
		const float LIGHT_START_RANGE = 50F; // default 10F

// DayNightCycle ->
		static Vector3 E_DEF_SUNMOON_POS = new Vector3(-0.33F, -0.67F, -0.67F);
		const float E_DEF_SHADOW_VAL = 0F;

		// Colors ->
		internal static Color ? ColorDiffuse;
		internal static Color ? ColorSpecular;
		internal static Color ? ColorAmbient;

		internal static bool ColorCheckedDiffuse;
		internal static bool ColorCheckedSpecular;
		internal static bool ColorCheckedAmbient;
		#endregion Fields (static)


		#region Fields
		readonly CreVisF _f;

		Vector3      _pos; // position of the Model
		RHQuaternion _rot; // rotation of the Model
		Vector3      _sca; // scale    of the Model

		Vector3 _posLight = LIGHT_START_POS;
		#endregion Fields


		#region Properties
		// Note that when an Instance is selected the visualizer uses that
		// Instance. But when a Blueprint is selected the visualizer creates
		// its own instance of the Blueprint. The instance will then be used to
		// instantiate a NetDisplayObject aka the 'Model'.
		//
		// That is, in order to test whether a Blueprint or an Instance is
		// currently displayed, test Blueprint==null (NOT Instance==null)
		// since objects based on Blueprints *do have an Instance*.

		/// <summary>
		/// The currently selected Blueprint. Can be changed by the "Apply"
		/// operation.
		/// </summary>
		internal INWN2Blueprint Blueprint_base
		{ get; set; }

		/// <summary>
		/// The currently displayed Blueprint (ie, instantiate a duplicate of
		/// 'Blueprint_base'). Can be changed by the "Display" operation.
		/// </summary>
		internal INWN2Blueprint Blueprint
		{ get; set; }

		/// <summary>
		/// The currently selected Instance. Can be changed by the "Apply"
		/// operation.
		/// </summary>
		internal INWN2Instance Instance_base
		{ get; set; }

		INWN2Instance _instance;
		/// <summary>
		/// The currently displayed Instance (ie, instantiate a duplicate of
		/// 'Instance_base'). Can be changed by the "Display" operation.
		/// </summary>
		internal INWN2Instance Instance
		{
			get { return _instance; }
			private set
			{
				bool valid = (_instance = value) != null;

				_f.EnableSaveAs(valid);
				_f.EnableSaveToCampaign(valid);
			}
		}


		internal NetDisplayObject Model
		{ get; private set; }

		internal NetDisplayLightPoint Light
		{ get; private set; }

		internal ModelViewerInputCameraReceiver Receiver
		{ get; private set; }
		#endregion Properties

		//handle SelectionChanged event


/*		/// <summary>
		/// This works great. Absolutely kills flicker on redraws.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;
				return cp;
			}
		} */


		#region cTor
		internal ElectronPanel_(CreVisF f)
		{
			_f = f;

//			DoubleBuffered = true;

			RecreateMousePanel();

			OpenWindow();

			_t2.Tick += tick;
			_t2.Interval = 35;
		}


/*		/// <summary>
		/// Calls SetDoubleBuffered(object) on an array of objects.
		/// </summary>
		/// <param name="controls"></param>
		static void SetDoubleBuffered(object[] controls)
		{
			foreach (var control in controls)
				SetDoubleBuffered(control);
		}

		/// <summary>
		/// Some controls, such as the DataGridView, do not allow setting the
		/// DoubleBuffered property. It is set as a protected property. This
		/// method is a work-around to allow setting it. Call this in the
		/// constructor just after InitializeComponent().
		/// https://stackoverflow.com/questions/118528/horrible-redraw-performance-of-the-datagridview-on-one-of-my-two-screens#answer-16625788
		/// @note I wonder if this works on Mono. It stops the redraw-flick when
		/// setting the sprite-phase on return from SpritesetviewF on my system
		/// (Win7-64). Also stops flicker on the IsoLoft panel. etc.
		/// </summary>
		/// <param name="control">the Control on which to set DoubleBuffered to true</param>
		static void SetDoubleBuffered(object control)
		{
			// if not remote desktop session then enable double-buffering optimization
			if (!SystemInformation.TerminalServerSession)
			{
				// set instance non-public property with name "DoubleBuffered" to true
				typeof(Control).InvokeMember("DoubleBuffered",
											 System.Reflection.BindingFlags.SetProperty
										   | System.Reflection.BindingFlags.Instance
										   | System.Reflection.BindingFlags.NonPublic,
											 null,
											 control,
											 new object[] { true });
			}
		} */


		/// <summary>
		/// Disposes their MousePanel and instantiates a new MousePanel to
		/// replace it. CreatureVisualizer-specific inputhandlers can be hooked
		/// up to this ElectronPanel that totally bypass the tangle of Obsidian
		/// inputhandlers. good luck
		/// @note A 'MousePanel' is not a panel nor does it handle mouse-input
		/// exclusively; it should have been labeled 'InputController'.
		/// </summary>
		void RecreateMousePanel()
		{
			MousePanel.Dispose(); // OMG it effin worked.
			MousePanel = new MousePanel();

			var resourcer = new ComponentResourceManager(typeof(ElectronPanel));
			resourcer.ApplyResources(MousePanel, "MousePanel");

			MousePanel.Name = "MousePanel";

			MousePanel.ThrottleMouse = true; // no idea ...

			CameraMovementReceiver = new ModelViewerInputCameraReceiver();
			AddInputReceiver(CameraMovementReceiver);

			Receiver = CameraMovementReceiver as ModelViewerInputCameraReceiver;

//			MousePanel.MouseDown       += ᐁ;
//			MousePanel.MouseMove       += ᐄ;
//			MousePanel.MouseButtonHeld += ᐃ;
//			MousePanel.KeyUp           += ᐂ;
//			MousePanel.DragHandler     += ᐁ;
//			MousePanel.KeyPress        += ᐁ;
//			MousePanel.MouseUp         += ᐂ;
//			MousePanel.KeyDown         += ᐁ;
//			MousePanel.MouseWheel      += ᐃ;
//			MousePanel.LostFocus       += ᐁ;

			MousePanel.MouseDown += mousedown;
			MousePanel.MouseUp   += mouseup;

			Controls.Add(MousePanel);
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Initializes the scene. Clears any objects, sets up default lighting,
		/// and adds a lightpoint.
		/// </summary>
		bool Initialize()
		{
			if (_f.WindowState != FormWindowState.Minimized)
			{
				CloseWindow(); // safety - try not to confuse the NWN2NetDisplayManager.Instance ...

				if (NDWindow == null)
					OpenWindow();

				if (NDWindow != null && Scene != null)
				{
					ClearObjects();

					if (Scene.DayNightCycleStages[(int)DayNightStageType.Default] != null)
					{
						Scene.DayNightCycleStages[(int)DayNightStageType.Default].SunMoonDirection = E_DEF_SUNMOON_POS;
						Scene.DayNightCycleStages[(int)DayNightStageType.Default].ShadowIntensity  = E_DEF_SHADOW_VAL;
					}

/*		case DayNightStageType.Default:								// OEIShared.NetDisplay.DayNightState ->
			SunMoonDirection = new Vector3(-0.05f, 0.08f, -0.1f);
			SunMoon.DiffuseColor = Color.FromArgb(194, 139, 87);
			SunMoon.SpecularColor = Color.FromArgb(173, 188, 163);
			SunMoon.AmbientColor = Color.FromArgb(255, 255, 255);
			SunMoon.Intensity = 1f;
			SkyLight.DiffuseColor = Color.FromArgb(215, 229, 250);
			SkyLight.SpecularColor = Color.FromArgb(194, 139, 87);
			SkyLight.AmbientColor = Color.FromArgb(255, 255, 255);
			SkyLight.Intensity = 0.2f;
			GroundLight.DiffuseColor = Color.FromArgb(221, 194, 161);
			GroundLight.SpecularColor = Color.FromArgb(221, 194, 161);
			GroundLight.AmbientColor = Color.FromArgb(255, 255, 255);
			GroundLight.Intensity = 0.45f;
			SkyHorizon = Color.FromArgb(163, 189, 255);
			SkyZenith = Color.FromArgb(173, 194, 255);
			Fog.FogColor = Color.FromArgb(134, 153, 211);
			Fog.FarClip = 200f;
			Fog.FogStart = 60f;
			Fog.FogEnd = 170f;
			AvgLuminance = 0.65f;
			BloomBlurRadius = 6.4f;
			BloomGlowIntensity = 0f;
			BloomHighlightIntensity = 0.54f;
			BloomHighlightThreshold = 0.86f;
			BloomSceneIntensity = 1f;
			CloudCover = 0.8f;
			CloudMovementRateX = 0.04f;
			CloudMovementRateY = 0f;
			Exposure = 5f;
			HighlightThreshold = 5f;
			MaxLuminance = 3.65f;
			ShadowIntensity = 0.42f;
			SunCoronaIntensity = 0.33f;
			break;
*/

					CreateLight();

					_f.PrintLightIntensity(Light.Color.Intensity);
					_f.PrintDiffuseColor();
					_f.PrintSpecularColor();
					_f.PrintAmbientColor();

//					SetDoubleBuffered(NDWindow);
//					SetDoubleBuffered(NWN2NetDisplayManager.Instance.Windows);

//					var a = new NetDisplayWindow[NWN2NetDisplayManager.Instance.Windows.Count];
//					for (int i = 0; i != NWN2NetDisplayManager.Instance.Windows.Count; ++i)
//						a[i] = NWN2NetDisplayManager.Instance.Windows[i];
//					SetDoubleBuffered(a);

					return true;
				}
			}
			return false;
		}

		void CreateLight()
		{
			Light = new NetDisplayLightPoint();

			Light.Position        = _posLight;

			Light.Color.Intensity = CreatureVisualizerPreferences.that.LightIntensity;
			Light.Range           = LIGHT_START_RANGE;
			Light.CastsShadow     = false;

			Light.ID              = NetDisplayManager.Instance.NextObjectID;	// doesn't appear to be req'd.
			Light.Tag             = Light;										// doesn't appear to be req'd. (light gets tagged w/ a pointer to itself)


			if (ColorCheckedDiffuse)  Light.Color.DiffuseColor  = (Color)ColorDiffuse;
			if (ColorCheckedSpecular) Light.Color.SpecularColor = (Color)ColorSpecular;
			if (ColorCheckedAmbient)  Light.Color.AmbientColor  = (Color)ColorAmbient;


			lock (Scene.Objects.SyncRoot)
			{
				Scene.Objects.Add(Light);
			}
			lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)
			{
				NWN2NetDisplayManager.Instance.Objects.Add(Light);				// doesn't appear to be req'd.
			}
//lock (this.m_ᐁ.NDWindow.Scene.Objects.SyncRoot) // TODO: figure out what 'SyncRoot' etc is about ...
//{
//	this.m_ᐁ.NDWindow.Scene.Objects.Remove(this.m_ᐂ);
//}
//lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)
//{
//	NWN2NetDisplayManager.Instance.Objects.Remove(this.m_ᐂ);
//}

			NWN2NetDisplayManager.Instance.LightParameters(Scene, Light);
			_f.PrintLightPosition(Light.Position);
		}

		/// <summary>
		/// Creates an instance of a blueprint and tries to render it in the
		/// ElectronPanel.
		/// </summary>
		internal void CreateModel()
		{
			// IMPORTANT: Policy #256 - *never* allow the instance to be other
			// than the Blueprint or Instance that the user has currently
			// selected. This will greatly ease the "Apply to ..." operation.
			// THE INSTANCE DISPLAYED IN THE VISUALIZER MUST ALWAYS REFERENCE
			// THE BLUEPRINT OR INSTANCE THAT THE USER HAS CURRENTLY SELECTED.
			//
			// That is, if the user clicks away from his/her currently selected
			// Blueprint or Instance (ie. by selecting a different Blueprint or
			// Instance) clear the visualizer-display after asking to either
			// Save or Clear - but only if the currently displayed instance has
			// actually been changed ofc w/ "Display". Note that if an
			// instance's displayed characteristics have been "Applied" to the
			// current Blueprint/Instance then no confirmation is required to
			// instantiate an instance that's different than the current
			// Blueprint/Instance.
			//
			// IMPORTANT: Policy #257 - it is the user's responsibility to save
			// any applied changes before closing the toolset. Note, "save to
			// file" needs to be wired up to write the applied instance; it can
			// and will bypass most of these shenanigans.


			if (!CreVisF.BypassCreate) // don't recreate the instance when returning from a dialog when "RefreshOnFocus" is enabled.
			{
				if (Blueprint != null)
				{
					// ask to ignore, Apply (if not stock resource), or save-to-file (disable the Cancel option)
					_f.ConfirmClose(false);
				}


				_f.ClearResourceInfo();
				_f.Changed = CreVisF.ChangedType.ct_nul; // set '_f.Text'

				Blueprint = null;
				Instance  = null;

				_f.bu_creature_apply.Enabled = true;
				_f.EnableCreaturePage(false);

				if (MousePanel != null && !MousePanel.IsDisposed) // safety. ElectronPanel.MousePanel could go disposed for no good reason.
				{
					NWN2AreaViewer viewer;
					NWN2InstanceCollection collection;

					//viewer.AreaNetDisplayWindow.Scene
					//viewer.SelectedNDOs

					bool different = false;

// first check areaviewer for a selected Instance ->
					if ((viewer = NWN2ToolsetMainForm.App.GetActiveViewer() as NWN2AreaViewer) != null
						&& (collection = viewer.SelectedInstances) != null && collection.Count == 1
						&& (   collection[0] is NWN2CreatureInstance
							|| collection[0] is NWN2DoorInstance
							|| collection[0] is NWN2PlaceableInstance))
					{
//						if (!(collection[0] as INWN2Instance).Equals(Instance_pre))
/*						if (Instance_pre == null || (collection[0] as INWN2Instance) != Instance_pre)
						{
//							if (_f.ConfirmChange())
//							{
//							}
							Instance = Instance_pre = collection[0] as INWN2Instance;
							different = true;
						} */
						Instance = collection[0] as INWN2Instance;
						_f.PrintResourceInfo(Instance); // INWN2Template

						if ((Instance as NWN2CreatureInstance) != null)
						{
							_f.EnableCreaturePage(true);

							_f.InitGender((Instance as NWN2CreatureInstance).Gender);
						}

						_f.Changed = CreVisF.ChangedType.ct_not;
						_f.bu_creature_apply.Text = "APPLY to Instance";
					}
// second check blueprint lists for a selected Blueprint ->
					else
					{
						NWN2BlueprintView tslist = NWN2ToolsetMainForm.App.BlueprintView;

						object[] selection = tslist.Selection;
						if (selection != null && selection.Length == 1)
						{
							NWN2ObjectType type = tslist.GetFocusedListObjectType();

							switch (type)
							{
								case NWN2ObjectType.Creature:
								case NWN2ObjectType.Door:
								case NWN2ObjectType.Placeable:
								case NWN2ObjectType.PlacedEffect:
								case NWN2ObjectType.Item:
									if (Blueprint_base == null || (selection[0] as INWN2Blueprint) != Blueprint_base)
									{
										Blueprint_base = selection[0] as INWN2Blueprint;
										different = true;
									}

									Blueprint = CreateBlueprint(Blueprint_base);
									_f.PrintResourceInfo(Blueprint);

									_f.bu_creature_apply.Enabled = (Blueprint.Resource.Repository as DirectoryResourceRepository) != null;


									switch (type)
									{
										case NWN2ObjectType.Creature:
											_f.EnableCreaturePage(true);

											_f.InitGender((Blueprint as NWN2CreatureBlueprint).Gender);

/*
//											((NWN2CreatureBlueprint)blueprint).AppearanceHair; // byte
											// etc ...

											// bool ->
											((NWN2CreatureBlueprint)blueprint).AppearanceFacialHair;

											((NWN2CreatureBlueprint)blueprint).HasBelt;
											((NWN2CreatureBlueprint)blueprint).HasBoots;
											((NWN2CreatureBlueprint)blueprint).HasCloak;
											((NWN2CreatureBlueprint)blueprint).HasGloves;
											((NWN2CreatureBlueprint)blueprint).HasHelm;

											((NWN2CreatureBlueprint)blueprint).NeverDrawHelmet;
											((NWN2CreatureBlueprint)blueprint).NeverShowArmor;

											// TwoDAReference ->
											((NWN2CreatureBlueprint)blueprint).Tail;
											((NWN2CreatureBlueprint)blueprint).Wings;

											// OEITintSet ->
											((NWN2CreatureBlueprint)blueprint).BaseTint;
											((NWN2CreatureBlueprint)blueprint).Tint;
											((NWN2CreatureBlueprint)blueprint).TintHair;
											((NWN2CreatureBlueprint)blueprint).TintHead;

											// Color ->
											((NWN2CreatureBlueprint)blueprint).TintArmor1;
											((NWN2CreatureBlueprint)blueprint).TintArmor2;
											((NWN2CreatureBlueprint)blueprint).TintEyes;
											((NWN2CreatureBlueprint)blueprint).TintFacialHair;
											((NWN2CreatureBlueprint)blueprint).TintHair1;
											((NWN2CreatureBlueprint)blueprint).TintHair2;
											((NWN2CreatureBlueprint)blueprint).TintHairAccessory;
											((NWN2CreatureBlueprint)blueprint).TintSkin;
*/

											goto case NWN2ObjectType.Item;

										case NWN2ObjectType.Door:
										case NWN2ObjectType.Placeable:
										case NWN2ObjectType.PlacedEffect:
										case NWN2ObjectType.Item: // <- TODO: works for weapons (see Preview tab) but clothes appear on a default creature (in the ArmorSet tab)
											Instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(Blueprint);
											_f.Changed = CreVisF.ChangedType.ct_not;
											_f.bu_creature_apply.Text = "APPLY to Blueprint";
											break;
									}
									break;
							}
						}
					}
					AddModel(different);
				}
			}
		}

		/// <summary>
		/// - based on
		/// NWN2Toolset.NWN2.Data.Blueprints.NWN2CreatureBlueprint.CreateFromBlueprint()
		/// </summary>
		/// <param name="iblueprint"></param>
		/// <returns></returns>
		INWN2Blueprint CreateBlueprint(INWN2Blueprint iblueprint)
		{
			// cf Io.CreateBlueprint()

			var current = (iblueprint as NWN2CreatureBlueprint);

			var blueprint = new NWN2CreatureBlueprint();
			blueprint.CopyFromTemplate(current);

			// 'data' is private ->
			// NWN2Toolset.NWN2.Data.Blueprints.NWN2CreatureBlueprint.NWN2BlueprintData -> (OEIResRef)TemplateResRef (+ load and save functs)
//			blueprint.data = (NWN2BlueprintData)CommonUtils.SerializationClone(current.data);

			blueprint.TemplateResRef = current.TemplateResRef; // not sure how that's gonna play out.


			blueprint.Comment = current.Comment;

			// TODO: if (prefs.HandleEquippedItems)
			blueprint.EquippedItems = (NWN2EquipmentSlotCollection)CommonUtils.SerializationClone(current.EquippedItems);

			// TODO: if (prefs.HandleInventoryItems)
			blueprint.Inventory = (NWN2BlueprintInventoryItemCollection)CommonUtils.SerializationClone(current.Inventory);


			OEIResRef resref = null;
			IResourceRepository repo = null;
			// Theory #187: if 'IResourceRepository' derives to 'ResourceRepository'
			// then the blueprint is a stock blueprint in the data/.zip files; but
			// if 'IResourceRepository' can be further derived to 'DirectoryResourceRepository'
			// then the blueprint is loose in a directory ... eg, the Module, Campaign,
			// or Override folder.

			if (current.Resource != null && current.Resource.Repository != null)
			{
				resref = current.Resource.ResRef; // 'Resource.Resref' IS 'ResourceName'
				repo = current.Resource.Repository;
			}
			else if (!String.IsNullOrEmpty(current.Name)) // should never happen.
			{
				// use tag as resref value and Module as the repository
				resref = new OEIResRef(current.Name);

				// module dir ->
				repo = NWN2ToolsetMainForm.App.BlueprintView.Module.Repository;

				// override dir ->
//				repo = NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.UserOverrideDirectory;
//				if (repo == null)
//					repo = NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.OverrideDirectory;

				// campaign dir ->
//				repo = NWN2Toolset.NWN2.Data.Campaign.NWN2CampaignManager.Instance.ActiveCampaign.Repository;
			}

			if (resref != null && repo != null) // should always happen.
				blueprint.Resource = repo.CreateResource(resref, current.ResourceType);

			return blueprint;
		}

		internal void RecreateModel()
		{
			Instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(Blueprint);
			AddModel();
		}

		/// <summary>
		/// Adds a model-instance to the scene.
		/// </summary>
		/// <param name="different"></param>
		void AddModel(bool different = false)
		{
			if (Instance != null && Initialize())
			{
				bool first;
				if (Model != null) // is NOT 'first' display - cache the previous model's telemetry since it's about to go byebye.
				{
					first = false;

					_pos = Model.Position;

					// NOTE: RotateObject() won't change the object's Orientation value;
					// Orientation needs to be updated explicitly if rotation is changed.
					_rot = Model.Orientation;

					// NOTE: SetObjectScale() won't change the object's Scale value;
					// Scale needs to be updated explicitly if x/y/z-scale is changed.
					_sca = Model.Scale;
				}
				else
					first = true;


				Instance.BeginAppearanceUpdate();

// create Model ->
				Model = NWN2NetDisplayManager.Instance.CreateNDOForInstance(Instance, Scene, 0); // 0=NetDisplayModel

				_f.PrintOriginalScale(Model.Scale.X.ToString("N2"));

				ScaInitial = Model.Scale;	// NOTE: Scale comes from the creature blueprint/instance/template/whatver.
											// That is, there's no default parameter for scale in this Scene like
											// there is for position and rotation.

// set Model position ->
				var objects = new NetDisplayObjectCollection() { Model }; // can't move a single object - only a collection (of 1).
				NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, _pos);

				Vector3 scale; // don't ask. It works unlike 'Object.Scale'.
				if (first)
				{
					Model.Orientation = RHQuaternion.RotationZ(MODEL_START_ROT);
					scale = ScaInitial;

//					var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;
//					state.FocusTheta = (float)Math.PI /  2F;
//					state.FocusPhi   = (float)Math.PI / 32F;
//					state.Distance   = 4.5F;

					Receiver.CameraAngleXY = CAM_START_TET; // FocusTheta - revolutions 0=east, lookin' west
					Receiver.CameraAngleYZ = CAM_START_PHI; // FocusPhi   - pitch 0= flat, inc to pitch forward and raise camera
					Receiver.Distance = CAM_START_DIST;
					Receiver.DistanceMin = 0.001F;

//					Receiver.FocusPoint = Object.Position + OFF_Zd;
//					Receiver.PitchMin = -(float)Math.PI / 2f;// + 0.145F;
//					Receiver.PitchMax =  (float)Math.PI / 2f - 0.010F;

					CAM_START_POS = CameraPosition;
					CameraPosition += CAM_BASEHEIGHT;
					_f.PrintCameraPosition();


//					float yaw = 0F, pitch = 0F, roll = 0F;
//					CameraOrientation.GetYawPitchRoll(out yaw, out pitch, out roll);
//					MessageBox.Show("yaw= " + yaw + " pitch= " + pitch + " roll= " + roll);
				}
				else
				{
					Model.Orientation = _rot;

					if (different) scale = ScaInitial;
					else           scale = _sca;
				}
				// else I'm gonna go bananas.

// set Model rotation ->
				NWN2NetDisplayManager.Instance.RotateObject(Model, ChangeType.Absolute, Model.Orientation);
				_f.PrintModelPosition(Model);

				Instance.EndAppearanceUpdate();

// set Model scale ->
				Model.Scale = scale; // NOTE: after EndAppearanceUpdate().
				NWN2NetDisplayManager.Instance.SetObjectScale(Model, Model.Scale); // TODO: does this work
				ResetModel(ResetType.RESET_scale); // this is needed to reset placed instance scale
				_f.PrintModelScale();
			}
			else if (Blueprint == null && Scene != null) // clear the scene iff a placed instance was last loaded ->
			{
				ClearObjects();
				// TODO: disable Creature page
			}
		}

		void ClearObjects()
		{
			var objects = new NetDisplayObjectCollection();
			foreach (NetDisplayObject @object in Scene.Objects)
			{
				//OEIShared.NetDisplay.NetDisplayLightPoint
				//OEIShared.NetDisplay.NetDisplayModel
				objects.Add(@object);
			}
			NWN2NetDisplayManager.Instance.RemoveObjects(objects);
		}

		/// <summary>
		/// Moves the light by recreating it at a given position.
		/// @note Simply re-setting 'Light.Position' doesn't work since I can't
		/// find an update call for it.
		/// </summary>
		/// <param name="pos"></param>
		internal void MoveLight(Vector3 pos)
		{
			var objects = new NetDisplayObjectCollection() { Light }; // TODO: cache that
			NWN2NetDisplayManager.Instance.RemoveObjects(objects);
	
			_posLight = pos;
			CreateLight();
		}

/*		void ClearLight()
		{
			var objects = new NetDisplayObjectCollection();
			foreach (NetDisplayObject @object in Scene.Objects)
			{
//				if (@object.Tag == @object)
//				if ((@object as NetDisplayLight).GetDisplayType() == NetDisplayType.NETDISPLAY_TYPE_LIGHT_POINT)
				if ((@object as NetDisplayLight) != null)
					objects.Add(@object);
			}
			NWN2NetDisplayManager.Instance.RemoveObjects(objects);
		} */


//		NWN2NetDisplayManager.NWN2CreatureTemplate.AppearanceChanged;
//		internal void UpdateScene()
//		{
//			NWN2NetDisplayManager.Instance.UpdateAppearanceForInstance(_instance);
//		}
		#endregion Methods


		#region Methods (model)
		internal static Vector3 off_xpos = new Vector3( 0.1F, 0F, 0F);
		internal static Vector3 off_xneg = new Vector3(-0.1F, 0F, 0F);

		internal static Vector3 off_ypos = new Vector3(0F,  0.1F, 0F);
		internal static Vector3 off_yneg = new Vector3(0F, -0.1F, 0F);

		internal static Vector3 off_zpos = new Vector3(0F, 0F,  0.1F);
		internal static Vector3 off_zneg = new Vector3(0F, 0F, -0.1F);

		internal static float rotpos =  0.1F;
		internal static float rotneg = -0.1F;


		internal void MoveModel(Vector3 posrel)
		{
			var objects = new NetDisplayObjectCollection() { Model }; // TODO: cache that
			NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Relative, false, posrel);
			_f.PrintModelPosition(Model);
		}

		internal void RotateModel(float f)
		{
			RHQuaternion rotate = RHQuaternion.RotationZ(f);
			NWN2NetDisplayManager.Instance.RotateObject(Model, ChangeType.Relative, rotate);

			Model.Orientation = RHQuaternion.Multiply(Model.Orientation, rotate);
			_f.PrintModelPosition(Model);
		}

		internal void ScaleModel(Vector3 vec)
		{
			NWN2NetDisplayManager.Instance.SetObjectScale(Model, (Model.Scale += vec));
			_f.PrintModelScale();
		}

		internal void ScaleModel(int dir)
		{
			var vec = _f.grader(new Vector3(0.1F, 0.1F, 0.1F));
			switch (dir)
			{
				case +1: Model.Scale += vec; break;
				case -1: Model.Scale -= vec; break;
			}

			NWN2NetDisplayManager.Instance.SetObjectScale(Model, Model.Scale);
			_f.PrintModelScale();
		}

		internal void ResetModel()
		{
			var objects = new NetDisplayObjectCollection() { Model }; // TODO: cache that
			NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, new Vector3());

			Model.Orientation = RHQuaternion.RotationZ(MODEL_START_ROT);
			NWN2NetDisplayManager.Instance.RotateObject(Model, ChangeType.Absolute, Model.Orientation);
			_f.PrintModelPosition(Model);

			Model.Scale = ScaInitial;
			NWN2NetDisplayManager.Instance.SetObjectScale(Model, Model.Scale);
			_f.PrintModelScale();
		}

		internal void ResetModel(ResetType reset)
		{
			switch (reset)
			{
				case ResetType.RESET_z:
				{
					var pos = new Vector3();
					pos.X = Model.Position.X;
					pos.Y = Model.Position.Y;
					pos.Z = 0;

					var objects = new NetDisplayObjectCollection() { Model }; // TODO: cache that
					NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, pos);
					_f.PrintModelPosition(Model);
					break;
				}

				case ResetType.RESET_xy:
				{
					var pos = new Vector3();
					pos.X = 0;
					pos.Y = 0;
					pos.Z = Model.Position.Z;

					var objects = new NetDisplayObjectCollection() { Model }; // TODO: cache that
					NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, pos);
					_f.PrintModelPosition(Model);
					break;
				}

				case ResetType.RESET_rot:
					Model.Orientation = RHQuaternion.RotationZ(MODEL_START_ROT);
					NWN2NetDisplayManager.Instance.RotateObject(Model, ChangeType.Absolute, Model.Orientation);
					_f.PrintModelPosition(Model);
					break;

				case ResetType.RESET_scale:
					Model.Scale = ScaInitial;
					NWN2NetDisplayManager.Instance.SetObjectScale(Model, Model.Scale);
					_f.PrintModelScale();
					break;
			}
		}
		#endregion Methods (model)


		#region Methods (camera)
		internal void RaiseCameraPolar()
		{
			var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;
			state.FocusPhi += _f.grader(0.1F);
			state.FocusPhi = Math.Min(state.PitchMax, state.FocusPhi);
			state.FocusPhi = Math.Max(state.PitchMin, state.FocusPhi);

			UpdateCamera();
		}

		internal void LowerCameraPolar()
		{
			var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;

			state.FocusPhi -= _f.grader(0.1F);
			state.FocusPhi = Math.Min(state.PitchMax, state.FocusPhi);
			state.FocusPhi = Math.Max(state.PitchMin, state.FocusPhi);

			UpdateCamera();
		}

		internal void UpdateCamera()
		{
			var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;

// position ->
			var y = new Vector3(0F,1F,0F);
			y = RHMatrix.RotationZ(state.FocusTheta).TransformCoordinate(y);

			var z = new Vector3(0F,0F,1F);
			z = RHMatrix.RotationZ(state.FocusTheta)    .TransformCoordinate(z);
			z = RHMatrix.RotationAxis(y, state.FocusPhi).TransformCoordinate(z);

			var pos = new Vector3(1F,0F,0F);
			pos = RHMatrix.RotationZ(state.FocusTheta)    .TransformCoordinate(pos);
			pos = RHMatrix.RotationAxis(y, state.FocusPhi).TransformCoordinate(pos);

			pos.Scale(state.Distance);
			pos += state.FocusPoint;

			CameraPosition = pos + CreVisF.Offset + CAM_BASEHEIGHT;

// orientation ->
			Vector3 focusPoint = state.FocusPoint;
			focusPoint.Subtract(pos);
			focusPoint = MathUtils.NormalizeVector3(focusPoint);

			CameraOrientation = RHQuaternion.RotationMatrix(RHMatrix.LookAtRH(Vector3.Empty, focusPoint, z));
		}
		#endregion Methods (camera)


		#region Handlers (mouse)
		void mousedown(object sender, MouseEventArgs e)
		{
			_btn = e.Button;
			_p0 = _p = PointToClient(Cursor.Position);
			_t2.Start();
		}

		void mouseup(object sender, MouseEventArgs e)
		{
			_t2.Stop();
			_btn = MouseButtons.None;
			Cursor.Current = Cursors.Default;
		}


		// as in the ArmorSet tab ->
		// LMB      : select model (or weapon/shield)
		// RMB+Ctrl : revolve around modal/focuspt (polar)
		// RMB      : +/- z, +- x/y (orthagonal on the x/y-plane)
		// LMB+Alt  : +/- model z-axis iff model is selected
		//
		// coded below ->
		// LMB+Alt  : +/- camera z-axis
		// LMB+Ctrl : +/- camera x/y-plane
		// RMB      : +/- camera z, x/y
		// RMB+Ctrl : full polar

		MouseButtons _btn;

		Timer _t2 = new Timer();
		Point _p, _p0;

		/// <summary>
		/// Tracks mousecursor location for either LMB or RMB drag motions.
		/// Changes the camera's position and/or orientation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tick(object sender, EventArgs e)
		{
			if (_btn != MouseButtons.None)
			{
				switch (_btn)
				{
					case MouseButtons.Right:
						switch (Control.ModifierKeys)
						{
							case Keys.Control: Cursor.Current = Cursors.Cross;   break;
							case Keys.None:    Cursor.Current = Cursors.SizeAll; break;
						}
						break;

					case MouseButtons.Left:
						switch (Control.ModifierKeys)
						{
							case Keys.Alt:     Cursor.Current = Cursors.SizeNS; break;
							case Keys.Control: Cursor.Current = Cursors.SizeWE; break;
						}
						break;
				}

				if (_p != _p0)
				{
					switch (_btn)
					{
						case MouseButtons.Right:
							switch (Control.ModifierKeys)
							{
								case Keys.Control: // full-polar movement around model/focuspt ->
								{
									int deltahori = _p.X - _p0.X;
									int deltavert = _p.Y - _p0.Y;

									float hori = (float)deltahori * 0.01f;
									float vert = (float)deltavert * 0.01f;

									var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;
									state.FocusTheta += hori;
									state.FocusPhi   += vert;

									while ((double)state.FocusTheta < -Math.PI)
										state.FocusTheta += (float)Math.PI * 2f;

									while ((double)state.FocusTheta > Math.PI)
										state.FocusTheta -= (float)Math.PI * 2f;

									state.FocusPhi = Math.Min(state.PitchMax, state.FocusPhi);
									state.FocusPhi = Math.Max(state.PitchMin, state.FocusPhi);

									UpdateCamera();
									_f.PrintCameraPosition();
									break;
								}

								case Keys.None: // up/down, left/right
								{
									// cf. LMB+Ctrl
									// vertical shift ->
									float z = (float)(_p.Y - _p0.Y) * 0.01F;

									// horizontal shifts ->
									float rot = _f.getrot();
									rot *= (float)Math.PI / 180F; // to rads

									float cos = (float)(Math.Cos((double)rot));
									float x = (float)(_p0.X - _p.X) * 0.01F * cos;

									float sin = -(float)(Math.Sin((double)rot));
									float y = (float)(_p0.X - _p.X) * 0.01F * sin;

									var shift = new Vector3(x,y,z);
									CameraPosition += shift;
									CreVisF.Offset += shift;

									_f.PrintCameraPosition();
									break;
								}
							}
							break;

						case MouseButtons.Left:
							switch (Control.ModifierKeys)
							{
								case Keys.Alt: // z-axis +/-
								{
									float z = (float)(_p.Y - _p0.Y) * 0.01F;

									var shift = new Vector3(0F, 0F, z);
									CameraPosition += shift;
									CreVisF.Offset += shift;
									_f.PrintCameraPosition();
									break;
								}

								case Keys.Control: // x/y-plane shift.
								{
									// cf. RMB
									float rot = _f.getrot();
									rot *= (float)Math.PI / 180F; // to rads

									float cos = (float)(Math.Cos((double)rot));
									float x = (float)(_p0.X - _p.X) * 0.01F * cos;

									float sin = -(float)(Math.Sin((double)rot));
									float y = (float)(_p0.X - _p.X) * 0.01F * sin;

									var shift = new Vector3(x,y, 0F);
									CameraPosition += shift;
									CreVisF.Offset += shift;

									_f.PrintCameraPosition();
									break;
								}
							}
							break;
					}
				}
			}

			_p0 = _p;
			_p = PointToClient(Cursor.Position);
		}


		/// <summary>
		/// Zooms the camera in/out ... overrides the stock mousewheel handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if      (e.Delta > 0) _f.click_bu_camera_distneg(null, EventArgs.Empty);
			else if (e.Delta < 0) _f.click_bu_camera_distpos(null, EventArgs.Empty);
		}
		#endregion Handlers (mouse)
	}
}
