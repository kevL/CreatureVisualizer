using System;
using System.ComponentModel;
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
				Info = "This doc describes actions that are not listed in the menus." + L + L;

				Info += "MOUSE"                             + L;
				Info += "-----"                             + L;
				Info += "LMB+Alt  - pan up/down"            + L;
				Info += "LMB+Ctrl - pan left/right"         + L;
				Info += "RMB      - pan up/down/left/right" + L;
				Info += "RMB+Ctrl - pan polar"              + L + L;

				Info += "KEYBOARD"                                                 + L;
				Info += "--------"                                                 + L;
				Info += "Ctrl+click = increases adjustment rate by a factor of 10" + L;
				Info += "Shft+click = decreases adjustment rate by a factor of 10" + L + L;

				Info += "TEXT"                                                + L;
				Info += "----"                                                + L;
				Info += "keypad +/- increases/decreases a value; Ctrl/Shft"   + L;
				Info += "increase/decrease adjustment rate by a factor of 10" + L + L;

				Info += "COLORS"                                                + L;
				Info += "------"                                                + L;
				Info += "Click a color-swatch to open the color-picker dialog." + L;
				Info += "Accept the dialog to enable a color."                  + L + L;
				Info += "LMB - toggles the color on/off"                        + L;
				Info += "RMB - resets the color to default and disables it"     + L + L;
				Info += "Note that [F1] opens Help in the color-chooser."      + L + L;

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
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		IContainer components = null;

		TextBox tb_help;


		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}


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
			this.tb_help.Size = new System.Drawing.Size(392, 439);
			this.tb_help.TabIndex = 0;
			this.tb_help.WordWrap = false;
			// 
			// HelpF
			// 
			this.ClientSize = new System.Drawing.Size(392, 439);
			this.Controls.Add(this.tb_help);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "HelpF";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Help";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
