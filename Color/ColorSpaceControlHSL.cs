using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HsbColorSpace
	sealed class ColorSpaceControlHSL
		: ColorSpaceControl
	{
		#region Properties (override)
		internal HSL hsl
		{
			get
			{
				return new HSL(cH.Val, cS.Val, cL.Val);
			}
			set
			{
				cH.Val = value.H;
				cS.Val = value.S;
				cL.Val = value.L;
			}
		}
		#endregion Properties (override)


		#region cTor
		public ColorSpaceControlHSL()
		{
			InitializeComponent();

			cH.tb_Val.SetRestrict(TextboxRestrictive.Type.Degree);
			cS.tb_Val.SetRestrict(TextboxRestrictive.Type.Percent);
			cL.tb_Val.SetRestrict(TextboxRestrictive.Type.Percent);

			SpaceControls.Add(cH);
			SpaceControls.Add(cS);
			SpaceControls.Add(cL);
		}
		#endregion cTor



		#region Designer
		internal ColorSpaceControlCisco cH; // initial selected.
		internal ColorSpaceControlCisco cS;
		internal ColorSpaceControlCisco cL;


		void InitializeComponent()
		{
			this.cH = new creaturevisualizer.ColorSpaceControlCisco();
			this.cS = new creaturevisualizer.ColorSpaceControlCisco();
			this.cL = new creaturevisualizer.ColorSpaceControlCisco();
			this.SuspendLayout();
			// 
			// cH
			// 
			this.cH.DisplayCharacter = 'H';
			this.cH.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cH.Location = new System.Drawing.Point(0, 0);
			this.cH.Margin = new System.Windows.Forms.Padding(0);
			this.cH.Max = 359;
			this.cH.Name = "cH";
			this.cH.Selected = false;
			this.cH.Size = new System.Drawing.Size(75, 20);
			this.cH.TabIndex = 0;
			this.cH.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Degree;
			this.cH.Val = 0;
			this.cH.CiscoSelected_lo += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.cH.CiscoValueChanged_lo += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// cS
			// 
			this.cS.DisplayCharacter = 'S';
			this.cS.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cS.Location = new System.Drawing.Point(0, 19);
			this.cS.Margin = new System.Windows.Forms.Padding(0);
			this.cS.Max = 100;
			this.cS.Name = "cS";
			this.cS.Selected = false;
			this.cS.Size = new System.Drawing.Size(75, 20);
			this.cS.TabIndex = 1;
			this.cS.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Percent;
			this.cS.Val = 0;
			this.cS.CiscoSelected_lo += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.cS.CiscoValueChanged_lo += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// cL
			// 
			this.cL.DisplayCharacter = 'L';
			this.cL.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cL.Location = new System.Drawing.Point(0, 38);
			this.cL.Margin = new System.Windows.Forms.Padding(0);
			this.cL.Max = 100;
			this.cL.Name = "cL";
			this.cL.Selected = false;
			this.cL.Size = new System.Drawing.Size(75, 20);
			this.cL.TabIndex = 2;
			this.cL.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Percent;
			this.cL.Val = 0;
			this.cL.CiscoSelected_lo += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.cL.CiscoValueChanged_lo += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// ColorSpaceControlHSL
			// 
			this.Controls.Add(this.cH);
			this.Controls.Add(this.cS);
			this.Controls.Add(this.cL);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceControlHSL";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
