using System;

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


namespace creaturevisualizer
{
	/// <summary>
	/// Credit: The Grinning Fool's Creature Creation Wizard
	/// https://neverwintervault.org/project/nwn2/other/grinning-fools-creature-creation-wizard
	/// </summary>
	sealed class CreatureVisualizerP
		: ElectronPanel
	{
		#region Fields (static)
		const float INIT_INSTANCE_ROTATION = 2.6F;
		#endregion Fields (static)


		#region Fields
		NetDisplayObject _object;
		INWN2Instance _instance;

		Vector3 _vec_Instance;
		RHQuaternion _qua_Instance;
		bool _first;
		#endregion Fields


		#region Methods
		/// <summary>
		/// Creates an instance of a blueprint and renders it to the
		/// ElectronPanel.
		/// </summary>
		internal void CreateInstance()
		{
			_instance = null;

			NWN2BlueprintView tslist = NWN2ToolsetMainForm.App.BlueprintView;

			object[] selection = tslist.Selection;
			if (selection != null && selection.Length == 1)
			{
				var blueprint = selection[0] as INWN2Blueprint;

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

//		internal void UpdateScene()
//		{
//			NWN2NetDisplayManager.Instance.UpdateAppearanceForInstance(_instance);
			//NWN2NetDisplayManager.NWN2CreatureTemplate.AppearanceChanged
//		}


		/// <summary>
		/// Adds a blueprint-instance to the scene.
		/// </summary>
		void RenderScene()
		{
			if (_instance != null)
			{
				if (_object != null) // NOT '_first' display
				{
					_first = false;

					_vec_Instance = _object.Position;
					_qua_Instance = _object.Orientation;	// NOTE: RotateObject() won't change the cached Orientation value;
				}											// Orientation needs to be updated explicitly if rotation is changed.
				else
					_first = true;


				if (InitScene())
				{
					_instance.BeginAppearanceUpdate();

					_object = NWN2NetDisplayManager.Instance.CreateNDOForInstance(_instance, Scene, 0);

					var objects = new NetDisplayObjectCollection();
					objects.Add(_object);
					NWN2NetDisplayManager.Instance.MoveObjects(objects,
															   ChangeType.Relative,
															   false,
															   _vec_Instance);


					if (_first)
						_object.Orientation = RHQuaternion.RotationZ(INIT_INSTANCE_ROTATION);
					else
						_object.Orientation = _qua_Instance;

					NWN2NetDisplayManager.Instance.RotateObject(_object, ChangeType.Absolute, _object.Orientation);

					_instance.EndAppearanceUpdate();
				}
			}
		}

		/// <summary>
		/// Initializes the scene. Clears its objects and adds a lightpoint.
		/// </summary>
		bool InitScene()
		{
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


				var light = new NetDisplayLightPoint();
				light.Position        = new Vector3(0F, -4F, 1.5F);
				light.Color.Intensity = 0.72F;
				light.Range           = 50F; // default 10F
				light.ID              = NetDisplayManager.Instance.NextObjectID;	// doesn't appear to be req'd.
				light.Tag             = light;										// doesn't appear to be req'd.

				lock (Scene.Objects.SyncRoot)
				{
					Scene.Objects.Add(light);
				}
				lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)				// doesn't appear to be req'd.
				{
					NWN2NetDisplayManager.Instance.Objects.Add(light);
				}

				NWN2NetDisplayManager.Instance.LightParameters(light.Scene, light);
				return true;
			}
			return false;
		}
		#endregion Methods


		#region Methods (model)
		internal static Vector3 vec_zpos = new Vector3(0F, 0F,  0.1F);
		internal static Vector3 vec_zneg = new Vector3(0F, 0F, -0.1F);

		internal static Vector3 vec_ypos = new Vector3(0F,  0.1F, 0F);
		internal static Vector3 vec_yneg = new Vector3(0F, -0.1F, 0F);

		internal static Vector3 vec_xpos = new Vector3( 0.1F, 0F, 0F);
		internal static Vector3 vec_xneg = new Vector3(-0.1F, 0F, 0F);

		internal static float rotpos =  0.1F;
		internal static float rotneg = -0.1F;


		internal void MoveModel(Vector3 vec)
		{
			var objects = new NetDisplayObjectCollection(); // TODO: cache that
			objects.Add(_object);
			NWN2NetDisplayManager.Instance.MoveObjects(objects,
													   ChangeType.Relative,
													   false,
													   vec);
		}

		internal void RotateModel(float f)
		{
//			RHQuaternion rotate = RHQuaternion.Identity;
//			rotate.RotateZ(f);
			RHQuaternion rotate = RHQuaternion.RotationZ(f);
			NWN2NetDisplayManager.Instance.RotateObject(_object, ChangeType.Relative, rotate);

			_object.Orientation = RHQuaternion.Multiply(_object.Orientation, rotate);
		}


/*		internal void MovePosZ()
		{
			var objects = new NetDisplayObjectCollection();
			objects.Add(_object);
			NWN2NetDisplayManager.Instance.MoveObjects(objects,
													   ChangeType.Relative,
													   false,
													   vec_zpos);
		}

		internal void MoveNegZ()
		{
			var objects = new NetDisplayObjectCollection();
			objects.Add(_object);
			NWN2NetDisplayManager.Instance.MoveObjects(objects,
													   ChangeType.Relative,
													   false,
													   vec_zneg);
		}

		internal void MovePosY()
		{
			var objects = new NetDisplayObjectCollection();
			objects.Add(_object);
			NWN2NetDisplayManager.Instance.MoveObjects(objects,
													   ChangeType.Relative,
													   false,
													   vec_ypos);
		}

		internal void MoveNegY()
		{
			var objects = new NetDisplayObjectCollection();
			objects.Add(_object);
			NWN2NetDisplayManager.Instance.MoveObjects(objects,
													   ChangeType.Relative,
													   false,
													   vec_yneg);
		} */
		#endregion Methods (model)
	}
}
