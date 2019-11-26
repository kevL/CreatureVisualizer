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
		#region Fields
		INWN2Instance _instance;
		#endregion Fields


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		internal void CreateInstance()
		{
			_instance = null;

			NWN2BlueprintView tslist = NWN2ToolsetMainForm.App.BlueprintView;

			switch (tslist.GetFocusedListObjectType())
			{
				case NWN2ObjectType.Creature:
				case NWN2ObjectType.Item:	// <- TODO: this works for weapons (Preview tab) but
				{							// clothing appears on a default creature (ArmorSet tab)
					object[] selection = tslist.Selection;
					if (selection != null && selection.Length == 1)
					{
						_instance = NWN2GlobalBlueprintManager.CreateInstanceFromBlueprint(selection[0] as INWN2Blueprint);
					}
					break;
				}
			}
			RenderScene();
		}

//		internal void UpdateScene()
//		{
//			NWN2NetDisplayManager.Instance.UpdateAppearanceForInstance(_instance);
			//NWN2NetDisplayManager.NWN2CreatureTemplate.AppearanceChanged
//		}


		NetDisplayObject _object;
		float _zPos;

		/// <summary>
		/// 
		/// </summary>
		void RenderScene()
		{
			if (_instance != null)
			{
				if (_object != null)
				{
					_zPos = _object.Position.Z;
//					string pos = _object.Position.ToString();
//					string ori = _object.Orientation.ToString();
//					string sca = _object.Scale.ToString();
//					System.Windows.Forms.MessageBox.Show(pos + "\n" + ori + "\n" + sca);
				}

//				string pos = CameraPosition.ToString();
//				string ori = CameraOrientation.ToString();
//				System.Windows.Forms.MessageBox.Show(pos + "\n" + ori);


				if (InitScene())
				{
					_instance.BeginAppearanceUpdate();

					_object = NWN2NetDisplayManager.Instance.CreateNDOForInstance(_instance, Scene, 0);

					var objects = new NetDisplayObjectCollection();
					objects.Add(_object);
					NWN2NetDisplayManager.Instance.MoveObjects(objects,
															   ChangeType.Relative,
															   false,
															   new Vector3(0F, 0F, _zPos));

					RHQuaternion rotate = RHQuaternion.Identity;
					rotate.RotateZ(2.6F);

					NWN2NetDisplayManager.Instance.RotateObject(_object, ChangeType.Absolute, rotate);

//					pos = _object.Position.ToString();
//					ori = _object.Orientation.ToString();
//					sca = _object.Scale.ToString();
//					System.Windows.Forms.MessageBox.Show(pos + "\n" + ori + "\n" + sca);

					_instance.EndAppearanceUpdate();
				}
			}
		}

		/// <summary>
		/// Initializes the scene.
		/// </summary>
		bool InitScene()
		{
			if (NDWindow == null)
				OpenWindow();

			if (NDWindow != null && Scene != null)
			{
				var objects = new NetDisplayObjectCollection();
				foreach (NetDisplayObject @object in Scene.Objects)
					objects.Add(@object);
				NWN2NetDisplayManager.Instance.RemoveObjects(objects);

				if (Scene.DayNightCycleStages[7] != null)
				{
					Scene.DayNightCycleStages[7].SunMoonDirection = new Vector3(-0.33F, -0.67F, -0.67F);
					Scene.DayNightCycleStages[7].ShadowIntensity = 0F;
				}


				var light = new NetDisplayLightPoint();
				light.Position        = new Vector3(0F, -4F, 1.5F);
				light.Color.Intensity = 0.72F;
//				light.Range           = 50F; // default 10F
//				light.ID              = NetDisplayManager.Instance.NextObjectID;	// doesn't appear to be req'd.
//				light.Tag             = light;										// doesn't appear to be req'd.

				lock (Scene.Objects.SyncRoot)
				{
					Scene.Objects.Add(light);
				}
/*				lock (NWN2NetDisplayManager.Instance.Objects.SyncRoot)				// doesn't appear to be req'd.
				{
					NWN2NetDisplayManager.Instance.Objects.Add(light);
				} */

				NWN2NetDisplayManager.Instance.LightParameters(light.Scene, light);
				return true;
			}
			return false;
		}
		#endregion Methods
	}
}
