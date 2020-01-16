﻿using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace creaturevisualizer
{
	sealed partial class CreVisF
	{
		#region Designer
		/// <summary>
		/// Cleans up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();

			base.Dispose(disposing);
		}


		Panel pa_gui;

		Panel pa_con;

		CompositedTabControl tc1;


		TabPage tp_controls;

		GroupBox gb_camera;
		Label la_camera_zaxis;
		Label la_camera_xyaxis;
		Label la_camera_dist;
		Label la_camera_angle;
		Button bu_camera_zreset;
		Button bu_camera_xyreset;
		Button bu_camera_distreset;
		Button bu_camera_resetpolar;
		Button bu_camera_focusobject;
		Button bu_camera_zpos;
		Button bu_camera_zneg;
		Button bu_camera_ypos;
		Button bu_camera_yneg;
		Button bu_camera_xpos;
		Button bu_camera_xneg;
		Button bu_camera_distpos;
		Button bu_camera_distneg;
		Button bu_camera_pitchpos;
		Button bu_camera_pitchneg;
		Button bu_camera_yawpos;
		Button bu_camera_yawneg;
		Label la_camera_yaw;
		Label la_camera_pitch;
		Label la_camera_baseheight;
		TextBox tb_camera_baseheight;

		GroupBox gb_model;
		Label la_model_zaxis;
		Label la_model_xyaxis;
		Label la_model_rotate;
		Label la_model_scale;
		Button bu_model_zpos;
		Button bu_model_zneg;
		Button bu_model_ypos;
		Button bu_model_yneg;
		Button bu_model_xpos;
		Button bu_model_xneg;
		Button bu_model_rotneg;
		Button bu_model_rotpos;
		Button bu_model_xscalepos;
		Button bu_model_xscaleneg;
		Button bu_model_yscalepos;
		Button bu_model_yscaleneg;
		Button bu_model_zscalepos;
		Button bu_model_zscaleneg;
		Button bu_model_reset;
		Button bu_model_scalereset;
		Button bu_model_rotreset;
		Button bu_model_xyreset;
		Button bu_model_zreset;
		Label la_model_scaleorg;
		Label la_model_xscale;
		Label la_model_yscale;
		Label la_model_zscale;
		Button bu_model_scaleneg;
		Button bu_model_scalepos;

		GroupBox gb_Light;
		Label la_light_zaxis;
		Label la_light_xyaxis;
		Button bu_light_zpos;
		Button bu_light_zneg;
		Button bu_light_ypos;
		Button bu_light_yneg;
		Button bu_light_xpos;
		Button bu_light_xneg;
		Button bu_light_zreset;
		Button bu_light_xyreset;
		Button bu_light_reset;
		Label la_light_color;
		CheckBox cb_light_diffuse;
		CheckBox cb_light_specular;
		CheckBox cb_light_ambient;
		Panel pa_light_diffuse;
		Panel pa_light_specular;
		Panel pa_light_ambient;
		Label la_light_intensity;
		TextBox tb_light_intensity;

		Label la_dx;
		Label la_dy;
		Label la_dz;


		TabPage tp_creature;

		Button bu_creature_display;
		internal Button bu_creature_apply;

		ComboBox cbo_creature_gender;
		Label la_creature_gender;


		TabPage tp_resource;

		Label la_head_blueprint;
		Label la_head_template;
		Label la_head_resource;
		Label la_head_instance;
		Label la_repotype_;
		Label la_repotype;
		Label la_name_;
		Label la_name;
		Label la_type_;
		Label la_type;
		Label la_itype_;
		Label la_itype;
		Label la_resref_;
		Label la_resref;
		Label la_template_;
		Label la_template;
		Label la_file_inst;
		Label la_file_inst_;
		Label la_template_inst;
		Label la_template_inst_;
		Label la_restype_inst;
		Label la_restype_inst_;
		Label la_repo_inst;
		Label la_repo_inst_;
		Label la_areatag;
		Label la_areatag_;


		StatusStrip ss_camera;
		ToolStripStatusLabel tssl_camera_label;
		ToolStripStatusLabel tssl_camera_xpos;
		ToolStripStatusLabel tssl_camera_ypos;
		ToolStripStatusLabel tssl_camera_zpos;
		ToolStripStatusLabel tssl_camera_rot;
		ToolStripStatusLabel tssl_camera_dist;
		StatusStrip ss_model;
		ToolStripStatusLabel tssl_model_label;
		ToolStripStatusLabel tssl_model_xpos;
		ToolStripStatusLabel tssl_model_ypos;
		ToolStripStatusLabel tssl_model_zpos;
		ToolStripStatusLabel tssl_model_rot;
		StatusStrip ss_light;
		ToolStripStatusLabel tssl_light_label;
		ToolStripStatusLabel tssl_light_xpos;
		ToolStripStatusLabel tssl_light_ypos;
		ToolStripStatusLabel tssl_light_zpos;
		ToolStripStatusLabel tssl_light_blank;
		ToolStripStatusLabel tssl_light_intensity;

		IContainer components;
		ToolTip toolTip1;


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The
		/// Forms designer might not be able to load this method if it was
		/// changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pa_con = new System.Windows.Forms.Panel();
			this.tc1 = new creaturevisualizer.CompositedTabControl();
			this.tp_controls = new System.Windows.Forms.TabPage();
			this.gb_Light = new System.Windows.Forms.GroupBox();
			this.cb_light_ambient = new System.Windows.Forms.CheckBox();
			this.cb_light_specular = new System.Windows.Forms.CheckBox();
			this.cb_light_diffuse = new System.Windows.Forms.CheckBox();
			this.pa_light_ambient = new System.Windows.Forms.Panel();
			this.la_light_color = new System.Windows.Forms.Label();
			this.pa_light_specular = new System.Windows.Forms.Panel();
			this.pa_light_diffuse = new System.Windows.Forms.Panel();
			this.la_light_intensity = new System.Windows.Forms.Label();
			this.tb_light_intensity = new System.Windows.Forms.TextBox();
			this.la_dz = new System.Windows.Forms.Label();
			this.bu_light_xyreset = new System.Windows.Forms.Button();
			this.la_dy = new System.Windows.Forms.Label();
			this.la_dx = new System.Windows.Forms.Label();
			this.bu_light_zreset = new System.Windows.Forms.Button();
			this.bu_light_reset = new System.Windows.Forms.Button();
			this.la_light_xyaxis = new System.Windows.Forms.Label();
			this.la_light_zaxis = new System.Windows.Forms.Label();
			this.bu_light_ypos = new System.Windows.Forms.Button();
			this.bu_light_yneg = new System.Windows.Forms.Button();
			this.bu_light_xneg = new System.Windows.Forms.Button();
			this.bu_light_xpos = new System.Windows.Forms.Button();
			this.bu_light_zneg = new System.Windows.Forms.Button();
			this.bu_light_zpos = new System.Windows.Forms.Button();
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
			this.bu_model_rotneg = new System.Windows.Forms.Button();
			this.bu_model_rotpos = new System.Windows.Forms.Button();
			this.bu_model_ypos = new System.Windows.Forms.Button();
			this.bu_model_yneg = new System.Windows.Forms.Button();
			this.bu_model_xneg = new System.Windows.Forms.Button();
			this.bu_model_xpos = new System.Windows.Forms.Button();
			this.bu_model_zneg = new System.Windows.Forms.Button();
			this.bu_model_zpos = new System.Windows.Forms.Button();
			this.gb_camera = new System.Windows.Forms.GroupBox();
			this.la_camera_baseheight = new System.Windows.Forms.Label();
			this.tb_camera_baseheight = new System.Windows.Forms.TextBox();
			this.bu_camera_yawpos = new System.Windows.Forms.Button();
			this.la_camera_yaw = new System.Windows.Forms.Label();
			this.la_camera_pitch = new System.Windows.Forms.Label();
			this.bu_camera_resetpolar = new System.Windows.Forms.Button();
			this.la_camera_angle = new System.Windows.Forms.Label();
			this.bu_camera_yawneg = new System.Windows.Forms.Button();
			this.bu_camera_pitchneg = new System.Windows.Forms.Button();
			this.bu_camera_pitchpos = new System.Windows.Forms.Button();
			this.bu_camera_xyreset = new System.Windows.Forms.Button();
			this.bu_camera_zreset = new System.Windows.Forms.Button();
			this.bu_camera_focusobject = new System.Windows.Forms.Button();
			this.la_camera_dist = new System.Windows.Forms.Label();
			this.la_camera_xyaxis = new System.Windows.Forms.Label();
			this.la_camera_zaxis = new System.Windows.Forms.Label();
			this.bu_camera_distreset = new System.Windows.Forms.Button();
			this.bu_camera_distneg = new System.Windows.Forms.Button();
			this.bu_camera_distpos = new System.Windows.Forms.Button();
			this.bu_camera_ypos = new System.Windows.Forms.Button();
			this.bu_camera_yneg = new System.Windows.Forms.Button();
			this.bu_camera_xneg = new System.Windows.Forms.Button();
			this.bu_camera_xpos = new System.Windows.Forms.Button();
			this.bu_camera_zneg = new System.Windows.Forms.Button();
			this.bu_camera_zpos = new System.Windows.Forms.Button();
			this.tp_creature = new System.Windows.Forms.TabPage();
			this.bu_creature_apply = new System.Windows.Forms.Button();
			this.la_creature_gender = new System.Windows.Forms.Label();
			this.cbo_creature_gender = new System.Windows.Forms.ComboBox();
			this.bu_creature_display = new System.Windows.Forms.Button();
			this.tp_resource = new System.Windows.Forms.TabPage();
			this.la_repo_inst = new System.Windows.Forms.Label();
			this.la_areatag = new System.Windows.Forms.Label();
			this.la_areatag_ = new System.Windows.Forms.Label();
			this.la_head_instance = new System.Windows.Forms.Label();
			this.la_repo_inst_ = new System.Windows.Forms.Label();
			this.la_restype_inst = new System.Windows.Forms.Label();
			this.la_restype_inst_ = new System.Windows.Forms.Label();
			this.la_template_inst = new System.Windows.Forms.Label();
			this.la_template_inst_ = new System.Windows.Forms.Label();
			this.la_file_inst = new System.Windows.Forms.Label();
			this.la_file_inst_ = new System.Windows.Forms.Label();
			this.la_head_resource = new System.Windows.Forms.Label();
			this.la_head_blueprint = new System.Windows.Forms.Label();
			this.la_head_template = new System.Windows.Forms.Label();
			this.la_template = new System.Windows.Forms.Label();
			this.la_template_ = new System.Windows.Forms.Label();
			this.la_resref = new System.Windows.Forms.Label();
			this.la_resref_ = new System.Windows.Forms.Label();
			this.la_itype = new System.Windows.Forms.Label();
			this.la_itype_ = new System.Windows.Forms.Label();
			this.la_name = new System.Windows.Forms.Label();
			this.la_name_ = new System.Windows.Forms.Label();
			this.la_type = new System.Windows.Forms.Label();
			this.la_type_ = new System.Windows.Forms.Label();
			this.la_repotype = new System.Windows.Forms.Label();
			this.la_repotype_ = new System.Windows.Forms.Label();
			this.ss_camera = new System.Windows.Forms.StatusStrip();
			this.tssl_camera_label = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_camera_xpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_camera_ypos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_camera_zpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_camera_rot = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_camera_dist = new System.Windows.Forms.ToolStripStatusLabel();
			this.ss_model = new System.Windows.Forms.StatusStrip();
			this.tssl_model_label = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_model_xpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_model_ypos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_model_zpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_model_rot = new System.Windows.Forms.ToolStripStatusLabel();
			this.ss_light = new System.Windows.Forms.StatusStrip();
			this.tssl_light_label = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_light_xpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_light_ypos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_light_zpos = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_light_blank = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssl_light_intensity = new System.Windows.Forms.ToolStripStatusLabel();
			this.pa_gui = new System.Windows.Forms.Panel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.pa_con.SuspendLayout();
			this.tc1.SuspendLayout();
			this.tp_controls.SuspendLayout();
			this.gb_Light.SuspendLayout();
			this.gb_model.SuspendLayout();
			this.gb_camera.SuspendLayout();
			this.tp_creature.SuspendLayout();
			this.tp_resource.SuspendLayout();
			this.ss_camera.SuspendLayout();
			this.ss_model.SuspendLayout();
			this.ss_light.SuspendLayout();
			this.SuspendLayout();
			// 
			// pa_con
			// 
			this.pa_con.Controls.Add(this.tc1);
			this.pa_con.Controls.Add(this.ss_camera);
			this.pa_con.Controls.Add(this.ss_model);
			this.pa_con.Controls.Add(this.ss_light);
			this.pa_con.Dock = System.Windows.Forms.DockStyle.Right;
			this.pa_con.Location = new System.Drawing.Point(327, 0);
			this.pa_con.Margin = new System.Windows.Forms.Padding(0);
			this.pa_con.Name = "pa_con";
			this.pa_con.Size = new System.Drawing.Size(285, 478);
			this.pa_con.TabIndex = 1;
			this.pa_con.Visible = false;
			// 
			// tc1
			// 
			this.tc1.Controls.Add(this.tp_controls);
			this.tc1.Controls.Add(this.tp_creature);
			this.tc1.Controls.Add(this.tp_resource);
			this.tc1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tc1.ItemSize = new System.Drawing.Size(80, 15);
			this.tc1.Location = new System.Drawing.Point(0, 0);
			this.tc1.Margin = new System.Windows.Forms.Padding(0);
			this.tc1.Name = "tc1";
			this.tc1.Padding = new System.Drawing.Point(0, 0);
			this.tc1.SelectedIndex = 0;
			this.tc1.Size = new System.Drawing.Size(285, 412);
			this.tc1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tc1.TabIndex = 6;
			this.tc1.SelectedIndexChanged += new System.EventHandler(this.selectedindexchanged_TabControl);
			// 
			// tp_controls
			// 
			this.tp_controls.Controls.Add(this.gb_Light);
			this.tp_controls.Controls.Add(this.gb_model);
			this.tp_controls.Controls.Add(this.gb_camera);
			this.tp_controls.Location = new System.Drawing.Point(4, 19);
			this.tp_controls.Margin = new System.Windows.Forms.Padding(0);
			this.tp_controls.Name = "tp_controls";
			this.tp_controls.Size = new System.Drawing.Size(277, 389);
			this.tp_controls.TabIndex = 0;
			this.tp_controls.Text = "Controls";
			this.tp_controls.UseVisualStyleBackColor = true;
			// 
			// gb_Light
			// 
			this.gb_Light.Controls.Add(this.cb_light_ambient);
			this.gb_Light.Controls.Add(this.cb_light_specular);
			this.gb_Light.Controls.Add(this.cb_light_diffuse);
			this.gb_Light.Controls.Add(this.pa_light_ambient);
			this.gb_Light.Controls.Add(this.la_light_color);
			this.gb_Light.Controls.Add(this.pa_light_specular);
			this.gb_Light.Controls.Add(this.pa_light_diffuse);
			this.gb_Light.Controls.Add(this.la_light_intensity);
			this.gb_Light.Controls.Add(this.tb_light_intensity);
			this.gb_Light.Controls.Add(this.la_dz);
			this.gb_Light.Controls.Add(this.bu_light_xyreset);
			this.gb_Light.Controls.Add(this.la_dy);
			this.gb_Light.Controls.Add(this.la_dx);
			this.gb_Light.Controls.Add(this.bu_light_zreset);
			this.gb_Light.Controls.Add(this.bu_light_reset);
			this.gb_Light.Controls.Add(this.la_light_xyaxis);
			this.gb_Light.Controls.Add(this.la_light_zaxis);
			this.gb_Light.Controls.Add(this.bu_light_ypos);
			this.gb_Light.Controls.Add(this.bu_light_yneg);
			this.gb_Light.Controls.Add(this.bu_light_xneg);
			this.gb_Light.Controls.Add(this.bu_light_xpos);
			this.gb_Light.Controls.Add(this.bu_light_zneg);
			this.gb_Light.Controls.Add(this.bu_light_zpos);
			this.gb_Light.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_Light.Location = new System.Drawing.Point(0, 259);
			this.gb_Light.Margin = new System.Windows.Forms.Padding(0);
			this.gb_Light.Name = "gb_Light";
			this.gb_Light.Padding = new System.Windows.Forms.Padding(0);
			this.gb_Light.Size = new System.Drawing.Size(277, 131);
			this.gb_Light.TabIndex = 2;
			this.gb_Light.TabStop = false;
			this.gb_Light.Text = " Light ";
			// 
			// cb_light_ambient
			// 
			this.cb_light_ambient.Appearance = System.Windows.Forms.Appearance.Button;
			this.cb_light_ambient.Enabled = false;
			this.cb_light_ambient.Location = new System.Drawing.Point(125, 80);
			this.cb_light_ambient.Margin = new System.Windows.Forms.Padding(0);
			this.cb_light_ambient.Name = "cb_light_ambient";
			this.cb_light_ambient.Size = new System.Drawing.Size(65, 22);
			this.cb_light_ambient.TabIndex = 16;
			this.cb_light_ambient.Text = "AMBIENT";
			this.cb_light_ambient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cb_light_ambient.UseVisualStyleBackColor = true;
			this.cb_light_ambient.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_cb_light_ambient);
			// 
			// cb_light_specular
			// 
			this.cb_light_specular.Appearance = System.Windows.Forms.Appearance.Button;
			this.cb_light_specular.Enabled = false;
			this.cb_light_specular.Location = new System.Drawing.Point(125, 55);
			this.cb_light_specular.Margin = new System.Windows.Forms.Padding(0);
			this.cb_light_specular.Name = "cb_light_specular";
			this.cb_light_specular.Size = new System.Drawing.Size(65, 22);
			this.cb_light_specular.TabIndex = 14;
			this.cb_light_specular.Text = "SPECULAR";
			this.cb_light_specular.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cb_light_specular.UseVisualStyleBackColor = true;
			this.cb_light_specular.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_cb_light_specular);
			// 
			// cb_light_diffuse
			// 
			this.cb_light_diffuse.Appearance = System.Windows.Forms.Appearance.Button;
			this.cb_light_diffuse.Enabled = false;
			this.cb_light_diffuse.Location = new System.Drawing.Point(125, 30);
			this.cb_light_diffuse.Margin = new System.Windows.Forms.Padding(0);
			this.cb_light_diffuse.Name = "cb_light_diffuse";
			this.cb_light_diffuse.Size = new System.Drawing.Size(65, 22);
			this.cb_light_diffuse.TabIndex = 12;
			this.cb_light_diffuse.Text = "DIFFUSE";
			this.cb_light_diffuse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cb_light_diffuse.UseVisualStyleBackColor = true;
			this.cb_light_diffuse.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_cb_light_diffuse);
			// 
			// pa_light_ambient
			// 
			this.pa_light_ambient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_light_ambient.Location = new System.Drawing.Point(195, 81);
			this.pa_light_ambient.Margin = new System.Windows.Forms.Padding(0);
			this.pa_light_ambient.Name = "pa_light_ambient";
			this.pa_light_ambient.Size = new System.Drawing.Size(30, 20);
			this.pa_light_ambient.TabIndex = 17;
			this.pa_light_ambient.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_pa_light_ambient);
			// 
			// la_light_color
			// 
			this.la_light_color.Location = new System.Drawing.Point(125, 15);
			this.la_light_color.Margin = new System.Windows.Forms.Padding(0);
			this.la_light_color.Name = "la_light_color";
			this.la_light_color.Size = new System.Drawing.Size(65, 15);
			this.la_light_color.TabIndex = 11;
			this.la_light_color.Text = "Colors";
			// 
			// pa_light_specular
			// 
			this.pa_light_specular.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_light_specular.Location = new System.Drawing.Point(195, 56);
			this.pa_light_specular.Margin = new System.Windows.Forms.Padding(0);
			this.pa_light_specular.Name = "pa_light_specular";
			this.pa_light_specular.Size = new System.Drawing.Size(30, 20);
			this.pa_light_specular.TabIndex = 15;
			this.pa_light_specular.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_pa_light_specular);
			// 
			// pa_light_diffuse
			// 
			this.pa_light_diffuse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pa_light_diffuse.Location = new System.Drawing.Point(195, 31);
			this.pa_light_diffuse.Margin = new System.Windows.Forms.Padding(0);
			this.pa_light_diffuse.Name = "pa_light_diffuse";
			this.pa_light_diffuse.Size = new System.Drawing.Size(30, 20);
			this.pa_light_diffuse.TabIndex = 13;
			this.pa_light_diffuse.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_pa_light_diffuse);
			// 
			// la_light_intensity
			// 
			this.la_light_intensity.Location = new System.Drawing.Point(125, 105);
			this.la_light_intensity.Margin = new System.Windows.Forms.Padding(0);
			this.la_light_intensity.Name = "la_light_intensity";
			this.la_light_intensity.Size = new System.Drawing.Size(65, 20);
			this.la_light_intensity.TabIndex = 18;
			this.la_light_intensity.Text = "intensity";
			this.la_light_intensity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tb_light_intensity
			// 
			this.tb_light_intensity.BackColor = System.Drawing.Color.White;
			this.tb_light_intensity.Location = new System.Drawing.Point(195, 105);
			this.tb_light_intensity.Margin = new System.Windows.Forms.Padding(0);
			this.tb_light_intensity.Name = "tb_light_intensity";
			this.tb_light_intensity.Size = new System.Drawing.Size(40, 20);
			this.tb_light_intensity.TabIndex = 19;
			this.tb_light_intensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_light_intensity.TextChanged += new System.EventHandler(this.textchanged_tb_light_intensity);
			this.tb_light_intensity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keydown_tb_light_intensity);
			// 
			// la_dz
			// 
			this.la_dz.Location = new System.Drawing.Point(230, 55);
			this.la_dz.Margin = new System.Windows.Forms.Padding(0);
			this.la_dz.Name = "la_dz";
			this.la_dz.Size = new System.Drawing.Size(40, 20);
			this.la_dz.TabIndex = 22;
			this.la_dz.Text = "dz";
			this.la_dz.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.la_dz.Visible = false;
			// 
			// bu_light_xyreset
			// 
			this.bu_light_xyreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_light_xyreset.Location = new System.Drawing.Point(60, 80);
			this.bu_light_xyreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_xyreset.Name = "bu_light_xyreset";
			this.bu_light_xyreset.Size = new System.Drawing.Size(22, 22);
			this.bu_light_xyreset.TabIndex = 9;
			this.bu_light_xyreset.Text = "r";
			this.bu_light_xyreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_xyreset.UseVisualStyleBackColor = true;
			this.bu_light_xyreset.Click += new System.EventHandler(this.click_bu_light_xyreset);
			// 
			// la_dy
			// 
			this.la_dy.Location = new System.Drawing.Point(230, 35);
			this.la_dy.Margin = new System.Windows.Forms.Padding(0);
			this.la_dy.Name = "la_dy";
			this.la_dy.Size = new System.Drawing.Size(40, 20);
			this.la_dy.TabIndex = 21;
			this.la_dy.Text = "dy";
			this.la_dy.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.la_dy.Visible = false;
			// 
			// la_dx
			// 
			this.la_dx.Location = new System.Drawing.Point(230, 15);
			this.la_dx.Margin = new System.Windows.Forms.Padding(0);
			this.la_dx.Name = "la_dx";
			this.la_dx.Size = new System.Drawing.Size(40, 20);
			this.la_dx.TabIndex = 20;
			this.la_dx.Text = "dx";
			this.la_dx.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.la_dx.Visible = false;
			// 
			// bu_light_zreset
			// 
			this.bu_light_zreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_light_zreset.Location = new System.Drawing.Point(5, 80);
			this.bu_light_zreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_zreset.Name = "bu_light_zreset";
			this.bu_light_zreset.Size = new System.Drawing.Size(22, 22);
			this.bu_light_zreset.TabIndex = 3;
			this.bu_light_zreset.Text = "r";
			this.bu_light_zreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_zreset.UseVisualStyleBackColor = true;
			this.bu_light_zreset.Click += new System.EventHandler(this.click_bu_light_zreset);
			// 
			// bu_light_reset
			// 
			this.bu_light_reset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_light_reset.Location = new System.Drawing.Point(5, 105);
			this.bu_light_reset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_reset.Name = "bu_light_reset";
			this.bu_light_reset.Size = new System.Drawing.Size(102, 20);
			this.bu_light_reset.TabIndex = 10;
			this.bu_light_reset.Text = "reset";
			this.bu_light_reset.UseVisualStyleBackColor = true;
			this.bu_light_reset.Click += new System.EventHandler(this.click_bu_light_reset);
			// 
			// la_light_xyaxis
			// 
			this.la_light_xyaxis.Location = new System.Drawing.Point(35, 15);
			this.la_light_xyaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_light_xyaxis.Name = "la_light_xyaxis";
			this.la_light_xyaxis.Size = new System.Drawing.Size(75, 15);
			this.la_light_xyaxis.TabIndex = 4;
			this.la_light_xyaxis.Text = "x/y";
			this.la_light_xyaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_light_zaxis
			// 
			this.la_light_zaxis.Location = new System.Drawing.Point(5, 15);
			this.la_light_zaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_light_zaxis.Name = "la_light_zaxis";
			this.la_light_zaxis.Size = new System.Drawing.Size(25, 15);
			this.la_light_zaxis.TabIndex = 0;
			this.la_light_zaxis.Text = "z";
			this.la_light_zaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// bu_light_ypos
			// 
			this.bu_light_ypos.Location = new System.Drawing.Point(85, 40);
			this.bu_light_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_ypos.Name = "bu_light_ypos";
			this.bu_light_ypos.Size = new System.Drawing.Size(22, 22);
			this.bu_light_ypos.TabIndex = 8;
			this.bu_light_ypos.Text = "+";
			this.bu_light_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_ypos.UseVisualStyleBackColor = true;
			this.bu_light_ypos.Click += new System.EventHandler(this.click_bu_light_xpos);
			this.bu_light_ypos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_light_ypos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_light_yneg
			// 
			this.bu_light_yneg.Location = new System.Drawing.Point(35, 40);
			this.bu_light_yneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_yneg.Name = "bu_light_yneg";
			this.bu_light_yneg.Size = new System.Drawing.Size(22, 22);
			this.bu_light_yneg.TabIndex = 7;
			this.bu_light_yneg.Text = "-";
			this.bu_light_yneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_yneg.UseVisualStyleBackColor = true;
			this.bu_light_yneg.Click += new System.EventHandler(this.click_bu_light_xneg);
			this.bu_light_yneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_light_yneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_light_xneg
			// 
			this.bu_light_xneg.Location = new System.Drawing.Point(60, 55);
			this.bu_light_xneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_xneg.Name = "bu_light_xneg";
			this.bu_light_xneg.Size = new System.Drawing.Size(22, 22);
			this.bu_light_xneg.TabIndex = 6;
			this.bu_light_xneg.Text = "-";
			this.bu_light_xneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_xneg.UseVisualStyleBackColor = true;
			this.bu_light_xneg.Click += new System.EventHandler(this.click_bu_light_yneg);
			this.bu_light_xneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_light_xneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_light_xpos
			// 
			this.bu_light_xpos.Location = new System.Drawing.Point(60, 30);
			this.bu_light_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_xpos.Name = "bu_light_xpos";
			this.bu_light_xpos.Size = new System.Drawing.Size(22, 22);
			this.bu_light_xpos.TabIndex = 5;
			this.bu_light_xpos.Text = "+";
			this.bu_light_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_xpos.UseVisualStyleBackColor = true;
			this.bu_light_xpos.Click += new System.EventHandler(this.click_bu_light_ypos);
			this.bu_light_xpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_light_xpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_light_zneg
			// 
			this.bu_light_zneg.Location = new System.Drawing.Point(5, 55);
			this.bu_light_zneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_zneg.Name = "bu_light_zneg";
			this.bu_light_zneg.Size = new System.Drawing.Size(22, 22);
			this.bu_light_zneg.TabIndex = 2;
			this.bu_light_zneg.Text = "-";
			this.bu_light_zneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_zneg.UseVisualStyleBackColor = true;
			this.bu_light_zneg.Click += new System.EventHandler(this.click_bu_light_zneg);
			this.bu_light_zneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_light_zneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_light_zpos
			// 
			this.bu_light_zpos.Location = new System.Drawing.Point(5, 30);
			this.bu_light_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_light_zpos.Name = "bu_light_zpos";
			this.bu_light_zpos.Size = new System.Drawing.Size(22, 22);
			this.bu_light_zpos.TabIndex = 1;
			this.bu_light_zpos.Text = "+";
			this.bu_light_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_light_zpos.UseVisualStyleBackColor = true;
			this.bu_light_zpos.Click += new System.EventHandler(this.click_bu_light_zpos);
			this.bu_light_zpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_light_zpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
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
			this.gb_model.Controls.Add(this.bu_model_rotneg);
			this.gb_model.Controls.Add(this.bu_model_rotpos);
			this.gb_model.Controls.Add(this.bu_model_ypos);
			this.gb_model.Controls.Add(this.bu_model_yneg);
			this.gb_model.Controls.Add(this.bu_model_xneg);
			this.gb_model.Controls.Add(this.bu_model_xpos);
			this.gb_model.Controls.Add(this.bu_model_zneg);
			this.gb_model.Controls.Add(this.bu_model_zpos);
			this.gb_model.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_model.Location = new System.Drawing.Point(0, 130);
			this.gb_model.Margin = new System.Windows.Forms.Padding(0);
			this.gb_model.Name = "gb_model";
			this.gb_model.Padding = new System.Windows.Forms.Padding(0);
			this.gb_model.Size = new System.Drawing.Size(277, 129);
			this.gb_model.TabIndex = 1;
			this.gb_model.TabStop = false;
			this.gb_model.Text = " Model ";
			// 
			// la_model_scaleorg
			// 
			this.la_model_scaleorg.ForeColor = System.Drawing.Color.RoyalBlue;
			this.la_model_scaleorg.Location = new System.Drawing.Point(205, 15);
			this.la_model_scaleorg.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_scaleorg.Name = "la_model_scaleorg";
			this.la_model_scaleorg.Size = new System.Drawing.Size(40, 15);
			this.la_model_scaleorg.TabIndex = 15;
			this.la_model_scaleorg.Text = "0";
			this.la_model_scaleorg.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// bu_model_scaleneg
			// 
			this.bu_model_scaleneg.Location = new System.Drawing.Point(250, 55);
			this.bu_model_scaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_scaleneg.Name = "bu_model_scaleneg";
			this.bu_model_scaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_scaleneg.TabIndex = 26;
			this.bu_model_scaleneg.Text = "-";
			this.bu_model_scaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_scaleneg.UseVisualStyleBackColor = true;
			this.bu_model_scaleneg.Click += new System.EventHandler(this.click_bu_model_scaleall);
			this.bu_model_scaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_scaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_scalepos
			// 
			this.bu_model_scalepos.Location = new System.Drawing.Point(250, 30);
			this.bu_model_scalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_scalepos.Name = "bu_model_scalepos";
			this.bu_model_scalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_scalepos.TabIndex = 25;
			this.bu_model_scalepos.Text = "+";
			this.bu_model_scalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_scalepos.UseVisualStyleBackColor = true;
			this.bu_model_scalepos.Click += new System.EventHandler(this.click_bu_model_scaleall);
			this.bu_model_scalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_scalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// la_model_zscale
			// 
			this.la_model_zscale.Location = new System.Drawing.Point(205, 80);
			this.la_model_zscale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_zscale.Name = "la_model_zscale";
			this.la_model_zscale.Size = new System.Drawing.Size(40, 20);
			this.la_model_zscale.TabIndex = 24;
			this.la_model_zscale.Text = "z";
			this.la_model_zscale.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// la_model_yscale
			// 
			this.la_model_yscale.Location = new System.Drawing.Point(205, 55);
			this.la_model_yscale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_yscale.Name = "la_model_yscale";
			this.la_model_yscale.Size = new System.Drawing.Size(40, 20);
			this.la_model_yscale.TabIndex = 21;
			this.la_model_yscale.Text = "y";
			this.la_model_yscale.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// la_model_xscale
			// 
			this.la_model_xscale.Location = new System.Drawing.Point(205, 30);
			this.la_model_xscale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_xscale.Name = "la_model_xscale";
			this.la_model_xscale.Size = new System.Drawing.Size(40, 20);
			this.la_model_xscale.TabIndex = 18;
			this.la_model_xscale.Text = "x";
			this.la_model_xscale.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// bu_model_scalereset
			// 
			this.bu_model_scalereset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_model_scalereset.Location = new System.Drawing.Point(250, 80);
			this.bu_model_scalereset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_scalereset.Name = "bu_model_scalereset";
			this.bu_model_scalereset.Size = new System.Drawing.Size(22, 22);
			this.bu_model_scalereset.TabIndex = 27;
			this.bu_model_scalereset.Text = "r";
			this.bu_model_scalereset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_scalereset.UseVisualStyleBackColor = true;
			this.bu_model_scalereset.Click += new System.EventHandler(this.click_bu_model_scalereset);
			// 
			// bu_model_rotreset
			// 
			this.bu_model_rotreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_model_rotreset.Location = new System.Drawing.Point(115, 80);
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
			this.bu_model_xyreset.Location = new System.Drawing.Point(60, 80);
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
			this.bu_model_zreset.Location = new System.Drawing.Point(5, 80);
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
			this.bu_model_reset.Location = new System.Drawing.Point(5, 105);
			this.bu_model_reset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_reset.Name = "bu_model_reset";
			this.bu_model_reset.Size = new System.Drawing.Size(267, 20);
			this.bu_model_reset.TabIndex = 28;
			this.bu_model_reset.Text = "reset";
			this.bu_model_reset.UseVisualStyleBackColor = true;
			this.bu_model_reset.Click += new System.EventHandler(this.click_bu_model_reset);
			// 
			// la_model_scale
			// 
			this.la_model_scale.Location = new System.Drawing.Point(155, 15);
			this.la_model_scale.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_scale.Name = "la_model_scale";
			this.la_model_scale.Size = new System.Drawing.Size(50, 15);
			this.la_model_scale.TabIndex = 14;
			this.la_model_scale.Text = "scale";
			// 
			// la_model_rotate
			// 
			this.la_model_rotate.Location = new System.Drawing.Point(115, 15);
			this.la_model_rotate.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_rotate.Name = "la_model_rotate";
			this.la_model_rotate.Size = new System.Drawing.Size(25, 15);
			this.la_model_rotate.TabIndex = 10;
			this.la_model_rotate.Text = "rot";
			this.la_model_rotate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_model_xyaxis
			// 
			this.la_model_xyaxis.Location = new System.Drawing.Point(35, 15);
			this.la_model_xyaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_xyaxis.Name = "la_model_xyaxis";
			this.la_model_xyaxis.Size = new System.Drawing.Size(75, 15);
			this.la_model_xyaxis.TabIndex = 4;
			this.la_model_xyaxis.Text = "x/y";
			this.la_model_xyaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_model_zaxis
			// 
			this.la_model_zaxis.Location = new System.Drawing.Point(5, 15);
			this.la_model_zaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_model_zaxis.Name = "la_model_zaxis";
			this.la_model_zaxis.Size = new System.Drawing.Size(25, 15);
			this.la_model_zaxis.TabIndex = 0;
			this.la_model_zaxis.Text = "z";
			this.la_model_zaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// bu_model_zscaleneg
			// 
			this.bu_model_zscaleneg.Location = new System.Drawing.Point(155, 80);
			this.bu_model_zscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zscaleneg.Name = "bu_model_zscaleneg";
			this.bu_model_zscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zscaleneg.TabIndex = 22;
			this.bu_model_zscaleneg.Text = "-";
			this.bu_model_zscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_zscaleneg.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_zscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yscaleneg
			// 
			this.bu_model_yscaleneg.Location = new System.Drawing.Point(155, 55);
			this.bu_model_yscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yscaleneg.Name = "bu_model_yscaleneg";
			this.bu_model_yscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yscaleneg.TabIndex = 19;
			this.bu_model_yscaleneg.Text = "-";
			this.bu_model_yscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_yscaleneg.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_yscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xscaleneg
			// 
			this.bu_model_xscaleneg.Location = new System.Drawing.Point(155, 30);
			this.bu_model_xscaleneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xscaleneg.Name = "bu_model_xscaleneg";
			this.bu_model_xscaleneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xscaleneg.TabIndex = 16;
			this.bu_model_xscaleneg.Text = "-";
			this.bu_model_xscaleneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xscaleneg.UseVisualStyleBackColor = true;
			this.bu_model_xscaleneg.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_xscaleneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xscaleneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_zscalepos
			// 
			this.bu_model_zscalepos.Location = new System.Drawing.Point(180, 80);
			this.bu_model_zscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zscalepos.Name = "bu_model_zscalepos";
			this.bu_model_zscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zscalepos.TabIndex = 23;
			this.bu_model_zscalepos.Text = "+";
			this.bu_model_zscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zscalepos.UseVisualStyleBackColor = true;
			this.bu_model_zscalepos.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_zscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yscalepos
			// 
			this.bu_model_yscalepos.Location = new System.Drawing.Point(180, 55);
			this.bu_model_yscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yscalepos.Name = "bu_model_yscalepos";
			this.bu_model_yscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yscalepos.TabIndex = 20;
			this.bu_model_yscalepos.Text = "+";
			this.bu_model_yscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yscalepos.UseVisualStyleBackColor = true;
			this.bu_model_yscalepos.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_yscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xscalepos
			// 
			this.bu_model_xscalepos.Location = new System.Drawing.Point(180, 30);
			this.bu_model_xscalepos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xscalepos.Name = "bu_model_xscalepos";
			this.bu_model_xscalepos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xscalepos.TabIndex = 17;
			this.bu_model_xscalepos.Text = "+";
			this.bu_model_xscalepos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xscalepos.UseVisualStyleBackColor = true;
			this.bu_model_xscalepos.Click += new System.EventHandler(this.click_bu_model_scale);
			this.bu_model_xscalepos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xscalepos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_rotneg
			// 
			this.bu_model_rotneg.Location = new System.Drawing.Point(115, 55);
			this.bu_model_rotneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_rotneg.Name = "bu_model_rotneg";
			this.bu_model_rotneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_rotneg.TabIndex = 12;
			this.bu_model_rotneg.Text = "-";
			this.bu_model_rotneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_rotneg.UseVisualStyleBackColor = true;
			this.bu_model_rotneg.Click += new System.EventHandler(this.click_bu_model_rotneg);
			this.bu_model_rotneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_rotneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_rotpos
			// 
			this.bu_model_rotpos.Location = new System.Drawing.Point(115, 30);
			this.bu_model_rotpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_rotpos.Name = "bu_model_rotpos";
			this.bu_model_rotpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_rotpos.TabIndex = 11;
			this.bu_model_rotpos.Text = "+";
			this.bu_model_rotpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_rotpos.UseVisualStyleBackColor = true;
			this.bu_model_rotpos.Click += new System.EventHandler(this.click_bu_model_rotpos);
			this.bu_model_rotpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_rotpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_ypos
			// 
			this.bu_model_ypos.Location = new System.Drawing.Point(85, 40);
			this.bu_model_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_ypos.Name = "bu_model_ypos";
			this.bu_model_ypos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_ypos.TabIndex = 8;
			this.bu_model_ypos.Text = "+";
			this.bu_model_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_ypos.UseVisualStyleBackColor = true;
			this.bu_model_ypos.Click += new System.EventHandler(this.click_bu_model_xpos);
			this.bu_model_ypos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_ypos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_yneg
			// 
			this.bu_model_yneg.Location = new System.Drawing.Point(35, 40);
			this.bu_model_yneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_yneg.Name = "bu_model_yneg";
			this.bu_model_yneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_yneg.TabIndex = 7;
			this.bu_model_yneg.Text = "-";
			this.bu_model_yneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_yneg.UseVisualStyleBackColor = true;
			this.bu_model_yneg.Click += new System.EventHandler(this.click_bu_model_xneg);
			this.bu_model_yneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_yneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xneg
			// 
			this.bu_model_xneg.Location = new System.Drawing.Point(60, 55);
			this.bu_model_xneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xneg.Name = "bu_model_xneg";
			this.bu_model_xneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xneg.TabIndex = 6;
			this.bu_model_xneg.Text = "-";
			this.bu_model_xneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xneg.UseVisualStyleBackColor = true;
			this.bu_model_xneg.Click += new System.EventHandler(this.click_bu_model_yneg);
			this.bu_model_xneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_xpos
			// 
			this.bu_model_xpos.Location = new System.Drawing.Point(60, 30);
			this.bu_model_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_xpos.Name = "bu_model_xpos";
			this.bu_model_xpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_xpos.TabIndex = 5;
			this.bu_model_xpos.Text = "+";
			this.bu_model_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_xpos.UseVisualStyleBackColor = true;
			this.bu_model_xpos.Click += new System.EventHandler(this.click_bu_model_ypos);
			this.bu_model_xpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_xpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_zneg
			// 
			this.bu_model_zneg.Location = new System.Drawing.Point(5, 55);
			this.bu_model_zneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zneg.Name = "bu_model_zneg";
			this.bu_model_zneg.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zneg.TabIndex = 2;
			this.bu_model_zneg.Text = "-";
			this.bu_model_zneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zneg.UseVisualStyleBackColor = true;
			this.bu_model_zneg.Click += new System.EventHandler(this.click_bu_model_zneg);
			this.bu_model_zneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_model_zpos
			// 
			this.bu_model_zpos.Location = new System.Drawing.Point(5, 30);
			this.bu_model_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_model_zpos.Name = "bu_model_zpos";
			this.bu_model_zpos.Size = new System.Drawing.Size(22, 22);
			this.bu_model_zpos.TabIndex = 1;
			this.bu_model_zpos.Text = "+";
			this.bu_model_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_model_zpos.UseVisualStyleBackColor = true;
			this.bu_model_zpos.Click += new System.EventHandler(this.click_bu_model_zpos);
			this.bu_model_zpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_model_zpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// gb_camera
			// 
			this.gb_camera.Controls.Add(this.la_camera_baseheight);
			this.gb_camera.Controls.Add(this.tb_camera_baseheight);
			this.gb_camera.Controls.Add(this.bu_camera_yawpos);
			this.gb_camera.Controls.Add(this.la_camera_yaw);
			this.gb_camera.Controls.Add(this.la_camera_pitch);
			this.gb_camera.Controls.Add(this.bu_camera_resetpolar);
			this.gb_camera.Controls.Add(this.la_camera_angle);
			this.gb_camera.Controls.Add(this.bu_camera_yawneg);
			this.gb_camera.Controls.Add(this.bu_camera_pitchneg);
			this.gb_camera.Controls.Add(this.bu_camera_pitchpos);
			this.gb_camera.Controls.Add(this.bu_camera_xyreset);
			this.gb_camera.Controls.Add(this.bu_camera_zreset);
			this.gb_camera.Controls.Add(this.bu_camera_focusobject);
			this.gb_camera.Controls.Add(this.la_camera_dist);
			this.gb_camera.Controls.Add(this.la_camera_xyaxis);
			this.gb_camera.Controls.Add(this.la_camera_zaxis);
			this.gb_camera.Controls.Add(this.bu_camera_distreset);
			this.gb_camera.Controls.Add(this.bu_camera_distneg);
			this.gb_camera.Controls.Add(this.bu_camera_distpos);
			this.gb_camera.Controls.Add(this.bu_camera_ypos);
			this.gb_camera.Controls.Add(this.bu_camera_yneg);
			this.gb_camera.Controls.Add(this.bu_camera_xneg);
			this.gb_camera.Controls.Add(this.bu_camera_xpos);
			this.gb_camera.Controls.Add(this.bu_camera_zneg);
			this.gb_camera.Controls.Add(this.bu_camera_zpos);
			this.gb_camera.Dock = System.Windows.Forms.DockStyle.Top;
			this.gb_camera.Location = new System.Drawing.Point(0, 0);
			this.gb_camera.Margin = new System.Windows.Forms.Padding(0);
			this.gb_camera.Name = "gb_camera";
			this.gb_camera.Padding = new System.Windows.Forms.Padding(0);
			this.gb_camera.Size = new System.Drawing.Size(277, 130);
			this.gb_camera.TabIndex = 0;
			this.gb_camera.TabStop = false;
			this.gb_camera.Text = " Camera ";
			// 
			// la_camera_baseheight
			// 
			this.la_camera_baseheight.Location = new System.Drawing.Point(225, 65);
			this.la_camera_baseheight.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_baseheight.Name = "la_camera_baseheight";
			this.la_camera_baseheight.Size = new System.Drawing.Size(45, 15);
			this.la_camera_baseheight.TabIndex = 22;
			this.la_camera_baseheight.Text = "base h";
			this.la_camera_baseheight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tb_camera_baseheight
			// 
			this.tb_camera_baseheight.BackColor = System.Drawing.Color.White;
			this.tb_camera_baseheight.Location = new System.Drawing.Point(227, 80);
			this.tb_camera_baseheight.Margin = new System.Windows.Forms.Padding(0);
			this.tb_camera_baseheight.Name = "tb_camera_baseheight";
			this.tb_camera_baseheight.Size = new System.Drawing.Size(40, 20);
			this.tb_camera_baseheight.TabIndex = 23;
			this.tb_camera_baseheight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tb_camera_baseheight.TextChanged += new System.EventHandler(this.textchanged_tb_camera_baseheight);
			this.tb_camera_baseheight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keydown_tb_camera_baseheight);
			// 
			// bu_camera_yawpos
			// 
			this.bu_camera_yawpos.Location = new System.Drawing.Point(200, 40);
			this.bu_camera_yawpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_yawpos.Name = "bu_camera_yawpos";
			this.bu_camera_yawpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_yawpos.TabIndex = 18;
			this.bu_camera_yawpos.Text = "-";
			this.bu_camera_yawpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_yawpos.UseVisualStyleBackColor = true;
			this.bu_camera_yawpos.Click += new System.EventHandler(this.click_bu_camera_rotneg);
			this.bu_camera_yawpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_yawpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// la_camera_yaw
			// 
			this.la_camera_yaw.Location = new System.Drawing.Point(230, 45);
			this.la_camera_yaw.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_yaw.Name = "la_camera_yaw";
			this.la_camera_yaw.Size = new System.Drawing.Size(40, 15);
			this.la_camera_yaw.TabIndex = 21;
			this.la_camera_yaw.Text = "yaw";
			this.la_camera_yaw.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// la_camera_pitch
			// 
			this.la_camera_pitch.Location = new System.Drawing.Point(230, 30);
			this.la_camera_pitch.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_pitch.Name = "la_camera_pitch";
			this.la_camera_pitch.Size = new System.Drawing.Size(40, 15);
			this.la_camera_pitch.TabIndex = 20;
			this.la_camera_pitch.Text = "pitch";
			this.la_camera_pitch.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// bu_camera_resetpolar
			// 
			this.bu_camera_resetpolar.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_resetpolar.Location = new System.Drawing.Point(175, 80);
			this.bu_camera_resetpolar.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_resetpolar.Name = "bu_camera_resetpolar";
			this.bu_camera_resetpolar.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_resetpolar.TabIndex = 19;
			this.bu_camera_resetpolar.Text = "f";
			this.bu_camera_resetpolar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_resetpolar.UseVisualStyleBackColor = true;
			this.bu_camera_resetpolar.Click += new System.EventHandler(this.click_bu_camera_resetpolar);
			// 
			// la_camera_angle
			// 
			this.la_camera_angle.Location = new System.Drawing.Point(165, 15);
			this.la_camera_angle.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_angle.Name = "la_camera_angle";
			this.la_camera_angle.Size = new System.Drawing.Size(40, 15);
			this.la_camera_angle.TabIndex = 14;
			this.la_camera_angle.Text = "Po";
			this.la_camera_angle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// bu_camera_yawneg
			// 
			this.bu_camera_yawneg.Location = new System.Drawing.Point(150, 40);
			this.bu_camera_yawneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_yawneg.Name = "bu_camera_yawneg";
			this.bu_camera_yawneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_yawneg.TabIndex = 17;
			this.bu_camera_yawneg.Text = "+";
			this.bu_camera_yawneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_yawneg.UseVisualStyleBackColor = true;
			this.bu_camera_yawneg.Click += new System.EventHandler(this.click_bu_camera_rotpos);
			this.bu_camera_yawneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_yawneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_pitchneg
			// 
			this.bu_camera_pitchneg.Location = new System.Drawing.Point(175, 55);
			this.bu_camera_pitchneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_pitchneg.Name = "bu_camera_pitchneg";
			this.bu_camera_pitchneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_pitchneg.TabIndex = 16;
			this.bu_camera_pitchneg.Text = "-";
			this.bu_camera_pitchneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_pitchneg.UseVisualStyleBackColor = true;
			this.bu_camera_pitchneg.Click += new System.EventHandler(this.click_bu_camera_pitchneg);
			this.bu_camera_pitchneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_pitchneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_pitchpos
			// 
			this.bu_camera_pitchpos.Location = new System.Drawing.Point(175, 30);
			this.bu_camera_pitchpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_pitchpos.Name = "bu_camera_pitchpos";
			this.bu_camera_pitchpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_pitchpos.TabIndex = 15;
			this.bu_camera_pitchpos.Text = "+";
			this.bu_camera_pitchpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_pitchpos.UseVisualStyleBackColor = true;
			this.bu_camera_pitchpos.Click += new System.EventHandler(this.click_bu_camera_pitchpos);
			this.bu_camera_pitchpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_pitchpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_xyreset
			// 
			this.bu_camera_xyreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_xyreset.Location = new System.Drawing.Point(60, 80);
			this.bu_camera_xyreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_xyreset.Name = "bu_camera_xyreset";
			this.bu_camera_xyreset.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_xyreset.TabIndex = 9;
			this.bu_camera_xyreset.Text = "f";
			this.bu_camera_xyreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_xyreset.UseVisualStyleBackColor = true;
			this.bu_camera_xyreset.Click += new System.EventHandler(this.click_bu_camera_xyreset);
			// 
			// bu_camera_zreset
			// 
			this.bu_camera_zreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_zreset.Location = new System.Drawing.Point(5, 80);
			this.bu_camera_zreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_zreset.Name = "bu_camera_zreset";
			this.bu_camera_zreset.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_zreset.TabIndex = 3;
			this.bu_camera_zreset.Text = "f";
			this.bu_camera_zreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_zreset.UseVisualStyleBackColor = true;
			this.bu_camera_zreset.Click += new System.EventHandler(this.click_bu_camera_zreset);
			// 
			// bu_camera_focusobject
			// 
			this.bu_camera_focusobject.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_focusobject.Location = new System.Drawing.Point(5, 105);
			this.bu_camera_focusobject.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_focusobject.Name = "bu_camera_focusobject";
			this.bu_camera_focusobject.Size = new System.Drawing.Size(267, 20);
			this.bu_camera_focusobject.TabIndex = 24;
			this.bu_camera_focusobject.Text = "focus";
			this.bu_camera_focusobject.UseVisualStyleBackColor = true;
			this.bu_camera_focusobject.Click += new System.EventHandler(this.click_bu_camera_focus);
			// 
			// la_camera_dist
			// 
			this.la_camera_dist.Location = new System.Drawing.Point(115, 15);
			this.la_camera_dist.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_dist.Name = "la_camera_dist";
			this.la_camera_dist.Size = new System.Drawing.Size(35, 15);
			this.la_camera_dist.TabIndex = 10;
			this.la_camera_dist.Text = "Zo";
			this.la_camera_dist.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_camera_xyaxis
			// 
			this.la_camera_xyaxis.Location = new System.Drawing.Point(35, 15);
			this.la_camera_xyaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_xyaxis.Name = "la_camera_xyaxis";
			this.la_camera_xyaxis.Size = new System.Drawing.Size(75, 15);
			this.la_camera_xyaxis.TabIndex = 4;
			this.la_camera_xyaxis.Text = "x/y";
			this.la_camera_xyaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// la_camera_zaxis
			// 
			this.la_camera_zaxis.Location = new System.Drawing.Point(5, 15);
			this.la_camera_zaxis.Margin = new System.Windows.Forms.Padding(0);
			this.la_camera_zaxis.Name = "la_camera_zaxis";
			this.la_camera_zaxis.Size = new System.Drawing.Size(25, 15);
			this.la_camera_zaxis.TabIndex = 0;
			this.la_camera_zaxis.Text = "z";
			this.la_camera_zaxis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// bu_camera_distreset
			// 
			this.bu_camera_distreset.ForeColor = System.Drawing.Color.Crimson;
			this.bu_camera_distreset.Location = new System.Drawing.Point(119, 80);
			this.bu_camera_distreset.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_distreset.Name = "bu_camera_distreset";
			this.bu_camera_distreset.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_distreset.TabIndex = 13;
			this.bu_camera_distreset.Text = "f";
			this.bu_camera_distreset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_distreset.UseVisualStyleBackColor = true;
			this.bu_camera_distreset.Click += new System.EventHandler(this.click_bu_camera_resetdist);
			// 
			// bu_camera_distneg
			// 
			this.bu_camera_distneg.Location = new System.Drawing.Point(120, 30);
			this.bu_camera_distneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_distneg.Name = "bu_camera_distneg";
			this.bu_camera_distneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_distneg.TabIndex = 11;
			this.bu_camera_distneg.Text = "+";
			this.bu_camera_distneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_distneg.UseVisualStyleBackColor = true;
			this.bu_camera_distneg.Click += new System.EventHandler(this.click_bu_camera_distneg);
			this.bu_camera_distneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_distneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_distpos
			// 
			this.bu_camera_distpos.Location = new System.Drawing.Point(119, 55);
			this.bu_camera_distpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_distpos.Name = "bu_camera_distpos";
			this.bu_camera_distpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_distpos.TabIndex = 12;
			this.bu_camera_distpos.Text = "-";
			this.bu_camera_distpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_distpos.UseVisualStyleBackColor = true;
			this.bu_camera_distpos.Click += new System.EventHandler(this.click_bu_camera_distpos);
			this.bu_camera_distpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_distpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_ypos
			// 
			this.bu_camera_ypos.Location = new System.Drawing.Point(85, 40);
			this.bu_camera_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_ypos.Name = "bu_camera_ypos";
			this.bu_camera_ypos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_ypos.TabIndex = 8;
			this.bu_camera_ypos.Text = "+";
			this.bu_camera_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_ypos.UseVisualStyleBackColor = true;
			this.bu_camera_ypos.Click += new System.EventHandler(this.click_bu_camera_xpos);
			this.bu_camera_ypos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_ypos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_yneg
			// 
			this.bu_camera_yneg.Location = new System.Drawing.Point(35, 40);
			this.bu_camera_yneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_yneg.Name = "bu_camera_yneg";
			this.bu_camera_yneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_yneg.TabIndex = 7;
			this.bu_camera_yneg.Text = "-";
			this.bu_camera_yneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_yneg.UseVisualStyleBackColor = true;
			this.bu_camera_yneg.Click += new System.EventHandler(this.click_bu_camera_xneg);
			this.bu_camera_yneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_yneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_xneg
			// 
			this.bu_camera_xneg.Location = new System.Drawing.Point(60, 55);
			this.bu_camera_xneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_xneg.Name = "bu_camera_xneg";
			this.bu_camera_xneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_xneg.TabIndex = 6;
			this.bu_camera_xneg.Text = "-";
			this.bu_camera_xneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_xneg.UseVisualStyleBackColor = true;
			this.bu_camera_xneg.Click += new System.EventHandler(this.click_bu_camera_yneg);
			this.bu_camera_xneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_xneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_xpos
			// 
			this.bu_camera_xpos.Location = new System.Drawing.Point(60, 30);
			this.bu_camera_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_xpos.Name = "bu_camera_xpos";
			this.bu_camera_xpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_xpos.TabIndex = 5;
			this.bu_camera_xpos.Text = "+";
			this.bu_camera_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_xpos.UseVisualStyleBackColor = true;
			this.bu_camera_xpos.Click += new System.EventHandler(this.click_bu_camera_ypos);
			this.bu_camera_xpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_xpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_zneg
			// 
			this.bu_camera_zneg.Location = new System.Drawing.Point(5, 55);
			this.bu_camera_zneg.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_zneg.Name = "bu_camera_zneg";
			this.bu_camera_zneg.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_zneg.TabIndex = 2;
			this.bu_camera_zneg.Text = "-";
			this.bu_camera_zneg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_zneg.UseVisualStyleBackColor = true;
			this.bu_camera_zneg.Click += new System.EventHandler(this.click_bu_camera_zneg);
			this.bu_camera_zneg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_zneg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// bu_camera_zpos
			// 
			this.bu_camera_zpos.Location = new System.Drawing.Point(5, 30);
			this.bu_camera_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.bu_camera_zpos.Name = "bu_camera_zpos";
			this.bu_camera_zpos.Size = new System.Drawing.Size(22, 22);
			this.bu_camera_zpos.TabIndex = 1;
			this.bu_camera_zpos.Text = "+";
			this.bu_camera_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bu_camera_zpos.UseVisualStyleBackColor = true;
			this.bu_camera_zpos.Click += new System.EventHandler(this.click_bu_camera_zpos);
			this.bu_camera_zpos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mousedown_EnableRepeater);
			this.bu_camera_zpos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseup_DisableRepeater);
			// 
			// tp_creature
			// 
			this.tp_creature.Controls.Add(this.bu_creature_apply);
			this.tp_creature.Controls.Add(this.la_creature_gender);
			this.tp_creature.Controls.Add(this.cbo_creature_gender);
			this.tp_creature.Controls.Add(this.bu_creature_display);
			this.tp_creature.Location = new System.Drawing.Point(4, 19);
			this.tp_creature.Margin = new System.Windows.Forms.Padding(0);
			this.tp_creature.Name = "tp_creature";
			this.tp_creature.Size = new System.Drawing.Size(277, 389);
			this.tp_creature.TabIndex = 1;
			this.tp_creature.Text = "Creature";
			this.tp_creature.UseVisualStyleBackColor = true;
			// 
			// bu_creature_apply
			// 
			this.bu_creature_apply.ForeColor = System.Drawing.Color.Crimson;
			this.bu_creature_apply.Location = new System.Drawing.Point(5, 25);
			this.bu_creature_apply.Margin = new System.Windows.Forms.Padding(0);
			this.bu_creature_apply.Name = "bu_creature_apply";
			this.bu_creature_apply.Size = new System.Drawing.Size(267, 20);
			this.bu_creature_apply.TabIndex = 28;
			this.bu_creature_apply.UseVisualStyleBackColor = true;
			this.bu_creature_apply.Click += new System.EventHandler(this.click_bu_creature_apply);
			// 
			// la_creature_gender
			// 
			this.la_creature_gender.Location = new System.Drawing.Point(115, 50);
			this.la_creature_gender.Margin = new System.Windows.Forms.Padding(0);
			this.la_creature_gender.Name = "la_creature_gender";
			this.la_creature_gender.Size = new System.Drawing.Size(50, 20);
			this.la_creature_gender.TabIndex = 27;
			this.la_creature_gender.Text = "Gender";
			this.la_creature_gender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbo_creature_gender
			// 
			this.cbo_creature_gender.BackColor = System.Drawing.Color.White;
			this.cbo_creature_gender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbo_creature_gender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cbo_creature_gender.FormattingEnabled = true;
			this.cbo_creature_gender.Items.AddRange(new object[] {
			"Male",
			"Female",
			"Both",
			"Other",
			"None"});
			this.cbo_creature_gender.Location = new System.Drawing.Point(10, 50);
			this.cbo_creature_gender.Margin = new System.Windows.Forms.Padding(0);
			this.cbo_creature_gender.Name = "cbo_creature_gender";
			this.cbo_creature_gender.Size = new System.Drawing.Size(100, 21);
			this.cbo_creature_gender.TabIndex = 0;
			// 
			// bu_creature_display
			// 
			this.bu_creature_display.ForeColor = System.Drawing.Color.Crimson;
			this.bu_creature_display.Location = new System.Drawing.Point(5, 5);
			this.bu_creature_display.Margin = new System.Windows.Forms.Padding(0);
			this.bu_creature_display.Name = "bu_creature_display";
			this.bu_creature_display.Size = new System.Drawing.Size(267, 20);
			this.bu_creature_display.TabIndex = 25;
			this.bu_creature_display.Text = "display";
			this.bu_creature_display.UseVisualStyleBackColor = true;
			this.bu_creature_display.Click += new System.EventHandler(this.click_bu_creature_display);
			// 
			// tp_resource
			// 
			this.tp_resource.Controls.Add(this.la_repo_inst);
			this.tp_resource.Controls.Add(this.la_areatag);
			this.tp_resource.Controls.Add(this.la_areatag_);
			this.tp_resource.Controls.Add(this.la_head_instance);
			this.tp_resource.Controls.Add(this.la_repo_inst_);
			this.tp_resource.Controls.Add(this.la_restype_inst);
			this.tp_resource.Controls.Add(this.la_restype_inst_);
			this.tp_resource.Controls.Add(this.la_template_inst);
			this.tp_resource.Controls.Add(this.la_template_inst_);
			this.tp_resource.Controls.Add(this.la_file_inst);
			this.tp_resource.Controls.Add(this.la_file_inst_);
			this.tp_resource.Controls.Add(this.la_head_resource);
			this.tp_resource.Controls.Add(this.la_head_blueprint);
			this.tp_resource.Controls.Add(this.la_head_template);
			this.tp_resource.Controls.Add(this.la_template);
			this.tp_resource.Controls.Add(this.la_template_);
			this.tp_resource.Controls.Add(this.la_resref);
			this.tp_resource.Controls.Add(this.la_resref_);
			this.tp_resource.Controls.Add(this.la_itype);
			this.tp_resource.Controls.Add(this.la_itype_);
			this.tp_resource.Controls.Add(this.la_name);
			this.tp_resource.Controls.Add(this.la_name_);
			this.tp_resource.Controls.Add(this.la_type);
			this.tp_resource.Controls.Add(this.la_type_);
			this.tp_resource.Controls.Add(this.la_repotype);
			this.tp_resource.Controls.Add(this.la_repotype_);
			this.tp_resource.Location = new System.Drawing.Point(4, 19);
			this.tp_resource.Margin = new System.Windows.Forms.Padding(0);
			this.tp_resource.Name = "tp_resource";
			this.tp_resource.Size = new System.Drawing.Size(277, 389);
			this.tp_resource.TabIndex = 2;
			this.tp_resource.Text = "Resource";
			this.tp_resource.UseVisualStyleBackColor = true;
			// 
			// la_repo_inst
			// 
			this.la_repo_inst.Location = new System.Drawing.Point(70, 304);
			this.la_repo_inst.Margin = new System.Windows.Forms.Padding(0);
			this.la_repo_inst.Name = "la_repo_inst";
			this.la_repo_inst.Size = new System.Drawing.Size(200, 40);
			this.la_repo_inst.TabIndex = 79;
			// 
			// la_areatag
			// 
			this.la_areatag.Location = new System.Drawing.Point(70, 195);
			this.la_areatag.Margin = new System.Windows.Forms.Padding(0);
			this.la_areatag.Name = "la_areatag";
			this.la_areatag.Size = new System.Drawing.Size(200, 20);
			this.la_areatag.TabIndex = 82;
			this.la_areatag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_areatag_
			// 
			this.la_areatag_.Location = new System.Drawing.Point(10, 195);
			this.la_areatag_.Margin = new System.Windows.Forms.Padding(0);
			this.la_areatag_.Name = "la_areatag_";
			this.la_areatag_.Size = new System.Drawing.Size(55, 20);
			this.la_areatag_.TabIndex = 81;
			this.la_areatag_.Text = "areatag";
			this.la_areatag_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_head_instance
			// 
			this.la_head_instance.Location = new System.Drawing.Point(5, 175);
			this.la_head_instance.Margin = new System.Windows.Forms.Padding(0);
			this.la_head_instance.Name = "la_head_instance";
			this.la_head_instance.Size = new System.Drawing.Size(100, 20);
			this.la_head_instance.TabIndex = 80;
			this.la_head_instance.Text = "INSTANCE";
			this.la_head_instance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_repo_inst_
			// 
			this.la_repo_inst_.Location = new System.Drawing.Point(10, 300);
			this.la_repo_inst_.Margin = new System.Windows.Forms.Padding(0);
			this.la_repo_inst_.Name = "la_repo_inst_";
			this.la_repo_inst_.Size = new System.Drawing.Size(55, 20);
			this.la_repo_inst_.TabIndex = 78;
			this.la_repo_inst_.Text = "repo";
			this.la_repo_inst_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_restype_inst
			// 
			this.la_restype_inst.Location = new System.Drawing.Point(70, 280);
			this.la_restype_inst.Margin = new System.Windows.Forms.Padding(0);
			this.la_restype_inst.Name = "la_restype_inst";
			this.la_restype_inst.Size = new System.Drawing.Size(200, 20);
			this.la_restype_inst.TabIndex = 77;
			this.la_restype_inst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_restype_inst_
			// 
			this.la_restype_inst_.Location = new System.Drawing.Point(10, 280);
			this.la_restype_inst_.Margin = new System.Windows.Forms.Padding(0);
			this.la_restype_inst_.Name = "la_restype_inst_";
			this.la_restype_inst_.Size = new System.Drawing.Size(55, 20);
			this.la_restype_inst_.TabIndex = 76;
			this.la_restype_inst_.Text = "restype";
			this.la_restype_inst_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_template_inst
			// 
			this.la_template_inst.Location = new System.Drawing.Point(70, 260);
			this.la_template_inst.Margin = new System.Windows.Forms.Padding(0);
			this.la_template_inst.Name = "la_template_inst";
			this.la_template_inst.Size = new System.Drawing.Size(200, 20);
			this.la_template_inst.TabIndex = 75;
			this.la_template_inst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_template_inst_
			// 
			this.la_template_inst_.Location = new System.Drawing.Point(10, 260);
			this.la_template_inst_.Margin = new System.Windows.Forms.Padding(0);
			this.la_template_inst_.Name = "la_template_inst_";
			this.la_template_inst_.Size = new System.Drawing.Size(55, 20);
			this.la_template_inst_.TabIndex = 74;
			this.la_template_inst_.Text = "template";
			this.la_template_inst_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_file_inst
			// 
			this.la_file_inst.Location = new System.Drawing.Point(70, 240);
			this.la_file_inst.Margin = new System.Windows.Forms.Padding(0);
			this.la_file_inst.Name = "la_file_inst";
			this.la_file_inst.Size = new System.Drawing.Size(200, 20);
			this.la_file_inst.TabIndex = 73;
			this.la_file_inst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_file_inst_
			// 
			this.la_file_inst_.Location = new System.Drawing.Point(10, 240);
			this.la_file_inst_.Margin = new System.Windows.Forms.Padding(0);
			this.la_file_inst_.Name = "la_file_inst_";
			this.la_file_inst_.Size = new System.Drawing.Size(55, 20);
			this.la_file_inst_.TabIndex = 72;
			this.la_file_inst_.Text = "file";
			this.la_file_inst_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_head_resource
			// 
			this.la_head_resource.Location = new System.Drawing.Point(5, 220);
			this.la_head_resource.Margin = new System.Windows.Forms.Padding(0);
			this.la_head_resource.Name = "la_head_resource";
			this.la_head_resource.Size = new System.Drawing.Size(100, 20);
			this.la_head_resource.TabIndex = 71;
			this.la_head_resource.Text = "RESOURCE";
			this.la_head_resource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_head_blueprint
			// 
			this.la_head_blueprint.Location = new System.Drawing.Point(5, 90);
			this.la_head_blueprint.Margin = new System.Windows.Forms.Padding(0);
			this.la_head_blueprint.Name = "la_head_blueprint";
			this.la_head_blueprint.Size = new System.Drawing.Size(100, 20);
			this.la_head_blueprint.TabIndex = 70;
			this.la_head_blueprint.Text = "BLUEPRINT";
			this.la_head_blueprint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_head_template
			// 
			this.la_head_template.Location = new System.Drawing.Point(5, 5);
			this.la_head_template.Margin = new System.Windows.Forms.Padding(0);
			this.la_head_template.Name = "la_head_template";
			this.la_head_template.Size = new System.Drawing.Size(100, 20);
			this.la_head_template.TabIndex = 69;
			this.la_head_template.Text = "TEMPLATE";
			this.la_head_template.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_template
			// 
			this.la_template.Location = new System.Drawing.Point(70, 130);
			this.la_template.Margin = new System.Windows.Forms.Padding(0);
			this.la_template.Name = "la_template";
			this.la_template.Size = new System.Drawing.Size(200, 20);
			this.la_template.TabIndex = 68;
			this.la_template.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_template_
			// 
			this.la_template_.Location = new System.Drawing.Point(10, 130);
			this.la_template_.Margin = new System.Windows.Forms.Padding(0);
			this.la_template_.Name = "la_template_";
			this.la_template_.Size = new System.Drawing.Size(55, 20);
			this.la_template_.TabIndex = 67;
			this.la_template_.Text = "template";
			this.la_template_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_resref
			// 
			this.la_resref.Location = new System.Drawing.Point(70, 110);
			this.la_resref.Margin = new System.Windows.Forms.Padding(0);
			this.la_resref.Name = "la_resref";
			this.la_resref.Size = new System.Drawing.Size(200, 20);
			this.la_resref.TabIndex = 66;
			this.la_resref.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_resref_
			// 
			this.la_resref_.Location = new System.Drawing.Point(10, 110);
			this.la_resref_.Margin = new System.Windows.Forms.Padding(0);
			this.la_resref_.Name = "la_resref_";
			this.la_resref_.Size = new System.Drawing.Size(55, 20);
			this.la_resref_.TabIndex = 65;
			this.la_resref_.Text = "resref";
			this.la_resref_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_itype
			// 
			this.la_itype.Location = new System.Drawing.Point(70, 25);
			this.la_itype.Margin = new System.Windows.Forms.Padding(0);
			this.la_itype.Name = "la_itype";
			this.la_itype.Size = new System.Drawing.Size(200, 20);
			this.la_itype.TabIndex = 64;
			this.la_itype.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_itype_
			// 
			this.la_itype_.Location = new System.Drawing.Point(10, 25);
			this.la_itype_.Margin = new System.Windows.Forms.Padding(0);
			this.la_itype_.Name = "la_itype_";
			this.la_itype_.Size = new System.Drawing.Size(55, 20);
			this.la_itype_.TabIndex = 63;
			this.la_itype_.Text = "IType";
			this.la_itype_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_name
			// 
			this.la_name.Location = new System.Drawing.Point(70, 65);
			this.la_name.Margin = new System.Windows.Forms.Padding(0);
			this.la_name.Name = "la_name";
			this.la_name.Size = new System.Drawing.Size(200, 20);
			this.la_name.TabIndex = 62;
			this.la_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_name_
			// 
			this.la_name_.Location = new System.Drawing.Point(10, 65);
			this.la_name_.Margin = new System.Windows.Forms.Padding(0);
			this.la_name_.Name = "la_name_";
			this.la_name_.Size = new System.Drawing.Size(55, 20);
			this.la_name_.TabIndex = 61;
			this.la_name_.Text = "tag";
			this.la_name_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_type
			// 
			this.la_type.Location = new System.Drawing.Point(70, 45);
			this.la_type.Margin = new System.Windows.Forms.Padding(0);
			this.la_type.Name = "la_type";
			this.la_type.Size = new System.Drawing.Size(200, 20);
			this.la_type.TabIndex = 60;
			this.la_type.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_type_
			// 
			this.la_type_.Location = new System.Drawing.Point(10, 45);
			this.la_type_.Margin = new System.Windows.Forms.Padding(0);
			this.la_type_.Name = "la_type_";
			this.la_type_.Size = new System.Drawing.Size(55, 20);
			this.la_type_.TabIndex = 59;
			this.la_type_.Text = "object";
			this.la_type_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_repotype
			// 
			this.la_repotype.Location = new System.Drawing.Point(70, 150);
			this.la_repotype.Margin = new System.Windows.Forms.Padding(0);
			this.la_repotype.Name = "la_repotype";
			this.la_repotype.Size = new System.Drawing.Size(200, 20);
			this.la_repotype.TabIndex = 58;
			this.la_repotype.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// la_repotype_
			// 
			this.la_repotype_.Location = new System.Drawing.Point(10, 150);
			this.la_repotype_.Margin = new System.Windows.Forms.Padding(0);
			this.la_repotype_.Name = "la_repotype_";
			this.la_repotype_.Size = new System.Drawing.Size(55, 20);
			this.la_repotype_.TabIndex = 57;
			this.la_repotype_.Text = "repotype";
			this.la_repotype_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ss_camera
			// 
			this.ss_camera.Font = new System.Drawing.Font("Consolas", 7F);
			this.ss_camera.GripMargin = new System.Windows.Forms.Padding(0);
			this.ss_camera.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tssl_camera_label,
			this.tssl_camera_xpos,
			this.tssl_camera_ypos,
			this.tssl_camera_zpos,
			this.tssl_camera_rot,
			this.tssl_camera_dist});
			this.ss_camera.Location = new System.Drawing.Point(0, 412);
			this.ss_camera.Name = "ss_camera";
			this.ss_camera.Size = new System.Drawing.Size(285, 22);
			this.ss_camera.SizingGrip = false;
			this.ss_camera.TabIndex = 3;
			// 
			// tssl_camera_label
			// 
			this.tssl_camera_label.AutoSize = false;
			this.tssl_camera_label.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.tssl_camera_label.Name = "tssl_camera_label";
			this.tssl_camera_label.Size = new System.Drawing.Size(43, 22);
			this.tssl_camera_label.Text = "Camera";
			this.tssl_camera_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tssl_camera_xpos
			// 
			this.tssl_camera_xpos.AutoSize = false;
			this.tssl_camera_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_camera_xpos.Name = "tssl_camera_xpos";
			this.tssl_camera_xpos.Size = new System.Drawing.Size(45, 22);
			this.tssl_camera_xpos.Text = "x";
			this.tssl_camera_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_camera_ypos
			// 
			this.tssl_camera_ypos.AutoSize = false;
			this.tssl_camera_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_camera_ypos.Name = "tssl_camera_ypos";
			this.tssl_camera_ypos.Size = new System.Drawing.Size(45, 22);
			this.tssl_camera_ypos.Text = "y";
			this.tssl_camera_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_camera_zpos
			// 
			this.tssl_camera_zpos.AutoSize = false;
			this.tssl_camera_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_camera_zpos.Name = "tssl_camera_zpos";
			this.tssl_camera_zpos.Size = new System.Drawing.Size(45, 22);
			this.tssl_camera_zpos.Text = "z";
			this.tssl_camera_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_camera_rot
			// 
			this.tssl_camera_rot.AutoSize = false;
			this.tssl_camera_rot.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_camera_rot.Name = "tssl_camera_rot";
			this.tssl_camera_rot.Size = new System.Drawing.Size(45, 22);
			this.tssl_camera_rot.Text = "r";
			this.tssl_camera_rot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_camera_dist
			// 
			this.tssl_camera_dist.AutoSize = false;
			this.tssl_camera_dist.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_camera_dist.Name = "tssl_camera_dist";
			this.tssl_camera_dist.Size = new System.Drawing.Size(45, 22);
			this.tssl_camera_dist.Text = "d";
			this.tssl_camera_dist.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ss_model
			// 
			this.ss_model.Font = new System.Drawing.Font("Consolas", 7F);
			this.ss_model.GripMargin = new System.Windows.Forms.Padding(0);
			this.ss_model.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tssl_model_label,
			this.tssl_model_xpos,
			this.tssl_model_ypos,
			this.tssl_model_zpos,
			this.tssl_model_rot});
			this.ss_model.Location = new System.Drawing.Point(0, 434);
			this.ss_model.Name = "ss_model";
			this.ss_model.Size = new System.Drawing.Size(285, 22);
			this.ss_model.SizingGrip = false;
			this.ss_model.TabIndex = 4;
			// 
			// tssl_model_label
			// 
			this.tssl_model_label.AutoSize = false;
			this.tssl_model_label.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.tssl_model_label.Name = "tssl_model_label";
			this.tssl_model_label.Size = new System.Drawing.Size(43, 22);
			this.tssl_model_label.Text = "Model";
			this.tssl_model_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tssl_model_xpos
			// 
			this.tssl_model_xpos.AutoSize = false;
			this.tssl_model_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_model_xpos.Name = "tssl_model_xpos";
			this.tssl_model_xpos.Size = new System.Drawing.Size(45, 22);
			this.tssl_model_xpos.Text = "x";
			this.tssl_model_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_model_ypos
			// 
			this.tssl_model_ypos.AutoSize = false;
			this.tssl_model_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_model_ypos.Name = "tssl_model_ypos";
			this.tssl_model_ypos.Size = new System.Drawing.Size(45, 22);
			this.tssl_model_ypos.Text = "y";
			this.tssl_model_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_model_zpos
			// 
			this.tssl_model_zpos.AutoSize = false;
			this.tssl_model_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_model_zpos.Name = "tssl_model_zpos";
			this.tssl_model_zpos.Size = new System.Drawing.Size(45, 22);
			this.tssl_model_zpos.Text = "z";
			this.tssl_model_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_model_rot
			// 
			this.tssl_model_rot.AutoSize = false;
			this.tssl_model_rot.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_model_rot.Name = "tssl_model_rot";
			this.tssl_model_rot.Size = new System.Drawing.Size(45, 22);
			this.tssl_model_rot.Text = "r";
			this.tssl_model_rot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ss_light
			// 
			this.ss_light.Font = new System.Drawing.Font("Consolas", 7F);
			this.ss_light.GripMargin = new System.Windows.Forms.Padding(0);
			this.ss_light.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tssl_light_label,
			this.tssl_light_xpos,
			this.tssl_light_ypos,
			this.tssl_light_zpos,
			this.tssl_light_blank,
			this.tssl_light_intensity});
			this.ss_light.Location = new System.Drawing.Point(0, 456);
			this.ss_light.Name = "ss_light";
			this.ss_light.Size = new System.Drawing.Size(285, 22);
			this.ss_light.SizingGrip = false;
			this.ss_light.TabIndex = 5;
			// 
			// tssl_light_label
			// 
			this.tssl_light_label.AutoSize = false;
			this.tssl_light_label.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.tssl_light_label.Name = "tssl_light_label";
			this.tssl_light_label.Size = new System.Drawing.Size(43, 22);
			this.tssl_light_label.Text = "Light";
			this.tssl_light_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tssl_light_xpos
			// 
			this.tssl_light_xpos.AutoSize = false;
			this.tssl_light_xpos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_light_xpos.Name = "tssl_light_xpos";
			this.tssl_light_xpos.Size = new System.Drawing.Size(45, 22);
			this.tssl_light_xpos.Text = "x";
			this.tssl_light_xpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_light_ypos
			// 
			this.tssl_light_ypos.AutoSize = false;
			this.tssl_light_ypos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_light_ypos.Name = "tssl_light_ypos";
			this.tssl_light_ypos.Size = new System.Drawing.Size(45, 22);
			this.tssl_light_ypos.Text = "y";
			this.tssl_light_ypos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_light_zpos
			// 
			this.tssl_light_zpos.AutoSize = false;
			this.tssl_light_zpos.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_light_zpos.Name = "tssl_light_zpos";
			this.tssl_light_zpos.Size = new System.Drawing.Size(45, 22);
			this.tssl_light_zpos.Text = "z";
			this.tssl_light_zpos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_light_blank
			// 
			this.tssl_light_blank.AutoSize = false;
			this.tssl_light_blank.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_light_blank.Name = "tssl_light_blank";
			this.tssl_light_blank.Size = new System.Drawing.Size(45, 22);
			this.tssl_light_blank.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tssl_light_intensity
			// 
			this.tssl_light_intensity.AutoSize = false;
			this.tssl_light_intensity.Margin = new System.Windows.Forms.Padding(0);
			this.tssl_light_intensity.Name = "tssl_light_intensity";
			this.tssl_light_intensity.Size = new System.Drawing.Size(45, 22);
			this.tssl_light_intensity.Text = "i";
			this.tssl_light_intensity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pa_gui
			// 
			this.pa_gui.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pa_gui.Location = new System.Drawing.Point(0, 0);
			this.pa_gui.Margin = new System.Windows.Forms.Padding(0);
			this.pa_gui.Name = "pa_gui";
			this.pa_gui.Size = new System.Drawing.Size(327, 478);
			this.pa_gui.TabIndex = 0;
			// 
			// toolTip1
			// 
			this.toolTip1.Active = false;
			this.toolTip1.AutoPopDelay = 30000;
			this.toolTip1.InitialDelay = 50;
			this.toolTip1.ReshowDelay = 100;
			// 
			// CreVisF
			// 
			this.ClientSize = new System.Drawing.Size(612, 478);
			this.Controls.Add(this.pa_gui);
			this.Controls.Add(this.pa_con);
			this.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Name = "CreVisF";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.pa_con.ResumeLayout(false);
			this.pa_con.PerformLayout();
			this.tc1.ResumeLayout(false);
			this.tp_controls.ResumeLayout(false);
			this.gb_Light.ResumeLayout(false);
			this.gb_Light.PerformLayout();
			this.gb_model.ResumeLayout(false);
			this.gb_camera.ResumeLayout(false);
			this.gb_camera.PerformLayout();
			this.tp_creature.ResumeLayout(false);
			this.tp_resource.ResumeLayout(false);
			this.ss_camera.ResumeLayout(false);
			this.ss_camera.PerformLayout();
			this.ss_model.ResumeLayout(false);
			this.ss_model.PerformLayout();
			this.ss_light.ResumeLayout(false);
			this.ss_light.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion Designer
	}



	/// <summary>
	/// Derived class for TabControl.
	/// </summary>
	public sealed class CompositedTabControl
		: TabControl
	{
		#region Properties (override)
		/// <summary>
		/// Prevents flicker.
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000; // enable 'WS_EX_COMPOSITED'
				return cp;
			}
		}
		#endregion Properties (override)
	}
}
