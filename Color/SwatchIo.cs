using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatchXml
	static class SwatchIo
	{
		#region Fields (static)
		internal static string Fullpath;
		internal static string SwatchFile = "NWN2 Toolset" + Path.DirectorySeparatorChar
										  + "CustomSwatches.xml";

		static bool _created;
		internal static bool _errored;
		#endregion Fields (static)


		#region Methods (static)
		internal static List<Swatch> Read(string path)
		{
			XmlReader reader = null;
			try
			{
				var swatches = new List<Swatch>();

				reader = XmlReader.Create(path);

				string val;
				int r = 0;
				int g = 0;
				int b = 0;
				int a = 255;

				bool checkfortext = false;

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "color")
					{
						if ((val = reader.GetAttribute("red")) == null
							|| !Int32.TryParse(val, out r)
							|| r < 0 || r > 255)
						{
							r = 0;
						}

						if ((val = reader.GetAttribute("green")) == null
							|| !Int32.TryParse(val, out g)
							|| g < 0 || g > 255)
						{
							g = 0;
						}

						if ((val = reader.GetAttribute("blue")) == null
							|| !Int32.TryParse(val, out b)
							|| b < 0 || b > 255)
						{
							b = 0;
						}

						if ((val = reader.GetAttribute("alpha")) == null
							|| !Int32.TryParse(val, out a)
							|| a < 0 || a > 255)
						{
							a = 255;
						}

						checkfortext = true;	// -> deal w/ text on next iteration (ie. the text is not in
					}							// an element per se, so the reader has to do its next read)
					else if (checkfortext)
					{
						checkfortext = false;
						if (reader.NodeType == XmlNodeType.Text)
						{
							Color color = Color.FromArgb(a,r,g,b);
							swatches.Add(new Swatch(color, reader.ReadString()));
						}
					}
				}

				if (!_created)
					ColorF.That.Print("Swatch file loaded", true);
				else
					ColorF.That.Print("Swatch file created", true);

				return swatches;
			}
			catch
			{
				ColorF.That.Print("ERROR reading swatch file", true);
				return null;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
					((IDisposable)reader).Dispose(); // not sure if both are needed
				}
			}
		}

		internal static void Write(Swatch[] tiles)
		{
			XmlWriter writer = null;
			try
			{
				var @set = new XmlWriterSettings();
				@set.Indent = true;

				writer = XmlWriter.Create(Fullpath, @set);

				writer.WriteStartDocument();

				writer.WriteStartElement("swatches");
				writer.WriteStartElement("swatch");
				writer.WriteAttributeString("id", "GeneralRgb");	// kL_note: used to be "CustomSwatches"
				writer.WriteStartElement("colors");					// but my CustomSwatches.xml file, which hasn't been touched
																	// says "GeneralRgb"

				Swatch swatch;
				for (int i = 0; i != tiles.Length; ++i)
				{
					swatch = tiles[i];
					if (swatch.Color != Color.Empty)
					{
						writer.WriteStartElement("color");

						writer.WriteAttributeString("red",   swatch.Color.R.ToString());
						writer.WriteAttributeString("green", swatch.Color.G.ToString());
						writer.WriteAttributeString("blue",  swatch.Color.B.ToString());
						writer.WriteAttributeString("alpha", swatch.Color.A.ToString());

						writer.WriteString(swatch.Description);

						writer.WriteEndElement(); // "/color"
					}
					else
						break;
				}

				writer.WriteEndElement(); // "/colors"
				writer.WriteEndElement(); // "/swatch"
				writer.WriteEndElement(); // "/swatches"
				writer.WriteEndDocument();

				ColorF.That.Print("Swatch file saved", true);
			}
			catch
			{
				_errored = true;
				ColorF.That.Print("ERROR writing swatch file", true);
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
					((IDisposable)writer).Dispose(); // not sure if both are needed
				}
			}
		}


		const string RES = "CreatureVisualizer.Color.CustomSwatches.xml";

		internal static void Create()
		{
			StreamReader reader = null;
			StreamWriter writer = null;
			try
			{
				var ass = Assembly.GetExecutingAssembly();
				var str = ass.GetManifestResourceStream(RES);
				reader = new StreamReader(str);

				Directory.CreateDirectory(Path.GetDirectoryName(Fullpath));

				writer = new StreamWriter(Fullpath);
				writer.Write(reader.ReadToEnd());
				writer.Flush();

				_created = true;
			}
			catch
			{
				_errored = true;
				ColorF.That.Print("ERROR creating swatch file", true);
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
					((IDisposable)writer).Dispose(); // not sure if both are needed
				}

				if (reader != null)
				{
					reader.Close();
					((IDisposable)reader).Dispose(); // not sure if both are needed
				}
			}
		}
		#endregion Methods (static)
	}
}
