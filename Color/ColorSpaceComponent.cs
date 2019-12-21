using System;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponent
	sealed class ColorSpaceComponent
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
			get { return rb_Component.Checked; }
			set { rb_Component.Checked = value; }
		}

		char _displayCharacter;
		public char DisplayCharacter
		{
			get { return _displayCharacter; }
			set
			{
				_displayCharacter = value;

				rb_Component.Text = _displayCharacter.ToString().ToUpper();
				la_Component.Text = _displayCharacter.ToString().ToUpper();
			}
		}

		int _val;
		public int Value
		{
			get // TODO ->
			{
				if (csctb_Val.Text.Length == 0)
					return 0;

				int val = Int32.Parse(csctb_Val.Text);

				if (val > MaximumValue)
					val = Int32.Parse(csctb_Val.Text.Substring(0, csctb_Val.Text.Length - 1));

				return val;
			}
			set
			{
				csctb_Val.Text = (_val = value).ToString();
				csctb_Val.SelectionStart = csctb_Val.Text.Length;
			}
		}

		int MinimumValue
		{ get; set; }

		int _max = 255;
		public int MaximumValue
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
		public ColorSpaceComponent()
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
			if (csctb_Val.Text.Length > 0
				&& (   (e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.RButton | Keys.MButton)
					|| (e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.Back)))
			{
				int val1 = Int16.Parse(csctb_Val.Text);
				int num2 = (((e.KeyData & Keys.Shift) != Keys.Shift) ? 1 : 10);

				if ((e.KeyData & Keys.Up) == Keys.Up)
				{
					val1 = ((val1 + num2 > MaximumValue) ? MaximumValue : (val1 + num2));
				}
				else if ((e.KeyData | Keys.Shift) == (Keys.Shift | Keys.Space | Keys.Back))
				{
					num2 = ((e.KeyData != (Keys.Shift | Keys.Space | Keys.Back)) ? (-1) : (-10));
					val1 = ((val1 + num2 <= MinimumValue) ? MinimumValue : (val1 + num2));
				}

				csctb_Val.Text = val1.ToString();
				this.ComponentTextKeyUp(this, EventArgs.Empty);

				if (e.KeyData == (Keys.Back | Keys.Space | Keys.Shift))
				{
					csctb_Val.SelectionStart = csctb_Val.Text.Length;
				}
			}
		}

		void txtComponentValue_LostFocus(object sender, EventArgs e)
		{
			var tb = (ColorSpaceComponentTextBox)sender;

			int val;

			if (!String.IsNullOrEmpty(tb.Text))
			{
				val = Int32.Parse(tb.Text);
				if      (val > MaximumValue) val = MaximumValue;
				else if (val < MinimumValue) val = MinimumValue;
			}
			else
				val = MinimumValue;

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
		ColorSpaceComponentTextBox csctb_Val;

		Label la_Component;
		Label la_Units;
		RadioButton rb_Component;


		void InitializeComponent()
		{
			this.csctb_Val = new creaturevisualizer.ColorSpaceComponentTextBox();
			this.la_Units = new System.Windows.Forms.Label();
			this.rb_Component = new System.Windows.Forms.RadioButton();
			this.la_Component = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// csctb_Val
			// 
			this.csctb_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.csctb_Val.Location = new System.Drawing.Point(30, 0);
			this.csctb_Val.Margin = new System.Windows.Forms.Padding(0);
			this.csctb_Val.MaxLength = 3;
			this.csctb_Val.Name = "csctb_Val";
			this.csctb_Val.Size = new System.Drawing.Size(25, 20);
			this.csctb_Val.TabIndex = 2;
			this.csctb_Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.csctb_Val.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtComponentValue_KeyDown);
			this.csctb_Val.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtComponentValue_KeyUp);
			this.csctb_Val.LostFocus += new System.EventHandler(this.txtComponentValue_LostFocus);
			// 
			// la_Units
			// 
			this.la_Units.Location = new System.Drawing.Point(60, 0);
			this.la_Units.Margin = new System.Windows.Forms.Padding(0);
			this.la_Units.Name = "la_Units";
			this.la_Units.Size = new System.Drawing.Size(15, 20);
			this.la_Units.TabIndex = 3;
			this.la_Units.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rb_Component
			// 
			this.rb_Component.Location = new System.Drawing.Point(0, 0);
			this.rb_Component.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Component.Name = "rb_Component";
			this.rb_Component.Size = new System.Drawing.Size(15, 20);
			this.rb_Component.TabIndex = 0;
			this.rb_Component.Click += new System.EventHandler(this.rdoComponent_Click);
			// 
			// la_Component
			// 
			this.la_Component.Location = new System.Drawing.Point(15, 0);
			this.la_Component.Margin = new System.Windows.Forms.Padding(0);
			this.la_Component.Name = "la_Component";
			this.la_Component.Size = new System.Drawing.Size(15, 20);
			this.la_Component.TabIndex = 1;
			this.la_Component.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ColorSpaceComponent
			// 
			this.Controls.Add(this.rb_Component);
			this.Controls.Add(this.csctb_Val);
			this.Controls.Add(this.la_Component);
			this.Controls.Add(this.la_Units);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceComponent";
			this.Size = new System.Drawing.Size(75, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}



	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponentEventHandler
	internal delegate void ColorSpaceComponentEventHandler(ColorSpaceComponent sender, EventArgs e);
}
