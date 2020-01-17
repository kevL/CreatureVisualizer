using System;
using System.IO;
using System.Windows.Forms;

using NWN2Toolset.NWN2.Data.Blueprints;
using NWN2Toolset.NWN2.Data.Instances;
using NWN2Toolset.NWN2.Data.Templates;

using OEIShared.IO;
//using OEIShared.IO.GFF;
using OEIShared.Utils;


namespace creaturevisualizer
{
	static class Io
	{
		// NWN2Toolset.NWN2.Views.NWN2BlueprintView.ᐌ(object P_0, EventArgs P_1)
		/// <summary>
		/// Saves the currently selected blueprint in the Blueprint tree to a
		/// user-labeled file.
		/// @note Check that blueprint is valid before call.
		/// </summary>
		/// <param name="blueprint"></param>
		internal static void SaveTo(INWN2Blueprint blueprint)
		{
			string ext = BWResourceTypes.GetFileExtension(blueprint.Resource.ResourceType);

			var sfd = new SaveFileDialog();
			sfd.Title      = "Save blueprint as ...";
			sfd.FileName   = blueprint.Resource.FullName;
			sfd.DefaultExt = ext;
			sfd.Filter     = "blueprints (*." + ext + ")|*." + ext + "|all files (*.*)|*.*";

			if (!String.IsNullOrEmpty( CreatureVisualizerPreferences.that.LastSaveDirectory))
				sfd.InitialDirectory = CreatureVisualizerPreferences.that.LastSaveDirectory;

			// else TODO: get BlueprintLocation dir

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				CreatureVisualizerPreferences.that.LastSaveDirectory = Path.GetDirectoryName(sfd.FileName);

				IOEISerializable serializable = blueprint;
				if (serializable != null)
					serializable.OEISerialize(sfd.FileName);
			}
		}

		/// <summary>
		/// Saves the currently selected instance in the Area viewer to a
		/// user-labeled file.
		/// @note Check that instance is valid before call.
		/// </summary>
		/// <param name="instance"></param>
		internal static void SaveTo(INWN2Instance instance)
		{
			//PrintResourceTypes(); // test

			// NWN2Toolset.NWN2.Views.NWN2AreaViewer.ᐠ(object P_0, EventArgs P_1)
			//
			// INWN2Blueprint        NWN2Toolset.NWN2.Data.Blueprints.NWN2GlobalBlueprintManager.CreateBlueprintOfInstance(INWN2Instance cInstance,
			//																											   INWN2BlueprintSet cSet,
			//																											   IResourceRepository cRepository,
			//																											   bool bNewName)
			// NWN2CreatureBlueprint NWN2Toolset.NWN2.Data.Instances .NWN2CreatureInstance      .CreateBlueprintFromInstance(NWN2CreatureInstance cInstance,
			//																												 IResourceRepository cRepository,
			//																												 bool bNewName)


			var inst = instance as NWN2CreatureInstance;
			if (inst != null)
			{
				INWN2Blueprint blueprint = CreateBlueprint(inst);
				if (blueprint != null)
				{
//					blueprint.BlueprintLocation = NWN2BlueprintLocationType.Module;

/*					string info = String.Empty;
					if (blueprint.Name != null)
						info = "blueprint.Name= " + blueprint.Name + "\n";											// d_sicklady_tag

					if (blueprint.Resource != null)
					{
						if (blueprint.Resource.FullName != null)
							info += "blueprint.Resource.FullName= " + blueprint.Resource.FullName + "\n";			// d_sicklady.RES, test_d_sicklady2.UTC, .RES, test_d_sicklady2_template.RES

						if (blueprint.Resource.ResRef != null && blueprint.Resource.ResRef.Value != null)
							info += "blueprint.Resource.ResRef.Value= " + blueprint.Resource.ResRef.Value + "\n";	// test_d_sicklady2, "", test_d_sicklady2_template


						if (blueprint.Resource.Repository != null && blueprint.Resource.Repository.Name != null)
							info += "blueprint.Resource.Repository.Name= " + blueprint.Resource.Repository.Name;
					}

					if (blueprint.ResourceName != null && blueprint.ResourceName.Value != null)
						info += "blueprint.ResourceName.Value= " + blueprint.ResourceName.Value + "\n";				// test_d_sicklady2, "", test_d_sicklady2_template

					if (blueprint.TemplateResRef != null && blueprint.TemplateResRef.Value != null)
						info += "blueprint.TemplateResRef.Value= " + blueprint.TemplateResRef.Value + "\n\n";		// test_d_sicklady2, "", test_d_sicklady2_template


					if (inst.Name != null)
						info += "inst.Name= " + inst.Name + "\n";													// d_sicklady_tag

					if (inst.Template != null)
					{
						if (inst.Template.FullName != null)
							info += "inst.Template.FullName= " + inst.Template.FullName + "\n";						// d_sicklady.RES, test_d_sicklady2.UTC, .RES, test_d_sicklady2_template.RES

						if (inst.Template.ResRef != null && inst.Template.ResRef.Value != null)
							info += "inst.Template.ResRef.Value= " + inst.Template.ResRef.Value + "\n";				// test_d_sicklady2, "", test_d_sicklady2_template


						if (inst.Template.Repository != null && inst.Template.Repository.Name != null)
							info += "inst.Template.Repository.Name= " + inst.Template.Repository.Name;				// null, C:\Users\User\Documents\Neverwinter Nights 2\Override, null
					}
					MessageBox.Show(info); */

					SaveTo(blueprint);
				}
			}
		}

