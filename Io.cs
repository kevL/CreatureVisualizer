using System;
using System.IO;
using System.Windows.Forms;

using NWN2Toolset;
using NWN2Toolset.NWN2.Data.TypedCollections;
using NWN2Toolset.NWN2.Data.Blueprints;
using NWN2Toolset.NWN2.Data.Campaign;
using NWN2Toolset.NWN2.Data.Instances;
using NWN2Toolset.NWN2.Data.Templates;
using NWN2Toolset.NWN2.IO;

using OEIShared.IO;
using OEIShared.Utils;


namespace creaturevisualizer
{
	static class Io
	{
		#region Fields (static)
		internal static IResourceEntry AppearanceSEF;
		#endregion Fields (static)


		#region Methods (internal)
		/// <summary>
		/// Performs a case-insensitive path comparison (valid on Windows only).
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		static bool IsOverride(string dir)
		{
			string overrideroot = NWN2ResourceManager.Instance.UserOverrideDirectory.DirectoryName.ToLower();
			var di2 = new DirectoryInfo(dir);

			if (di2.FullName.ToLower() == overrideroot)
				return true;

			while (di2.Parent != null)
			{
				if (di2.Parent.FullName.ToLower() == overrideroot)
					return true;

				di2 = di2.Parent;
			}
			return false;
		}


		// NWN2Toolset.NWN2.Views.NWN2BlueprintView.ᐌ(object P_0, EventArgs P_1)
		// yeah whatever. Those idiots were too clever for anybody's good.
		/// <summary>
		/// Saves a specified blueprint to a user-labeled file.
		/// IMPORTANT: Allow only creature-blueprints to be saved!
		/// - NWN2ObjectType.Creature
		/// - resourcetype #2027
		/// @note Check that blueprint is valid before call.
		/// </summary>
		/// <param name="iblueprint">ElectronPanel_.Blueprint</param>
		/// <param name="repo"></param>
		internal static void SaveBlueprintToFile(INWN2Blueprint iblueprint, DirectoryResourceRepository repo = null)
		{
			string fil = iblueprint.Resource.ResRef.Value;
			string ext = BWResourceTypes.GetFileExtension(iblueprint.Resource.ResourceType);

			var sfd = new SaveFileDialog();
			sfd.Title      = "Save blueprint as ...";
			sfd.FileName   = fil + "." + ext; // iblueprint.Resource.FullName
			sfd.Filter     = "blueprints (*." + ext + ")|*." + ext + "|all files (*.*)|*.*";
//			sfd.DefaultExt = ext;

			string dir;
			if (repo != null) dir = repo.DirectoryName;
			else              dir = String.Empty;

			if (!String.IsNullOrEmpty(dir))
			{
				if (Directory.Exists(dir))
				{
					sfd.InitialDirectory = dir;
					sfd.RestoreDirectory = true;
				}
			}
			else if (!String.IsNullOrEmpty(CreatureVisualizerPreferences.that.LastSaveDirectory)
				&& Directory.Exists(CreatureVisualizerPreferences.that.LastSaveDirectory))
			{
				sfd.InitialDirectory = CreatureVisualizerPreferences.that.LastSaveDirectory;
			}
			// else TODO: use NWN2ResourceManager.Instance.UserOverrideDirectory 
			// else TODO: get BlueprintLocation dir if exists


			if (sfd.ShowDialog() == DialogResult.OK)
			{
				if (String.IsNullOrEmpty(dir))
					CreatureVisualizerPreferences.that.LastSaveDirectory = Path.GetDirectoryName(sfd.FileName);


				// NOTE: Add 'AppearanceSEF' back in from the original blueprint
				// since it was removed when the original was duplicated by
				// ElectronPanel_.DuplicateBlueprint().
				(iblueprint as NWN2CreatureTemplate).AppearanceSEF = AppearanceSEF;


				IOEISerializable iserializable = iblueprint;
				if (iserializable != null)
				{
					iserializable.OEISerialize(sfd.FileName);

					if (File.Exists(sfd.FileName)) // test that file exists before proceeding
					{
						INWN2BlueprintSet blueprintset;

						dir = Path.GetDirectoryName(sfd.FileName).ToLower();
						if (dir == NWN2ToolsetMainForm.App.Module.Repository.DirectoryName.ToLower())
						{
							repo         = NWN2ToolsetMainForm.App.Module.Repository;
							blueprintset = NWN2ToolsetMainForm.App.Module as INWN2BlueprintSet;

							iblueprint.BlueprintLocation = NWN2BlueprintLocationType.Module;
						}
						else if (     NWN2CampaignManager.Instance.ActiveCampaign != null
							&& dir == NWN2CampaignManager.Instance.ActiveCampaign.Repository.DirectoryName.ToLower())
						{
							repo         = NWN2CampaignManager.Instance.ActiveCampaign.Repository;
							blueprintset = NWN2CampaignManager.Instance.ActiveCampaign as INWN2BlueprintSet;

							iblueprint.BlueprintLocation = NWN2BlueprintLocationType.Campaign;
						}
						else if (IsOverride(dir))
						{
							repo         = NWN2ResourceManager       .Instance.UserOverrideDirectory;
							blueprintset = NWN2GlobalBlueprintManager.Instance as INWN2BlueprintSet;

							iblueprint.BlueprintLocation = NWN2BlueprintLocationType.Global;
						}
						else
							return;


						string info = "dir= " + repo.DirectoryName + "\n";

						// so, which should be tested for first: resource or blueprint?
						// and is there even any point in having one w/out the other?

						string filelabel = Path.GetFileNameWithoutExtension(sfd.FileName);
						info += "filelabel= " + filelabel + "\n";

						NWN2BlueprintCollection collection = blueprintset.GetBlueprintCollectionForType(NWN2ObjectType.Creature);

						INWN2Blueprint extantblueprint = NWN2GlobalBlueprintManager.FindBlueprint(NWN2ObjectType.Creature,
																								  new OEIResRef(filelabel),
																								  iblueprint.BlueprintLocation == NWN2BlueprintLocationType.Global,
																								  iblueprint.BlueprintLocation == NWN2BlueprintLocationType.Module,
																								  iblueprint.BlueprintLocation == NWN2BlueprintLocationType.Campaign);

						if (extantblueprint != null && extantblueprint.Resource.Repository is DirectoryResourceRepository) // ie. exclude Data\Templates*.zip
						{
							info += "extantblueprint.ResourceName= " + extantblueprint.ResourceName + "\n";
							info += "remove extantblueprint from collection ...\n";
							collection.Remove(extantblueprint); // so, does removing a blueprint also remove its resource? no.
						}
						else
							info += "extantblueprint NOT found\n";

						IResourceEntry extantresource = repo.FindResource(new OEIResRef(filelabel), 2027); // it's maaaaaagick
						if (extantresource != null)
						{
							info += "resource= "     + extantresource.ResRef.Value    + "\n";
							info += "repo= "         + extantresource.Repository.Name + "\n";
							info += "resourcetype= " + extantresource.ResourceType    + "\n";

							info += "remove extantresource from repo ...\n";
							repo.Resources.Remove(extantresource); // so, does removing a resource also remove its blueprint? no.
						}
						else
							info += "extantresource NOT found\n";


						iblueprint.Resource = repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						collection.Add(iblueprint);

						var viewer = NWN2ToolsetMainForm.App.BlueprintView;
						var list = viewer.GetFocusedList();
						info += "resort blueprint-collection list\n";
						list.Resort();

						var objects = new object[1] { iblueprint as INWN2Object };
						viewer.Selection = objects;

						MessageBox.Show(info);
						MessageBox.Show(GetResourceInfo(iblueprint as INWN2Template));


/*						INWN2Blueprint blueprint = NWN2GlobalBlueprintManager.FindBlueprint(NWN2ObjectType.Creature, new OEIResRef(filelabel), false, true, false);
						MessageBox.Show(GetResourceInfo(blueprint as INWN2Template));
						MessageBox.Show(GetResourceInfo(iblueprint as INWN2Template));

						var resource0 = iblueprint.Resource;
						var resource1 = repo.FindResource(new OEIResRef(iblueprint.Resource.ResRef.Value), 2027);
						MessageBox.Show("is resource0 valid= " + (resource0 != null) + "\n"
						                + "is resource1 valid= " + (resource1 != null) + "\n"
						                + "is resource0 resource1= " + (resource0 == resource1) + "\n"			// FALSE
						                + "is resource0 equal to resource1= " + resource0.Equals(resource1));	// TRUE ... */
//						MessageBox.Show("is blueprint iblueprint= " + (blueprint == iblueprint) + "\n" +
//						                "is resource iresource= " + (iblueprint.Resource == repo.FindResource(new OEIResRef(filelabel), 2027)));
					}
				}
			}
		}


