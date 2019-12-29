using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponent
	sealed class ColorSpaceControlCo
		: UserControl
	{
		// Sano.PersonalProjects.ColorPicker.Controls.ComponentUnit
		internal enum Units
		{
			Degree,
			Percent,
			Byte
		}


		#region Events
		public event CoSelectedEventHandler CoSelected;
		#endregion Events


		#region Properties
		public bool Selected
		{
			get { return rb_Co.Checked; }
			set { rb_Co.Checked = value; }
		}

		char _displayCharacter;
		public char DisplayCharacter
		{
			get { return _displayCharacter; }
			set
			{
				rb_Co.Text = (_displayCharacter = value).ToString();
			}
		}

		public int Val
		{
			get
			{
				if (!String.IsNullOrEmpty(tb_Val.Text))
					return Int32.Parse(tb_Val.Text);

				return 0;
			}
			set
			{
				tb_Val.Text = Math.Max(0, Math.Min(value, Max)).ToString();

				tb_Val.SelectionLength = 0;
				tb_Val.SelectionStart = tb_Val.Text.Length;
			}
		}

		int _max = Byte.MaxValue;
		[DefaultValue(Byte.MaxValue)]
		public int Max
		{
			get { return _max; }
			set { _max = value; }
		}

		Units _unit = Units.Byte;
//		[DefaultValue(Units.Byte)] // don't default this. It needs to be called so the text gets set.
		public Units Unit
		{
			get { return _unit; }
			set
			{
				switch (_unit = value)
				{
					case Units.Degree:  la_Units.Text = "d"; break;
					case Units.Percent: la_Units.Text = "p"; break;
					case Units.Byte:    la_Units.Text = "b"; break;
				}
			}
		}
		#endregion Properties


		#region cTor
		public ColorSpaceControlCo()
		{
			InitializeComponent();
		}
		#endregion cTor


		#region Handlers
/*		void checkedchanged_rb(object sender, EventArgs e)
		{
//			if (((RadioButton)sender).Checked)
//			{
//				if (CscSelected != null)
//					CscSelected(this);
//			}
		} */
		void click_rb(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked)
			{
				if (CoSelected != null)
					CoSelected(this);
			}
		}

		void leave_val(object sender, EventArgs e)
		{
			var tb = sender as TextboxRestrictive;
			if (String.IsNullOrEmpty(tb.Text))
				tb.Text = "0"; // WARNING: That will fire the TextChanged event but the control's value shall already be 0.
		}
		#endregion Handlers


		#region Designer
		internal TextboxRestrictive tb_Val;

		Label la_Units;
		RadioButton rb_Co;


		void InitializeComponent()
		{
			this.tb_Val = new creaturevisualizer.TextboxRestrictive();
			this.la_Units = new System.Windows.Forms.Label();
			this.rb_Co = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// tb_Val
			// 
			this.tb_Val.BackColor = System.Drawing.Color.White;
			this.tb_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Val.Location = new System.Drawing.Point(30, 0);
			this.tb_Val.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Val.MaxLength = 3;
			this.tb_Val.Name = "tb_Val";
			this.tb_Val.Restrict = creaturevisualizer.TextboxRestrictive.Type.Byte;
			this.tb_Val.Size = new System.Drawing.Size(25, 20);
			this.tb_Val.TabIndex = 1;
			this.tb_Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_Val.Leave += new System.EventHandler(this.leave_val);
			// 
			// la_Units
			// 
			this.la_Units.Location = new System.Drawing.Point(60, 0);
			this.la_Units.Margin = new System.Windows.Forms.Padding(0);
			this.la_Units.Name = "la_Units";
			this.la_Units.Size = new System.Drawing.Size(15, 20);
			this.la_Units.TabIndex = 2;
			this.la_Units.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rb_Co
			// 
			this.rb_Co.Location = new System.Drawing.Point(0, 0);
			this.rb_Co.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Co.Name = "rb_Co";
			this.rb_Co.Size = new System.Drawing.Size(30, 20);
			this.rb_Co.TabIndex = 0;
			this.rb_Co.TabStop = true;
			this.rb_Co.Click += new System.EventHandler(this.click_rb);
			// 
			// ColorSpaceControlCo
			// 
			this.Controls.Add(this.rb_Co);
			this.Controls.Add(this.tb_Val);
			this.Controls.Add(this.la_Units);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceControlCo";
			this.Size = new System.Drawing.Size(75, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}


	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponentEventHandler
	internal delegate void CoSelectedEventHandler(ColorSpaceControlCo sender);
}
