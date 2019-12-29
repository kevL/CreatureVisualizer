using System;
using System.Collections.Generic;
using System.Drawing;
//using System.IO;
//using System.Globalization;
//using System.Text;
using System.Xml;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatchXml
	static class SwatchIo
	{
		internal static List<Swatch> Read(string path)
		{
//			XmlTextReader reader = null;
			XmlReader reader = null;
			try
			{
				var swatches = new List<Swatch>();

//				reader = new XmlTextReader(path);
//				var @set = new XmlReaderSettings();
//				@set.IgnoreComments   = true;
//				@set.IgnoreWhitespace = true;
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
							|| r < 0 || r > Byte.MaxValue)
						{
							r = 0;
						}

						if ((val = reader.GetAttribute("green")) == null
							|| !Int32.TryParse(val, out g)
							|| g < 0 || g > Byte.MaxValue)
						{
							g = 0;
						}

						if ((val = reader.GetAttribute("blue")) == null
							|| !Int32.TryParse(val, out b)
							|| b < 0 || b > Byte.MaxValue)
						{
							b = 0;
						}

						if ((val = reader.GetAttribute("alpha")) == null
							|| !Int32.TryParse(val, out a)
							|| a < 0 || a > Byte.MaxValue)
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
				ColorF.That.Print("Swatches file loaded");
				return swatches;
			}
			catch
			{
				ColorF.That.Print("ERROR reading swatches file");
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

		internal static void Write(string path, Swatch[] tiles)
		{
			var @set = new XmlWriterSettings();
			@set.Indent = true;

//			XmlTextWriter writer = null;
			XmlWriter writer = null;
			try
			{
//				writer = new XmlTextWriter(path, Encoding.UTF8);
//				writer.Formatting = Formatting.Indented;

				writer = XmlWriter.Create(path, @set);

				writer.WriteStartDocument(false);

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

						writer.WriteEndElement(); // "color"
					}
					else
						break;
				}

				writer.WriteEndElement(); // "colors"
				writer.WriteEndElement(); // "swatch"
				writer.WriteEndElement(); // "swatches"
				writer.WriteEndDocument();

				ColorF.That.Print("Swatch file saved");
			}
			catch //(IOException)
			{
				ColorF.That.Print("ERROR writing swatch file");
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


		internal static void Create()
		{
			// TODO: using etc.

/*			var streamReader = new StreamReader(SanoResources.GetFileResource("ColorSwatches.xml")); // TODO ->>
			if (streamReader != null)
			{
				StreamWriter streamWriter = null;
				try
				{
					string directoryName = Path.GetDirectoryName(CustomSwatchesFile);
					if (!Directory.Exists(directoryName))
						Directory.CreateDirectory(directoryName);

					streamWriter = new StreamWriter(CustomSwatchesFile);
					streamWriter.Write(streamReader.ReadToEnd());
					streamWriter.Flush();
				}
				catch (DirectoryNotFoundException)
				{}
//				catch (Exception)
//				{}
				finally
				{
					streamReader.Close();
					streamWriter.Close();
				}
			} */
		}
	}
}
/*		internal static List<ColorSwatch> ReadSwatches(string path, bool isResourceFile)
		{
			var swatches = new List<ColorSwatch>();
			XmlTextReader reader = null;

			try
			{
				if (!isResourceFile)
					reader = new XmlTextReader(path);
				else
					reader = new XmlTextReader(SanoResources.GetFileResource(path)); // TODO ->>

				int result1 = 0;
				int result2 = 0;
				int result3 = 0;
				int result4 = 255;

				bool flag = false;

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element
						&& reader.Name == "color")
//						&& reader.Name.CompareTo("color", CultureInfo.InvariantCulture) == 0)
					{
						string text = string.Empty;
						try
						{
							text = reader.GetAttribute("red");
						}
						catch (ArgumentOutOfRangeException)
						{}

						if (text == null || !Int32.TryParse(text, out result1))
							result1 = 0;

						text = String.Empty;
						try
						{
							text = reader.GetAttribute("green");
						}
						catch (ArgumentOutOfRangeException)
						{}

						if (text == null || !Int32.TryParse(text, out result2))
							result2 = 0;

						text = String.Empty;
						try
						{
							text = reader.GetAttribute("blue");
						}
						catch (ArgumentOutOfRangeException)
						{}

						if (text == null || !Int32.TryParse(text, out result3))
							result3 = 0;

						text = string.Empty;
						try
						{
							text = reader.GetAttribute("alpha");
						}
						catch (ArgumentOutOfRangeException)
						{}

						if (text == null || !Int32.TryParse(text, out result4))
							result4 = 255;

						flag = true;
					}
					else if (flag && reader.NodeType == XmlNodeType.Text)
					{
						Color color = Color.FromArgb(result4, result1, result2, result3);
						swatches.Add(new ColorSwatch(color, reader.ReadString()));

						flag = false;
					}
				}
				return swatches;
			}
			finally
			{
				reader.Close();
			}
		} */