		/// <summary>
		/// Saves the currently selected instance in the Area viewer to a
		/// user-labeled file.
		/// @note Check that instance is valid before call.
		/// </summary>
		/// <param name="iinstance"></param>
		internal static void SaveInstanceToModule(INWN2Instance iinstance)
		{
			SaveInstanceToFile(iinstance, NWN2BlueprintLocationType.Module, NWN2ToolsetMainForm.App.Module.Repository.Name);
		}

		internal static void SaveInstanceToCampaign(INWN2Instance iinstance)
		{
			SaveInstanceToFile(iinstance, NWN2BlueprintLocationType.Campaign, NWN2CampaignManager.Instance.ActiveCampaign.Repository.DirectoryName);
		}

//		internal static void SaveInstanceToFile(INWN2Instance iinstance)
//		{
//			INWN2Blueprint iblueprint = CreateBlueprint(iinstance, NWN2BlueprintLocationType.Global);
//			if (iblueprint != null)
//				SaveBlueprintToFile(iblueprint);
//		}

		internal static void SaveInstanceToFile(INWN2Instance iinstance, NWN2BlueprintLocationType location, string dir = "")
		{
			string file;
			if (   iinstance.Template == null
				|| iinstance.Template.ResRef == null
				|| iinstance.Template.ResRef.IsEmpty())
			{
				file = iinstance.Name; // tag
				if (String.IsNullOrEmpty(file))
					file = "object";
			}
			else
				file = iinstance.Template.ResRef.Value;

			string ext;
			if (   iinstance.Template == null
				|| iinstance.Template.ResourceType == 0) // .RES file
			{
				ushort utype = 0;
				if      (iinstance is NWN2CreatureInstance)  utype = (ushort)2027;
				else if (iinstance is NWN2DoorInstance)      utype = (ushort)2042;
				else if (iinstance is NWN2PlaceableInstance) utype = (ushort)2044;

				ext = BWResourceTypes.GetFileExtension(utype);
			}
			else
				ext = BWResourceTypes.GetFileExtension(iinstance.Template.ResourceType);


			var sfd = new SaveFileDialog();
			sfd.Title      = "Save blueprint as ...";
			sfd.FileName   = file + "." + ext; // iblueprint.Resource.FullName
			sfd.Filter     = "blueprints (*." + ext + ")|*." + ext + "|all files (*.*)|*.*";
//			sfd.DefaultExt = ext;

			if (!String.IsNullOrEmpty(dir))
			{
				if (Directory.Exists(dir))
				{
					sfd.InitialDirectory = dir;
					sfd.RestoreDirectory = true;
				}
			}
			else if (!String.IsNullOrEmpty(CreatureVisualizerPreferences.that.LastSaveDirectory)
				&& Directory.Exists(CreatureVisualizerPreferences.that.LastSaveDirectory))
			{
				sfd.InitialDirectory = CreatureVisualizerPreferences.that.LastSaveDirectory;
			}
			// else TODO: use NWN2ResourceManager.Instance.UserOverrideDirectory 
			// else TODO: get BlueprintLocation dir if exists


			if (sfd.ShowDialog() == DialogResult.OK)
			{
				if (String.IsNullOrEmpty(dir))
					CreatureVisualizerPreferences.that.LastSaveDirectory = Path.GetDirectoryName(sfd.FileName);

				INWN2Blueprint iblueprint = CreateBlueprint(iinstance, location);

				IOEISerializable iserializable = iblueprint;
				if (iserializable != null)
				{
					iserializable.OEISerialize(sfd.FileName);

					string info = "test for previous resource/blueprint ...\n";

					dir = Path.GetDirectoryName(sfd.FileName);
// MODULE ->
					if (dir == NWN2ToolsetMainForm.App.Module.Repository.Name)
					{
						info += "module dir= " + NWN2ToolsetMainForm.App.Module.Repository.Name + "\n";

						IResourceRepository repo = NWN2ToolsetMainForm.App.Module.Repository;

						string filelabel = Path.GetFileNameWithoutExtension(sfd.FileName);
						if (filelabel == iblueprint.Resource.ResRef.Value) // TODO: check if this could go south when saving an Instance
						{
							// if an Instance is selected, then created in 'ElectronPanel_', then turned into a blueprint by
							// Io.CreateBlueprint() the blueprint doesn't have its Resource.Repository set yet ->

//							iblueprint.Resource.Repository = repo; // pointer to Repository is readonly
//							if (iblueprint.Resource.Repository == null)
//							{
/*							switch (location)
							{
								case NWN2BlueprintLocationType.Module:
//									iblueprint.Resource.Repository = NWN2ToolsetMainForm.App.Module.Repository;
//									iblueprint.Resource.Repository = ResourceManager.Instance.GetRepositoryByName("");
									break;

								case NWN2BlueprintLocationType.Campaign:
//									iblueprint.Resource.Repository = NWN2CampaignManager.Instance.ActiveCampaign.Repository;
									break;

								case NWN2BlueprintLocationType.Global:
//									iblueprint.Resource.Repository = NWN2ResourceManager.Instance.UserOverrideDirectory;
									break;
							} */
//							}

//							iblueprint.BlueprintLocation = NWN2BlueprintLocationType.Module; // default (shall not be needed/used since the location shall be set as necessary by SaveTo())


							info += "file was overwritten, nothing need be done to its resource\n";
							info += "but replace the blueprint in the blueprint-collection with the new blueprint\n";

							info += "remove blueprint from collection\n";
							var blueprintset = NWN2ToolsetMainForm.App.Module as INWN2BlueprintSet;
							NWN2BlueprintCollection collection = blueprintset.GetBlueprintCollectionForType(NWN2ObjectType.Creature);
//							collection.Remove(oblueprint);

							iblueprint.BlueprintLocation = blueprintset.BlueprintLocation;
							info += "location= " + iblueprint.BlueprintLocation + "\n";

//							if (!collection.Contains(iblueprint))
//							{
							info += "add blueprint to collection\n";
							collection.Add(iblueprint);

							var viewer = NWN2ToolsetMainForm.App.BlueprintView;
//							if (viewer.GetFocusedListObjectType() == (iblueprint as INWN2Template).ObjectType)
//							{
							var list = viewer.GetFocusedList();
							info += "resort blueprint-collection list\n";
							list.Resort();

							var objects = new object[1] { iblueprint as INWN2Object };
							viewer.Selection = objects;
//							}
//							}
						}
						else
						{
							// search to see if a different resource-file was overwritten ...
							IResourceEntry resource = repo.FindResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
							if (resource != null)
							{
								info += "a different resource-file was overwritten\n";
							}
							else
							{
								info += "keep the base blueprint's resource intact and create a new resource for the new file\n";

								repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
							}
						}

						MessageBox.Show(info);


/*						IResourceRepository repo = NWN2ToolsetMainForm.App.Module.Repository;
//						IResourceEntry resource = repo.FindResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						IResourceEntry resource = repo.FindResource(oblueprint.Resource.ResRef, oblueprint.Resource.ResourceType);
						if (resource != null)
						{
							info += "remove prior resource and blueprint\n";

							var blueprintset = NWN2ToolsetMainForm.App.Module as INWN2BlueprintSet;
							NWN2BlueprintCollection collection = blueprintset.GetBlueprintCollectionForType(NWN2ObjectType.Creature);
							collection.Remove(iblueprint);

							repo.Resources.Remove(resource);
						}
						else
							info += "no prior blueprint found in resource\n";

						MessageBox.Show(info);

//						IResourceEntry resource = repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						AddBlueprint(iblueprint, NWN2BlueprintLocationType.Module); */
					}
// CAMPAIGN ->
					else if (     NWN2CampaignManager.Instance.ActiveCampaign != null
						&& dir == NWN2CampaignManager.Instance.ActiveCampaign.Repository.DirectoryName)
					{
						info += "campaign dir= " + NWN2CampaignManager.Instance.ActiveCampaign.Repository.DirectoryName + "\n";

						IResourceRepository repo = NWN2CampaignManager.Instance.ActiveCampaign.Repository;
						IResourceEntry resource = repo.FindResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						if (resource != null)
						{
							info += "remove prior resource and blueprint\n";
							repo.Resources.Remove(resource);
						}
						else
							info += "no prior blueprint found in resource\n";

						MessageBox.Show(info);

//						IResourceEntry resource = repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
//						AddBlueprint(iblueprint, NWN2BlueprintLocationType.Campaign);
					}
// OVERRIDE ->
//					NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.UserOverrideDirectory;
//					NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.OverrideDirectory;
//					NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.BaseDirectory;
					else if (dir == NWN2ResourceManager.Instance.UserOverrideDirectory.DirectoryName)
					{
						info += "useroverride dir= " + NWN2ResourceManager.Instance.UserOverrideDirectory + "\n";

						IResourceRepository repo = NWN2ResourceManager.Instance.UserOverrideDirectory;
						IResourceEntry resource = repo.FindResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						if (resource != null)
						{
							info += "remove prior resource and blueprint\n";
							repo.Resources.Remove(resource);
						}
						else
							info += "no prior blueprint found in resource\n";

						MessageBox.Show(info);

//						IResourceEntry resource = repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
						repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType);
//						AddBlueprint(iblueprint, NWN2BlueprintLocationType.Global);
					}
				}
				MessageBox.Show("New resource:\n" + GetResourceInfo(iblueprint as INWN2Template));
			}
		}
		#endregion Methods (internal)


