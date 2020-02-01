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
	/// Etc etc etc.
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
		static Vector3 DEF_SUNMOON_DIRECTION = new Vector3(-0.33F, -0.67F, -0.67F);
		const float DEF_SHADOW_INTENSITY = 0F;

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

				_f.EnableSaveToModule(valid);
				_f.EnableSaveToCampaign(valid);
				_f.EnableSaveToFile(valid);
			}
		}


		readonly NetDisplayObjectCollection _objects = new NetDisplayObjectCollection();
		NetDisplayObjectCollection _objectsModel;
		NetDisplayObjectCollection _objectsLight;

		internal NetDisplayObject Model
		{ get; private set; }

		internal NetDisplayLightPoint Light
		{ get; private set; }

		internal ModelViewerInputCameraReceiver Receiver
		{ get; private set; }
		#endregion Properties


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
		/// Creates an instance of a blueprint and tries to render it in the
		/// ElectronPanel.
		/// </summary>
		internal void CreateModel()
		{
			//MessageBox.Show("CreateModel()");
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
//				_f._bypassInsert = true; // prevent anything in here from firing CreVisF.OnObjectsInserted()
				_f._bypassAppearanceChanged = true; // prevent anything in here from firing CreVisF.OnAppearanceChanged()

//				if (Instance != null)
//				{
//					// ask to ignore, Apply (if not stock resource), or save-to-file (disable the Cancel option)
//					_f.ConfirmClose(false);
//				}

				StoreTelemetry();

				lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot) // not sure ...
				{
					NWN2NetDisplayManager.Instance.RemoveObjects(_objects);
				}
				_objects.Clear();


				_f.ClearResourceInfo();
//				_f.Changed = CreVisF.ChangedType.ct_nul; // set '_f.Text'

				Model     = null;
				Blueprint = null; // is instantiated only by a Blueprint
				Instance  = null; // is instantiated by either a Blueprint or an Instance

