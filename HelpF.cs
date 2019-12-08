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

			tb_help.Text = "help screen";

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
			this.tb_help.Size = new System.Drawing.Size(292, 274);
			this.tb_help.TabIndex = 0;
			this.tb_help.WordWrap = false;
			// 
			// HelpF
			// 
			this.ClientSize = new System.Drawing.Size(292, 274);
			this.Controls.Add(this.tb_help);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "HelpF";
			this.Text = "Help";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}
}
