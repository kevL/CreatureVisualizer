using System;
using System.IO;
using System.Windows.Forms;

using NWN2Toolset.NWN2.Data.Blueprints;

using OEIShared.IO;
using OEIShared.Utils;


namespace creaturevisualizer
{
	static class Io
	{
		// NWN2Toolset.NWN2.Views.NWN2BlueprintView.ᐌ(object P_0, EventArgs P_1)

		/// <summary>
		/// Saves the current blueprint to a user-labeled file.
		/// @note Check that blueprint is valid before call.
		/// </summary>
		/// <param name="blueprint"></param>
		internal static void SaveAs(INWN2Blueprint blueprint)
		{
			string ext = BWResourceTypes.GetFileExtension(blueprint.Resource.ResourceType);

			var sfd = new SaveFileDialog();
			sfd.Title      = "Save blueprint as ...";
			sfd.FileName   = blueprint.Resource.FullName;
			sfd.DefaultExt = ext;
			sfd.Filter     = "blueprints (*." + ext + ")|*." + ext + "|all files (*.*)|*.*";

			if (!String.IsNullOrEmpty( CreatureVisualizerPreferences.that.LastSaveDirectory))
				sfd.InitialDirectory = CreatureVisualizerPreferences.that.LastSaveDirectory;

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				CreatureVisualizerPreferences.that.LastSaveDirectory = Path.GetDirectoryName(sfd.FileName);

				IOEISerializable serializable = blueprint;
				if (serializable != null)
					serializable.OEISerialize(sfd.FileName);
			}
		}


/*		static string StringDecryptor(string st)
		{
			char[] array0;
			char[] array1 = (array0 = st.ToCharArray());
			int p1 = array1.Length;
			while (p1 != 0)
			{
				int p0 = p1 - 1;
				array1[p0] = (char)(array0[p0] - 5225);
				array1 = array0; // wtf.
				p1 = p0;
			}
			return String.Intern(new string(array1));
		} */
	}
}