		#region Methods (private)
		/// <summary>
		/// Creates a blueprint and its resource for a given instance.
		/// </summary>
		/// <param name="iinstance"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		static INWN2Blueprint CreateBlueprint(INWN2Instance iinstance, NWN2BlueprintLocationType location)
		{
			INWN2Blueprint iblueprint = null;

			switch (iinstance.ObjectType)
			{
				case NWN2ObjectType.Creature:  iblueprint = new NWN2CreatureBlueprint();  break;
				case NWN2ObjectType.Door:      iblueprint = new NWN2DoorBlueprint();      break;
				case NWN2ObjectType.Placeable: iblueprint = new NWN2PlaceableBlueprint(); break;
			}

			if (iblueprint != null)
			{
				// NWN2Toolset.NWN2.Data.Instances.NWN2CreatureInstance.CreateBlueprintFromInstance()

				iblueprint.CopyFromTemplate(iinstance as INWN2Template); // does not copy inventory or equipped!

				// workaround toolset glitch re. template resref string ->
				// Loading a module with an instance that doesn't have a template
				// resref string isn't the same as deleting that string. And vice
				// versa: loading a module with an instance that has an invalid
				// template resref string isn't the same as filling a blank string
				// with an invalid resref string.
				//
				// So basically go through things step by step to ensure that all
				// this resource crap exits the poop chute w/out constipation.

				OEIResRef resref;
				if (   iinstance.Template == null
					|| iinstance.Template.ResRef == null
					|| iinstance.Template.ResRef.IsEmpty())
				{
					string val = iinstance.Name; // create a resref based on tag
					if (String.IsNullOrEmpty(val))
						val = "object";

					resref = new OEIResRef(val);
				}
				else
					resref = iinstance.Template.ResRef;

				iblueprint.TemplateResRef = resref;

				iblueprint.Resource = iinstance.Template;
				if (iblueprint.Resource == null)
					iblueprint.Resource = new MissingResourceEntry(resref);

				if (iblueprint.Resource.ResourceType == 0) // .RES file
				{
					ushort utype = 0;
					if      (iblueprint is NWN2CreatureBlueprint)  utype = (ushort)2027;
					else if (iblueprint is NWN2DoorBlueprint)      utype = (ushort)2042;
					else if (iblueprint is NWN2PlaceableBlueprint) utype = (ushort)2044;

					iblueprint.Resource.ResourceType = utype;
				}

				if (   iblueprint.Resource.ResRef == null
					|| iblueprint.Resource.ResRef.IsEmpty())
				{
					iblueprint.Resource.ResRef = resref;
				}

				IResourceRepository repo = null;

				switch (location)
				{
					case NWN2BlueprintLocationType.Module:
						repo = NWN2ToolsetMainForm.App.Module.Repository;
						break;

					case NWN2BlueprintLocationType.Campaign:
						repo = NWN2CampaignManager.Instance.ActiveCampaign.Repository;
						break;

					case NWN2BlueprintLocationType.Global:
						repo = NWN2ResourceManager.Instance.UserOverrideDirectory;
						break;
				}
				iblueprint.Resource = repo.CreateResource(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType); // god they're fucking luny: Does a resource belong to a blueprint or to a repository(!)

//				DirectoryResourceEntry(string sFilename, IResourceRepository cRepository)
//				OEIResRef cResRef = null;
//				CommonUtils.ConvertFilenameToResRefAndType(sFilename, out cResRef, out m_usResourceType);
//				m_cRepository = cRepository;
//				m_cResRef.Value = cResRef.Value;
//				Initialize(); ->

//				OEIResRef cResRef = m_cResRef;
//				cResRef.ValueChanged = (ResRefChangedHandler)Delegate.Combine(cResRef.ValueChanged, new ResRefChangedHandler(HandleResourceRenamed));
//				base.Initialize(); ->

//				OEIResRef cResRef = m_cResRef;
//				cResRef.ValueChanged = (ResRefChangedHandler)Delegate.Combine(cResRef.ValueChanged, new ResRefChangedHandler(ᐁ));




//				iblueprint.BlueprintLocation = NWN2BlueprintLocationType.Module; // default (shall not be needed/used since the location shall be set as necessary by SaveTo())



				iblueprint.Comment = iinstance.Comment;

				if ((iinstance as INWN2Template).ObjectType == NWN2ObjectType.Creature)
				{
// copy equipped ->
					(iblueprint as NWN2CreatureTemplate).EquippedItems = new NWN2EquipmentSlotCollection();
					NWN2InventoryItem equipped;
					for (int i = 0; i != 18; ++i)
					{
						if ((equipped = (iinstance as NWN2CreatureTemplate).EquippedItems[i].Item) != null && equipped.ValidItem)
						{
							(iblueprint as NWN2CreatureTemplate).EquippedItems[i].Item = new NWN2BlueprintInventoryItem(equipped as NWN2InstanceInventoryItem);
						}
					}
// copy inventory ->
					(iblueprint as NWN2CreatureBlueprint).Inventory = new NWN2BlueprintInventoryItemCollection();
					foreach (NWN2InstanceInventoryItem item in (iinstance as NWN2CreatureInstance).Inventory)
					{
						(iblueprint as NWN2CreatureBlueprint).Inventory.Add(new NWN2BlueprintInventoryItem(item));
					}
				}
			}

			return iblueprint; // a blueprint with a resource for an Instance
		}

/*		static void CreateResource(INWN2Blueprint iblueprint)
		{
			// NWN2Toolset.NWN2.Views.NWN2BlueprintView.ᐁ(NWN2ObjectType, NWN2BlueprintLocationType)
			// return: INWN2Blueprint

//			NWN2BlueprintCollection blueprintCollectionForType = iNWN2BlueprintSet.GetBlueprintCollectionForType(P_0);
//			INWN2Object iNWN2Object = iNWN2Blueprint as INWN2Object;
//			iNWN2Object.Tag = iNWN2Blueprint.ResourceName.Value;
//			iNWN2Object.LocalizedName[BWLanguages.CurrentLanguage] = iNWN2Blueprint.ResourceName.Value;
//			iNWN2Blueprint.BlueprintLocation = iNWN2BlueprintSet.BlueprintLocation;
//			blueprintCollectionForType.Add(iNWN2Blueprint);


//			IResourceRepository repo = NWN2ToolsetMainForm.App.BlueprintView.Module.Repository;
//			switch (NWN2BlueprintLocationType.Module)
//			{
//				case NWN2BlueprintLocationType.Global:
//					repo = NWN2Toolset.NWN2.IO.NWN2ResourceManager.Instance.UserOverrideDirectory;
//					if (repo != null)
//					{
//						repo = NWN2ResourceManager.Instance.OverrideDirectory;
//					}
//					blueprints = NWN2GlobalBlueprintManager.Instance;
//					break;
//
//				case NWN2BlueprintLocationType.Module:
//					repo = NWN2ToolsetMainForm.App.BlueprintView.Module.Repository;
//					blueprints = NWN2ToolsetMainForm.App.BlueprintView.Module;
//					break;
//
//				case NWN2BlueprintLocationType.Campaign:
//					repo = NWN2CampaignManager.Instance.ActiveCampaign.Repository;
//					blueprints = NWN2CampaignManager.Instance.ActiveCampaign;
//					break;
//			}

//			var @object = _panel.Blueprint as INWN2Object;
//			@object.Tag = _panel.Blueprint.ResourceName.Value; // aka. _panel.Blueprint.Resource.ResRef.Value
//			@object.LocalizedName[BWLanguages.CurrentLanguage] = _panel.Blueprint.ResourceName.Value;

			if (!NWN2ResourceManager.Instance.Exists(iblueprint.Resource.ResRef, iblueprint.Resource.ResourceType)) // doesn't match repositories
			{
				MessageBox.Show("create resource:\n" + GetResourceInfo(iblueprint as INWN2Template));

				var blueprints = NWN2ToolsetMainForm.App.Module as INWN2BlueprintSet;
				iblueprint.BlueprintLocation = blueprints.BlueprintLocation;

				string info = "location= " + iblueprint.BlueprintLocation + "\n";

				NWN2BlueprintCollection collection = blueprints.GetBlueprintCollectionForType(NWN2ObjectType.Creature);
				if (!collection.Contains(iblueprint))
				{
					info += "collection does not contain blueprint: add it\n";
					collection.Add(iblueprint);

					var viewer = NWN2ToolsetMainForm.App.BlueprintView;
					if (viewer.GetFocusedListObjectType() == (iblueprint as INWN2Template).ObjectType)
					{
						var list = viewer.GetFocusedList();
						info += "focused list is ObjectType (" + (iblueprint as INWN2Template).ObjectType + "): resort it";
						list.Resort();
					}
					else
						info += "focused list is not ObjectType: ignore it\n";
				}
				else
					info += "collection already contains blueprint\n";

				MessageBox.Show(info);
			}
			else
				MessageBox.Show("resource already exists:\n" + GetResourceInfo(iblueprint as INWN2Template));
		} */
		#endregion Methods (private)



