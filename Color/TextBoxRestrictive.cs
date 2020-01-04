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
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			Keys                  keydata = Keys.None;
			if      (e.Delta > 0) keydata = Keys.Add;
			else if (e.Delta < 0) keydata = Keys.Subtract;

			if (keydata != Keys.None)
				OnKeyDown(new KeyEventArgs(keydata));
		}

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


				case Keys.Add: // Keys.Oemplus
					if (Restrict != Type.Hecate)
					{
						int val = Int32.Parse(Text) + 1;
						switch (Restrict)
						{
							case Type.Degree:  if (val <= 359) goto case Type.Hecate; break;
							case Type.Percent: if (val <= 100) goto case Type.Hecate; break;
							case Type.Byte:    if (val <= 255) goto case Type.Hecate; break;
							case Type.Hecate:  Text = val.ToString();                 break;
						}
					}
					e.Handled = e.SuppressKeyPress = true;
					return;

				case Keys.Subtract: // Keys.OemMinus
					if (Restrict != Type.Hecate)
					{
						int val = Int32.Parse(Text) - 1;
						if (val >= 0) Text = val.ToString();
					}
					e.Handled = e.SuppressKeyPress = true;
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
					case Type.Degree:
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

		protected override void OnLeave(EventArgs e)
		{
			if (String.IsNullOrEmpty(Text)) // Recurse.
			{
				if (Restrict == Type.Hecate)
				{
					ColorControl._bypassHecate = true; // <- relevant only to the hecate-text
					Text = "000000";
					ColorControl._bypassHecate = false;
				}
				else
				{
					ColorControl._bypassCisco = true; // <- relevant only to the ciscos
					ColorControl._bypassAlpha = true; // <- relevant only to the alpha-text
					Text = "0";
					ColorControl._bypassAlpha = false;
					ColorControl._bypassCisco = false;
				}
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			string ifo = String.Empty;

			var parent = Parent as ColorSpaceControlCisco;
			if (parent != null)
			{
				switch (parent.DisplayCharacter)
				{
					case 'H': ifo = "Hue 0..359 degrees";        break;
					case 'S': ifo = "Saturation 0..100 percent"; break;
					case 'L': ifo = "Lightness 0..100 percent";  break;

					case 'R': ifo = "Red 0..255 byte";           break;
					case 'G': ifo = "Green 0..255 byte";         break;
					case 'B': ifo = "Blue 0..255 byte";          break;
				}
			}
			else
			{
				switch (Restrict)
				{
					case Type.Hecate: ifo = "rgb 000000..FFFFFF"; break;
					case Type.Byte:   ifo = "alpha 0..255";       break;
				}
			}

			ColorF.That.Print(ifo);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			ColorF.That.Print(String.Empty);
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