		static INWN2Blueprint CreateBlueprint(NWN2CreatureInstance instance)
		{
			// cf CreVisF.click_bu_creature_display()

			if (instance.Template == null || instance.Template.ResourceType != (ushort)2027) // utc
			{
				CreVisF.BypassCreate = true;

				using (var f = new ErrorF("The instance's Template is invalid."))
					f.ShowDialog();

				CreVisF.BypassCreate = false;
			}
			else
			{
				// TODO: This is all suspect ->>

				var blueprint = new NWN2CreatureBlueprint();
				blueprint.CopyFromTemplate(instance);

				blueprint.Resource = instance.Template;
				blueprint.TemplateResRef = instance.Template.ResRef;

				blueprint.Comment = instance.Comment;

				if (CreatureVisualizerPreferences.that.HandleEquippedItems)
				{
					NWN2InventoryItem it;
//					blueprint.EquippedItems = CommonUtils.SerializationClone(instance.EquippedItems) as NWN2EquipmentSlotCollection; // huh
					blueprint.EquippedItems = new NWN2EquipmentSlotCollection();
					for (int i = 0; i != 18; ++i)
					{
						if ((it = instance.EquippedItems[i].Item) != null && it.ValidItem)
						{
							blueprint.EquippedItems[i].Item = new NWN2BlueprintInventoryItem(it as NWN2InstanceInventoryItem);
						}
					}
				}
				//else TODO: Might have to clear equipment here

				if (CreatureVisualizerPreferences.that.HandleInventoryItems)
				{
					blueprint.Inventory = new NWN2BlueprintInventoryItemCollection();
					foreach (NWN2InstanceInventoryItem itInst in instance.Inventory)
					{
						blueprint.Inventory.Add(new NWN2BlueprintInventoryItem(itInst));
					}
				}
				//else TODO: Might have to clear inventory here

				return blueprint;
			}
			return null;
		}
		// NWN2Toolset.NWN2.Data.Instances.NWN2CreatureInstance
/*		public static NWN2CreatureBlueprint CreateBlueprintFromInstance(NWN2CreatureInstance instance, IResourceRepository repository, bool rename)
		{
			var blueprint = new NWN2CreatureBlueprint();
			blueprint.CopyFromTemplate(instance);
			blueprint.Resource = instance.Template;
			blueprint.Comment = instance.Comment;
			blueprint.TemplateResRef = instance.Template.ResRef;

			blueprint.EquippedItems = CommonUtils.SerializationClone(instance.EquippedItems) as NWN2EquipmentSlotCollection;
			blueprint.EquippedItems = new NWN2EquipmentSlotCollection();

			for (int i = 0; i != 18; ++i)
			{
				if (instance.EquippedItems[i].Item != null && instance.EquippedItems[i].Item.ValidItem)
				{
					blueprint.EquippedItems[i].Item = null;// new NWN2BlueprintInventoryItem(instance.EquippedItems[i].Item as NWN2InstanceInventoryItem);
				}
			}

			blueprint.Inventory = new NWN2BlueprintInventoryItemCollection();

			foreach (NWN2InstanceInventoryItem itInstance in instance.Inventory)
			{
				var itBlueprint = new NWN2BlueprintInventoryItem(itInstance);
				blueprint.Inventory.Add(itBlueprint);
			}

			if (repository != null)
			{
				OEIResRef resref = rename ? repository.GetTempResRef(instance.Template.ResRef, blueprint.ResourceType) : instance.Template.ResRef;

				blueprint.Resource = repository.CreateResource(resref, blueprint.ResourceType);
				var file = new GFFFile();
				file.FileHeader.FileType = BWResourceTypes.GetFileExtension(blueprint.ResourceType);

				blueprint.SaveEverythingIntoGFFStruct(file.TopLevelStruct, true);
				using (Stream str = blueprint.Resource.GetStream(true))
				{
					file.Save(str);
					blueprint.Resource.Release();
				}
			}
			return blueprint;
		} */


		static void PrintResourceTypes()
		{
/*			// Create a file to write to.
			string createText = "Hello and Welcome" + Environment.NewLine;
			File.WriteAllText(path, createText);
			
			// Open the file to read from.
			string readText = File.ReadAllText(path); */

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
		}
	}
}