//				_f.bu_creature_apply1.Enabled =
//				_f.bu_creature_apply2.Enabled = false;
//				_f.EnableCreaturePages(false);

				if (MousePanel != null && !MousePanel.IsDisposed) // safety. ElectronPanel.MousePanel could go disposed for no good reason.
				{
					bool different = false;

// first check areaviewer for a selected Instance ->
					NWN2AreaViewer areaviewer;
					NWN2InstanceCollection collection;
					if ((areaviewer = NWN2ToolsetMainForm.App.GetActiveViewer() as NWN2AreaViewer) != null
						&& (collection = areaviewer.SelectedInstances) != null && collection.Count == 1
						&& (   collection[0] is NWN2CreatureTemplate
							|| collection[0] is NWN2DoorTemplate
							|| collection[0] is NWN2PlaceableTemplate))
					{
						if (Instance_base == null || (collection[0] as INWN2Instance) != Instance_base)
						{
							Instance_base = collection[0] as INWN2Instance;
							different = true;
						}

						// NOTE: Instances without any value for "Template" have a null template ResourceEntry
						// while Instances with an invalid value for "Template" are ResourceType 0 .RES

						Instance = CommonUtils.SerializationClone(Instance_base) as INWN2Instance;
						Instance.Area = Instance_base.Area; // req'd.
//						string info = String.Empty;
//						info += "area= "           + (Instance.Area != null ? Instance.Area.Name + " (" + Instance.Area.Tag + ")" : "null");
//						info += "\ncomment= "      +  Instance.Comment;
//						info += "\nobjectId= "     +  Instance.ObjectID;
//						info += "\nresourcetype= " + (Instance.Template != null ? Instance.Template.ResourceType.ToString() : "null");
//						info += "\nfullname= "     + (Instance.Template != null ? Instance.Template.FullName : "null");
//						info += "\nresref= "       + (Instance.Template.ResRef != null ? Instance.Template.ResRef.Value : "null");
//						info += "\nrepo= "         + (Instance.Template.Repository != null ? Instance.Template.Repository.Name : "null");
//						MessageBox.Show(info);


						if ((Instance as INWN2Template).ObjectType == NWN2ObjectType.Creature)
						{
							ProcessEquippedItems(Instance as NWN2CreatureTemplate);
							ProcessInventory(Instance as INWN2Template);
						}

						_f.PrintResourceInfo(Instance);


/*						Instance = CreateInstance();
						if (Instance != null)
						{
							_f.PrintResourceInfo(Instance);


//							if (Instance.ObjectType == NWN2ObjectType.Creature)
//							{
//								_f.bu_creature_apply1.Enabled =
//								_f.bu_creature_apply2.Enabled = Instance.Template != null;		// NOTE: 'Template' should actually be 'Resource' or 'TemplateResource'
//								_f.EnableCreaturePages(true);									// like ya know 'Blueprint.Resource' is ... since it's not a 'Template'
//																								// it's a 'ResourceEntry'. 'Template' has a distinct meaning and it's
//								_f.InitializeCreaturePages(Instance as NWN2CreatureTemplate);	// not 'ResourceEntry'.
//							}
						} */

//						_f.bu_creature_apply1.Text =
//						_f.bu_creature_apply2.Text = "APPLY to Instance";
					}
// second check blueprint tree for a selected Blueprint ->
					else
					{
						NWN2BlueprintView blueprintview = NWN2ToolsetMainForm.App.BlueprintView;

						object[] selection = blueprintview.Selection;
						if (selection != null && selection.Length == 1)
						{
							switch ((selection[0] as INWN2Template).ObjectType) // better not be null
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

									DuplicateBlueprint();
									_f.PrintResourceInfo(Blueprint);

//									var resource0 = Blueprint.Resource;
//									IResourceRepository repo = NWN2ToolsetMainForm.App.Module.Repository;
//									var resource1 = repo.FindResource(new OEIResRef(Blueprint.Resource.ResRef.Value), 2027);
//									MessageBox.Show("is resource0 valid= " + (resource0 != null) + "\n"
//									                + "is resource1 valid= " + (resource1 != null) + "\n"
//									                + "is resource0 resource1= " + (resource0 == resource1) + "\n"
//									                + "is resource0 equal to resource1= " + resource0.Equals(resource1));

									Instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(Blueprint);

/*									switch (type)
									{
										case NWN2ObjectType.Creature:
//											_f.bu_creature_apply1.Enabled =
//											_f.bu_creature_apply2.Enabled = (Blueprint.Resource.Repository as DirectoryResourceRepository) != null;
//											_f.EnableCreaturePages(true);
//											_f.InitializeCreaturePages(Blueprint as NWN2CreatureTemplate);

//											((NWN2CreatureBlueprint)blueprint).AppearanceHair; // byte
//											// etc ...
//
//											// bool ->
//											((NWN2CreatureBlueprint)blueprint).AppearanceFacialHair;
//
//											((NWN2CreatureBlueprint)blueprint).HasBelt;
//											((NWN2CreatureBlueprint)blueprint).HasBoots;
//											((NWN2CreatureBlueprint)blueprint).HasCloak;
//											((NWN2CreatureBlueprint)blueprint).HasGloves;
//											((NWN2CreatureBlueprint)blueprint).HasHelm;
//
//											((NWN2CreatureBlueprint)blueprint).NeverDrawHelmet;
//											((NWN2CreatureBlueprint)blueprint).NeverShowArmor;
//
//											// TwoDAReference ->
//											((NWN2CreatureBlueprint)blueprint).Tail;
//											((NWN2CreatureBlueprint)blueprint).Wings;
//
//											// OEITintSet ->
//											((NWN2CreatureBlueprint)blueprint).BaseTint;
//											((NWN2CreatureBlueprint)blueprint).Tint;
//											((NWN2CreatureBlueprint)blueprint).TintHair;
//											((NWN2CreatureBlueprint)blueprint).TintHead;
//
//											// Color ->
//											((NWN2CreatureBlueprint)blueprint).TintArmor1;
//											((NWN2CreatureBlueprint)blueprint).TintArmor2;
//											((NWN2CreatureBlueprint)blueprint).TintEyes;
//											((NWN2CreatureBlueprint)blueprint).TintFacialHair;
//											((NWN2CreatureBlueprint)blueprint).TintHair1;
//											((NWN2CreatureBlueprint)blueprint).TintHair2;
//											((NWN2CreatureBlueprint)blueprint).TintHairAccessory;
//											((NWN2CreatureBlueprint)blueprint).TintSkin;

											goto case NWN2ObjectType.Item;

										case NWN2ObjectType.Door:
										case NWN2ObjectType.Placeable:
										case NWN2ObjectType.PlacedEffect:
										case NWN2ObjectType.Item: // <- TODO: works for weapons (see Preview tab) but clothes appear on a default creature (in the ArmorSet tab)
											Instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(Blueprint);
											break;
									} */
									break;
							}
						}

//						_f.bu_creature_apply1.Text =
//						_f.bu_creature_apply2.Text = "APPLY to Blueprint";
					}

					AddModel(different);
				}

