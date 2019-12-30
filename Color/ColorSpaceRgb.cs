using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.RgbColorSpace
	sealed class ColorSpaceRgb
		: ColorSpaceControl
	{
		#region Properties (override)
		internal object Structure
		{
			get
			{
				return new RGB(coRed.Val, coGreen.Val, coBlue.Val);
			}
			set
			{
				var rgb = (RGB)value;

				coRed  .Val = rgb.Red;
				coGreen.Val = rgb.Green;
				coBlue .Val = rgb.Blue;
			}
		}
		#endregion Properties (override)


		#region cTor
		public ColorSpaceRgb()
		{
			InitializeComponent();

			ColorSpaceControls.Add(coRed);
			ColorSpaceControls.Add(coGreen);
			ColorSpaceControls.Add(coBlue);
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


		#region Methods
		internal Color GetColor()
		{
			return Color.FromArgb(coRed.Val, coGreen.Val, coBlue.Val);
		}

		internal string GetHecate()
		{
			return coRed  .Val.ToString("X2")
				 + coGreen.Val.ToString("X2")
				 + coBlue .Val.ToString("X2");
		}
		#endregion Methods



		#region Designer
		ColorSpaceControlCo coRed;
		ColorSpaceControlCo coGreen;
		ColorSpaceControlCo coBlue;


		void InitializeComponent()
		{
			this.coRed = new creaturevisualizer.ColorSpaceControlCo();
			this.coGreen = new creaturevisualizer.ColorSpaceControlCo();
			this.coBlue = new creaturevisualizer.ColorSpaceControlCo();
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
			this.coRed.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Byte;
			this.coRed.Val = 0;
			this.coRed.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			// 
			// coGreen
			// 
			this.coGreen.DisplayCharacter = 'G';
			this.coGreen.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coGreen.Location = new System.Drawing.Point(0, 19);
			this.coGreen.Margin = new System.Windows.Forms.Padding(0);
			this.coGreen.Max = 255;
			this.coGreen.Name = "coGreen";
			this.coGreen.Selected = false;
			this.coGreen.Size = new System.Drawing.Size(75, 20);
			this.coGreen.TabIndex = 1;
			this.coGreen.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Byte;
			this.coGreen.Val = 0;
			this.coGreen.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			// 
			// coBlue
			// 
			this.coBlue.DisplayCharacter = 'B';
			this.coBlue.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coBlue.Location = new System.Drawing.Point(0, 38);
			this.coBlue.Margin = new System.Windows.Forms.Padding(0);
			this.coBlue.Max = 255;
			this.coBlue.Name = "coBlue";
			this.coBlue.Selected = false;
			this.coBlue.Size = new System.Drawing.Size(75, 20);
			this.coBlue.TabIndex = 2;
			this.coBlue.Units = creaturevisualizer.ColorSpaceControlCo.Unit.Byte;
			this.coBlue.Val = 0;
			this.coBlue.CoSelected += new creaturevisualizer.CoSelectedEventHandler(this.OnCoSelected);
			// 
			// ColorSpaceRgb
			// 
			this.Controls.Add(this.coRed);
			this.Controls.Add(this.coGreen);
			this.Controls.Add(this.coBlue);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "ColorSpaceRgb";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
