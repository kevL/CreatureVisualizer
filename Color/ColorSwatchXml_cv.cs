using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Globalization;
using System.Text;
using System.Xml;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatchXml
	static class ColorSwatchXml
	{
		internal static List<ColorSwatch> ReadSwatches(string file, bool isResourceFile)
		{
			var swatches = new List<ColorSwatch>();
			XmlTextReader reader = null;

			try
			{
				if (!isResourceFile)
					reader = new XmlTextReader(file);
				else
					reader = new XmlTextReader(SanoResources.GetFileResource(file)); // TODO ->>

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
						string description = reader.ReadString();
						swatches.Add(new ColorSwatch(color, description));

						flag = false;
					}
				}
				return swatches;
			}
			finally
			{
				reader.Close();
			}
		}

		internal static void WriteSwatches(string file, ColorSwatch[] colors)
		{
			XmlTextWriter writer = null;
			try
			{
				writer = new XmlTextWriter(file, Encoding.UTF8);
				writer.Formatting = Formatting.Indented;
				writer.WriteStartDocument(standalone: false);
				writer.WriteStartElement("swatches");
				writer.WriteStartElement("swatch");
				writer.WriteAttributeString("id", "CustomSwatches");
				writer.WriteStartElement("colors");

				for (int i = 0; i != colors.Length; ++i)
				{
					ColorSwatch colorSwatch = colors[i];
					if (colorSwatch.Color != Color.Empty)
					{
						writer.WriteStartElement("color");
						writer.WriteAttributeString("red",   colorSwatch.Color.R.ToString());
						writer.WriteAttributeString("green", colorSwatch.Color.G.ToString());
						writer.WriteAttributeString("blue",  colorSwatch.Color.B.ToString());
						writer.WriteAttributeString("alpha", colorSwatch.Color.A.ToString());
						writer.WriteString(colorSwatch.Description);
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
	}
}