//				_f._bypassInsert = false;
				_f._bypassAppearanceChanged = false;
			}

			_f.SetTitleText();
		}

		/// <summary>
		/// Clones the toolset blueprint to 'Blueprint' ... and creates a valid
		/// IResourceEntry for it.
		/// TODO: A new resource doesn't have to be created here, but only if
		/// the Blueprint/Instance is saved to a file that doesn't already have
		/// a resource.
		/// - based on
		/// NWN2Toolset.NWN2.Data.Blueprints.NWN2CreatureBlueprint.CreateFromBlueprint(NWN2CreatureBlueprint, IResourceRepository, bool)
		/// - see also
		/// NWN2Toolset.NWN2.Data.Blueprints.NWN2GlobalBlueprintManager.CreateCopyOfBlueprint(INWN2Blueprint, INWN2BlueprintSet, IResourceRepository, bool)
		/// </summary>
		void DuplicateBlueprint() // attempt at cloning the toolset blueprint to 'Blueprint' ... and creating a valid Resource for it
		{
			switch (Blueprint_base.ObjectType)
			{
				case NWN2ObjectType.Creature:     Blueprint = new NWN2CreatureBlueprint();     break;
				case NWN2ObjectType.Door:         Blueprint = new NWN2DoorBlueprint();         break;
				case NWN2ObjectType.Placeable:    Blueprint = new NWN2PlaceableBlueprint();    break;
				case NWN2ObjectType.PlacedEffect: Blueprint = new NWN2PlacedEffectBlueprint(); break;
				case NWN2ObjectType.Item:         Blueprint = new NWN2ItemBlueprint();         break;
			}

			(Blueprint as INWN2Template).CopyFromTemplate(Blueprint_base as INWN2Template);

//			OEIResRef resref = Blueprint_base.Resource.ResRef; // 'Resource.Resref' IS 'ResourceName'
//			IResourceRepository repo = Blueprint_base.Resource.Repository;
//			Blueprint.Resource = repo.CreateResource(resref, (Blueprint_base as INWN2Object).ResourceType);

			//this.ᐂ = (NWN2ObjectTemplateData)CommonUtils.SerializationClone(nWN2CreatureTemplate.ᐂ);
			Blueprint.BlueprintLocation = Blueprint_base.BlueprintLocation; // note: enum 'NWN2BlueprintLocationType' ought be value-type
			Blueprint.Resource          = (IResourceEntry)CommonUtils.SerializationClone(Blueprint_base.Resource);
			Blueprint.TemplateResRef    = (OEIResRef)CommonUtils.SerializationClone(Blueprint_base.TemplateResRef);
			Blueprint.Comment           = String.Copy(Blueprint_base.Comment); // note: strings are immutable

			if ((Blueprint as INWN2Template).ObjectType == NWN2ObjectType.Creature)
			{
				ProcessEquippedItems(Blueprint as NWN2CreatureTemplate);
				ProcessInventory(Blueprint as INWN2Template);
			}
		}

