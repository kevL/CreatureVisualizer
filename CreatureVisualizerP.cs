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

using OEIShared.NetDisplay;
using OEIShared.OEIMath;
using OEIShared.UI;
using OEIShared.UI.Input;


namespace creaturevisualizer
{
	/// <summary>
	/// Reset values for the model-instance.
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
	sealed class CreatureVisualizerP
		: ElectronPanel
	{
		#region Fields (static)
		internal static Vector3 POS_START_CAMERA;

		internal static Vector3 POS_START_LIGHT = new Vector3(-0.5F, -4F, 2.0F);
		internal static Vector3 POS_OFF_Zd      = new Vector3( 0.0F,  0F, 1.5F); // base height

		internal const  float DIST_START = 5F;

		internal static Color ? ColorDiffuse;
		internal static Color ? ColorSpecular;
		internal static Color ? ColorAmbient;

		internal static bool ColorCheckedDiffuse;
		internal static bool ColorCheckedSpecular;
		internal static bool ColorCheckedAmbient;


		const float ROT_START_OBJECT = (float)Math.PI * 3F / 4F;

		static Vector3 ScaInitial;
		#endregion Fields (static)


		#region Fields
		readonly CreatureVisualizerF _f;

		INWN2Instance  _instance;
		INWN2Blueprint _blueprint0; // ref to previous blueprint-object (to track 'changed').

		bool _changed;
		bool _isplaced;

		Vector3      _pos_Instance;
		RHQuaternion _rot_Instance;
		Vector3      _sca_Instance;
		#endregion Fields


		#region Properties
		internal NetDisplayObject Object
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
		internal CreatureVisualizerP(CreatureVisualizerF f)
		{
			_f = f;

//			DoubleBuffered = true;

			RecreateMousePanel();

			NetDisplayScene scene = Scene;
			NetDisplayManager.Instance.AddScene(out scene);

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
		/// Disposes of their MousePanel and instantiates a CreatureVisualizer
		/// MousePanel to replace it. CreatureVisualizer-specific inputhandlers
		/// can be hooked up to this ElectronPanel that totally bypass the
		/// tangle of Obsidian inputhandlers. good luck
		/// </summary>
		void RecreateMousePanel()
		{
			MousePanel.Dispose(); // OMG it effin worked.
			MousePanel = new MousePanel();

			var resourcer = new ComponentResourceManager(typeof(ElectronPanel));
			resourcer.ApplyResources(MousePanel, "MousePanel");

			MousePanel.Name = "MousePanel";

			MousePanel.ThrottleMouse = true;

			CameraMovementReceiver = null;
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
		internal void CreateInstance()
		{
			if (!CreatureVisualizerF.BypassRefreshOnFocus)
			{
				if (MousePanel != null && !MousePanel.IsDisposed) // safety. ElectronPanel.MousePanel could go disposed for no good reason.
				{
					_instance = null;

					// TODO: print FirstName or resref to titlebar.

					NWN2AreaViewer viewer;
					NWN2InstanceCollection collection;
					NWN2CreatureInstance placed;

					//viewer.AreaNetDisplayWindow.Scene
					//viewer.SelectedNDOs

					if ((viewer = NWN2ToolsetMainForm.App.GetActiveViewer() as NWN2AreaViewer) != null
						&& (collection = viewer.SelectedInstances) != null && collection.Count == 1
						&& (placed = collection[0] as NWN2CreatureInstance) != null)
					{
						_instance = placed;
						_f.EnableCharacterPage(false);
						_isplaced = true;
					}
					else
					{
						_f.EnableCharacterPage(true);

						NWN2BlueprintView tslist = NWN2ToolsetMainForm.App.BlueprintView;

						object[] selection = tslist.Selection;
						if (selection != null && selection.Length == 1)
						{
							_isplaced = false;

							var blueprint = selection[0] as INWN2Blueprint;
							if (!blueprint.Equals(_blueprint0))
							{
								_blueprint0 = blueprint;
								_changed = true;
							}
							else
								_changed = false;

							switch (tslist.GetFocusedListObjectType())
							{
								case NWN2ObjectType.Creature:
									if (CreatureVisualizerPreferences.that.char_Female)
									{
										((NWN2CreatureBlueprint)blueprint).Gender = CreatureGender.Female;
									}
									else
										((NWN2CreatureBlueprint)blueprint).Gender = CreatureGender.Male;

/*	
//									((NWN2CreatureBlueprint)blueprint).AppearanceHair; // byte
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

								case NWN2ObjectType.Item:	// <- TODO: works for weapons (see Preview tab) but clothes
								{							//          appear on a default creature (in the ArmorSet tab)
									_instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(blueprint);
									break;
								}
							}
						}
					}
					CreateScene();
				}
				else
					MessageBox.Show(this, "ElectronPanel.MousePanel is invalid. Please see your chiropractor.");
			}
		}

//		NWN2NetDisplayManager.NWN2CreatureTemplate.AppearanceChanged;
//		internal void UpdateScene()
//		{
//			NWN2NetDisplayManager.Instance.UpdateAppearanceForInstance(_instance);
//		}


		/// <summary>
		/// Adds a model-instance to the scene.
		/// </summary>
		void CreateScene()
		{
			if (_instance != null && StartScene())
			{
				bool first;
				if (Object != null) // is NOT 'first' display - cache the previous model's telemetry since it's about to go byebye.
				{
					first = false;

					_pos_Instance = Object.Position;

					// NOTE: RotateObject() won't change the object's Orientation value;
					// Orientation needs to be updated explicitly if rotation is changed.
					_rot_Instance = Object.Orientation;

					// NOTE: SetObjectScale() won't change the object's Scale value;
					// Scale needs to be updated explicitly if x/y/z-scale is changed.
					_sca_Instance = Object.Scale;
				}
				else
					first = true;


				_instance.BeginAppearanceUpdate();

				// create display object ->
				Object = NWN2NetDisplayManager.Instance.CreateNDOForInstance(_instance, Scene, 0); // 0=NetDisplayModel

				Object.PositionChanged += positionchanged_Object;

				_f.PrintOriginalScale(Object.Scale.X.ToString("N2"));

				ScaInitial = Object.Scale;	// NOTE: Scale comes from the creature blueprint/instance/template/whatver.
											// That is, there's no default parameter for scale in this Scene like
											// there is for position and rotation.

				// set object position ->
				var objects = new NetDisplayObjectCollection() { Object }; // can't move a single object - only a collection (of 1).
				NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, _pos_Instance);

				Vector3 scale; // don't ask. It works unlike 'Object.Scale'.
				if (first)
				{
					Object.Orientation = RHQuaternion.RotationZ(ROT_START_OBJECT);
					scale = ScaInitial;

					var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;
//					state.FocusTheta = (float)Math.PI /  2F;
//					state.FocusPhi   = (float)Math.PI / 32F;
//					state.Distance   = 4.5F;

					Receiver.CameraAngleXY = (float)Math.PI /  2F; // FocusTheta revolutions 0=east, lookin' west
					Receiver.CameraAngleYZ = (float)Math.PI / 32F; // FocusPhi   pitch 0= flat, inc to pitch forward and raise camera
					Receiver.Distance = DIST_START;
					Receiver.DistanceMin = 0.001F;

//					Receiver.FocusPoint = Object.Position + OFF_Zd;
//					Receiver.PitchMin = -(float)Math.PI / 2f;// + 0.145F;
//					Receiver.PitchMax =  (float)Math.PI / 2f - 0.010F;

					CameraPosition += POS_OFF_Zd;
					_f.PrintCameraPosition();

					POS_START_CAMERA = CameraPosition;


//					float yaw = 0F, pitch = 0F, roll = 0F;
//					CameraOrientation.GetYawPitchRoll(out yaw, out pitch, out roll);
//					MessageBox.Show("yaw= " + yaw + " pitch= " + pitch + " roll= " + roll);
				}
				else if (_changed)
				{
					Object.Orientation = _rot_Instance;
					scale = ScaInitial;
				}
				else
				{
					Object.Orientation = _rot_Instance;
					scale = _sca_Instance;
				}
				// else I'm gonna go bananas.

				// set object rotation ->
				NWN2NetDisplayManager.Instance.RotateObject(Object, ChangeType.Absolute, Object.Orientation);
				_f.PrintModelPosition(Object);

				_instance.EndAppearanceUpdate();

				// set object scale ->
				Object.Scale = scale; // NOTE: after EndAppearanceUpdate().
				NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale); // TODO: does this work
				ResetModel(ResetType.RESET_scale); // this is needed to reset placed instance scale
				_f.PrintModelScale();
			}
			else if (_isplaced && Scene != null) // clear the scene iff a placed instance was last loaded ->
			{
				ClearObjects();
			}
		}

		/// <summary>
		/// Initializes the scene. Clears its objects and adds a lightpoint.
		/// </summary>
		bool StartScene()
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
						Scene.DayNightCycleStages[(int)DayNightStageType.Default].SunMoonDirection = new Vector3(-0.33F, -0.67F, -0.67F);
						Scene.DayNightCycleStages[(int)DayNightStageType.Default].ShadowIntensity = 0F;
					}


					Light = new NetDisplayLightPoint();

					Light.Position        = POS_START_LIGHT;

					Light.Color.Intensity = CreatureVisualizerPreferences.that.LightIntensity;
					Light.Range           = 50F; // default 10F
					Light.CastsShadow     = false;

					Light.ID              = NetDisplayManager.Instance.NextObjectID;	// doesn't appear to be req'd.
					Light.Tag             = Light;										// doesn't appear to be req'd.


					if (ColorCheckedDiffuse)  Light.Color.DiffuseColor  = (Color)ColorDiffuse;
					if (ColorCheckedSpecular) Light.Color.SpecularColor = (Color)ColorSpecular;
					if (ColorCheckedAmbient)  Light.Color.AmbientColor  = (Color)ColorAmbient;


					lock (Scene.Objects.SyncRoot)
					{
						Scene.Objects.Add(Light);
					}
					lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)				// doesn't appear to be req'd.
					{
						NWN2NetDisplayManager.Instance.Objects.Add(Light);
					}
					NWN2NetDisplayManager.Instance.LightParameters(Light.Scene, Light);

					_f.PrintLightPosition(Light.Position);
					_f.PrintLightIntensity(Light.Color.Intensity);
					_f.PrintDiffuseColor();
					_f.PrintSpecularColor();
					_f.PrintAmbientColor();

//					SetDoubleBuffered(NDWindow);
//					SetDoubleBuffered(NWN2NetDisplayManager.Instance.Windows);

//					var a = new NetDisplayWindow[NWN2NetDisplayManager.Instance.Windows.Count];
//					for (int i = 0; i != NWN2NetDisplayManager.Instance.Windows.Count; ++i)
//						a[i] = NWN2NetDisplayManager.Instance.Windows[i];
//
//					SetDoubleBuffered(a);

					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Moves the light by recreating it at a specified position.
		/// </summary>
		/// <param name="pos"></param>
		internal void MoveLight(Vector3 pos)
		{
			ClearLight();

			if (Scene.DayNightCycleStages[(int)DayNightStageType.Default] != null)
			{
				Scene.DayNightCycleStages[(int)DayNightStageType.Default].SunMoonDirection = new Vector3(-0.33F, -0.67F, -0.67F);
				Scene.DayNightCycleStages[(int)DayNightStageType.Default].ShadowIntensity = 0F;
			}


			Light = new NetDisplayLightPoint();

			Light.Position        = pos;

			Light.Color.Intensity = CreatureVisualizerPreferences.that.LightIntensity;
			Light.Range           = 50F;
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
			lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)				// doesn't appear to be req'd.
			{
				NWN2NetDisplayManager.Instance.Objects.Add(Light);
			}
			NWN2NetDisplayManager.Instance.LightParameters(Light.Scene, Light);

			_f.PrintLightPosition(Light.Position);
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

		void ClearLight()
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
		}


/*		static string StringDecryptor(string P_0)
		{
			char[] array;
			char[] array2 = array = P_0.ToCharArray();
			int num = array2.Length;
			while (num > 0)
			{
				int num2;
				array2[num2 = num + -1] = (char)(array[num2] - 5225);
				array2 = array;
				num = num2;
			}
			return String.Intern(new string(array2));
		} */
		#endregion Methods


		#region Handlers
		float _zObject;
		void positionchanged_Object(object sender, EventArgs e)
		{
			if (!Object.Position.Z.Equals(_zObject))
			{
				_zObject = Object.Position.Z;
				_f.PrintModelPosition(Object);
			}
		}
		#endregion Handlers


		#region Methods (model)
		internal static Vector3 off_xpos = new Vector3( 0.1F, 0F, 0F);
		internal static Vector3 off_xneg = new Vector3(-0.1F, 0F, 0F);

		internal static Vector3 off_ypos = new Vector3(0F,  0.1F, 0F);
		internal static Vector3 off_yneg = new Vector3(0F, -0.1F, 0F);

		internal static Vector3 off_zpos = new Vector3(0F, 0F,  0.1F);
		internal static Vector3 off_zneg = new Vector3(0F, 0F, -0.1F);

		internal static float rotpos =  0.1F;
		internal static float rotneg = -0.1F;


		internal void MoveModel(Vector3 vec)
		{
			var objects = new NetDisplayObjectCollection() { Object }; // TODO: cache that
			NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Relative, false, vec);
			_f.PrintModelPosition(Object);
		}

		internal void RotateModel(float f)
		{
			RHQuaternion rotate = RHQuaternion.RotationZ(f);
			NWN2NetDisplayManager.Instance.RotateObject(Object, ChangeType.Relative, rotate);

			Object.Orientation = RHQuaternion.Multiply(Object.Orientation, rotate);
			_f.PrintModelPosition(Object);
		}

		internal void ScaleModel(Vector3 vec)
		{
			NWN2NetDisplayManager.Instance.SetObjectScale(Object, (Object.Scale += vec));
			_f.PrintModelScale();
		}

		internal void ScaleModel(int dir)
		{
			var vec = _f.grader(new Vector3(0.1F, 0.1F, 0.1F));
			switch (dir)
			{
				case +1: Object.Scale += vec; break;
				case -1: Object.Scale -= vec; break;
			}

			NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale);
			_f.PrintModelScale();
		}

		internal void ResetModel()
		{
			var objects = new NetDisplayObjectCollection() { Object }; // TODO: cache that
			NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, new Vector3());

			Object.Orientation = RHQuaternion.RotationZ(ROT_START_OBJECT);
			NWN2NetDisplayManager.Instance.RotateObject(Object, ChangeType.Absolute, Object.Orientation);
			_f.PrintModelPosition(Object);

			Object.Scale = ScaInitial;
			NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale);
			_f.PrintModelScale();
		}

		internal void ResetModel(ResetType reset)
		{
			switch (reset)
			{
				case ResetType.RESET_z:
				{
					var pos = new Vector3();
					pos.X = Object.Position.X;
					pos.Y = Object.Position.Y;
					pos.Z = 0;

					var objects = new NetDisplayObjectCollection() { Object }; // TODO: cache that
					NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, pos);
					_f.PrintModelPosition(Object);
					break;
				}

				case ResetType.RESET_xy:
				{
					var pos = new Vector3();
					pos.X = 0;
					pos.Y = 0;
					pos.Z = Object.Position.Z;

					var objects = new NetDisplayObjectCollection() { Object }; // TODO: cache that
					NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, pos);
					_f.PrintModelPosition(Object);
					break;
				}

				case ResetType.RESET_rot:
					Object.Orientation = RHQuaternion.RotationZ(ROT_START_OBJECT);
					NWN2NetDisplayManager.Instance.RotateObject(Object, ChangeType.Absolute, Object.Orientation);
					_f.PrintModelPosition(Object);
					break;

				case ResetType.RESET_scale:
					Object.Scale = ScaInitial;
					NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale);
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
			var y = new Vector3(0f, 1f, 0f);
			y = RHMatrix.RotationZ(state.FocusTheta).TransformCoordinate(y);

			var z = new Vector3(0f, 0f, 1f);
			z = RHMatrix.RotationZ(state.FocusTheta)    .TransformCoordinate(z);
			z = RHMatrix.RotationAxis(y, state.FocusPhi).TransformCoordinate(z);

			var pos = new Vector3(1f, 0f, 0f);
			pos = RHMatrix.RotationZ(state.FocusTheta)    .TransformCoordinate(pos);
			pos = RHMatrix.RotationAxis(y, state.FocusPhi).TransformCoordinate(pos);

			pos.Scale(state.Distance);
			pos += state.FocusPoint;

			CameraPosition = pos + CreatureVisualizerF.Offset + POS_OFF_Zd;

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
			_pos0 = _pos = PointToClient(Cursor.Position);
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
		Point _pos, _pos0;

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

				if (_pos != _pos0)
				{
					switch (_btn)
					{
						case MouseButtons.Right:
							switch (Control.ModifierKeys)
							{
								case Keys.Control: // full-polar movement around model/focuspt ->
								{
									int deltahori = _pos.X - _pos0.X;
									int deltavert = _pos.Y - _pos0.Y;

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
									float z = (float)(_pos.Y - _pos0.Y) * 0.01F;

									// horizontal shifts ->
									float rot = _f.getrot();
									rot *= (float)Math.PI / 180F; // to rads

									float cos = (float)(Math.Cos((double)rot));
									float x = (float)(_pos0.X - _pos.X) * 0.01F * cos;

									float sin = -(float)(Math.Sin((double)rot));
									float y = (float)(_pos0.X - _pos.X) * 0.01F * sin;

									var shift = new Vector3(x,y,z);
									CameraPosition += shift;
									CreatureVisualizerF.Offset += shift;

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
									float z = (float)(_pos.Y - _pos0.Y) * 0.01F;

									var shift = new Vector3(0F, 0F, z);
									CameraPosition += shift;
									CreatureVisualizerF.Offset += shift;
									_f.PrintCameraPosition();
									break;
								}

								case Keys.Control: // x/y-plane shift.
								{
									// cf. RMB
									float rot = _f.getrot();
									rot *= (float)Math.PI / 180F; // to rads

									float cos = (float)(Math.Cos((double)rot));
									float x = (float)(_pos0.X - _pos.X) * 0.01F * cos;

									float sin = -(float)(Math.Sin((double)rot));
									float y = (float)(_pos0.X - _pos.X) * 0.01F * sin;

									var shift = new Vector3(x,y, 0F);
									CameraPosition += shift;
									CreatureVisualizerF.Offset += shift;

									_f.PrintCameraPosition();
									break;
								}
							}
							break;
					}
				}
			}

			_pos0 = _pos;
			_pos = PointToClient(Cursor.Position);
		}


		/// <summary>
		/// Zooms the camera in/out ... overrides the stock mousewheel handler.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (e.Delta > 0)
				_f.click_bu_camera_distneg(null, EventArgs.Empty);
			else if (e.Delta < 0)
				_f.click_bu_camera_distpos(null, EventArgs.Empty);
		}
		#endregion Handlers (mouse)
	}
}
