using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class HelpF
		: Form
	{
		#region Fields (static)
		static string Info;
		#endregion Fields (static)


		#region Fields
		string L = Environment.NewLine;
		#endregion Fields


		#region cTor
		internal HelpF()
		{
			InitializeComponent();

			if (Info == null)
			{
				Info = "This doc describes actions that are not listed in menus." + L + L;

				Info += "MOUSE"                             + L;
				Info += "-----"                             + L;
				Info += "LMB+Alt  - pan up/down"            + L;
				Info += "LMB+Ctrl - pan left/right"         + L;
				Info += "RMB      - pan up/down/left/right" + L;
				Info += "RMB+Ctrl - pan polar"              + L + L;

				Info += "press Ctrl/Shift to increase/decrease rate by factor of 10" + L + L;

				Info += "TEXT"                                                       + L;
				Info += "----"                                                       + L;
				Info += "keypad +/- to increase/decrease the value"                  + L;
				Info += "press Ctrl/Shift to increase/decrease rate by factor of 10" + L + L;

				Info += "COLORS"                                                     + L;
				Info += "------"                                                     + L;
				Info += "Left-click a color-swatch to open the color-picker dialog." + L;
				Info += "Accept the dialog to enable a color."                       + L + L;
				Info += "LMB - toggles the color off/on"                             + L;
				Info += "RMB - resets the color to default and disables it"          + L + L;
				Info += "Note that [F1] opens Help in the Color picker."             + L + L;

				Info += "PREFS"                                                       + L;
				Info += "-----"                                                       + L;
				Info += @"%localappdata%\NWN2 Toolset\Plugins\CreatureVisualizer.xml" + L;
			}

			tb_help.Text = Info;

			tb_help.SelectionStart  =
			tb_help.SelectionLength = 0;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Close();
		}
		#endregion Handlers (override)



		#region Designer
		TextBox tb_help;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tb_help = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tb_help
			// 
			this.tb_help.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tb_help.Location = new System.Drawing.Point(0, 0);
			this.tb_help.Margin = new System.Windows.Forms.Padding(0);
			this.tb_help.Multiline = true;
			this.tb_help.Name = "tb_help";
			this.tb_help.ReadOnly = true;
			this.tb_help.Size = new System.Drawing.Size(392, 399);
			this.tb_help.TabIndex = 0;
			this.tb_help.WordWrap = false;
			// 
			// HelpF
			// 
			this.ClientSize = new System.Drawing.Size(392, 399);
			this.Controls.Add(this.tb_help);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "HelpF";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Help";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
