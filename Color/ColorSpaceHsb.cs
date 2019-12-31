using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HsbColorSpace
	sealed class ColorSpaceHsb
		: ColorSpaceControl
	{
		#region Properties (override)
		internal object Structure
		{
			get
			{
				return new HSB(coHue.Val, coSat.Val, coBri.Val);
			}
			set
			{
				var hsb = (HSB)value;

				coHue.Val = hsb.H;
				coSat.Val = hsb.S;
				coBri.Val = hsb.B;
			}
		}
		#endregion Properties (override)


		#region cTor
		public ColorSpaceHsb()
		{
			InitializeComponent();

			coHue.tb_Val.SetRestrict(TextboxRestrictive.Type.Degree);
			coSat.tb_Val.SetRestrict(TextboxRestrictive.Type.Percent);
			coBri.tb_Val.SetRestrict(TextboxRestrictive.Type.Percent);

			SpaceControls.Add(coHue);
			SpaceControls.Add(coSat);
			SpaceControls.Add(coBri);
		}
		#endregion cTor


		#region Handlers
//		void OnComponentSelected(ColorSpaceControlCo sender)
//		{
//			SelectComponent(sender);
//		}

//		void OnComponentTextKeyUp(ColorSpaceControlCo sender, EventArgs e)
//		{
//			OnComponentValueChanged();
//		}
		#endregion Handlers



		#region Designer
		internal ColorSpaceControlCo coHue; // default
		ColorSpaceControlCo coSat;
		ColorSpaceControlCo coBri;


		void InitializeComponent()
		{
			this.coHue = new creaturevisualizer.ColorSpaceControlCo();
			this.coSat = new creaturevisualizer.ColorSpaceControlCo();
			this.coBri = new creaturevisualizer.ColorSpaceControlCo();
			this.SuspendLayout();
			// 
			// coHue
			// 
			this.coHue.DisplayCharacter = 'H';
			this.coHue.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coHue.Location = new System.Drawing.Point(0, 0);
			this.coHue.Margin = new System.Windows.Forms.Padding(0);
			this.coHue.Max = 359;
			this.coHue.Name = "coHue";
			this.coHue.Selected = false;
			this.coHue.Size = new System.Drawing.Size(75, 20);
			this.coHue.TabIndex = 0;
			this.coHue.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Degree;
			this.coHue.Val = 0;
			this.coHue.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			this.coHue.CoValueChanged += new creaturevisualizer.CoValueChangedEventHandler(this.OnValueChanged);
			// 
			// coSat
			// 
			this.coSat.DisplayCharacter = 'S';
			this.coSat.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coSat.Location = new System.Drawing.Point(0, 19);
			this.coSat.Margin = new System.Windows.Forms.Padding(0);
			this.coSat.Max = 100;
			this.coSat.Name = "coSat";
			this.coSat.Selected = false;
			this.coSat.Size = new System.Drawing.Size(75, 20);
			this.coSat.TabIndex = 1;
			this.coSat.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Percent;
			this.coSat.Val = 0;
			this.coSat.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			this.coSat.CoValueChanged += new creaturevisualizer.CoValueChangedEventHandler(this.OnValueChanged);
			// 
			// coBri
			// 
			this.coBri.DisplayCharacter = 'B';
			this.coBri.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coBri.Location = new System.Drawing.Point(0, 38);
			this.coBri.Margin = new System.Windows.Forms.Padding(0);
			this.coBri.Max = 100;
			this.coBri.Name = "coBri";
			this.coBri.Selected = false;
			this.coBri.Size = new System.Drawing.Size(75, 20);
			this.coBri.TabIndex = 2;
			this.coBri.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Percent;
			this.coBri.Val = 0;
			this.coBri.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			this.coBri.CoValueChanged += new creaturevisualizer.CoValueChangedEventHandler(this.OnValueChanged);
			// 
			// ColorSpaceHsb
			// 
			this.Controls.Add(this.coHue);
			this.Controls.Add(this.coSat);
			this.Controls.Add(this.coBri);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceHsb";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
