using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class HelpF
		: Form
	{
		#region cTor
		internal HelpF()
		{
			InitializeComponent();

			string text = "This doc only describes actions that are not listed in" + Environment.NewLine
						+ "the menus." + Environment.NewLine + Environment.NewLine;

			text += "MOUSE" + Environment.NewLine + Environment.NewLine;

			text += "LMB+Alt  - pan up/down" + Environment.NewLine;
			text += "LMB+Ctrl - pan left/right" + Environment.NewLine;
			text += "RMB      - pan up/down/left/right" + Environment.NewLine;
			text += "RMB+Ctrl - pan polar" + Environment.NewLine + Environment.NewLine;

			text += "KEYBOARD" + Environment.NewLine + Environment.NewLine;

			text += "Ctrl+click = increases adjustment rate by a factor of 10" + Environment.NewLine;
			text += "Shft+click = decreases adjustment rate by a factor of 10" + Environment.NewLine + Environment.NewLine;

			text += "TEXT" + Environment.NewLine + Environment.NewLine;

			text += "keypad +/- increases/decreases a value; Ctrl/Shft" + Environment.NewLine
				  + "increase/decrease adjustment rate by a factor of 10" + Environment.NewLine + Environment.NewLine;

			text += "COLORS" + Environment.NewLine + Environment.NewLine;

			text += "Click a color-swatch to open the color-picker dialog." + Environment.NewLine;
			text += "Okay the dialog to enable a color." + Environment.NewLine;
			text += "LMB - applies/ignores a color" + Environment.NewLine;
			text += "RMB - clears a color" + Environment.NewLine;


			tb_help.Text = text;

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
			this.tb_help.Size = new System.Drawing.Size(367, 374);
			this.tb_help.TabIndex = 0;
			this.tb_help.WordWrap = false;
			// 
			// HelpF
			// 
			this.ClientSize = new System.Drawing.Size(367, 374);
			this.Controls.Add(this.tb_help);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "HelpF";
			this.ShowInTaskbar = false;
			this.Text = "Help";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
