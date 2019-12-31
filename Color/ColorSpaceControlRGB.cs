using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.RgbColorSpace
	sealed class ColorSpaceControlRGB
		: ColorSpaceControl
	{
		#region Properties (override)
		internal object Structure
		{
			get
			{
				return new RGB(coRed.Val, coGre.Val, coBlu.Val);
			}
			set
			{
				var rgb = (RGB)value;

				coRed.Val = rgb.R;
				coGre.Val = rgb.G;
				coBlu.Val = rgb.B;
			}
		}
		#endregion Properties (override)


		#region cTor
		public ColorSpaceControlRGB()
		{
			InitializeComponent();

			SpaceControls.Add(coRed);
			SpaceControls.Add(coGre);
			SpaceControls.Add(coBlu);
		}
		#endregion cTor


		#region Methods
		internal Color GetColor()
		{
			return Color.FromArgb(coRed.Val, coGre.Val, coBlu.Val);
		}

		internal string GetHecate()
		{
			return coRed.Val.ToString("X2")
				 + coGre.Val.ToString("X2")
				 + coBlu.Val.ToString("X2");
		}
		#endregion Methods



		#region Designer
		ColorSpaceControlCisco coRed;
		ColorSpaceControlCisco coGre;
		ColorSpaceControlCisco coBlu;


		void InitializeComponent()
		{
			this.coRed = new creaturevisualizer.ColorSpaceControlCisco();
			this.coGre = new creaturevisualizer.ColorSpaceControlCisco();
			this.coBlu = new creaturevisualizer.ColorSpaceControlCisco();
			this.SuspendLayout();
			// 
			// coRed
			// 
			this.coRed.DisplayCharacter = 'R';
			this.coRed.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coRed.Location = new System.Drawing.Point(0, 0);
			this.coRed.Margin = new System.Windows.Forms.Padding(0);
			this.coRed.Max = 255;
			this.coRed.Name = "coRed";
			this.coRed.Selected = false;
			this.coRed.Size = new System.Drawing.Size(75, 20);
			this.coRed.TabIndex = 0;
			this.coRed.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Byte;
			this.coRed.Val = 0;
			this.coRed.CiscoSelected += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.coRed.CiscoValueChanged += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// coGre
			// 
			this.coGre.DisplayCharacter = 'G';
			this.coGre.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coGre.Location = new System.Drawing.Point(0, 19);
			this.coGre.Margin = new System.Windows.Forms.Padding(0);
			this.coGre.Max = 255;
			this.coGre.Name = "coGre";
			this.coGre.Selected = false;
			this.coGre.Size = new System.Drawing.Size(75, 20);
			this.coGre.TabIndex = 1;
			this.coGre.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Byte;
			this.coGre.Val = 0;
			this.coGre.CiscoSelected += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.coGre.CiscoValueChanged += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// coBlu
			// 
			this.coBlu.DisplayCharacter = 'B';
			this.coBlu.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coBlu.Location = new System.Drawing.Point(0, 38);
			this.coBlu.Margin = new System.Windows.Forms.Padding(0);
			this.coBlu.Max = 255;
			this.coBlu.Name = "coBlu";
			this.coBlu.Selected = false;
			this.coBlu.Size = new System.Drawing.Size(75, 20);
			this.coBlu.TabIndex = 2;
			this.coBlu.Units = creaturevisualizer.ColorSpaceControlCisco.Unit.Byte;
			this.coBlu.Val = 0;
			this.coBlu.CiscoSelected += new creaturevisualizer.CiscoSelectedEvent(this.OnCiscoSelected);
			this.coBlu.CiscoValueChanged += new creaturevisualizer.CiscoValueChangedEvent(this.OnCiscoValueChanged);
			// 
			// ColorSpaceControlRGB
			// 
			this.Controls.Add(this.coRed);
			this.Controls.Add(this.coGre);
			this.Controls.Add(this.coBlu);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceControlRGB";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