/*		INWN2Instance CreateInstance()
		{
			// NOTE: Instances without any value for "Template" have a null template ResourceEntry
			// while Instances with an invalid value for "Template" are ResourceType 0 .RES

			if (Instance_base.Template == null // TODO: allow Instances w/out a valid Template ... (although it should be discouraged)
				|| (   Instance_base.Template.ResourceType != (ushort)2027		// utc
					&& Instance_base.Template.ResourceType != (ushort)2042		// utd
					&& Instance_base.Template.ResourceType != (ushort)2044))	// utp
			{
				CreVisF.BypassCreate = true;

				using (var f = new ErrorF("The instance's Template is invalid."))
					f.ShowDialog();

				CreVisF.BypassCreate = false;
			}
			else
			{
				var iinstance = CommonUtils.SerializationClone(Instance_base) as INWN2Instance;
				iinstance.Area = Instance_base.Area;

				if (iinstance.ObjectType == NWN2ObjectType.Creature)
				{
					ProcessEquippedItems(iinstance as NWN2CreatureTemplate);
					ProcessInventory(iinstance as INWN2Template);
				}

				return iinstance;
			}
			return null;
		} */

		void ProcessEquippedItems(NWN2CreatureTemplate template)
		{
			for (int i = 0; i != template.EquippedItems.Count; ++i)
			{
				switch (template.EquippedItems[i].Slot)
				{
					case "Head": // TODO: figure out how to use legit enums for these ->
					case "Chest":
					case "Boots":
					case "Arms":
					case "Cloak":
					case "Left Ring":
					case "Right Ring":
					case "Neck":
					case "Belt":
					case "Arrows":
					case "Bullets":
					case "Bolts":
					case "Creature Weapon 1":
					case "Creature Weapon 2":
					case "Creature Weapon 3":
					case "Creature Hide":
						if (!CreatureVisualizerPreferences.that.ProcessEquipped_body)
						{
							template.EquippedItems[i].Item = null;
						}
						break;

					case "Right Hand":
					case "Left Hand":
						if (!CreatureVisualizerPreferences.that.ProcessEquipped_held)
						{
							template.EquippedItems[i].Item = null;
						}
						break;
				}
			}
		}

		void ProcessInventory(INWN2Template itemplate)
		{
			if (CreatureVisualizerPreferences.that.ProcessInventory)
			{
				if ((itemplate as NWN2CreatureBlueprint) != null)
				{
					object inventory = CommonUtils.SerializationClone((Blueprint_base as NWN2CreatureBlueprint).Inventory);
					(itemplate as NWN2CreatureBlueprint).Inventory = inventory as NWN2BlueprintInventoryItemCollection;
				}
				else if ((itemplate as NWN2CreatureInstance) != null)
				{
					object inventory = CommonUtils.SerializationClone((Instance_base as NWN2CreatureInstance).Inventory);
					(itemplate as NWN2CreatureInstance).Inventory = inventory as NWN2InstanceInventoryItemCollection;
				}
			}
			else
			{
				if ((itemplate as NWN2CreatureBlueprint) != null)
				{
					(itemplate as NWN2CreatureBlueprint).Inventory.Clear();
				}
				else if ((itemplate as NWN2CreatureInstance) != null)
				{
					(itemplate as NWN2CreatureInstance).Inventory.Clear();
				}
			}
		}


/*		/// <summary>
		/// Updates the NetDisplayModel after changing creature characteristics.
		/// </summary>
		internal void UpdateModel()
		{
			//MessageBox.Show("RecreateModel()");
			if (Blueprint != null)
			{
				Instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(Blueprint); // do you really want to trust that
				AddModel();
//				NWN2NetDisplayManager.Instance.HandleAppearanceChange();
//				NWN2NetDisplayManager.Instance.UpdateAppearanceForCreatureInventory();
//				NWN2NetDisplayManager.Instance.UpdateAppearanceForInstance();
//				NWN2NetDisplayManager.Instance.Update();
			}
			else if (Instance != null)
			{
				AddModel();
			}
		} */


		bool _inited;
