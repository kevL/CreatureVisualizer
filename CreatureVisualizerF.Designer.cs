using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed partial class CreatureVisualizerF
	{
		#region Designer
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		IContainer components = null;

		Panel pa_bot;
		GroupBox gb_model;
		Button bu_model_zneg;
		Button bu_model_zpos;
		Button bu_model_ypos;
		Button bu_model_yneg;
		Button bu_model_xneg;
		Button bu_model_xpos;
		Button bu_model_rneg;
		Button bu_model_rpos;
		Button bu_model_zscalepos;
		Button bu_model_yscalepos;
		Button bu_model_xscalepos;
		Button bu_model_zscaleneg;
		Button bu_model_yscaleneg;
		Button bu_model_xscaleneg;
		Label la_scale;
		Label la_rotate;
		Label la_xyaxis;
		Label la_zaxis;
		Button bu_model_reset;


		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.pa_bot = new System.Windows.Forms.Panel();
			this.gb_model = new System.Windows.Forms.GroupBox();
			this.bu_model_reset = new System.Windows.Forms.Button();
			this.la_scale = new System.Windows.Forms.Label();
			this.la_rotate = new System.Windows.Forms.Label();
			this.la_xyaxis = new System.Windows.Forms.Label();
			this.la_zaxis = new System.Windows.Forms.Label();
			this.bu_model_zscaleneg = new System.Windows.Forms.Button();
			this.bu_model_yscaleneg = new System.Windows.Forms.Button();
			this.bu_model_xscaleneg = new System.Windows.Forms.Button();
			this.bu_model_zscalepos = new System.Windows.Forms.Button();
			this.bu_model_yscalepos = new System.Windows.Forms.Button();
			this.bu_model_xscalepos = new System.Windows.Forms.Button();
			this.bu_model_rneg = new System.Windows.Forms.Button();
			this.bu_model_rpos = new System.Windows.Forms.Button();
			this.bu_model_ypos = new System.Windows.Forms.Button();
			this.bu_model_yneg = new System.Windows.Forms.Button();
			this.bu_model_xneg = new System.Windows.Forms.Button();
			this.bu_model_xpos = new System.Windows.Forms.Button();
			this.bu_model_zneg = new System.Windows.Forms.Button();
			this.bu_model_zpos = new System.Windows.Forms.Button();
			this.pa_bot.SuspendLayout();
			this.gb_model.SuspendLayout();
			this.SuspendLayout();
			// 
			// pa_bot
			// 
			this.pa_bot.Controls.Add(this.gb_model);
			this.pa_bot.Dock = System.Windows.Forms.DockStyle.Right;
			this.pa_bot.Location = new System.Drawing.Point(292, 0);
			this.pa_bot.Margin = new System.Windows.Forms.Padding(0);
			this.pa_bot.Name = "pa_bot";
			this.pa_bot.Size = new System.Drawing.Size(300, 474);
			this.pa_bot.TabIndex = 0;
			// 
			// gb_model
			// 
			this.gb_model.Controls.Add(this.bu_model_reset);
			this.gb_model.Controls.Add(this.la_scale);
			this.gb_model.Controls.Add(this.la_rotate);
			this.gb_model.Controls.Add(this.la_xyaxis);
			this.gb_model.Controls.Add(this.la_zaxis);
			this.gb_model.Controls.Add(this.bu_model_zscaleneg);
			this.gb_model.Controls.Add(this.bu_model_yscaleneg);
			this.gb_model.Controls.Add(this.bu_model_xscaleneg);
			this.gb_model.Controls.Add(this.bu_model_zscalepos);
			this.gb_model.Controls.Add(this.bu_model_yscalepos);
			this.gb_model.Controls.Add(this.bu_model_xscalepos);
			this.gb_model.Controls.Add(this.bu_model_rneg);
			this.gb_model.Controls.Add(this.bu_model_rpos);
			this.gb_model.Controls.Add(this.bu_model_ypos);
			this.gb_model.Controls.Add(this.bu_model_yneg);
			this.gb_model.Controls.Add(this.bu_model_xneg);
			this.gb_model.Controls.Add(this.bu_model_xpos);
			this.gb_model.Controls.Add(this.bu_model_zneg);
			this.gb_model.Controls.Add(this.bu_model_zpos);
			this.gb_model.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_model.Location = new System.Drawing.Point(0, 0);
			this.gb_model.Margin = new System.Windows.Forms.Padding(0);
			this.gb_model.Name = "gb_model";
			this.gb_model.Padding = new System.Windows.Forms.Padding(0);
			this.gb_model.Size = new System.Drawing.Size(300, 105);
			this.gb_model.TabIndex = 0;
			this.gb_model.TabStop = false;
			this.gb_model.Text = " Model ";
			// 
			// bu_model_reset
			// 
			this.bu_model_reset.Location = new System.Drawing.Point(10, 80);
			this.bu_model_reset.Name = "bu_model_reset";
			this.bu_model_reset.Size = new System.Drawing.Size(232, 20);
			this.bu_model_reset.TabIndex = 18;
			this.bu_model_reset.Text = "reset";
			this.bu_model_reset.UseVisualStyleBackColor = true;
			this.bu_model_reset.Click += new System.EventHandler(this.bu_modelreset);
			// 
			// la_scale
			// 
			this.la_scale.Location = new System.Drawing.Point(170, 15);
			this.la_scale.Margin = new System.Windows.Forms.Padding(0);
			this.la_scale.Name = "la_scale";
			this.la_scale.Size = new System.Drawing.Size(70, 15);
			this.la_scale.TabIndex = 11;
			this.la_scale.Text = "scale";
			this.la_scale.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_rotate
			// 
			this.la_rotate.Location = new System.Drawing.Point(130, 15);
			this.la_rotate.Margin = new System.Windows.Forms.Padding(0);
			this.la_rotate.Name = "la_rotate";
			this.la_rotate.Size = new System.Drawing.Size(25, 15);
			this.la_rotate.TabIndex = 8;
			this.la_rotate.Text = "rot";
			this.la_rotate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_xyaxis
			// 
			this.la_xyaxis.Location = new System.Drawing.Point(45, 15);
			this.la_xyaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_xyaxis.Name = "la_xyaxis";
			this.la_xyaxis.Size = new System.Drawing.Size(70, 15);
			this.la_xyaxis.TabIndex = 3;
			this.la_xyaxis.Text = "x/y";
			this.la_xyaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_zaxis
			// 
			this.la_zaxis.Location = new System.Drawing.Point(10, 15);
			this.la_zaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_zaxis.Name = "la_zaxis";
			this.la_zaxis.Size = new System.Drawing.Size(20, 15);
			this.la_zaxis.TabIndex = 0;
			this.la_zaxis.Text = "z";
			this.la_zaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// bu_model_zscaleneg
			// 
			this.bu_model_zscaleneg.Location = new System.Drawing.Point(220, 55);
			this.bu_model_zscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zscaleneg.Name = "bu_model_zscaleneg";
			this.bu_model_zscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zscaleneg.TabIndex = 17;
			this.bu_model_zscaleneg.Text = "z";
			this.bu_model_zscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_zscaleneg.Click += new System.EventHandler(this.bu_scale);
			this.bu_model_zscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yscaleneg
			// 
			this.bu_model_yscaleneg.Location = new System.Drawing.Point(195, 55);
			this.bu_model_yscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yscaleneg.Name = "bu_model_yscaleneg";
			this.bu_model_yscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yscaleneg.TabIndex = 15;
			this.bu_model_yscaleneg.Text = "y";
			this.bu_model_yscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_yscaleneg.Click += new System.EventHandler(this.bu_scale);
			this.bu_model_yscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xscaleneg
			// 
			this.bu_model_xscaleneg.Location = new System.Drawing.Point(170, 55);
			this.bu_model_xscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xscaleneg.Name = "bu_model_xscaleneg";
			this.bu_model_xscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xscaleneg.TabIndex = 13;
			this.bu_model_xscaleneg.Text = "x";
			this.bu_model_xscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_xscaleneg.Click += new System.EventHandler(this.bu_scale);
			this.bu_model_xscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_zscalepos
			// 
			this.bu_model_zscalepos.Location = new System.Drawing.Point(220, 30);
			this.bu_model_zscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zscalepos.Name = "bu_model_zscalepos";
			this.bu_model_zscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zscalepos.TabIndex = 16;
			this.bu_model_zscalepos.Text = "z";
			this.bu_model_zscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zscalepos.UseVisualStyleBackColor = true;
			this.bu_model_zscalepos.Click += new System.EventHandler(this.bu_scale);
			this.bu_model_zscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yscalepos
			// 
			this.bu_model_yscalepos.Location = new System.Drawing.Point(195, 30);
			this.bu_model_yscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yscalepos.Name = "bu_model_yscalepos";
			this.bu_model_yscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yscalepos.TabIndex = 14;
			this.bu_model_yscalepos.Text = "y";
			this.bu_model_yscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yscalepos.UseVisualStyleBackColor = true;
			this.bu_model_yscalepos.Click += new System.EventHandler(this.bu_scale);
			this.bu_model_yscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xscalepos
			// 
			this.bu_model_xscalepos.Location = new System.Drawing.Point(170, 30);
			this.bu_model_xscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xscalepos.Name = "bu_model_xscalepos";
			this.bu_model_xscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xscalepos.TabIndex = 12;
			this.bu_model_xscalepos.Text = "x";
			this.bu_model_xscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xscalepos.UseVisualStyleBackColor = true;
			this.bu_model_xscalepos.Click += new System.EventHandler(this.bu_scale);
			this.bu_model_xscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_rneg
			// 
			this.bu_model_rneg.Location = new System.Drawing.Point(130, 55);
			this.bu_model_rneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_rneg.Name = "bu_model_rneg";
			this.bu_model_rneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_rneg.TabIndex = 10;
			this.bu_model_rneg.Text = "↶";
			this.bu_model_rneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_rneg.UseVisualStyleBackColor = true;
			this.bu_model_rneg.Click += new System.EventHandler(this.bu_rotneg);
			this.bu_model_rneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_rneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_rpos
			// 
			this.bu_model_rpos.Location = new System.Drawing.Point(130, 30);
			this.bu_model_rpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_rpos.Name = "bu_model_rpos";
			this.bu_model_rpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_rpos.TabIndex = 9;
			this.bu_model_rpos.Text = "↷";
			this.bu_model_rpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_rpos.UseVisualStyleBackColor = true;
			this.bu_model_rpos.Click += new System.EventHandler(this.bu_rotpos);
			this.bu_model_rpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_rpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_ypos
			// 
			this.bu_model_ypos.Location = new System.Drawing.Point(95, 40);
			this.bu_model_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_ypos.Name = "bu_model_ypos";
			this.bu_model_ypos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_ypos.TabIndex = 5;
			this.bu_model_ypos.Text = "▶";
			this.bu_model_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_ypos.UseVisualStyleBackColor = true;
			this.bu_model_ypos.Click += new System.EventHandler(this.bu_xpos);
			this.bu_model_ypos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_ypos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yneg
			// 
			this.bu_model_yneg.Location = new System.Drawing.Point(45, 40);
			this.bu_model_yneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yneg.Name = "bu_model_yneg";
			this.bu_model_yneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yneg.TabIndex = 4;
			this.bu_model_yneg.Text = "◀";
			this.bu_model_yneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yneg.UseVisualStyleBackColor = true;
			this.bu_model_yneg.Click += new System.EventHandler(this.bu_xneg);
			this.bu_model_yneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xneg
			// 
			this.bu_model_xneg.Location = new System.Drawing.Point(70, 55);
			this.bu_model_xneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xneg.Name = "bu_model_xneg";
			this.bu_model_xneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xneg.TabIndex = 7;
			this.bu_model_xneg.Text = "▼";
			this.bu_model_xneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xneg.UseVisualStyleBackColor = true;
			this.bu_model_xneg.Click += new System.EventHandler(this.bu_yneg);
			this.bu_model_xneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xpos
			// 
			this.bu_model_xpos.Location = new System.Drawing.Point(70, 30);
			this.bu_model_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xpos.Name = "bu_model_xpos";
			this.bu_model_xpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xpos.TabIndex = 6;
			this.bu_model_xpos.Text = "▲";
			this.bu_model_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xpos.UseVisualStyleBackColor = true;
			this.bu_model_xpos.Click += new System.EventHandler(this.bu_ypos);
			this.bu_model_xpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_zneg
			// 
			this.bu_model_zneg.Location = new System.Drawing.Point(10, 55);
			this.bu_model_zneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zneg.Name = "bu_model_zneg";
			this.bu_model_zneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zneg.TabIndex = 2;
			this.bu_model_zneg.Text = "↓";
			this.bu_model_zneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zneg.UseVisualStyleBackColor = true;
			this.bu_model_zneg.Click += new System.EventHandler(this.bu_zneg);
			this.bu_model_zneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_zpos
			// 
			this.bu_model_zpos.Location = new System.Drawing.Point(10, 30);
			this.bu_model_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zpos.Name = "bu_model_zpos";
			this.bu_model_zpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zpos.TabIndex = 1;
			this.bu_model_zpos.Text = "↑";
			this.bu_model_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zpos.UseVisualStyleBackColor = true;
			this.bu_model_zpos.Click += new System.EventHandler(this.bu_zpos);
			this.bu_model_zpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// CreatureVisualizerF
			// 
			this.ClientSize = new System.Drawing.Size(592, 474);
			this.Controls.Add(this.pa_bot);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "CreatureVisualizerF";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Creature Visualizer";
			this.Activated += new System.EventHandler(this.activated_Refresh);
			this.pa_bot.ResumeLayout(false);
			this.gb_model.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
