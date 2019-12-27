using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatch
	struct Swatch
	{
		#region Fields (static)
		const int width  = 10;
		const int height = 10;

		internal const string NoLabel = "unnamed";
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
		/// <summary>
		/// Creates a blank swatch for the tile-array.
		/// </summary>
		/// <param name="location"></param>
		internal Swatch(Point location)
			: this()
		{
			Color       = Color.Empty;
			Description = NoLabel;
			Location    = location;
		}

		/// <summary>
		/// Creates a colored swatch for the file-swatches.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="description"></param>
		internal Swatch(Color color, string description)
			: this()
		{
			Color       = color;
			Description = String.IsNullOrEmpty(description) ? NoLabel : description;
			Location    = new Point(0,0);
		}
		#endregion cTor


		#region Methods (override)
		public override bool Equals(object obj)
		{
			var swatch = (Swatch)obj;
			return swatch.Color == Color
				&& Description.Equals(swatch.Description);
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
		public static bool operator ==(Swatch swatch1, Swatch swatch2)
		{
			return swatch1.Equals(swatch2);
		}

		public static bool operator !=(Swatch swatch1, Swatch swatch2)
		{
			return !swatch1.Equals(swatch2);
		}
		#endregion Operators
	}
}
