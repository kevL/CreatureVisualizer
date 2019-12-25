using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatch
	sealed class ColorSwatch
	{
		#region Fields (static)
		const int width  = 10;
		const int height = 10;
		#endregion Fields (static)


		#region Properties
		internal Color Color
		{ get; set; }

		internal string Description
		{ get; set; }

		internal Point Location
		{ get; set; }

		internal Rectangle Rect
		{
			get { return new Rectangle(Location, new Size(width, height)); }
		}
		#endregion Properties


		#region cTor
		internal ColorSwatch(Point location)
		{
			Color       = Color.Empty;
			Description = String.Empty;
			Location    = location;
		}

		internal ColorSwatch(Color color, string description)
		{
			Color       = color;
			Description = description;
			Location    = new Point(0,0);
		}
		#endregion cTor


		#region Methods (override)
		public override bool Equals(object obj)
		{
			var colorSwatch = (ColorSwatch)obj;
			return colorSwatch.Color == Color
				&& Description != null && Description.Equals(colorSwatch.Description);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return "description: " + Description + " color: " + Color;
		}
		#endregion Methods (override)


		#region Operators
		public static bool operator ==(ColorSwatch swatch1, ColorSwatch swatch2)
		{
			return swatch1.Equals(swatch2);
		}

		public static bool operator !=(ColorSwatch swatch1, ColorSwatch swatch2)
		{
			return !swatch1.Equals(swatch2);
		}
		#endregion Operators
	}
}
