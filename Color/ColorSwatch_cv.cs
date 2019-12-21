using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSwatch
	struct ColorSwatch
	{
		#region Properties
		Color _color;
		internal Color Color
		{
			get { return _color; }
			set { _color = value; }
		}

		string _description;
		internal string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		Point _location;
		internal Point Location
		{
			get { return _location; }
			set { _location = value; }
		}

		Size _size;
		internal Size Size // always 10x10
		{
			get { return _size; }
			set { _size = value; }
		}

		internal Rectangle Region
		{
			get { return new Rectangle(Location, Size); }
		}
		#endregion Properties


		#region cTor
		internal ColorSwatch(Point location, Size size)
		{
			this = new ColorSwatch(Color.Empty, String.Empty, location, size);
		}

		internal ColorSwatch(Color color, string description)
		{
			this = new ColorSwatch(color, description, new Point(0,0), new Size(10,10));
		}

		ColorSwatch(Color color, string description, Point location, Size size)
		{
			_color       = color;
			_description = description;
			_location    = location;
			_size        = size;
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
			return ((ValueType)this).GetHashCode();
		}

		public override string ToString()
		{
			return "Description: " + Description + " Color: " + Color;
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
