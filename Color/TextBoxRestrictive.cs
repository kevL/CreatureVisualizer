using System;
using System.Globalization;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class TextboxRestrictive
		: TextBox
	{
		internal enum Type
		{
			Degree,
			Percent,
			Byte,
			Hecate
		}


		#region Fields
		string _pre = String.Empty;
		#endregion Fields


		#region Properties
		public Type Restrict
		{ get; set; }
		#endregion Properties


		#region Handlers (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Left:
				case Keys.Right:
				case Keys.Shift | Keys.Left:  // select char left
				case Keys.Shift | Keys.Right: // select char right
				case Keys.Home:
				case Keys.End:
				case Keys.Shift | Keys.Home: // select to start
				case Keys.Shift | Keys.End:  // select to end
				case Keys.Back:
				case Keys.Delete:

				// don't bother w/
				// Ctrl+Home
				// Ctrl+End
				// Shift+Ctrl+Home
				// Shift+Ctrl+End
				// PageUp
				// PageDown

				case Keys.Control | Keys.C: // copy
				case Keys.Control | Keys.X: // cut
				case Keys.Control | Keys.V: // paste

				case Keys.Control | Keys.Insert: // copy
				case Keys.Shift   | Keys.Delete: // cut
				case Keys.Shift   | Keys.Insert: // paste
					return;
			}


			int ascii = (int)e.KeyData;
			switch (Restrict)
			{
				case Type.Degree:
				case Type.Percent:
				case Type.Byte:
					if (ascii < 48 || ascii > 57)
					{
						e.Handled = e.SuppressKeyPress = true;
					}
					break;

				case Type.Hecate:
					if (    ascii < 48
						|| (ascii > 57 && ascii < 65)
						|| (ascii > 70 && ascii < 97)
						||  ascii > 102)
					{
						e.Handled = e.SuppressKeyPress = true;
					}
					break;
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			bool bork = !String.IsNullOrEmpty(Text); // allow blank string

			if (bork)
			{
				int result;
				switch (Restrict)
				{
					default:
//					case Type.Degree:
						bork = !Int32.TryParse(Text, out result)
							|| result < 0 || result > 359;
						break;

					case Type.Percent:
						bork = !Int32.TryParse(Text, out result)
							|| result < 0 | result > 100;
						break;

					case Type.Byte:
						bork = !Int32.TryParse(Text, out result)
							|| result < 0 || result > 255;
						break;

					case Type.Hecate:
						bork = !Int32.TryParse(Text, NumberStyles.AllowHexSpecifier, null, out result)
							|| result < 0 || result > 0xFFFFFF;
						break;
				}
			}

			if (!bork)
			{
				_pre = Text;
				base.OnTextChanged(e);
			}
			else
			{
				Text = _pre; // revert & Recurse.
				SelectionLength = 0;
				SelectionStart = Text.Length;
			}
		}
		#endregion Handlers (override)


		#region Methods
		internal void SetRestrict(Type type)
		{
			Restrict = type;
		}
		#endregion Methods
	}
}
