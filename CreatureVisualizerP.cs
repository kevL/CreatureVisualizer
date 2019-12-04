﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

using Microsoft.DirectX;

using NWN2Toolset;
using NWN2Toolset.NWN2.Data.Blueprints;
using NWN2Toolset.NWN2.Data.Instances;
using NWN2Toolset.NWN2.Data.Templates;
using NWN2Toolset.NWN2.NetDisplay;
using NWN2Toolset.NWN2.Views;

using OEIShared.NetDisplay;
using OEIShared.OEIMath;
using OEIShared.UI;
using OEIShared.UI.Input;


namespace creaturevisualizer
{
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
	/// and the NwN2 toolset's Appearance Wizard
	/// </summary>
	sealed class CreatureVisualizerP
		: ElectronPanel
	{
		#region Fields (static)
		internal static Vector3 POS_START_CAMERA;
		internal const float DIST_START = 4.8F;

		internal static Vector3 POS_START_LIGHT = new Vector3(-0.5F, -4F, 2.0F);
		internal static Vector3 POS_OFF_Zd      = new Vector3( 0.0F,  0F, 1.5F);

		const float ROT_START_OBJECT = (float)Math.PI * 3F / 4F;

		static Vector3 ScaInitial;
		#endregion Fields (static)


		#region Fields
		INWN2Instance  _instance;
		INWN2Blueprint _blueprint0; // ref to previous blueprint-object (to track 'changed').

		bool _changed;

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

		#region cTor
		internal CreatureVisualizerP()
		{
			RecreateMousePanel();

			NetDisplayScene scene = Scene;
			NetDisplayManager.Instance.AddScene(out scene);

			OpenWindow();
		}

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

			Controls.Add(MousePanel);
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Creates an instance of a blueprint and renders it to the
		/// ElectronPanel.
		/// </summary>
		internal void CreateInstance()
		{
			if (MousePanel != null && !MousePanel.IsDisposed) // safety. ElectronPanel.MousePanel could go disposed for no good reason.
			{
				_instance = null;

				NWN2BlueprintView tslist = NWN2ToolsetMainForm.App.BlueprintView;

				object[] selection = tslist.Selection;
				if (selection != null && selection.Length == 1)
				{
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
							if (CreatureVisualizerF.that.feline())
							{
								((NWN2CreatureBlueprint)blueprint).Gender = CreatureGender.Female; // NWN2Toolset.NWN2.Data.Templates
							}
							else
								((NWN2CreatureBlueprint)blueprint).Gender = CreatureGender.Male;

							goto case NWN2ObjectType.Item;

						case NWN2ObjectType.Item:	// <- TODO: works for weapons (see Preview tab) but clothes
						{							//          appear on a default creature (in the ArmorSet tab)
							_instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(blueprint);
							break;
						}
					}
				}
				RenderScene();
			}
			else
				MessageBox.Show(this, "ElectronPanel.MousePanel is invalid. Please see your chiropractor.");
		}

//		NWN2NetDisplayManager.NWN2CreatureTemplate.AppearanceChanged;
//		internal void UpdateScene()
//		{
//			NWN2NetDisplayManager.Instance.UpdateAppearanceForInstance(_instance);
//		}


