using System;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.HsbColorSpace
	sealed class HsbColorSpace
		: ColorSpace
	{
		#region Properties (override)
		internal object Structure
		{
			get { return new HSB(cscHue.Value, cscSaturation.Value, cscBrightness.Value); }
			set
			{
				var hsb = (HSB)value;

				cscHue       .Value = hsb.Hue;
				cscSaturation.Value = hsb.Saturation;
				cscBrightness.Value = hsb.Brightness;
			}
		}
		#endregion Properties (override)


		#region cTor
		public HsbColorSpace()
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
		void ComponentSelected(ColorSpaceControl sender, EventArgs e)
		{
			SelectComponent(sender);
		}

		void ComponentTextKeyUp(ColorSpaceControl sender, EventArgs e)
		{
			OnComponentValueChanged(e);
		}
		#endregion Handlers


		#region Methods (override)
		internal void SetDefaultSelection()
		{
			SelectComponent(cscHue);
		}
		#endregion Methods (override)



		#region Designer
		ColorSpaceControl cscBrightness;
		ColorSpaceControl cscSaturation;
		ColorSpaceControl cscHue;


		void InitializeComponent()
		{
			this.cscHue = new creaturevisualizer.ColorSpaceControl();
			this.cscSaturation = new creaturevisualizer.ColorSpaceControl();
			this.cscBrightness = new creaturevisualizer.ColorSpaceControl();
			this.SuspendLayout();
			// 
			// cscHue
			// 
			this.cscHue.DisplayCharacter = 'H';
			this.cscHue.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscHue.Location = new System.Drawing.Point(0, 0);
			this.cscHue.Margin = new System.Windows.Forms.Padding(0);
			this.cscHue.Max = 360;
			this.cscHue.Name = "cscHue";
			this.cscHue.Selected = false;
			this.cscHue.Size = new System.Drawing.Size(70, 20);
			this.cscHue.TabIndex = 0;
			this.cscHue.Unit = creaturevisualizer.ColorSpaceControl.Units.Degree;
			this.cscHue.Value = 0;
			this.cscHue.ComponentSelected += new creaturevisualizer.ColorSpaceComponentEventHandler(this.ComponentSelected);
			this.cscHue.ComponentTextKeyUp += new creaturevisualizer.ColorSpaceComponentEventHandler(this.ComponentTextKeyUp);
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
			this.cscSaturation.Size = new System.Drawing.Size(70, 20);
			this.cscSaturation.TabIndex = 1;
			this.cscSaturation.Unit = creaturevisualizer.ColorSpaceControl.Units.Percent;
			this.cscSaturation.Value = 0;
			this.cscSaturation.ComponentSelected += new creaturevisualizer.ColorSpaceComponentEventHandler(this.ComponentSelected);
			this.cscSaturation.ComponentTextKeyUp += new creaturevisualizer.ColorSpaceComponentEventHandler(this.ComponentTextKeyUp);
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
			this.cscBrightness.Size = new System.Drawing.Size(70, 20);
			this.cscBrightness.TabIndex = 2;
			this.cscBrightness.Unit = creaturevisualizer.ColorSpaceControl.Units.Percent;
			this.cscBrightness.Value = 0;
			this.cscBrightness.ComponentSelected += new creaturevisualizer.ColorSpaceComponentEventHandler(this.ComponentSelected);
			this.cscBrightness.ComponentTextKeyUp += new creaturevisualizer.ColorSpaceComponentEventHandler(this.ComponentTextKeyUp);
			// 
			// HsbColorSpace
			// 
			this.Controls.Add(this.cscHue);
			this.Controls.Add(this.cscSaturation);
			this.Controls.Add(this.cscBrightness);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "HsbColorSpace";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