// Camera ->
		float _xy   = CAM_START_TET;
		float _yz   = CAM_START_PHI;
		float _dist = CAM_START_DIST;
// Model ->
		Vector3      _pos = new Vector3(0F,0F,0F);						// position of the Model
		RHQuaternion _rot = RHQuaternion.RotationZ(MODEL_START_ROT);	// rotation of the Model
		Vector3      _sca;												// scale    of the Model // no default, usually 1,1,1 but each creature has its own set in Appearances.2da
// Light ->
		Vector3 _posLight = LIGHT_START_POS;

		/// <summary>
		/// Stores the previous state of this panel, before another Model loads,
		/// so that the scene can be created as before, after the Model loads.
		/// </summary>
		void StoreTelemetry()
		{
			if (_inited)
			{
				if (Receiver != null)
				{
					_xy   = Receiver.CameraAngleXY;
					_yz   = Receiver.CameraAngleYZ;
					_dist = Receiver.Distance;
				}

				if (Model != null)
				{
					_pos = Model.Position;
					_rot = Model.Orientation;
					_sca = Model.Scale;
				}

				if (Light != null)
				{
					_posLight = Light.Position;
				}
			}
		}


		/// <summary>
		/// Adds a model-instance to the scene.
		/// </summary>
		/// <param name="different"></param>
		void AddModel(bool different)
		{
			//MessageBox.Show("AddModel()");
			if (Instance != null && Initialize())
			{
//				_f.Changed = CreVisF.ChangedType.ct_non;

				Receiver.CameraAngleXY = _xy;
				Receiver.CameraAngleYZ = _yz;
				Receiver.Distance      = _dist;

				if (!_inited)
				{
					_inited = true;
					Receiver.DistanceMin = 0.001F;
					CAM_START_POS = CameraPosition;
				}

				CameraPosition += CAM_BASEHEIGHT + CreVisF.Offset;
				_f.PrintCameraPosition();


				Instance.BeginAppearanceUpdate();

// create Model ->
				Model = NWN2NetDisplayManager.Instance.CreateNDOForInstance(Instance, Scene, 0); // 0=NetDisplayModel
				_objects.Add(Model);

// set Model position ->
				_objectsModel = new NetDisplayObjectCollection() { Model }; // can't move a single object - only a collection (of 1).
				NWN2NetDisplayManager.Instance.MoveObjects(_objectsModel, ChangeType.Absolute, false, _pos);

// set Model rotation ->
				Model.Orientation = _rot;
				NWN2NetDisplayManager.Instance.RotateObject(Model, ChangeType.Absolute, Model.Orientation);
				_f.PrintModelPosition(Model);

				Instance.EndAppearanceUpdate();

// set Model scale ->
				_f.PrintOriginalScale(Model.Scale.X.ToString("N2"));

				ScaInitial = Model.Scale;	// NOTE: Scale comes from the creature blueprint/instance/template/whatver.
											// That is, there's no default parameter for scale in this Scene like
											// there is for position and rotation.

				Vector3 scale;
				if (different) scale = ScaInitial;
				else           scale = _sca;

				Model.Scale = scale; // NOTE: after EndAppearanceUpdate().
				NWN2NetDisplayManager.Instance.SetObjectScale(Model, Model.Scale);
				ResetModel(ResetType.RESET_scale); // this is needed to reset placed instance scale
				_f.PrintModelScale();
			}
		}
//					var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;
//					state.FocusTheta = (float)Math.PI /  2F;
//					state.FocusPhi   = (float)Math.PI / 32F;
//					state.Distance   = 4.5F;

//					Receiver.FocusPoint = Object.Position + OFF_Zd;
//					Receiver.PitchMin = -(float)Math.PI / 2f;// + 0.145F;
//					Receiver.PitchMax =  (float)Math.PI / 2f - 0.010F;

