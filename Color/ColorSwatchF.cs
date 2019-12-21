using System;
using System.Drawing;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.AddNewColorSwatchForm
	sealed class ColorSwatchF
		: Form
	{
		#region Properties
		internal string ColorDescription
		{
			get { return tb_Description.Text; }
		}
		#endregion Properties


		#region cTor
		internal ColorSwatchF()
		{
			InitializeComponent();
		}

		internal ColorSwatchF(Color colorToAdd)
			: this()
		{
			pb_Color.BackColor = colorToAdd;
		}

		internal ColorSwatchF(ColorSwatch colorSwatch)
			: this()
		{
			pb_Color.BackColor = colorSwatch.Color;
			tb_Description.Text = colorSwatch.Description;
		}
		#endregion cTor


		#region Handlers
		void btnOk_Click(object sender, EventArgs e)
		{
			if (tb_Description.Text == String.Empty)
				tb_Description.Text = "unnamed";

			Close();
		}

		void txtColorDescription_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Return:
					if (tb_Description.Text == string.Empty)
						tb_Description.Text = "unnamed";

					DialogResult = DialogResult.OK;
					Close();
					break;

				case Keys.Escape:
					DialogResult = DialogResult.Cancel;
					Close();
					break;
			}
		}
		#endregion Handlers



		#region Designer
		TextBox tb_Description;
		Button bu_Ok;
		PictureBox pb_Color;
		Button bu_Cancel;
		Label la_Description;


		void InitializeComponent()
		{
			this.pb_Color = new System.Windows.Forms.PictureBox();
			this.la_Description = new System.Windows.Forms.Label();
			this.tb_Description = new System.Windows.Forms.TextBox();
			this.bu_Cancel = new System.Windows.Forms.Button();
			this.bu_Ok = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pb_Color)).BeginInit();
			this.SuspendLayout();
			// 
			// pb_Color
			// 
			this.pb_Color.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pb_Color.Location = new System.Drawing.Point(5, 55);
			this.pb_Color.Margin = new System.Windows.Forms.Padding(0);
			this.pb_Color.Name = "pb_Color";
			this.pb_Color.Size = new System.Drawing.Size(280, 185);
			this.pb_Color.TabIndex = 4;
			this.pb_Color.TabStop = false;
			// 
			// la_Description
			// 
			this.la_Description.Location = new System.Drawing.Point(5, 5);
			this.la_Description.Margin = new System.Windows.Forms.Padding(0);
			this.la_Description.Name = "la_Description";
			this.la_Description.Size = new System.Drawing.Size(280, 20);
			this.la_Description.TabIndex = 3;
			this.la_Description.Text = "description";
			this.la_Description.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tb_Description
			// 
			this.tb_Description.Location = new System.Drawing.Point(5, 30);
			this.tb_Description.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Description.Name = "tb_Description";
			this.tb_Description.Size = new System.Drawing.Size(280, 20);
			this.tb_Description.TabIndex = 2;
			this.tb_Description.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtColorDescription_KeyUp);
			// 
			// bu_Cancel
			// 
			this.bu_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bu_Cancel.Location = new System.Drawing.Point(130, 245);
			this.bu_Cancel.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Cancel.Name = "bu_Cancel";
			this.bu_Cancel.Size = new System.Drawing.Size(75, 25);
			this.bu_Cancel.TabIndex = 1;
			this.bu_Cancel.Text = "Cancel";
			// 
			// bu_Ok
			// 
			this.bu_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bu_Ok.Location = new System.Drawing.Point(210, 245);
			this.bu_Ok.Margin = new System.Windows.Forms.Padding(0);
			this.bu_Ok.Name = "bu_Ok";
			this.bu_Ok.Size = new System.Drawing.Size(75, 25);
			this.bu_Ok.TabIndex = 0;
			this.bu_Ok.Text = "Ok";
			this.bu_Ok.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// ColorSwatchF
			// 
			this.ClientSize = new System.Drawing.Size(292, 274);
			this.Controls.Add(this.bu_Ok);
			this.Controls.Add(this.bu_Cancel);
			this.Controls.Add(this.tb_Description);
			this.Controls.Add(this.la_Description);
			this.Controls.Add(this.pb_Color);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColorSwatchF";
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.pb_Color)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
/*		void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(creaturevisualizer.ColorSwatchF));
			picColor = new System.Windows.Forms.PictureBox();
			lblDescription = new System.Windows.Forms.Label();
			txtColorDescription = new System.Windows.Forms.TextBox();
			btnCancel = new System.Windows.Forms.Button();
			btnOk = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)picColor).BeginInit();
			SuspendLayout();
			picColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(picColor, "picColor");
			picColor.Name = "picColor";
			picColor.TabStop = false;
			resources.ApplyResources(lblDescription, "lblDescription");
			lblDescription.Name = "lblDescription";
			resources.ApplyResources(txtColorDescription, "txtColorDescription");
			txtColorDescription.Name = "txtColorDescription";
			txtColorDescription.KeyUp += new System.Windows.Forms.KeyEventHandler(txtColorDescription_KeyUp);
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(btnCancel, "btnCancel");
			btnCancel.Name = "btnCancel";
			btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(btnOk, "btnOk");
			btnOk.Name = "btnOk";
			btnOk.Click += new System.EventHandler(btnOk_Click);
			resources.ApplyResources(this, "$this");
			base.Controls.Add(btnOk);
			base.Controls.Add(btnCancel);
			base.Controls.Add(txtColorDescription);
			base.Controls.Add(lblDescription);
			base.Controls.Add(picColor);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ColorSwatchF";
			((System.ComponentModel.ISupportInitialize)picColor).EndInit();
			ResumeLayout(false);
			PerformLayout();
		} */
		#endregion Designer
	}
}
