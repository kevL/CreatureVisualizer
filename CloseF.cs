using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class CloseF
		: Form
	{
		#region cTor
		internal CloseF()
		{
			InitializeComponent();

			DialogResult = DialogResult.Cancel;
		}
		#endregion cTor



		#region Designer
		Label la_head;
		Button bu_Cancel;
		Button bu_Close;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.la_head = new System.Windows.Forms.Label();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Close = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// la_head
			// 
			this.la_head.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_head.Location = new System.Drawing.Point(0, 0);
			this.la_head.Margin = new System.Windows.Forms.Padding(0);
			this.la_head.Name = "la_head";
			this.la_head.Size = new System.Drawing.Size(217, 20);
			this.la_head.TabIndex = 0;
			this.la_head.Text = "The blueprint is changed ...";
			this.la_head.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(15, 25);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(88, 25);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "Cancel";
			this.bu_Cancel.UseVisualStyleBackColor = true;
			// 
			// bu_Close
			// 
			this.bu_Close.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Close.Location = new System.Drawing.Point(113, 25);
			this.bu_Close.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Close.Name = "bu_Close";
			this.bu_Close.Size = new System.Drawing.Size(87, 25);
			this.bu_Close.TabIndex = 2;
			this.bu_Close.Text = "Close";
			this.bu_Close.UseVisualStyleBackColor = true;
			// 
			// CloseF
			// 
			this.AcceptButton = this.bu_Close;
			this.CancelButton = this.bu_Cancel;
			this.ClientSize = new System.Drawing.Size(217, 54);
			this.Controls.Add(this.bu_Close);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.la_head);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CloseF";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Changed";
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
