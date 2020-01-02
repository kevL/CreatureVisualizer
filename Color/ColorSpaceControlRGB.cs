using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.RgbColorSpace
	sealed class ColorSpaceControlRGB
		: ColorSpaceControl
	{
		#region Properties (override)
		internal RGB rgb
		{
			get
			{
				return new RGB(cR.Val, cG.Val, cB.Val);
			}
			set
			{
				cR.Val = value.R;
				cG.Val = value.G;
				cB.Val = value.B;
			}
		}
		#endregion Properties (override)


		#region cTor
		public ColorSpaceControlRGB()
		{
			InitializeComponent();

			SpaceControls.Add(cR);
			SpaceControls.Add(cG);
			SpaceControls.Add(cB);
		}
		#endregion cTor


		#region Methods
		internal Color GetColor()
		{
			return Color.FromArgb(cR.Val, cG.Val, cB.Val);
		}

		internal string GetHecate()
		{
			return cR.Val.ToString("X2")
				 + cG.Val.ToString("X2")
				 + cB.Val.ToString("X2");
		}
		#endregion Methods



		#region Designer
		ColorSpaceControlCisco cR;
		ColorSpaceControlCisco cG;
		ColorSpaceControlCisco cB;


		void InitializeComponent()
		{
			this.cR = new creaturevisualizer.ColorSpaceControlCisco();
			this.cG = new creaturevisualizer.ColorSpaceControlCisco();
			this.cB = new creaturevisualizer.ColorSpaceControlCisco();
			this.SuspendLayout();
			// 
			// cR
			// 
			this.cR.DisplayCharacter = 'R';
			this.cR.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cR.Location = new System.Drawing.Point(0, 0);
			this.cR.Margin = new System.Windows.Forms.Padding(0);
			this.cR.Max = 255;
			this.cR.Name = "cR";
			this.cR.Selected = false;
			this.cR.Size = new System.Drawing.Size(75, 20);
			this.cR.TabIndex = 0;
			this.cR.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Byte;
			this.cR.Val = 0;
			this.cR.CiscoSelected_lo += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.cR.CiscoValueChanged_lo += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// cG
			// 
			this.cG.DisplayCharacter = 'G';
			this.cG.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cG.Location = new System.Drawing.Point(0, 19);
			this.cG.Margin = new System.Windows.Forms.Padding(0);
			this.cG.Max = 255;
			this.cG.Name = "cG";
			this.cG.Selected = false;
			this.cG.Size = new System.Drawing.Size(75, 20);
			this.cG.TabIndex = 1;
			this.cG.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Byte;
			this.cG.Val = 0;
			this.cG.CiscoSelected_lo += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.cG.CiscoValueChanged_lo += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// cB
			// 
			this.cB.DisplayCharacter = 'B';
			this.cB.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cB.Location = new System.Drawing.Point(0, 38);
			this.cB.Margin = new System.Windows.Forms.Padding(0);
			this.cB.Max = 255;
			this.cB.Name = "cB";
			this.cB.Selected = false;
			this.cB.Size = new System.Drawing.Size(75, 20);
			this.cB.TabIndex = 2;
			this.cB.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Byte;
			this.cB.Val = 0;
			this.cB.CiscoSelected_lo += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.cB.CiscoValueChanged_lo += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// ColorSpaceControlRGB
			// 
			this.Controls.Add(this.cR);
			this.Controls.Add(this.cG);
			this.Controls.Add(this.cB);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceControlRGB";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