		/// <summary>
		/// Adds a blueprint-instance to the scene.
		/// </summary>
		void RenderScene()
		{
			if (_instance != null && InitScene())
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
				Object = NWN2NetDisplayManager.Instance.CreateNDOForInstance(_instance, Scene, NetDisplayManager.Instance.NextObjectID);

				Object.PositionChanged += positionchanged_Object;

				CreatureVisualizerF.that.PrintOriginalScale(Object.Scale.X.ToString("N2"));

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

					Receiver.CameraAngleXY = (float)Math.PI /  2F; // revolutions 0=east, lookin' west
					Receiver.CameraAngleYZ = (float)Math.PI / 32F; // pitch 0= flat, inc to pitch forward and raise camera
					Receiver.Distance = DIST_START;
					Receiver.DistanceMin = 0.1F;

//					Receiver.FocusPoint = Object.Position + OFF_Zd;
					Receiver.PitchMin = -(float)Math.PI / 2f + 0.145F;
//					Receiver.PitchMax =  (float)Math.PI / 2f - 0.010F;

					CameraPosition += POS_OFF_Zd;
//					UpdateCamera();
					CreatureVisualizerF.that.PrintCameraPosition();

					POS_START_CAMERA = CameraPosition;


/*					float yaw = 0F, pitch = 0F, roll = 0F;
					CameraOrientation.GetYawPitchRoll(out yaw, out pitch, out roll);
					MessageBox.Show("yaw= " + yaw + " pitch= " + pitch + " roll= " + roll); */
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
				CreatureVisualizerF.that.PrintModelPosition(Object);

				_instance.EndAppearanceUpdate();

				// set object scale ->
				Object.Scale = scale; // NOTE: after EndAppearanceUpdate().
				NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale);
				CreatureVisualizerF.that.PrintModelScale();
			}
		}

		/// <summary>
		/// Initializes the scene. Clears its objects and adds a lightpoint.
		/// </summary>
		bool InitScene()
		{
			CloseWindow(); // safety - try not to confuse the NWN2NetDisplayManager.Instance ...

			if (NDWindow == null)
				OpenWindow();

			if (NDWindow != null && Scene != null)
			{
				var objects = new NetDisplayObjectCollection();
				foreach (NetDisplayObject @object in Scene.Objects)
				{
					//OEIShared.NetDisplay.NetDisplayLightPoint
					//OEIShared.NetDisplay.NetDisplayModel
					objects.Add(@object);
				}
				NWN2NetDisplayManager.Instance.RemoveObjects(objects);

				if (Scene.DayNightCycleStages[(int)DayNightStageType.Default] != null)
				{
					Scene.DayNightCycleStages[(int)DayNightStageType.Default].SunMoonDirection = new Vector3(-0.33F, -0.67F, -0.67F);
					Scene.DayNightCycleStages[(int)DayNightStageType.Default].ShadowIntensity = 0F;
				}

				Light = new NetDisplayLightPoint();

				Light.Position        = POS_START_LIGHT;

				Light.Color.Intensity = 0.72F;
				Light.Range           = 50F; // default 10F
				Light.CastsShadow     = false;

				Light.ID              = NetDisplayManager.Instance.NextObjectID;	// doesn't appear to be req'd.
				Light.Tag             = Light;										// doesn't appear to be req'd.

				lock (Scene.Objects.SyncRoot)
				{
					Scene.Objects.Add(Light);
				}
				lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)				// doesn't appear to be req'd.
				{
					NWN2NetDisplayManager.Instance.Objects.Add(Light);
				}
				NWN2NetDisplayManager.Instance.LightParameters(Light.Scene, Light);

				CreatureVisualizerF.that.PrintLightPosition(Light.Position, Light.Color.Intensity);
				return true;
			}
			return false;
		}

		internal void RecreateLight(Vector3 pos)
		{
			var objects = new NetDisplayObjectCollection();
			foreach (NetDisplayObject @object in Scene.Objects)
			{
				if (@object.Tag == @object) // TODO: <- that
					objects.Add(@object);
			}
			NWN2NetDisplayManager.Instance.RemoveObjects(objects);

			if (Scene.DayNightCycleStages[(int)DayNightStageType.Default] != null)
			{
				Scene.DayNightCycleStages[(int)DayNightStageType.Default].SunMoonDirection = new Vector3(-0.33F, -0.67F, -0.67F);
				Scene.DayNightCycleStages[(int)DayNightStageType.Default].ShadowIntensity = 0F;
			}

			Light = new NetDisplayLightPoint();

			Light.Position        = pos;

			Light.Color.Intensity = 0.72F;
			Light.Range           = 50F;
			Light.CastsShadow     = false;

			Light.ID              = NetDisplayManager.Instance.NextObjectID;	// doesn't appear to be req'd.
			Light.Tag             = Light;										// doesn't appear to be req'd.

			lock (Scene.Objects.SyncRoot)
			{
				Scene.Objects.Add(Light);
			}
			lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)				// doesn't appear to be req'd.
			{
				NWN2NetDisplayManager.Instance.Objects.Add(Light);
			}
			NWN2NetDisplayManager.Instance.LightParameters(Light.Scene, Light);
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
				CreatureVisualizerF.that.PrintModelPosition(Object);
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
			CreatureVisualizerF.that.PrintModelPosition(Object);
		}

		internal void RotateModel(float f)
		{
			RHQuaternion rotate = RHQuaternion.RotationZ(f);
			NWN2NetDisplayManager.Instance.RotateObject(Object, ChangeType.Relative, rotate);

			Object.Orientation = RHQuaternion.Multiply(Object.Orientation, rotate);
			CreatureVisualizerF.that.PrintModelPosition(Object);
		}

		internal void ScaleModel(Vector3 vec)
		{
			NWN2NetDisplayManager.Instance.SetObjectScale(Object, (Object.Scale += vec));
			CreatureVisualizerF.that.PrintModelScale();
		}

		internal void ScaleModel(int dir)
		{
			var vec = CreatureVisualizerF.that.grader(new Vector3(0.1F, 0.1F, 0.1F));
			switch (dir)
			{
				case +1: Object.Scale += vec; break;
				case -1: Object.Scale -= vec; break;
			}

			NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale);
			CreatureVisualizerF.that.PrintModelScale();
		}

		internal void ResetModel()
		{
			var objects = new NetDisplayObjectCollection() { Object }; // TODO: cache that
			NWN2NetDisplayManager.Instance.MoveObjects(objects, ChangeType.Absolute, false, new Vector3());

			Object.Orientation = RHQuaternion.RotationZ(ROT_START_OBJECT);
			NWN2NetDisplayManager.Instance.RotateObject(Object, ChangeType.Absolute, Object.Orientation);
			CreatureVisualizerF.that.PrintModelPosition(Object);

			Object.Scale = ScaInitial;
			NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale);
			CreatureVisualizerF.that.PrintModelScale();
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
					CreatureVisualizerF.that.PrintModelPosition(Object);
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
					CreatureVisualizerF.that.PrintModelPosition(Object);
					break;
				}

				case ResetType.RESET_rot:
					Object.Orientation = RHQuaternion.RotationZ(ROT_START_OBJECT);
					NWN2NetDisplayManager.Instance.RotateObject(Object, ChangeType.Absolute, Object.Orientation);
					CreatureVisualizerF.that.PrintModelPosition(Object);
					break;

				case ResetType.RESET_scale:
					Object.Scale = ScaInitial;
					NWN2NetDisplayManager.Instance.SetObjectScale(Object, Object.Scale);
					CreatureVisualizerF.that.PrintModelScale();
					break;
			}
		}
		#endregion Methods (model)


		#region Methods (camera)
		internal void RaiseCameraPolar()
		{
			var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;
			state.FocusPhi += CreatureVisualizerF.that.grader(0.1F);
			state.FocusPhi = Math.Min(state.PitchMax, state.FocusPhi);
			state.FocusPhi = Math.Max(state.PitchMin, state.FocusPhi);

			UpdateCamera();
		}

		internal void LowerCameraPolar()
		{
			var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;

			state.FocusPhi -= CreatureVisualizerF.that.grader(0.1F);
			state.FocusPhi = Math.Min(state.PitchMax, state.FocusPhi);
			state.FocusPhi = Math.Max(state.PitchMin, state.FocusPhi);

			UpdateCamera();
		}

		internal void UpdateCamera()
		{
			var state = Receiver.CameraState as ModelViewerInputCameraReceiverState;

			var y = new Vector3(0f, 1f, 0f);
			y = RHMatrix.RotationZ(state.FocusTheta).TransformCoordinate(y);

			var z = new Vector3(0f, 0f, 1f);
			z = RHMatrix.RotationZ(state.FocusTheta).TransformCoordinate(z);
			z = RHMatrix.RotationAxis(y, state.FocusPhi).TransformCoordinate(z);

			var pos = new Vector3(1f, 0f, 0f);
			pos = RHMatrix.RotationZ(state.FocusTheta).TransformCoordinate(pos);
			pos = RHMatrix.RotationAxis(y, state.FocusPhi).TransformCoordinate(pos);

			pos.Scale(state.Distance);
			pos += state.FocusPoint;

			CameraPosition = pos + CreatureVisualizerF.Offset + POS_OFF_Zd;

			Vector3 focusPoint = state.FocusPoint;
			focusPoint.Subtract(pos);
			focusPoint = MathUtils.NormalizeVector3(focusPoint);

			CameraOrientation = RHQuaternion.RotationMatrix(RHMatrix.LookAtRH(Vector3.Empty, focusPoint, z));
		}
		#endregion Methods (camera)


		#region Handlers (override)
		#endregion Handlers (override)
	}
}
