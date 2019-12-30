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
				return new HSB(cscHue.Val, cscSaturation.Val, cscBrightness.Val);
			}
			set
			{
				var hsb = (HSB)value;

				cscHue       .Val = hsb.Hue;
				cscSaturation.Val = hsb.Saturation;
				cscBrightness.Val = hsb.Brightness;
			}
		}
		#endregion Properties (override)


		#region cTor
		public ColorSpaceHsb()
		{
			InitializeComponent();

			cscHue       .tb_Val.SetRestrict(TextboxRestrictive.Type.Degree);
			cscSaturation.tb_Val.SetRestrict(TextboxRestrictive.Type.Percent);
			cscBrightness.tb_Val.SetRestrict(TextboxRestrictive.Type.Percent);

			ColorSpaceControls.Add(cscHue);
			ColorSpaceControls.Add(cscSaturation);
			ColorSpaceControls.Add(cscBrightness);
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
		internal ColorSpaceControlCo cscHue; // default
		ColorSpaceControlCo cscSaturation;
		ColorSpaceControlCo cscBrightness;


		void InitializeComponent()
		{
			this.cscHue = new creaturevisualizer.ColorSpaceControlCo();
			this.cscSaturation = new creaturevisualizer.ColorSpaceControlCo();
			this.cscBrightness = new creaturevisualizer.ColorSpaceControlCo();
			this.SuspendLayout();
			// 
			// cscHue
			// 
			this.cscHue.DisplayCharacter = 'H';
			this.cscHue.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscHue.Location = new System.Drawing.Point(0, 0);
			this.cscHue.Margin = new System.Windows.Forms.Padding(0);
			this.cscHue.Max = 359;
			this.cscHue.Name = "cscHue";
			this.cscHue.Selected = false;
			this.cscHue.Size = new System.Drawing.Size(75, 20);
			this.cscHue.TabIndex = 0;
			this.cscHue.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Degree;
			this.cscHue.Val = 0;
			this.cscHue.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			// 
			// cscSaturation
			// 
			this.cscSaturation.DisplayCharacter = 'S';
			this.cscSaturation.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscSaturation.Location = new System.Drawing.Point(0, 19);
			this.cscSaturation.Margin = new System.Windows.Forms.Padding(0);
			this.cscSaturation.Max = 100;
			this.cscSaturation.Name = "cscSaturation";
			this.cscSaturation.Selected = false;
			this.cscSaturation.Size = new System.Drawing.Size(75, 20);
			this.cscSaturation.TabIndex = 1;
			this.cscSaturation.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Percent;
			this.cscSaturation.Val = 0;
			this.cscSaturation.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			// 
			// cscBrightness
			// 
			this.cscBrightness.DisplayCharacter = 'B';
			this.cscBrightness.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscBrightness.Location = new System.Drawing.Point(0, 38);
			this.cscBrightness.Margin = new System.Windows.Forms.Padding(0);
			this.cscBrightness.Max = 100;
			this.cscBrightness.Name = "cscBrightness";
			this.cscBrightness.Selected = false;
			this.cscBrightness.Size = new System.Drawing.Size(75, 20);
			this.cscBrightness.TabIndex = 2;
			this.cscBrightness.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Percent;
			this.cscBrightness.Val = 0;
			this.cscBrightness.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			// 
			// ColorSpaceHsb
			// 
			this.Controls.Add(this.cscHue);
			this.Controls.Add(this.cscSaturation);
			this.Controls.Add(this.cscBrightness);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceHsb";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
