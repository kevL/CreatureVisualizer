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
		internal ColorControl ColorControl
		{
			get { return colorControl; }
		}
		#endregion Properties


		#region cTor
		internal ColorF()
		{
			InitializeComponent();

			ActiveControl = bu_Okay;
		}
		#endregion cTor


		#region Handlers (override)
		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.B:
					if (!ColorControl.IsTextboxFocused(Controls))
					{
						e.Handled = e.SuppressKeyPress = true;

						var args = new MouseEventArgs(MouseButtons.Right, 0, 0,0, 0);
						ColorControl.mousedown_colorbox(null, args);
					}
					break;
			}
		}
		#endregion Handlers (override)



		#region Designer
		Container components = null;

		ColorControl colorControl;

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
			this.colorControl = new creaturevisualizer.ColorControl();
			this.bu_Okay = new System.Windows.Forms.Button();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// colorControl
			// 
			this.colorControl.AllowDrop = true;
			this.colorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorControl.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.colorControl.Location = new System.Drawing.Point(0, 0);
			this.colorControl.Margin = new System.Windows.Forms.Padding(0);
			this.colorControl.Name = "colorControl";
			this.colorControl.Size = new System.Drawing.Size(494, 322);
			this.colorControl.TabIndex = 2;
			// 
			// bu_Okay
			// 
			this.bu_Okay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Okay.Location = new System.Drawing.Point(90, 272);
			this.bu_Okay.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Okay.Name = "bu_Okay";
			this.bu_Okay.Size = new System.Drawing.Size(300, 46);
			this.bu_Okay.TabIndex = 1;
			this.bu_Okay.Text = "BhereNOW";
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(5, 272);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(80, 46);
			this.bu_Cancel.TabIndex = 0;
			this.bu_Cancel.Text = "Cancel";
			// 
			// ColorF
			// 
			this.AcceptButton = this.bu_Okay;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(494, 322);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.bu_Okay);
			this.Controls.Add(this.colorControl);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorF";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Color";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