//					float yaw = 0F, pitch = 0F, roll = 0F;
//					CameraOrientation.GetYawPitchRoll(out yaw, out pitch, out roll);
//					MessageBox.Show("yaw= " + yaw + " pitch= " + pitch + " roll= " + roll);


		/// <summary>
		/// Initializes the scene. Clears any objects, sets up default lighting,
		/// and adds a lightpoint.
		/// </summary>
		bool Initialize()
		{
			//MessageBox.Show("Initialize()");
			if (_f.WindowState != FormWindowState.Minimized)
			{
				// NOTE: CloseWindow() wipes out the Scene along with its Objects.
				// But does it clear 'NWN2NetDisplayManager.Instance.Objects' correctly.
				CloseWindow(); // safety - try not to confuse the NWN2NetDisplayManager.Instance ... too late.

				if (NDWindow == null)
					OpenWindow();

				if (NDWindow != null && Scene != null)
				{
					if (Scene.DayNightCycleStages[(int)DayNightStageType.Default] != null)
					{
						Scene.DayNightCycleStages[(int)DayNightStageType.Default].SunMoonDirection = DEF_SUNMOON_DIRECTION;
						Scene.DayNightCycleStages[(int)DayNightStageType.Default].ShadowIntensity  = DEF_SHADOW_INTENSITY;
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
			//MessageBox.Show("CreateLight()");
			Light = new NetDisplayLightPoint();

			Light.Position        = _posLight;

			Light.Color.Intensity = CreatureVisualizerPreferences.that.LightIntensity;
			Light.Range           = LIGHT_START_RANGE;
			Light.CastsShadow     = false;

			Light.ID              = NetDisplayManager.Instance.NextObjectID;
			Light.Tag             = Light;


			if (ColorCheckedDiffuse)  Light.Color.DiffuseColor  = (Color)ColorDiffuse;
			if (ColorCheckedSpecular) Light.Color.SpecularColor = (Color)ColorSpecular;
			if (ColorCheckedAmbient)  Light.Color.AmbientColor  = (Color)ColorAmbient;


			lock (Scene.Objects.SyncRoot)
			{
				Scene.Objects.Add(Light);
			}
			lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)
			{
				NWN2NetDisplayManager.Instance.Objects.Add(Light);
			}

			NWN2NetDisplayManager.Instance.LightParameters(Scene, Light);
			_f.PrintLightPosition(Light.Position);


			_objects.Add(Light);
			_objectsLight = new NetDisplayObjectCollection() { Light };
		}

//		int iter;
		/// <summary>
		/// Moves the light-object.
		/// </summary>
		/// <param name="posabs"></param>
		internal void MoveLight(Vector3 posabs)
		{
//			if (++iter == 5)
//			{
//				var t = new System.Diagnostics.StackTrace();
//				System.IO.File.WriteAllText(@"C:\GIT\CreatureVisualizer\t\stacktrace.txt", t.ToString());
//				System.Diagnostics.Process.GetCurrentProcess().Kill();
//			}

			//MessageBox.Show("MoveLight()");
			NWN2NetDisplayManager.Instance.MoveObjects(_objectsLight, ChangeType.Absolute, false, posabs);
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
			NWN2NetDisplayManager.Instance.MoveObjects(_objectsModel, ChangeType.Relative, false, posrel);
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
			NWN2NetDisplayManager.Instance.MoveObjects(_objectsModel, ChangeType.Absolute, false, new Vector3());

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

					NWN2NetDisplayManager.Instance.MoveObjects(_objectsModel, ChangeType.Absolute, false, pos);
					_f.PrintModelPosition(Model);
					break;
				}

				case ResetType.RESET_xy:
				{
					var pos = new Vector3();
					pos.X = 0;
					pos.Y = 0;
					pos.Z = Model.Position.Z;

					NWN2NetDisplayManager.Instance.MoveObjects(_objectsModel, ChangeType.Absolute, false, pos);
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
