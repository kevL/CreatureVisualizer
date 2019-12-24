using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponentTextBox
	class ColorSpaceComponentTextBox
		: TextBox
	{
//		protected override void OnKeyDown(KeyEventArgs e)
//		{
//			e.Handled = e.SuppressKeyPress = (e.KeyValue > 47 && e.KeyValue < 58);
//		}


/*		#region Fields
		bool _caretHidden;
		#endregion Fields


		#region Handlers (override)
		protected override bool IsInputKey(Keys keyData)
		{
			if (   keyData == (Keys)262259
				|| keyData == (Keys)131137
				|| keyData == (Keys)131139
				|| keyData == (Keys)131158
				|| keyData == (Keys)131160) // its Maaaaaaaaaaagic
			{
				return true;
			}
			return IsInputChar((char)keyData);
		}

		protected override bool IsInputChar(char charCode)
		{
			return ((Control.ModifierKeys != Keys.Shift && charCode >= '0' && charCode <= '9')
				|| (charCode >= '`' && charCode <= 'i')
				|| (charCode >= '%' && charCode <= '(')
				||  charCode == '\b'
				||  charCode == '.');
		}

		[DllImport("user32.dll")]
		public static extern int ShowCaret(IntPtr hwnd);

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyData == (Keys)131137)
				SelectAll();

			if (_caretHidden
				&& (   IsKeyOrShiftPressed(e.KeyData, Keys.Up)
					|| IsKeyOrShiftPressed(e.KeyData, Keys.Down)))
			{
				ShowCaret(Handle);
				SelectionStart = Text.Length;
				_caretHidden = false;
			}

			base.OnKeyUp(e);
		}

		[DllImport("user32.dll")]
		public static extern int HideCaret(IntPtr hwnd);

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!_caretHidden
				&& (   IsKeyOrShiftPressed(e.KeyData, Keys.Up)
					|| IsKeyOrShiftPressed(e.KeyData, Keys.Down)))
			{
				HideCaret(Handle);
				_caretHidden = true;
			}

			base.OnKeyDown(e);
		}

		protected override bool ProcessKeyEventArgs(ref Message m)
		{
			if (IsInputKey((Keys)((int)m.WParam | (int)Control.ModifierKeys)))
				return base.ProcessKeyEventArgs(ref m);

			return false;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (Control.ModifierKeys == Keys.Alt)
				return false;

			switch (keyData)
			{
				case Keys.Alt:
				case Keys.Tab:
				case Keys.Back:
				case Keys.Back | Keys.Shift | Keys.LButton:
					return base.ProcessDialogKey(keyData);
			}
			return false;
		}

		protected override void OnLostFocus(EventArgs e)
		{
			if (Text != null && Text.Length == 0)
			{
				AttributeCollection attribs = TypeDescriptor.GetProperties(this)["Text"].Attributes;
				var attrib = attribs[typeof(DefaultValueAttribute)] as DefaultValueAttribute;

				Text = attrib.Value.ToString();
			}

			base.OnLostFocus(e);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg != 770 || IsValidClipboardValue())
				base.WndProc(ref m);
		}
		#endregion Handlers (override)


		#region Methods (static)
		static bool IsKeyOrShiftPressed(Keys keyData, Keys keydata)
		{
			return (keyData | Keys.Shift) == (keydata | Keys.Shift);
		}

		static bool IsValidClipboardValue()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject.GetDataPresent(typeof(string)))
			{
				string data = dataObject.GetData(typeof(string)).ToString();

				int result;
				return Int32.TryParse(data, out result);
			}
			return false;
		}
		#endregion Methods (static) */
	}
}
