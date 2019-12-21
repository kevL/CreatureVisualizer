using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorEditForm
	sealed class ColorF
		: Form
	{
		#region Properties
		internal ColorPanel ColorPanel
		{
			get { return colorPanel; }
		}
		#endregion Properties


		#region cTor
		internal ColorF()
		{
			InitializeComponent();

			ActiveControl = bu_Okay;
		}
		#endregion cTor



		#region Designer
		Container components = null;

		ColorPanel colorPanel;

		Button bu_Okay;
		Button bu_Cancel;


		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}


		void InitializeComponent()
		{
			this.colorPanel = new creaturevisualizer.ColorPanel();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// colorPanel
			// 
			this.colorPanel.AllowDrop = true;
			this.colorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorPanel.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorPanel.Location = new System.Drawing.Point(0, 0);
			this.colorPanel.Margin = new System.Windows.Forms.Padding(0);
			this.colorPanel.Name = "colorPanel";
			this.colorPanel.Size = new System.Drawing.Size(514, 301);
			this.colorPanel.TabIndex = 2;
			// 
			// bu_Okay
			// 
			this.bu_Okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Okay.Location = new System.Drawing.Point(435, 275);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(75, 25);
			this.bu_Okay.TabIndex = 1;
			this.bu_Okay.Text = "BhereNOW";
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(355, 275);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(75, 25);
			this.bu_Cancel.TabIndex = 0;
			this.bu_Cancel.Text = "Cancel";
			// 
			// ColorF
			// 
			this.AcceptButton = this.bu_Okay;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(514, 301);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.bu_Okay);
			this.Controls.Add(this.colorPanel);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorF";
			this.ShowInTaskbar = false;
			this.Text = "Color";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
