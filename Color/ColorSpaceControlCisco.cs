﻿using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponent
	sealed class ColorSpaceControlCisco
		: UserControl
	{
		// Sano.PersonalProjects.ColorPicker.Controls.ComponentUnit
		internal enum Unit
		{
			Degree,
			Percent,
			Byte
		}


		#region Events
		public event CiscoSelectedEvent CiscoSelected_lo;
		public event CiscoValueChangedEvent CiscoValueChanged_lo;
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

		Unit _units;// = Unit.Byte;
//		[DefaultValue(Unit.Byte)] // don't default this. It needs to run so the 'DisplayCharacter' gets set.
		public Unit Units
		{
			get { return _units; }
			set
			{
				switch (_units = value)
				{
					case Unit.Degree:  la_Units.Text = "d"; break;
					case Unit.Percent: la_Units.Text = "p"; break;
					case Unit.Byte:    la_Units.Text = "b"; break;
				}
			}
		}

		int _max = 255;
		[DefaultValue(255)]
		public int Max
		{
			get { return _max; }
			set { _max = value; }
		}

		/// <summary>
		/// @note Strict conditions are placed on allowable text in 'tb_Val' by
		/// 'TextboxRestrictive'. The text will never have whitespace or
		/// anything but integers within the proper range of this cisco's
		/// 'Units'. So always check the value before tossing it into the
		/// setter in order to be consistent.
		/// </summary>
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
				tb_Val.Text = value.ToString();
			}
		}
		#endregion Properties


		#region cTor
		public ColorSpaceControlCisco()
		{
			InitializeComponent();
		}
		#endregion cTor


		#region Handlers
		void click_rb(object sender, EventArgs e)
		{
			if (CiscoSelected_lo != null)
				CiscoSelected_lo(this); // ColorSpaceControl.OnCiscoSelected()
		}

		void textchanged_tb(object sender, EventArgs e)
		{
			if (!ColorControl._bypassCisco)
			{
				if (CiscoValueChanged_lo != null)
					CiscoValueChanged_lo(); // ColorSpaceControl.OnCiscoValueChanged()
			}
		}

		/// <summary>
		/// Handles the KeyUp [Space] event to select the radio-button because
		/// their tabstops don't work correctly probably due to the control
		/// within a control within a control (within a control within a
		/// control) shenanigans. I mean what genius figured that would work
		/// out ... hint, it doesn't. Don't do it again. Ever
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void keyup_tb(object sender, KeyEventArgs e)
		{
			if (!Selected && e.KeyData == Keys.Space)
			{
				click_rb(rb_Co, EventArgs.Empty);
				e.Handled = e.SuppressKeyPress = true;
			}
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
			this.tb_Val.TextChanged += new System.EventHandler(this.textchanged_tb);
			this.tb_Val.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyup_tb);
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
			this.rb_Co.AutoCheck = false;
			this.rb_Co.Location = new System.Drawing.Point(0, 0);
			this.rb_Co.Margin = new System.Windows.Forms.Padding(0);
			this.rb_Co.Name = "rb_Co";
			this.rb_Co.Size = new System.Drawing.Size(30, 20);
			this.rb_Co.TabIndex = 0;
			this.rb_Co.Click += new System.EventHandler(this.click_rb);
			// 
			// ColorSpaceControlCisco
			// 
			this.Controls.Add(this.rb_Co);
			this.Controls.Add(this.tb_Val);
			this.Controls.Add(this.la_Units);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceControlCisco";
			this.Size = new System.Drawing.Size(75, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Designer
	}


	// Sano.PersonalProjects.ColorPicker.Controls.ColorSpaceComponentEventHandler
	internal delegate void CiscoSelectedEvent(ColorSpaceControlCisco sender);
	internal delegate void CiscoValueChangedEvent();
}
