using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed class CloseF
		: Form
	{
		internal enum ObjectType
		{
			ot_blueprint,
			ot_instance
		}


		#region cTor
		/// <summary>
		/// Returns:
		/// cancel = 'DialogResult.Cancel'
		/// lose   = 'DialogResult.Ignore'
		/// apply  = 'DialogResult.OK'
		/// save   = 'DialogResult.Yes'
		/// @note A stock resource cannot be autosaved.
		/// @note This dialog cannot be cancelled (ie. cannot return to state)
		/// if it is invoked because the instance is changing. That is, the
		/// instance *is* going to change, at present.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="cancancel"></param>
		/// <param name="hasresdir"></param>
		internal CloseF(ObjectType type, bool cancancel, bool hasresdir)
		{
			InitializeComponent();
			if (!cancancel)
			{
				DialogResult = DialogResult.Ignore;
				bu_cancel.Enabled = false;
			}
			else
				DialogResult = DialogResult.Cancel;

			bu_apply.Enabled = hasresdir;

			switch (type)
			{
				case ObjectType.ot_blueprint:
					bu_apply.Text += "blueprint";
					break;

				case ObjectType.ot_instance:
					bu_apply.Text += "instance";
					break;
			}
		}
		#endregion cTor



		#region Designer
		Label la_head;
		Button bu_cancel;
		Button bu_ignore;
		Button bu_save;
		Button bu_apply;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.la_head = new System.Windows.Forms.Label();
			this.bu_cancel = new System.Windows.Forms.Button();
			this.bu_ignore = new System.Windows.Forms.Button();
			this.bu_save = new System.Windows.Forms.Button();
			this.bu_apply = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// la_head
			// 
			this.la_head.Dock = System.Windows.Forms.DockStyle.Top;
			this.la_head.Location = new System.Drawing.Point(0, 0);
			this.la_head.Margin = new System.Windows.Forms.Padding(0);
			this.la_head.Name = "la_head";
			this.la_head.Size = new System.Drawing.Size(192, 20);
			this.la_head.TabIndex = 0;
			this.la_head.Text = "The creature is changed ...";
			this.la_head.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// bu_cancel
			// 
			this.bu_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_cancel.Location = new System.Drawing.Point(9, 25);
			this.bu_cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_cancel.Name = "bu_cancel";
			this.bu_cancel.Size = new System.Drawing.Size(85, 45);
			this.bu_cancel.TabIndex = 1;
			this.bu_cancel.Text = "cancel close";
			this.bu_cancel.UseVisualStyleBackColor = true;
			// 
			// bu_ignore
			// 
			this.bu_ignore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
			this.bu_ignore.Location = new System.Drawing.Point(99, 25);
			this.bu_ignore.Margin = new System.Windows.Forms.Padding(0);
			this.bu_ignore.Name = "bu_ignore";
			this.bu_ignore.Size = new System.Drawing.Size(85, 45);
			this.bu_ignore.TabIndex = 2;
			this.bu_ignore.Text = "lose changes";
			this.bu_ignore.UseVisualStyleBackColor = true;
			// 
			// bu_save
			// 
			this.bu_save.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.bu_save.Location = new System.Drawing.Point(99, 75);
			this.bu_save.Margin = new System.Windows.Forms.Padding(0);
			this.bu_save.Name = "bu_save";
			this.bu_save.Size = new System.Drawing.Size(85, 50);
			this.bu_save.TabIndex = 4;
			this.bu_save.Text = "save to file";
			this.bu_save.UseVisualStyleBackColor = true;
			// 
			// bu_apply
			// 
			this.bu_apply.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_apply.Location = new System.Drawing.Point(9, 75);
			this.bu_apply.Margin = new System.Windows.Forms.Padding(0);
			this.bu_apply.Name = "bu_apply";
			this.bu_apply.Size = new System.Drawing.Size(85, 50);
			this.bu_apply.TabIndex = 3;
			this.bu_apply.Text = "apply data to ";
			this.bu_apply.UseVisualStyleBackColor = true;
			// 
			// CloseF
			// 
			this.AcceptButton = this.bu_cancel;
			this.CancelButton = this.bu_cancel;
			this.ClientSize = new System.Drawing.Size(192, 129);
			this.Controls.Add(this.bu_apply);
			this.Controls.Add(this.bu_save);
			this.Controls.Add(this.bu_ignore);
			this.Controls.Add(this.bu_cancel);
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
