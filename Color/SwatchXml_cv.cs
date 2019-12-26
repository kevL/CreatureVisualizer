using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Globalization;
using System.Text;
using System.Xml;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatchXml
	static class SwatchXml
	{
		internal static List<Swatch> ReadSwatches(string path)
		{
			XmlTextReader reader = null;
			try
			{
				reader = new XmlTextReader(path);

				string text;
				int r = 0;
				int g = 0;
				int b = 0;
				int a = 255;

				bool flag = false;

				var swatchlist = new List<Swatch>();

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "color")
					{
						if ((text = reader.GetAttribute("red")) == null
							|| !Int32.TryParse(text, out r)
							|| r < 0 || r > Byte.MaxValue)
						{
							r = 0;
						}

						if ((text = reader.GetAttribute("green")) == null
							|| !Int32.TryParse(text, out g)
							|| g < 0 || g > Byte.MaxValue)
						{
							g = 0;
						}

						if ((text = reader.GetAttribute("blue")) == null
							|| !Int32.TryParse(text, out b)
							|| b < 0 || b > Byte.MaxValue)
						{
							b = 0;
						}

						if ((text = reader.GetAttribute("alpha")) == null
							|| !Int32.TryParse(text, out a)
							|| a < 0 || a > Byte.MaxValue)
						{
							a = 255;
						}

						flag = true;
					}
					else if (flag && reader.NodeType == XmlNodeType.Text)
					{
						Color color = Color.FromArgb(a,r,g,b);
						swatchlist.Add(new Swatch(color, reader.ReadString()));

						flag = false;
					}
				}
				return swatchlist;
			}
			finally
			{
				reader.Close();
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

		internal static void WriteSwatches(string path, Swatch[] colors)
		{
			XmlTextWriter writer = null;
			try
			{
				writer = new XmlTextWriter(path, Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument(standalone: false);
				writer.WriteStartElement("swatches");
				writer.WriteStartElement("swatch");
				writer.WriteAttributeString("id", "CustomSwatches"); // -> "GeneralRgb"
				writer.WriteStartElement("colors");

				for (int i = 0; i != colors.Length; ++i)
				{
					Swatch swatch = colors[i];
					if (swatch.Color != Color.Empty)
					{
						writer.WriteStartElement("color");
						writer.WriteAttributeString("red",   swatch.Color.R.ToString());
						writer.WriteAttributeString("green", swatch.Color.G.ToString());
						writer.WriteAttributeString("blue",  swatch.Color.B.ToString());
						writer.WriteAttributeString("alpha", swatch.Color.A.ToString());
						writer.WriteString(swatch.Description);
						writer.WriteEndElement();
					}
				}

				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
//			catch (IOException)
//			{
//				throw;
//			}
			finally
			{
				writer.Close();
			}
		}


		internal static void CreateCustomSwatchesFile()
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
