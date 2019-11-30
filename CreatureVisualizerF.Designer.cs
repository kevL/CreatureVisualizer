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

		Panel pa_controls;
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
		Label la_model_scale;
		Label la_model_rotate;
		Label la_model_xyaxis;
		Label la_model_zaxis;
		Button bu_model_reset;
		Button bu_model_scalereset;
		Button bu_model_rotreset;
		Button bu_model_xyreset;
		Button bu_model_zreset;
		Label la_model_zscale;
		Label la_model_yscale;
		Label la_model_xscale;
		Button bu_model_scaleneg;
		Button bu_model_scalepos;
		Label la_model_scaleorg;

		StatusStrip ss_bot;
		ToolStripStatusLabel tssl_xpos;
		ToolStripStatusLabel tssl_ypos;
		ToolStripStatusLabel tssl_zpos;
		ToolStripStatusLabel tssl_rot;

		GroupBox gb_camera;
		Label la_camera_pitch;
		Button button1;
		Button button2;
		Label label2;
		Label label3;
		Label label4;
		Button button3;
		Button bu_camera_rotreset;
		Button bu_camera_xyreset;
		Button bu_camera_zreset;
		Button bu_camera_reset;
		Label la_camera_zoom;
		Label la_camera_rotate;
		Label la_camera_xyaxis;
		Label la_camera_zaxis;
		Button button8;
		Button button9;
		Button button10;
		Button button11;
		Button button12;
		Button button13;
		Button bu_camera_rneg;
		Button bu_camera_rpos;
		Button bu_camera_ypos;
		Button bu_camera_yneg;
		Button bu_camera_xneg;
		Button bu_camera_xpos;
		Button bu_camera_zneg;
		Button bu_camera_zpos;


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
			this.pa_controls = new System.Windows.Forms.Panel();
			this.gb_camera = new System.Windows.Forms.GroupBox();
			this.la_camera_pitch = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.bu_camera_rotreset = new System.Windows.Forms.Button();
			this.bu_camera_xyreset = new System.Windows.Forms.Button();
			this.bu_camera_zreset = new System.Windows.Forms.Button();
			this.bu_camera_reset = new System.Windows.Forms.Button();
			this.la_camera_zoom = new System.Windows.Forms.Label();
			this.la_camera_rotate = new System.Windows.Forms.Label();
			this.la_camera_xyaxis = new System.Windows.Forms.Label();
			this.la_camera_zaxis = new System.Windows.Forms.Label();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.button10 = new System.Windows.Forms.Button();
			this.button11 = new System.Windows.Forms.Button();
			this.button12 = new System.Windows.Forms.Button();
			this.button13 = new System.Windows.Forms.Button();
			this.bu_camera_rneg = new System.Windows.Forms.Button();
			this.bu_camera_rpos = new System.Windows.Forms.Button();
			this.bu_camera_ypos = new System.Windows.Forms.Button();
			this.bu_camera_yneg = new System.Windows.Forms.Button();
			this.bu_camera_xneg = new System.Windows.Forms.Button();
			this.bu_camera_xpos = new System.Windows.Forms.Button();
			this.bu_camera_zneg = new System.Windows.Forms.Button();
			this.bu_camera_zpos = new System.Windows.Forms.Button();
			this.ss_bot = new System.Windows.Forms.StatusStrip();
			this.tssl_xpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_ypos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_zpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_rot = new System.Windows.Forms.ToolStripStatusLabel();
			this.gb_model = new System.Windows.Forms.GroupBox();
			this.la_model_scaleorg = new System.Windows.Forms.Label();
			this.bu_model_scaleneg = new System.Windows.Forms.Button();
			this.bu_model_scalepos = new System.Windows.Forms.Button();
			this.la_model_zscale = new System.Windows.Forms.Label();
			this.la_model_yscale = new System.Windows.Forms.Label();
			this.la_model_xscale = new System.Windows.Forms.Label();
			this.bu_model_scalereset = new System.Windows.Forms.Button();
			this.bu_model_rotreset = new System.Windows.Forms.Button();
			this.bu_model_xyreset = new System.Windows.Forms.Button();
			this.bu_model_zreset = new System.Windows.Forms.Button();
			this.bu_model_reset = new System.Windows.Forms.Button();
			this.la_model_scale = new System.Windows.Forms.Label();
			this.la_model_rotate = new System.Windows.Forms.Label();
			this.la_model_xyaxis = new System.Windows.Forms.Label();
			this.la_model_zaxis = new System.Windows.Forms.Label();
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
			this.pa_controls.SuspendLayout();
			this.gb_camera.SuspendLayout();
			this.ss_bot.SuspendLayout();
			this.gb_model.SuspendLayout();
			this.SuspendLayout();
			// 
			// pa_controls
			// 
			this.pa_controls.Controls.Add(this.gb_camera);
			this.pa_controls.Controls.Add(this.ss_bot);
			this.pa_controls.Controls.Add(this.gb_model);
			this.pa_controls.Dock = System.Windows.Forms.DockStyle.Right;
			this.pa_controls.Location = new System.Drawing.Point(292, 0);
			this.pa_controls.Margin = new System.Windows.Forms.Padding(0);
			this.pa_controls.Name = "pa_controls";
			this.pa_controls.Size = new System.Drawing.Size(290, 474);
			this.pa_controls.TabIndex = 0;
			this.pa_controls.Visible = false;
			// 
			// gb_camera
			// 
			this.gb_camera.Controls.Add(this.la_camera_pitch);
			this.gb_camera.Controls.Add(this.button1);
			this.gb_camera.Controls.Add(this.button2);
			this.gb_camera.Controls.Add(this.label2);
			this.gb_camera.Controls.Add(this.label3);
			this.gb_camera.Controls.Add(this.label4);
			this.gb_camera.Controls.Add(this.button3);
			this.gb_camera.Controls.Add(this.bu_camera_rotreset);
			this.gb_camera.Controls.Add(this.bu_camera_xyreset);
			this.gb_camera.Controls.Add(this.bu_camera_zreset);
			this.gb_camera.Controls.Add(this.bu_camera_reset);
			this.gb_camera.Controls.Add(this.la_camera_zoom);
			this.gb_camera.Controls.Add(this.la_camera_rotate);
			this.gb_camera.Controls.Add(this.la_camera_xyaxis);
			this.gb_camera.Controls.Add(this.la_camera_zaxis);
			this.gb_camera.Controls.Add(this.button8);
			this.gb_camera.Controls.Add(this.button9);
			this.gb_camera.Controls.Add(this.button10);
			this.gb_camera.Controls.Add(this.button11);
			this.gb_camera.Controls.Add(this.button12);
			this.gb_camera.Controls.Add(this.button13);
			this.gb_camera.Controls.Add(this.bu_camera_rneg);
			this.gb_camera.Controls.Add(this.bu_camera_rpos);
			this.gb_camera.Controls.Add(this.bu_camera_ypos);
			this.gb_camera.Controls.Add(this.bu_camera_yneg);
			this.gb_camera.Controls.Add(this.bu_camera_xneg);
			this.gb_camera.Controls.Add(this.bu_camera_xpos);
			this.gb_camera.Controls.Add(this.bu_camera_zneg);
			this.gb_camera.Controls.Add(this.bu_camera_zpos);
			this.gb_camera.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_camera.Location = new System.Drawing.Point(0, 130);
			this.gb_camera.Margin = new System.Windows.Forms.Padding(0);
			this.gb_camera.Name = "gb_camera";
			this.gb_camera.Padding = new System.Windows.Forms.Padding(0);
			this.gb_camera.Size = new System.Drawing.Size(290, 130);
			this.gb_camera.TabIndex = 2;
			this.gb_camera.TabStop = false;
			this.gb_camera.Text = " Camera ";
			// 
			// la_camera_pitch
			// 
			this.la_camera_pitch.ForeColor = System.Drawing.Color.RoyalBlue;
			this.la_camera_pitch.Location = new System.Drawing.Point(219, 15);
			this.la_camera_pitch.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_pitch.Name = "la_camera_pitch";
			this.la_camera_pitch.Size = new System.Drawing.Size(40, 15);
			this.la_camera_pitch.TabIndex = 57;
			this.la_camera_pitch.Text = "PITCH";
			this.la_camera_pitch.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(259, 55);
			this.button1.Margin = new System.Windows.Forms.Padding(0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(22, 22);
			this.button1.TabIndex = 56;
			this.button1.Text = "-";
			this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(259, 30);
			this.button2.Margin = new System.Windows.Forms.Padding(0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(22, 22);
			this.button2.TabIndex = 55;
			this.button2.Text = "+";
			this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(219, 80);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 20);
			this.label2.TabIndex = 52;
			this.label2.Text = "z";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(219, 55);
			this.label3.Margin = new System.Windows.Forms.Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 20);
			this.label3.TabIndex = 49;
			this.label3.Text = "y";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(219, 30);
			this.label4.Margin = new System.Windows.Forms.Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 20);
			this.label4.TabIndex = 46;
			this.label4.Text = "x";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// button3
			// 
			this.button3.ForeColor = System.Drawing.Color.Crimson;
			this.button3.Location = new System.Drawing.Point(259, 80);
			this.button3.Margin = new System.Windows.Forms.Padding(0);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(22, 22);
			this.button3.TabIndex = 53;
			this.button3.Text = "r";
			this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button3.UseVisualStyleBackColor = true;
			// 
			// bu_camera_rotreset
			// 
			this.bu_camera_rotreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_rotreset.Location = new System.Drawing.Point(129, 80);
			this.bu_camera_rotreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_rotreset.Name = "bu_camera_rotreset";
			this.bu_camera_rotreset.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_rotreset.TabIndex = 42;
			this.bu_camera_rotreset.Text = "r";
			this.bu_camera_rotreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_rotreset.UseVisualStyleBackColor = true;
			// 
			// bu_camera_xyreset
			// 
			this.bu_camera_xyreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_xyreset.Location = new System.Drawing.Point(69, 80);
			this.bu_camera_xyreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_xyreset.Name = "bu_camera_xyreset";
			this.bu_camera_xyreset.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_xyreset.TabIndex = 38;
			this.bu_camera_xyreset.Text = "r";
			this.bu_camera_xyreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_xyreset.UseVisualStyleBackColor = true;
			// 
			// bu_camera_zreset
			// 
			this.bu_camera_zreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_zreset.Location = new System.Drawing.Point(9, 80);
			this.bu_camera_zreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_zreset.Name = "bu_camera_zreset";
			this.bu_camera_zreset.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_zreset.TabIndex = 32;
			this.bu_camera_zreset.Text = "f";
			this.bu_camera_zreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_zreset.UseVisualStyleBackColor = true;
			this.bu_camera_zreset.Click += new System.EventHandler(this.click_bu_camera_focus);
			// 
			// bu_camera_reset
			// 
			this.bu_camera_reset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_reset.Location = new System.Drawing.Point(9, 105);
			this.bu_camera_reset.Name = "bu_camera_reset";
			this.bu_camera_reset.Size = new System.Drawing.Size(272, 20);
			this.bu_camera_reset.TabIndex = 54;
			this.bu_camera_reset.Text = "reset";
			this.bu_camera_reset.UseVisualStyleBackColor = true;
			// 
			// la_camera_zoom
			// 
			this.la_camera_zoom.Location = new System.Drawing.Point(169, 15);
			this.la_camera_zoom.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_zoom.Name = "la_camera_zoom";
			this.la_camera_zoom.Size = new System.Drawing.Size(50, 15);
			this.la_camera_zoom.TabIndex = 43;
			this.la_camera_zoom.Text = "ZOOM";
			this.la_camera_zoom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_camera_rotate
			// 
			this.la_camera_rotate.Location = new System.Drawing.Point(129, 15);
			this.la_camera_rotate.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_rotate.Name = "la_camera_rotate";
			this.la_camera_rotate.Size = new System.Drawing.Size(25, 15);
			this.la_camera_rotate.TabIndex = 39;
			this.la_camera_rotate.Text = "rot";
			this.la_camera_rotate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_camera_xyaxis
			// 
			this.la_camera_xyaxis.Location = new System.Drawing.Point(44, 15);
			this.la_camera_xyaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_xyaxis.Name = "la_camera_xyaxis";
			this.la_camera_xyaxis.Size = new System.Drawing.Size(75, 15);
			this.la_camera_xyaxis.TabIndex = 33;
			this.la_camera_xyaxis.Text = "x/y";
			this.la_camera_xyaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_camera_zaxis
			// 
			this.la_camera_zaxis.Location = new System.Drawing.Point(9, 15);
			this.la_camera_zaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_zaxis.Name = "la_camera_zaxis";
			this.la_camera_zaxis.Size = new System.Drawing.Size(25, 15);
			this.la_camera_zaxis.TabIndex = 29;
			this.la_camera_zaxis.Text = "z";
			this.la_camera_zaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(169, 80);
			this.button8.Margin = new System.Windows.Forms.Padding(0);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(22, 22);
			this.button8.TabIndex = 50;
			this.button8.Text = "z";
			this.button8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button8.UseVisualStyleBackColor = true;
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(169, 55);
			this.button9.Margin = new System.Windows.Forms.Padding(0);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(22, 22);
			this.button9.TabIndex = 47;
			this.button9.Text = "y";
			this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button9.UseVisualStyleBackColor = true;
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(169, 30);
			this.button10.Margin = new System.Windows.Forms.Padding(0);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(22, 22);
			this.button10.TabIndex = 44;
			this.button10.Text = "x";
			this.button10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button10.UseVisualStyleBackColor = true;
			// 
			// button11
			// 
			this.button11.Location = new System.Drawing.Point(194, 80);
			this.button11.Margin = new System.Windows.Forms.Padding(0);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(22, 22);
			this.button11.TabIndex = 51;
			this.button11.Text = "z";
			this.button11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button11.UseVisualStyleBackColor = true;
			// 
			// button12
			// 
			this.button12.Location = new System.Drawing.Point(194, 55);
			this.button12.Margin = new System.Windows.Forms.Padding(0);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(22, 22);
			this.button12.TabIndex = 48;
			this.button12.Text = "y";
			this.button12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button12.UseVisualStyleBackColor = true;
			// 
			// button13
			// 
			this.button13.Location = new System.Drawing.Point(194, 30);
			this.button13.Margin = new System.Windows.Forms.Padding(0);
			this.button13.Name = "button13";
			this.button13.Size = new System.Drawing.Size(22, 22);
			this.button13.TabIndex = 45;
			this.button13.Text = "x";
			this.button13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button13.UseVisualStyleBackColor = true;
			// 
			// bu_camera_rneg
			// 
			this.bu_camera_rneg.Location = new System.Drawing.Point(129, 55);
			this.bu_camera_rneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_rneg.Name = "bu_camera_rneg";
			this.bu_camera_rneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_rneg.TabIndex = 41;
			this.bu_camera_rneg.Text = "-";
			this.bu_camera_rneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_rneg.UseVisualStyleBackColor = true;
			this.bu_camera_rneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_rneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_rpos
			// 
			this.bu_camera_rpos.Location = new System.Drawing.Point(129, 30);
			this.bu_camera_rpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_rpos.Name = "bu_camera_rpos";
			this.bu_camera_rpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_rpos.TabIndex = 40;
			this.bu_camera_rpos.Text = "+";
			this.bu_camera_rpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_rpos.UseVisualStyleBackColor = true;
			this.bu_camera_rpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_rpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_ypos
			// 
			this.bu_camera_ypos.Location = new System.Drawing.Point(94, 40);
			this.bu_camera_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_ypos.Name = "bu_camera_ypos";
			this.bu_camera_ypos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_ypos.TabIndex = 35;
			this.bu_camera_ypos.Text = "r";
			this.bu_camera_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_ypos.UseVisualStyleBackColor = true;
			this.bu_camera_ypos.Click += new System.EventHandler(this.click_bu_camera_xpos);
			this.bu_camera_ypos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_ypos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_yneg
			// 
			this.bu_camera_yneg.Location = new System.Drawing.Point(44, 40);
			this.bu_camera_yneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_yneg.Name = "bu_camera_yneg";
			this.bu_camera_yneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_yneg.TabIndex = 37;
			this.bu_camera_yneg.Text = "l";
			this.bu_camera_yneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_yneg.UseVisualStyleBackColor = true;
			this.bu_camera_yneg.Click += new System.EventHandler(this.click_bu_camera_xneg);
			this.bu_camera_yneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_yneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_xneg
			// 
			this.bu_camera_xneg.Location = new System.Drawing.Point(69, 55);
			this.bu_camera_xneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_xneg.Name = "bu_camera_xneg";
			this.bu_camera_xneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_xneg.TabIndex = 36;
			this.bu_camera_xneg.Text = "b";
			this.bu_camera_xneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_xneg.UseVisualStyleBackColor = true;
			this.bu_camera_xneg.Click += new System.EventHandler(this.click_bu_camera_yneg);
			this.bu_camera_xneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_xneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_xpos
			// 
			this.bu_camera_xpos.Location = new System.Drawing.Point(69, 30);
			this.bu_camera_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_xpos.Name = "bu_camera_xpos";
			this.bu_camera_xpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_xpos.TabIndex = 34;
			this.bu_camera_xpos.Text = "f";
			this.bu_camera_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_xpos.UseVisualStyleBackColor = true;
			this.bu_camera_xpos.Click += new System.EventHandler(this.click_bu_camera_ypos);
			this.bu_camera_xpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_xpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_zneg
			// 
			this.bu_camera_zneg.Location = new System.Drawing.Point(9, 55);
			this.bu_camera_zneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_zneg.Name = "bu_camera_zneg";
			this.bu_camera_zneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_zneg.TabIndex = 31;
			this.bu_camera_zneg.Text = "d";
			this.bu_camera_zneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_zneg.UseVisualStyleBackColor = true;
			this.bu_camera_zneg.Click += new System.EventHandler(this.click_bu_camera_zneg);
			this.bu_camera_zneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_zneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_zpos
			// 
			this.bu_camera_zpos.Location = new System.Drawing.Point(9, 30);
			this.bu_camera_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_zpos.Name = "bu_camera_zpos";
			this.bu_camera_zpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_zpos.TabIndex = 30;
			this.bu_camera_zpos.Text = "u";
			this.bu_camera_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_zpos.UseVisualStyleBackColor = true;
			this.bu_camera_zpos.Click += new System.EventHandler(this.click_bu_camera_zpos);
			this.bu_camera_zpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_zpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// ss_bot
			// 
			this.ss_bot.Font = new System.Drawing.Font("Consolas", 8F);
			this.ss_bot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tssl_xpos,
			this.tssl_ypos,
			this.tssl_zpos,
			this.tssl_rot});
			this.ss_bot.Location = new System.Drawing.Point(0, 452);
			this.ss_bot.Name = "ss_bot";
			this.ss_bot.Size = new System.Drawing.Size(290, 22);
			this.ss_bot.TabIndex = 1;
			// 
			// tssl_xpos
			// 
			this.tssl_xpos.AutoSize = false;
			this.tssl_xpos.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.tssl_xpos.Name = "tssl_xpos";
			this.tssl_xpos.Size = new System.Drawing.Size(50, 22);
			this.tssl_xpos.Text = "x";
			// 
			// tssl_ypos
			// 
			this.tssl_ypos.AutoSize = false;
			this.tssl_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_ypos.Name = "tssl_ypos";
			this.tssl_ypos.Size = new System.Drawing.Size(50, 22);
			this.tssl_ypos.Text = "y";
			// 
			// tssl_zpos
			// 
			this.tssl_zpos.AutoSize = false;
			this.tssl_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_zpos.Name = "tssl_zpos";
			this.tssl_zpos.Size = new System.Drawing.Size(50, 22);
			this.tssl_zpos.Text = "z";
			// 
			// tssl_rot
			// 
			this.tssl_rot.AutoSize = false;
			this.tssl_rot.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_rot.Name = "tssl_rot";
			this.tssl_rot.Size = new System.Drawing.Size(50, 22);
			this.tssl_rot.Text = "r";
			// 
			// gb_model
			// 
			this.gb_model.Controls.Add(this.la_model_scaleorg);
			this.gb_model.Controls.Add(this.bu_model_scaleneg);
			this.gb_model.Controls.Add(this.bu_model_scalepos);
			this.gb_model.Controls.Add(this.la_model_zscale);
			this.gb_model.Controls.Add(this.la_model_yscale);
			this.gb_model.Controls.Add(this.la_model_xscale);
			this.gb_model.Controls.Add(this.bu_model_scalereset);
			this.gb_model.Controls.Add(this.bu_model_rotreset);
			this.gb_model.Controls.Add(this.bu_model_xyreset);
			this.gb_model.Controls.Add(this.bu_model_zreset);
			this.gb_model.Controls.Add(this.bu_model_reset);
			this.gb_model.Controls.Add(this.la_model_scale);
			this.gb_model.Controls.Add(this.la_model_rotate);
			this.gb_model.Controls.Add(this.la_model_xyaxis);
			this.gb_model.Controls.Add(this.la_model_zaxis);
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
			this.gb_model.Size = new System.Drawing.Size(290, 130);
			this.gb_model.TabIndex = 0;
			this.gb_model.TabStop = false;
			this.gb_model.Text = " Model ";
			// 
			// la_model_scaleorg
			// 
			this.la_model_scaleorg.ForeColor = System.Drawing.Color.RoyalBlue;
			this.la_model_scaleorg.Location = new System.Drawing.Point(220, 15);
			this.la_model_scaleorg.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_scaleorg.Name = "la_model_scaleorg";
			this.la_model_scaleorg.Size = new System.Drawing.Size(40, 15);
			this.la_model_scaleorg.TabIndex = 28;
			this.la_model_scaleorg.Text = "0";
			this.la_model_scaleorg.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// bu_model_scaleneg
			// 
			this.bu_model_scaleneg.Location = new System.Drawing.Point(260, 55);
			this.bu_model_scaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_scaleneg.Name = "bu_model_scaleneg";
			this.bu_model_scaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_scaleneg.TabIndex = 27;
			this.bu_model_scaleneg.Text = "-";
			this.bu_model_scaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_scaleneg.UseVisualStyleBackColor = true;
			this.bu_model_scaleneg.Click += new System.EventHandler(this.click_bu_model_scaleall);
			this.bu_model_scaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_scaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_scalepos
			// 
			this.bu_model_scalepos.Location = new System.Drawing.Point(260, 30);
			this.bu_model_scalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_scalepos.Name = "bu_model_scalepos";
			this.bu_model_scalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_scalepos.TabIndex = 26;
			this.bu_model_scalepos.Text = "+";
			this.bu_model_scalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_scalepos.UseVisualStyleBackColor = true;
			this.bu_model_scalepos.Click += new System.EventHandler(this.click_bu_model_scaleall);
			this.bu_model_scalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_scalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// la_model_zscale
			// 
			this.la_model_zscale.Location = new System.Drawing.Point(220, 80);
			this.la_model_zscale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_zscale.Name = "la_model_zscale";
			this.la_model_zscale.Size = new System.Drawing.Size(40, 20);
			this.la_model_zscale.TabIndex = 23;
			this.la_model_zscale.Text = "z";
			this.la_model_zscale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// la_model_yscale
			// 
			this.la_model_yscale.Location = new System.Drawing.Point(220, 55);
			this.la_model_yscale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_yscale.Name = "la_model_yscale";
			this.la_model_yscale.Size = new System.Drawing.Size(40, 20);
			this.la_model_yscale.TabIndex = 20;
			this.la_model_yscale.Text = "y";
			this.la_model_yscale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// la_model_xscale
			// 
			this.la_model_xscale.Location = new System.Drawing.Point(220, 30);
			this.la_model_xscale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_xscale.Name = "la_model_xscale";
			this.la_model_xscale.Size = new System.Drawing.Size(40, 20);
			this.la_model_xscale.TabIndex = 17;
			this.la_model_xscale.Text = "x";
			this.la_model_xscale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// bu_model_scalereset
			// 
			this.bu_model_scalereset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_model_scalereset.Location = new System.Drawing.Point(260, 80);
			this.bu_model_scalereset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_scalereset.Name = "bu_model_scalereset";
			this.bu_model_scalereset.Size = new System.Drawing.Size(22, 22);
			this.bu_model_scalereset.TabIndex = 24;
			this.bu_model_scalereset.Text = "r";
			this.bu_model_scalereset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_scalereset.UseVisualStyleBackColor = true;
			this.bu_model_scalereset.Click += new System.EventHandler(this.click_bu_model_scalereset);
			// 
			// bu_model_rotreset
			// 
			this.bu_model_rotreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_model_rotreset.Location = new System.Drawing.Point(130, 80);
			this.bu_model_rotreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_rotreset.Name = "bu_model_rotreset";
			this.bu_model_rotreset.Size = new System.Drawing.Size(22, 22);
			this.bu_model_rotreset.TabIndex = 13;
			this.bu_model_rotreset.Text = "r";
			this.bu_model_rotreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_rotreset.UseVisualStyleBackColor = true;
			this.bu_model_rotreset.Click += new System.EventHandler(this.click_bu_model_rotreset);
			// 
			// bu_model_xyreset
			// 
			this.bu_model_xyreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_model_xyreset.Location = new System.Drawing.Point(70, 80);
			this.bu_model_xyreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xyreset.Name = "bu_model_xyreset";
			this.bu_model_xyreset.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xyreset.TabIndex = 9;
			this.bu_model_xyreset.Text = "r";
			this.bu_model_xyreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xyreset.UseVisualStyleBackColor = true;
			this.bu_model_xyreset.Click += new System.EventHandler(this.click_bu_model_xyreset);
			// 
			// bu_model_zreset
			// 
			this.bu_model_zreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_model_zreset.Location = new System.Drawing.Point(10, 80);
			this.bu_model_zreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zreset.Name = "bu_model_zreset";
			this.bu_model_zreset.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zreset.TabIndex = 3;
			this.bu_model_zreset.Text = "r";
			this.bu_model_zreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zreset.UseVisualStyleBackColor = true;
			this.bu_model_zreset.Click += new System.EventHandler(this.click_bu_model_zreset);
			// 
			// bu_model_reset
			// 
			this.bu_model_reset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_model_reset.Location = new System.Drawing.Point(10, 105);
			this.bu_model_reset.Name = "bu_model_reset";
			this.bu_model_reset.Size = new System.Drawing.Size(272, 20);
			this.bu_model_reset.TabIndex = 25;
			this.bu_model_reset.Text = "reset";
			this.bu_model_reset.UseVisualStyleBackColor = true;
			this.bu_model_reset.Click += new System.EventHandler(this.click_bu_model_reset);
			// 
			// la_model_scale
			// 
			this.la_model_scale.Location = new System.Drawing.Point(170, 15);
			this.la_model_scale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_scale.Name = "la_model_scale";
			this.la_model_scale.Size = new System.Drawing.Size(50, 15);
			this.la_model_scale.TabIndex = 14;
			this.la_model_scale.Text = "scale";
			this.la_model_scale.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_model_rotate
			// 
			this.la_model_rotate.Location = new System.Drawing.Point(130, 15);
			this.la_model_rotate.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_rotate.Name = "la_model_rotate";
			this.la_model_rotate.Size = new System.Drawing.Size(25, 15);
			this.la_model_rotate.TabIndex = 10;
			this.la_model_rotate.Text = "rot";
			this.la_model_rotate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_model_xyaxis
			// 
			this.la_model_xyaxis.Location = new System.Drawing.Point(45, 15);
			this.la_model_xyaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_xyaxis.Name = "la_model_xyaxis";
			this.la_model_xyaxis.Size = new System.Drawing.Size(75, 15);
			this.la_model_xyaxis.TabIndex = 4;
			this.la_model_xyaxis.Text = "x/y";
			this.la_model_xyaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_model_zaxis
			// 
			this.la_model_zaxis.Location = new System.Drawing.Point(10, 15);
			this.la_model_zaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_zaxis.Name = "la_model_zaxis";
			this.la_model_zaxis.Size = new System.Drawing.Size(25, 15);
			this.la_model_zaxis.TabIndex = 0;
			this.la_model_zaxis.Text = "z";
			this.la_model_zaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// bu_model_zscaleneg
			// 
			this.bu_model_zscaleneg.Location = new System.Drawing.Point(170, 80);
			this.bu_model_zscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zscaleneg.Name = "bu_model_zscaleneg";
			this.bu_model_zscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zscaleneg.TabIndex = 21;
			this.bu_model_zscaleneg.Text = "z";
			this.bu_model_zscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_zscaleneg.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_zscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yscaleneg
			// 
			this.bu_model_yscaleneg.Location = new System.Drawing.Point(170, 55);
			this.bu_model_yscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yscaleneg.Name = "bu_model_yscaleneg";
			this.bu_model_yscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yscaleneg.TabIndex = 18;
			this.bu_model_yscaleneg.Text = "y";
			this.bu_model_yscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_yscaleneg.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_yscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xscaleneg
			// 
			this.bu_model_xscaleneg.Location = new System.Drawing.Point(170, 30);
			this.bu_model_xscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xscaleneg.Name = "bu_model_xscaleneg";
			this.bu_model_xscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xscaleneg.TabIndex = 15;
			this.bu_model_xscaleneg.Text = "x";
			this.bu_model_xscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_xscaleneg.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_xscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_zscalepos
			// 
			this.bu_model_zscalepos.Location = new System.Drawing.Point(195, 80);
			this.bu_model_zscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zscalepos.Name = "bu_model_zscalepos";
			this.bu_model_zscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zscalepos.TabIndex = 22;
			this.bu_model_zscalepos.Text = "z";
			this.bu_model_zscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zscalepos.UseVisualStyleBackColor = true;
			this.bu_model_zscalepos.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_zscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yscalepos
			// 
			this.bu_model_yscalepos.Location = new System.Drawing.Point(195, 55);
			this.bu_model_yscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yscalepos.Name = "bu_model_yscalepos";
			this.bu_model_yscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yscalepos.TabIndex = 19;
			this.bu_model_yscalepos.Text = "y";
			this.bu_model_yscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yscalepos.UseVisualStyleBackColor = true;
			this.bu_model_yscalepos.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_yscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xscalepos
			// 
			this.bu_model_xscalepos.Location = new System.Drawing.Point(195, 30);
			this.bu_model_xscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xscalepos.Name = "bu_model_xscalepos";
			this.bu_model_xscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xscalepos.TabIndex = 16;
			this.bu_model_xscalepos.Text = "x";
			this.bu_model_xscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xscalepos.UseVisualStyleBackColor = true;
			this.bu_model_xscalepos.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_xscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_rneg
			// 
			this.bu_model_rneg.Location = new System.Drawing.Point(130, 55);
			this.bu_model_rneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_rneg.Name = "bu_model_rneg";
			this.bu_model_rneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_rneg.TabIndex = 12;
			this.bu_model_rneg.Text = "-";
			this.bu_model_rneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_rneg.UseVisualStyleBackColor = true;
			this.bu_model_rneg.Click += new System.EventHandler(this.click_bu_model_rotneg);
			this.bu_model_rneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_rneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_rpos
			// 
			this.bu_model_rpos.Location = new System.Drawing.Point(130, 30);
			this.bu_model_rpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_rpos.Name = "bu_model_rpos";
			this.bu_model_rpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_rpos.TabIndex = 11;
			this.bu_model_rpos.Text = "+";
			this.bu_model_rpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_rpos.UseVisualStyleBackColor = true;
			this.bu_model_rpos.Click += new System.EventHandler(this.click_bu_model_rotpos);
			this.bu_model_rpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_rpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_ypos
			// 
			this.bu_model_ypos.Location = new System.Drawing.Point(95, 40);
			this.bu_model_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_ypos.Name = "bu_model_ypos";
			this.bu_model_ypos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_ypos.TabIndex = 6;
			this.bu_model_ypos.Text = "r";
			this.bu_model_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_ypos.UseVisualStyleBackColor = true;
			this.bu_model_ypos.Click += new System.EventHandler(this.click_bu_model_xpos);
			this.bu_model_ypos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_ypos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yneg
			// 
			this.bu_model_yneg.Location = new System.Drawing.Point(45, 40);
			this.bu_model_yneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yneg.Name = "bu_model_yneg";
			this.bu_model_yneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yneg.TabIndex = 8;
			this.bu_model_yneg.Text = "l";
			this.bu_model_yneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yneg.UseVisualStyleBackColor = true;
			this.bu_model_yneg.Click += new System.EventHandler(this.click_bu_model_xneg);
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
			this.bu_model_xneg.Text = "f";
			this.bu_model_xneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xneg.UseVisualStyleBackColor = true;
			this.bu_model_xneg.Click += new System.EventHandler(this.click_bu_model_yneg);
			this.bu_model_xneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xpos
			// 
			this.bu_model_xpos.Location = new System.Drawing.Point(70, 30);
			this.bu_model_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xpos.Name = "bu_model_xpos";
			this.bu_model_xpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xpos.TabIndex = 5;
			this.bu_model_xpos.Text = "b";
			this.bu_model_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xpos.UseVisualStyleBackColor = true;
			this.bu_model_xpos.Click += new System.EventHandler(this.click_bu_model_ypos);
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
			this.bu_model_zneg.Text = "d";
			this.bu_model_zneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zneg.UseVisualStyleBackColor = true;
			this.bu_model_zneg.Click += new System.EventHandler(this.click_bu_model_zneg);
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
			this.bu_model_zpos.Text = "u";
			this.bu_model_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zpos.UseVisualStyleBackColor = true;
			this.bu_model_zpos.Click += new System.EventHandler(this.click_bu_model_zpos);
			this.bu_model_zpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// CreatureVisualizerF
			// 
			this.ClientSize = new System.Drawing.Size(582, 474);
			this.Controls.Add(this.pa_controls);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "CreatureVisualizerF";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Creature Visualizer";
			this.Activated += new System.EventHandler(this.activated_Refresh);
			this.pa_controls.ResumeLayout(false);
			this.pa_controls.PerformLayout();
			this.gb_camera.ResumeLayout(false);
			this.ss_bot.ResumeLayout(false);
			this.ss_bot.PerformLayout();
			this.gb_model.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Designer
	}
}
