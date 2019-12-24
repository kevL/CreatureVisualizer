using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HexTextBox
	sealed class HexTextBox
		: ColorSpaceComponentTextBox
	{
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
			if (    ascii < 48
				|| (ascii > 57 && ascii < 65)
				|| (ascii > 70 && ascii < 97)
				||  ascii > 102)
			{
				e.Handled = e.SuppressKeyPress = true;
			}
		}
		#endregion Handlers (override)
	}
}