		#region Methods (stupid)
		static string GetResourceInfo(INWN2Template itemplate)
		{
			string info = "tag= " + (itemplate as INWN2Template).Name + "\n";														// string

			var iinstance = itemplate as INWN2Instance;
			if (iinstance != null)
			{
				if ((iinstance as INWN2Instance).Template != null)																	// IResourceEntry
				{
					info += "Template.ResourceType= " + (iinstance as INWN2Instance).Template.ResourceType + "\n";					// ushort
					info += "Template.FullName= "     + (iinstance as INWN2Instance).Template.FullName     + "\n";					// string

					if ((iinstance as INWN2Instance).Template.ResRef != null)														// OEIResRef
					{
						info += "Template.ResRef.Value= " + (iinstance as INWN2Instance).Template.ResRef.Value + "\n";				// string
					}
					else
						info += "Template.Resref NULL\n";

					if ((iinstance as INWN2Instance).Template.Repository != null)													// IResourceRepository
					{
						info += "Template.Repository.Name= " + (iinstance as INWN2Instance).Template.Repository.Name + "\n";		// string
					}
					else
						info += "Template.Repository NULL\n";
				}
				else
					info += "Template NULL\n";
			}
			else
			{
				var iblueprint = itemplate as INWN2Blueprint;
				if (iblueprint != null)
				{
//					iblueprint.ResourceName; // <- is Resource.ResRef

					info += "BlueprintLocation= " + Enum.GetName(typeof(NWN2BlueprintLocationType), iblueprint.BlueprintLocation) + "\n";

					if ((iblueprint as INWN2Blueprint).Resource != null)															// IResourceEntry
					{
						info += "Resource.ResourceType= " + (iblueprint as INWN2Blueprint).Resource.ResourceType + "\n";			// ushort
						info += "Resource.FullName= "     + (iblueprint as INWN2Blueprint).Resource.FullName     + "\n";			// string

						if (iblueprint.Resource.ResRef != null)
						{
							info += "Resource.ResRef.Value= " + (iblueprint as INWN2Blueprint).Resource.ResRef.Value + "\n";		// string
						}
						else
							info += "Resource.ResRef NULL\n";

						if ((iblueprint as INWN2Blueprint).Resource.Repository != null)												// IResourceRepository
						{
							info += "Resource.Repository.Name= " + (iblueprint as INWN2Blueprint).Resource.Repository.Name + "\n";	// string
						}
						else
							info += "iblueprint.Resource.Repository NULL\n";
					}
					else
						info += "Resource NULL\n";

					if ((iblueprint as INWN2Blueprint).TemplateResRef != null)														// OEIResRef
					{
						info += "TemplateResRef.Value= " + (iblueprint as INWN2Blueprint).TemplateResRef.Value + "\n";				// string
					}
					else
						info += "TemplateResRef NULL\n";
				}
			}
			return info;
		}


/*		static void PrintResourceTypes()
		{
//			// Create a file to write to.
//			string createText = "Hello and Welcome" + Environment.NewLine;
//			File.WriteAllText(path, createText);
//			
//			// Open the file to read from.
//			string readText = File.ReadAllText(path);

			string info = String.Empty;

			info += "OEIShared.Utils.BWResourceTypes" + Environment.NewLine + Environment.NewLine;

			info += "0= " + StringDecryptor.Decrypt("ᒻᒮᒼ") + Environment.NewLine;
			info += "1= " + StringDecryptor.Decrypt("ᒫᒶᒹ") + Environment.NewLine;
			info += "2= " + StringDecryptor.Decrypt("ᒶᒿᒮ") + Environment.NewLine;
			info += "3= " + StringDecryptor.Decrypt("ᒽᒰᒪ") + Environment.NewLine;
			info += "4= " + StringDecryptor.Decrypt("ᓀᒪᒿ") + Environment.NewLine;
			info += "5= " + StringDecryptor.Decrypt("ᓀᒯᓁ") + Environment.NewLine;
			info += "6= " + StringDecryptor.Decrypt("ᒹᒵᒽ") + Environment.NewLine;
			info += "7= " + StringDecryptor.Decrypt("ᓒᓗᓒ") + Environment.NewLine;
			info += "8= " + StringDecryptor.Decrypt("ᒶᒹᒜ") + Environment.NewLine;
			info += "9= " + StringDecryptor.Decrypt("ᒶᒹᒰ") + Environment.NewLine;
			info += "10= " + StringDecryptor.Decrypt("ᒽᓁᒽ") + Environment.NewLine;
			info += "2000= " + StringDecryptor.Decrypt("ᒹᒵᒱ") + Environment.NewLine;
			info += "2001= " + StringDecryptor.Decrypt("ᒽᒮᓁ") + Environment.NewLine;
			info += "2002= " + StringDecryptor.Decrypt("ᒶᒭᒵ") + Environment.NewLine;
			info += "2003= " + StringDecryptor.Decrypt("ᒽᒱᒰ") + Environment.NewLine;
			info += "2005= " + StringDecryptor.Decrypt("ᒯᒷᒽ") + Environment.NewLine;
			info += "2007= " + StringDecryptor.Decrypt("ᒵᒾᒪ") + Environment.NewLine;
			info += "2008= " + StringDecryptor.Decrypt("ᒼᒵᒽ") + Environment.NewLine;
			info += "2009= " + StringDecryptor.Decrypt("ᒷᒼᒼ") + Environment.NewLine;
			info += "2010= " + StringDecryptor.Decrypt("ᒷᒬᒼ") + Environment.NewLine;
			info += "2011= " + StringDecryptor.Decrypt("ᒶᒸᒭ") + Environment.NewLine;
			info += "2012= " + StringDecryptor.Decrypt("ᒪᒻᒮ") + Environment.NewLine;
			info += "2013= " + StringDecryptor.Decrypt("ᒼᒮᒽ") + Environment.NewLine;
			info += "2014= " + StringDecryptor.Decrypt("ᒲᒯᒸ") + Environment.NewLine;
			info += "2015= " + StringDecryptor.Decrypt("ᒫᒲᒬ") + Environment.NewLine;
			info += "2016= " + StringDecryptor.Decrypt("ᓀᒸᒴ") + Environment.NewLine;
			info += "2017= " + StringDecryptor.Decrypt("ᒛᒭᒪ") + Environment.NewLine;
			info += "2018= " + StringDecryptor.Decrypt("ᒽᒵᒴ") + Environment.NewLine;
			info += "2022= " + StringDecryptor.Decrypt("ᒽᓁᒲ") + Environment.NewLine;
			info += "2023= " + StringDecryptor.Decrypt("ᒰᒲᒽ") + Environment.NewLine;
			info += "2024= " + StringDecryptor.Decrypt("ᒫᒽᒲ") + Environment.NewLine;
			info += "2025= " + StringDecryptor.Decrypt("ᒾᒽᒲ") + Environment.NewLine;
			info += "2026= " + StringDecryptor.Decrypt("ᒫᒽᒬ") + Environment.NewLine;
			info += "2027= " + StringDecryptor.Decrypt("ᒾᒽᒬ") + Environment.NewLine;
			info += "2029= " + StringDecryptor.Decrypt("ᒭᒵᒰ") + Environment.NewLine;
			info += "2030= " + StringDecryptor.Decrypt("ᒲᒽᒹ") + Environment.NewLine;
			info += "2031= " + StringDecryptor.Decrypt("ᒫᒽᒽ") + Environment.NewLine;
			info += "2032= " + StringDecryptor.Decrypt("ᒾᒽᒽ") + Environment.NewLine;
			info += "2033= " + StringDecryptor.Decrypt("ᒭᒭᒼ") + Environment.NewLine;
			info += "2034= " + StringDecryptor.Decrypt("ᒫᒽᒼ") + Environment.NewLine;
			info += "2035= " + StringDecryptor.Decrypt("ᒾᒽᒼ") + Environment.NewLine;
			info += "2036= " + StringDecryptor.Decrypt("ᒵᒽᒻ") + Environment.NewLine;
			info += "2037= " + StringDecryptor.Decrypt("ᒰᒯᒯ") + Environment.NewLine;
			info += "2038= " + StringDecryptor.Decrypt("ᒯᒪᒬ") + Environment.NewLine;
			info += "2039= " + StringDecryptor.Decrypt("ᒫᒽᒮ") + Environment.NewLine;
			info += "2040= " + StringDecryptor.Decrypt("ᒾᒽᒮ") + Environment.NewLine;
			info += "2041= " + StringDecryptor.Decrypt("ᒫᒽᒭ") + Environment.NewLine;
			info += "2042= " + StringDecryptor.Decrypt("ᒾᒽᒭ") + Environment.NewLine;
			info += "2043= " + StringDecryptor.Decrypt("ᒫᒽᒹ") + Environment.NewLine;
			info += "2044= " + StringDecryptor.Decrypt("ᒾᒽᒹ") + Environment.NewLine;
			info += "2045= " + StringDecryptor.Decrypt("ᒭᒯᒽ") + Environment.NewLine;
			info += "2046= " + StringDecryptor.Decrypt("ᒰᒲᒬ") + Environment.NewLine;
			info += "2047= " + StringDecryptor.Decrypt("ᒰᒾᒲ") + Environment.NewLine;
			info += "2048= " + StringDecryptor.Decrypt("ᒬᒼᒼ") + Environment.NewLine;
			info += "2049= " + StringDecryptor.Decrypt("ᒬᒬᒼ") + Environment.NewLine;
			info += "2050= " + StringDecryptor.Decrypt("ᒫᒽᒶ") + Environment.NewLine;
			info += "2051= " + StringDecryptor.Decrypt("ᒾᒽᒶ") + Environment.NewLine;
			info += "2052= " + StringDecryptor.Decrypt("ᒭᓀᒴ") + Environment.NewLine;
			info += "2053= " + StringDecryptor.Decrypt("ᒹᓀᒴ") + Environment.NewLine;
			info += "2054= " + StringDecryptor.Decrypt("ᒫᒽᒰ") + Environment.NewLine;
			info += "2055= " + StringDecryptor.Decrypt("ᒾᒽᒰ") + Environment.NewLine;
			info += "2056= " + StringDecryptor.Decrypt("ᒳᒻᒵ") + Environment.NewLine;
			info += "2057= " + StringDecryptor.Decrypt("ᒼᒪᒿ") + Environment.NewLine;
			info += "2058= " + StringDecryptor.Decrypt("ᒾᒽᓀ") + Environment.NewLine;
			info += "2059= " + StringDecryptor.Decrypt("ᒝᒹᒬ") + Environment.NewLine;
			info += "2060= " + StringDecryptor.Decrypt("ᒼᒼᒯ") + Environment.NewLine;
			info += "2061= " + StringDecryptor.Decrypt("ᒱᒪᒴ") + Environment.NewLine;
			info += "2062= " + StringDecryptor.Decrypt("ᒷᓀᒶ") + Environment.NewLine;
			info += "2063= " + StringDecryptor.Decrypt("ᒫᒲᒴ") + Environment.NewLine;
			info += "2064= " + StringDecryptor.Decrypt("ᒷᒭᒫ") + Environment.NewLine;
			info += "2065= " + StringDecryptor.Decrypt("ᒹᒽᒶ") + Environment.NewLine;
			info += "2066= " + StringDecryptor.Decrypt("ᒹᒽᒽ") + Environment.NewLine;
			info += "2067= " + StringDecryptor.Decrypt("ᒫᒪᒴ") + Environment.NewLine;
			info += "3000= " + StringDecryptor.Decrypt("ᒸᒼᒬ") + Environment.NewLine;
			info += "3001= " + StringDecryptor.Decrypt("ᒾᒼᒬ") + Environment.NewLine;
			info += "3002= " + StringDecryptor.Decrypt("ᒽᒻᒷ") + Environment.NewLine;
			info += "3003= " + StringDecryptor.Decrypt("ᒾᒽᒻ") + Environment.NewLine;
			info += "3004= " + StringDecryptor.Decrypt("ᒾᒮᒷ") + Environment.NewLine;
			info += "3005= " + StringDecryptor.Decrypt("ᒾᒵᒽ") + Environment.NewLine;
			info += "3006= " + StringDecryptor.Decrypt("ᒼᒮᒯ") + Environment.NewLine;
			info += "3007= " + StringDecryptor.Decrypt("ᒹᒯᓁ") + Environment.NewLine;
			info += "3008= " + StringDecryptor.Decrypt("ᒬᒪᒶ") + Environment.NewLine;
			info += "3009= " + StringDecryptor.Decrypt("ᒵᒯᓁ") + Environment.NewLine;
			info += "3010= " + StringDecryptor.Decrypt("ᒫᒯᓁ") + Environment.NewLine;
			info += "3011= " + StringDecryptor.Decrypt("ᒾᒹᒮ") + Environment.NewLine;
			info += "3012= " + StringDecryptor.Decrypt("ᒻᒸᒼ") + Environment.NewLine;
			info += "3013= " + StringDecryptor.Decrypt("ᒻᒼᒽ") + Environment.NewLine;
			info += "3014= " + StringDecryptor.Decrypt("ᒲᒯᓁ") + Environment.NewLine;
			info += "3015= " + StringDecryptor.Decrypt("ᒹᒯᒫ") + Environment.NewLine;
			info += "3016= " + StringDecryptor.Decrypt("ᓃᒲᒹ") + Environment.NewLine;
			info += "3017= " + StringDecryptor.Decrypt("ᓀᒶᒹ") + Environment.NewLine;
			info += "3018= " + StringDecryptor.Decrypt("ᒫᒫᓁ") + Environment.NewLine;
			info += "3019= " + StringDecryptor.Decrypt("ᒽᒯᓁ") + Environment.NewLine;
			info += "3020= " + StringDecryptor.Decrypt("ᓀᒵᒴ") + Environment.NewLine;
			info += "3021= " + StringDecryptor.Decrypt("ᓁᒶᒵ") + Environment.NewLine;
			info += "3022= " + StringDecryptor.Decrypt("ᒼᒬᒬ") + Environment.NewLine;
			info += "3033= " + StringDecryptor.Decrypt("ᒹᒽᓁ") + Environment.NewLine;
			info += "3034= " + StringDecryptor.Decrypt("ᒵᒽᓁ") + Environment.NewLine;
			info += "3035= " + StringDecryptor.Decrypt("ᒽᒻᓁ") + Environment.NewLine;
			info += "4007= " + StringDecryptor.Decrypt("ᒳᒹᒰ") + Environment.NewLine;
			info += "4008= " + StringDecryptor.Decrypt("ᒹᓀᒬ") + Environment.NewLine;
			info += "4000= " + StringDecryptor.Decrypt("ᒶᒭᒫ") + Environment.NewLine;
			info += "4001= " + StringDecryptor.Decrypt("ᒶᒭᒪ") + Environment.NewLine;
			info += "4002= " + StringDecryptor.Decrypt("ᒼᒹᒽ") + Environment.NewLine;
			info += "4003= " + StringDecryptor.Decrypt("ᒰᒻᒛ") + Environment.NewLine;
			info += "4004= " + StringDecryptor.Decrypt("ᒯᓁᒪ") + Environment.NewLine;
			info += "4005= " + StringDecryptor.Decrypt("ᒯᓁᒮ") + Environment.NewLine;
			info += "9999= " + StringDecryptor.Decrypt("ᒴᒮᓂ") + Environment.NewLine;
			info += "9998= " + StringDecryptor.Decrypt("ᒫᒲᒯ") + Environment.NewLine;
			info += "9997= " + StringDecryptor.Decrypt("ᒮᒻᒯ") + Environment.NewLine;
			info += "9996= " + StringDecryptor.Decrypt("ᒲᒭᒼ") + Environment.NewLine;
			info += ushort.MaxValue + "= \"\"" + Environment.NewLine;

			File.WriteAllText(@"C:\GIT\CreatureVisualizer\t\BWResourceTypes.txt", info);
			// ps. Fuck you. If you're going to release a toolset and expect it
			// to be user-friendly for the creation of plugins don't obfuscate
			// the shit that's needed to write those plugins.
		} */
		#endregion Methods (stupid)
	}
}
