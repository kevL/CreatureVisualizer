using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponent
	sealed class ColorSpaceControl
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
		public event ColorSpaceComponentEventHandler ComponentSelected;
		public event ColorSpaceComponentEventHandler ComponentTextKeyUp;
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
				rb_Co.Text = (_displayCharacter = value).ToString().ToUpper();
			}
		}

		int _val;
		public int Value
		{
			get // TODO ->
			{
				if (tb_Val.Text.Length == 0)
					return 0;

				int val = Int32.Parse(tb_Val.Text);

				if (val > Max)
					val = Int32.Parse(tb_Val.Text.Substring(0, tb_Val.Text.Length - 1));

				return val;
			}
			set
			{
				tb_Val.Text = (_val = value).ToString();
				tb_Val.SelectionStart = tb_Val.Text.Length;
			}
		}

		int _max = Byte.MaxValue;
		public int Max
		{
			get { return _max; }
			set { _max = value; }
		}

		Units _unit = Units.Byte;
		public Units Unit
		{
			get { return _unit; }
			set
			{
				switch (_unit = value)
				{
					case Units.Degree:
						la_Units.Text = "d";
						break;

					case Units.Percent:
						la_Units.Text = "p";
						break;

					case Units.Byte:
						la_Units.Text = "b";
						break;
				}
			}
		}
		#endregion Properties


		#region cTor
		public ColorSpaceControl()
		{
			InitializeComponent();
		}
		#endregion cTor


		#region Handlers
		void rdoComponent_Click(object sender, EventArgs e)
		{
			var radioButton = (RadioButton)sender;
			if (radioButton.Checked)
				OnComponentSelected(EventArgs.Empty);
		}

		void txtComponentValue_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData != Keys.Tab)
			{
				OnComponentTextKeyUp(EventArgs.Empty);
			}
		}

		void txtComponentValue_KeyDown(object sender, KeyEventArgs e)
		{
			if (tb_Val.Text.Length > 0
				&& (   (e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.RButton | Keys.MButton)
					|| (e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.Back)))
			{
				int val1 = Int16.Parse(tb_Val.Text);
				int num2 = (((e.KeyData & Keys.Shift) != Keys.Shift) ? 1 : 10);

				if ((e.KeyData & Keys.Up) == Keys.Up)
				{
					val1 = ((val1 + num2 > Max) ? Max : (val1 + num2));
				}
				else if ((e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.Back))
				{
					num2 = ((e.KeyData != (Keys.Shift | Keys.Space | Keys.Back)) ? (-1) : (-10));
					val1 = ((val1 + num2 <= 0) ? 0 : (val1 + num2));
				}

				tb_Val.Text = val1.ToString();
				this.ComponentTextKeyUp(this, EventArgs.Empty);

				if (e.KeyData == (Keys.Back | Keys.Space | Keys.Shift))
				{
					tb_Val.SelectionStart = tb_Val.Text.Length;
				}
			}
		}

		void txtComponentValue_LostFocus(object sender, EventArgs e)
		{
			var tb = (TextboxRestrictive)sender;

			int val;

			if (!String.IsNullOrEmpty(tb.Text))
			{
				val = Int32.Parse(tb.Text);
				if      (val > Max) val = Max;
				else if (val < 0) val = 0;
			}
			else
				val = 0;

			tb.Text = val.ToString();
		}
		#endregion Handlers


		#region Handlers (virtual)
		void OnComponentSelected(EventArgs e)
		{
			if (ComponentSelected != null)
				ComponentSelected(this, EventArgs.Empty);
		}

		void OnComponentTextKeyUp(EventArgs e)
		{
			if (ComponentTextKeyUp != null)
				ComponentTextKeyUp(this, EventArgs.Empty);
		}
		#endregion Handlers (virtual)



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
			this.tb_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_Val.Location = new System.Drawing.Point(30, 0);
			this.tb_Val.Margin = new System.Windows.Forms.Padding(0);
			this.tb_Val.MaxLength = 3;
			this.tb_Val.Name = "tb_Val";
			this.tb_Val.Restrict = creaturevisualizer.TextboxRestrictive.Type.Byte;
			this.tb_Val.Size = new System.Drawing.Size(25, 20);
			this.tb_Val.TabIndex = 1;
			this.tb_Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_Val.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtComponentValue_KeyDown);
			this.tb_Val.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtComponentValue_KeyUp);
			this.tb_Val.LostFocus += new System.EventHandler(this.txtComponentValue_LostFocus);
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
			this.rb_Co.Click += new System.EventHandler(this.rdoComponent_Click);
			// 
			// ColorSpaceControl
			// 
			this.Controls.Add(this.rb_Co);
			this.Controls.Add(this.tb_Val);
			this.Controls.Add(this.la_Units);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceControl";
			this.Size = new System.Drawing.Size(75, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}



	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponentEventHandler
	internal delegate void ColorSpaceComponentEventHandler(ColorSpaceControl sender, EventArgs e);
}
