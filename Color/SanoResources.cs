using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;


namespace creaturevisualizer
{
	// Sano.Utility.Resources
	static class SanoResources
	{
		public static Bitmap GetBitmapResource(string resourceFile)
		{
			Bitmap bitmap = null;
			if ((resourceFile != null) & (resourceFile.Length > 0))
			{
				Stream resourceStream = GetResourceStream(resourceFile);
				bitmap = (Bitmap)Image.FromStream(resourceStream);
				resourceStream.Close();
				return bitmap;
			}
			throw new ArgumentNullException("resourceFile", "resourceFile cannot be empty");
		}

		public static Icon GetIconResource(string resourceFile)
		{
			Icon icon = null;
			if ((resourceFile != null) & (resourceFile.Length > 0))
			{
				Stream resourceStream = GetResourceStream(resourceFile);
				icon = new Icon(resourceStream);
				resourceStream.Close();
				return icon;
			}
			throw new ArgumentNullException("resourceFile", "resourceFile cannot be empty.");
		}

		public static Stream GetFileResource(string resourceFile)
		{
			if (!String.IsNullOrEmpty(resourceFile))
				return GetResourceStream(resourceFile);

			return null;
		}

		static string CreateFullResourcePath(string resourceNamespace, string resourceFile)
		{
			if (!String.IsNullOrEmpty(resourceNamespace))
				return resourceNamespace + "." + resourceFile;

			return resourceFile;
		}

		static Stream GetResourceStream(string resourceFile)
		{
			var stackTrace = new StackTrace();
			Type type = stackTrace.GetFrame(2).GetMethod().DeclaringType;
			Assembly ass = type.Assembly;
			return ass.GetManifestResourceStream(CreateFullResourcePath(type.Namespace, resourceFile));
		}
	}
}
