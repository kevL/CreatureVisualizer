using System;
using System.Drawing;


namespace creaturevisualizer
{
	// Sano.PersonalProjects.ColorPicker.Controls.RgbColorSpace
	sealed class RgbColorSpace
		: ColorSpace
	{
		#region Properties (override)
		internal object Structure
		{
			get
			{
				return new RGB(cscRed.Value, cscGreen.Value, cscBlue.Value);
			}
			set
			{
				var rgb = (RGB)value;

				cscRed  .Value = rgb.Red;
				cscGreen.Value = rgb.Green;
				cscBlue .Value = rgb.Blue;
			}
		}
		#endregion Properties (override)


		#region cTor
		public RgbColorSpace()
		{
			InitializeComponent();

			ColorSpaceControls.Add(cscRed);
			ColorSpaceControls.Add(cscGreen);
			ColorSpaceControls.Add(cscBlue);
		}
		#endregion cTor


		#region Handlers
//		void OnComponentSelected(ColorSpaceControl sender)
//		{
//			SelectComponent(sender);
//		}

//		void OnComponentTextKeyUp(ColorSpaceControl sender, EventArgs e)
//		{
//			OnComponentValueChanged();
//		}
		#endregion Handlers


		#region Methods (override)
		internal Color GetColor()
		{
			return Color.FromArgb(cscRed.Value, cscGreen.Value, cscBlue.Value);
		}
		#endregion Methods (override)


		#region Methods
		internal string ConvertToHex()
		{
			return cscRed  .Value.ToString("X2")
				 + cscGreen.Value.ToString("X2")
				 + cscBlue .Value.ToString("X2");
		}
		#endregion Methods



		#region Designer
		ColorSpaceControl cscRed;
		ColorSpaceControl cscGreen;
		ColorSpaceControl cscBlue;


		void InitializeComponent()
		{
			this.cscRed = new creaturevisualizer.ColorSpaceControl();
			this.cscGreen = new creaturevisualizer.ColorSpaceControl();
			this.cscBlue = new creaturevisualizer.ColorSpaceControl();
			this.SuspendLayout();
			// 
			// cscRed
			// 
			this.cscRed.DisplayCharacter = 'R';
			this.cscRed.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscRed.Location = new System.Drawing.Point(0, 0);
			this.cscRed.Margin = new System.Windows.Forms.Padding(0);
			this.cscRed.Max = 255;
			this.cscRed.Name = "cscRed";
			this.cscRed.Selected = false;
			this.cscRed.Size = new System.Drawing.Size(75, 20);
			this.cscRed.TabIndex = 0;
			this.cscRed.Unit = creaturevisualizer.ColorSpaceControl.Units.Byte;
			this.cscRed.Value = 0;
			this.cscRed.CscSelected += new creaturevisualizer.CscSelectedEventHandler(this.OnCscSelected);
			// 
			// cscGreen
			// 
			this.cscGreen.DisplayCharacter = 'G';
			this.cscGreen.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscGreen.Location = new System.Drawing.Point(0, 19);
			this.cscGreen.Margin = new System.Windows.Forms.Padding(0);
			this.cscGreen.Max = 255;
			this.cscGreen.Name = "cscGreen";
			this.cscGreen.Selected = false;
			this.cscGreen.Size = new System.Drawing.Size(75, 20);
			this.cscGreen.TabIndex = 1;
			this.cscGreen.Unit = creaturevisualizer.ColorSpaceControl.Units.Byte;
			this.cscGreen.Value = 0;
			this.cscGreen.CscSelected += new creaturevisualizer.CscSelectedEventHandler(this.OnCscSelected);
			// 
			// cscBlue
			// 
			this.cscBlue.DisplayCharacter = 'B';
			this.cscBlue.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cscBlue.Location = new System.Drawing.Point(0, 38);
			this.cscBlue.Margin = new System.Windows.Forms.Padding(0);
			this.cscBlue.Max = 255;
			this.cscBlue.Name = "cscBlue";
			this.cscBlue.Selected = false;
			this.cscBlue.Size = new System.Drawing.Size(75, 20);
			this.cscBlue.TabIndex = 2;
			this.cscBlue.Unit = creaturevisualizer.ColorSpaceControl.Units.Byte;
			this.cscBlue.Value = 0;
			this.cscBlue.CscSelected += new creaturevisualizer.CscSelectedEventHandler(this.OnCscSelected);
			// 
			// RgbColorSpace
			// 
			this.Controls.Add(this.cscRed);
			this.Controls.Add(this.cscGreen);
			this.Controls.Add(this.cscBlue);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "RgbColorSpace";
			this.Size = new System.Drawing.Size(75, 58);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
