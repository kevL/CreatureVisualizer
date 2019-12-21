using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HexTextBox
	sealed class HexTextBox
		: ColorSpaceComponentTextBox
	{
		#region Handlers (override)
		protected override bool IsInputChar(char charCode)
		{
			return base.IsInputChar(charCode)
				||  charCode == '.'
				|| (charCode >= 'A' && charCode <= 'F');
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (Text.Length > 0
				&& (   (e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.RButton | Keys.MButton)
					|| (e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.Back)))
			{
				string text = "0x" + Text;

				int val;
				int val1 = Convert.ToInt32(text, 16);
				int val2 = ((e.KeyData & Keys.Shift) != Keys.Shift) ? 1 : 10;

				if ((e.KeyData & Keys.Up) == Keys.Up)
				{
					val = Math.Min(val1 + val2, 16777215);
				}
				else
					val = Math.Max(val1 - val2, 0);

				Text = val.ToString("X6").ToUpper();

				if (e.KeyData == (Keys.Shift | Keys.Space | Keys.Back)) // wtf does that even mean
				{
					SelectionStart = Text.Length;
				}
			}
		}
		#endregion Handlers (override)
	}
}
