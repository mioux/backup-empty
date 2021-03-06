﻿namespace BackupEmpty
{
    partial class frm_main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_main));
        	this.lbl_server = new System.Windows.Forms.Label();
        	this.txt_server = new System.Windows.Forms.TextBox();
        	this.txt_login = new System.Windows.Forms.TextBox();
        	this.txt_passwd = new System.Windows.Forms.TextBox();
        	this.lbl_login = new System.Windows.Forms.Label();
        	this.lbl_passwd = new System.Windows.Forms.Label();
        	this.btn_purge = new System.Windows.Forms.Button();
        	this.btn_deconnect = new System.Windows.Forms.Button();
        	this.txt_file = new System.Windows.Forms.TextBox();
        	this.btn_browse = new System.Windows.Forms.Button();
        	this.btn_restore = new System.Windows.Forms.Button();
        	this.lbl_db = new System.Windows.Forms.Label();
        	this.btn_refresh = new System.Windows.Forms.Button();
        	this.cbx_dblist = new System.Windows.Forms.ComboBox();
        	this.chx_trusted = new System.Windows.Forms.CheckBox();
        	this.ofd_backup = new System.Windows.Forms.OpenFileDialog();
        	this.btn_listInstance = new System.Windows.Forms.Button();
        	this.mnu_dlList = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.patientezToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        	this.bgd_loading = new System.ComponentModel.BackgroundWorker();
        	this.chx_savePW = new System.Windows.Forms.CheckBox();
        	this.rtb_log = new System.Windows.Forms.RichTextBox();
        	this.btn_aot = new System.Windows.Forms.Button();
        	this.btn_optimize = new System.Windows.Forms.Button();
        	this.mnu_dlList.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// lbl_server
        	// 
        	this.lbl_server.AutoSize = true;
        	this.lbl_server.Location = new System.Drawing.Point(12, 9);
        	this.lbl_server.Name = "lbl_server";
        	this.lbl_server.Size = new System.Drawing.Size(102, 13);
        	this.lbl_server.TabIndex = 0;
        	this.lbl_server.Text = "Serveur (+ instance)";
        	// 
        	// txt_server
        	// 
        	this.txt_server.Location = new System.Drawing.Point(190, 6);
        	this.txt_server.Name = "txt_server";
        	this.txt_server.Size = new System.Drawing.Size(341, 20);
        	this.txt_server.TabIndex = 1;
        	this.txt_server.TextChanged += new System.EventHandler(this.txt_server_TextChanged);
        	// 
        	// txt_login
        	// 
        	this.txt_login.Location = new System.Drawing.Point(190, 32);
        	this.txt_login.Name = "txt_login";
        	this.txt_login.Size = new System.Drawing.Size(374, 20);
        	this.txt_login.TabIndex = 3;
        	this.txt_login.TextChanged += new System.EventHandler(this.txt_login_TextChanged);
        	// 
        	// txt_passwd
        	// 
        	this.txt_passwd.Location = new System.Drawing.Point(190, 58);
        	this.txt_passwd.Name = "txt_passwd";
        	this.txt_passwd.PasswordChar = '●';
        	this.txt_passwd.Size = new System.Drawing.Size(272, 20);
        	this.txt_passwd.TabIndex = 5;
        	this.txt_passwd.TextChanged += new System.EventHandler(this.txt_passwd_TextChanged);
        	// 
        	// lbl_login
        	// 
        	this.lbl_login.AutoSize = true;
        	this.lbl_login.Location = new System.Drawing.Point(12, 35);
        	this.lbl_login.Name = "lbl_login";
        	this.lbl_login.Size = new System.Drawing.Size(33, 13);
        	this.lbl_login.TabIndex = 2;
        	this.lbl_login.Text = "Login";
        	// 
        	// lbl_passwd
        	// 
        	this.lbl_passwd.AutoSize = true;
        	this.lbl_passwd.Location = new System.Drawing.Point(12, 61);
        	this.lbl_passwd.Name = "lbl_passwd";
        	this.lbl_passwd.Size = new System.Drawing.Size(71, 13);
        	this.lbl_passwd.TabIndex = 4;
        	this.lbl_passwd.Text = "Mot de passe";
        	// 
        	// btn_purge
        	// 
        	this.btn_purge.Location = new System.Drawing.Point(372, 134);
        	this.btn_purge.Name = "btn_purge";
        	this.btn_purge.Size = new System.Drawing.Size(103, 23);
        	this.btn_purge.TabIndex = 9;
        	this.btn_purge.Text = "Purger";
        	this.btn_purge.UseVisualStyleBackColor = true;
        	this.btn_purge.Click += new System.EventHandler(this.btn_purge_Click);
        	// 
        	// btn_deconnect
        	// 
        	this.btn_deconnect.Location = new System.Drawing.Point(246, 134);
        	this.btn_deconnect.Name = "btn_deconnect";
        	this.btn_deconnect.Size = new System.Drawing.Size(120, 23);
        	this.btn_deconnect.TabIndex = 8;
        	this.btn_deconnect.Text = "Déconnecter tous";
        	this.btn_deconnect.UseVisualStyleBackColor = true;
        	this.btn_deconnect.Click += new System.EventHandler(this.btn_deconnect_Click);
        	// 
        	// txt_file
        	// 
        	this.txt_file.Location = new System.Drawing.Point(12, 163);
        	this.txt_file.Name = "txt_file";
        	this.txt_file.Size = new System.Drawing.Size(428, 20);
        	this.txt_file.TabIndex = 11;
        	this.txt_file.TextChanged += new System.EventHandler(this.txt_file_TextChanged);
        	// 
        	// btn_browse
        	// 
        	this.btn_browse.Location = new System.Drawing.Point(446, 163);
        	this.btn_browse.Name = "btn_browse";
        	this.btn_browse.Size = new System.Drawing.Size(27, 23);
        	this.btn_browse.TabIndex = 12;
        	this.btn_browse.Text = "...";
        	this.btn_browse.UseVisualStyleBackColor = true;
        	this.btn_browse.Click += new System.EventHandler(this.btn_browse_Click);
        	// 
        	// btn_restore
        	// 
        	this.btn_restore.Location = new System.Drawing.Point(479, 163);
        	this.btn_restore.Name = "btn_restore";
        	this.btn_restore.Size = new System.Drawing.Size(85, 23);
        	this.btn_restore.TabIndex = 13;
        	this.btn_restore.Text = "Restaurer";
        	this.btn_restore.UseVisualStyleBackColor = true;
        	this.btn_restore.Click += new System.EventHandler(this.btn_restore_Click);
        	// 
        	// lbl_db
        	// 
        	this.lbl_db.AutoSize = true;
        	this.lbl_db.Location = new System.Drawing.Point(12, 87);
        	this.lbl_db.Name = "lbl_db";
        	this.lbl_db.Size = new System.Drawing.Size(90, 13);
        	this.lbl_db.TabIndex = 6;
        	this.lbl_db.Text = "Base de données";
        	// 
        	// btn_refresh
        	// 
        	this.btn_refresh.Location = new System.Drawing.Point(479, 134);
        	this.btn_refresh.Name = "btn_refresh";
        	this.btn_refresh.Size = new System.Drawing.Size(85, 23);
        	this.btn_refresh.TabIndex = 10;
        	this.btn_refresh.Text = "Raffraichir";
        	this.btn_refresh.UseVisualStyleBackColor = true;
        	this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
        	// 
        	// cbx_dblist
        	// 
        	this.cbx_dblist.DisplayMember = "name";
        	this.cbx_dblist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.cbx_dblist.FormattingEnabled = true;
        	this.cbx_dblist.Location = new System.Drawing.Point(190, 83);
        	this.cbx_dblist.Name = "cbx_dblist";
        	this.cbx_dblist.Size = new System.Drawing.Size(374, 21);
        	this.cbx_dblist.TabIndex = 7;
        	this.cbx_dblist.ValueMember = "name";
        	// 
        	// chx_trusted
        	// 
        	this.chx_trusted.AutoSize = true;
        	this.chx_trusted.Location = new System.Drawing.Point(190, 111);
        	this.chx_trusted.Name = "chx_trusted";
        	this.chx_trusted.Size = new System.Drawing.Size(120, 17);
        	this.chx_trusted.TabIndex = 14;
        	this.chx_trusted.Text = "Connexion windows";
        	this.chx_trusted.UseVisualStyleBackColor = true;
        	this.chx_trusted.CheckedChanged += new System.EventHandler(this.chx_trusted_CheckedChanged);
        	// 
        	// ofd_backup
        	// 
        	this.ofd_backup.FileName = "*.bak";
        	this.ofd_backup.Filter = "Fichier backup|*.bak,*.dat|Tous les fichiers|*.*";
        	this.ofd_backup.RestoreDirectory = true;
        	this.ofd_backup.Title = "Fichier source";
        	// 
        	// btn_listInstance
        	// 
        	this.btn_listInstance.Location = new System.Drawing.Point(537, 4);
        	this.btn_listInstance.Name = "btn_listInstance";
        	this.btn_listInstance.Size = new System.Drawing.Size(27, 23);
        	this.btn_listInstance.TabIndex = 2;
        	this.btn_listInstance.Text = "...";
        	this.btn_listInstance.UseVisualStyleBackColor = true;
        	this.btn_listInstance.Click += new System.EventHandler(this.btn_listInstance_Click);
        	// 
        	// mnu_dlList
        	// 
        	this.mnu_dlList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        	        	        	this.patientezToolStripMenuItem});
        	this.mnu_dlList.Name = "mnu_dlList";
        	this.mnu_dlList.Size = new System.Drawing.Size(143, 26);
        	// 
        	// patientezToolStripMenuItem
        	// 
        	this.patientezToolStripMenuItem.Enabled = false;
        	this.patientezToolStripMenuItem.Name = "patientezToolStripMenuItem";
        	this.patientezToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
        	this.patientezToolStripMenuItem.Text = "Patientez...";
        	// 
        	// bgd_loading
        	// 
        	this.bgd_loading.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgd_loading_DoWork);
        	this.bgd_loading.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgd_loading_RunWorkerCompleted);
        	// 
        	// chx_savePW
        	// 
        	this.chx_savePW.AutoSize = true;
        	this.chx_savePW.Location = new System.Drawing.Point(468, 60);
        	this.chx_savePW.Name = "chx_savePW";
        	this.chx_savePW.Size = new System.Drawing.Size(96, 17);
        	this.chx_savePW.TabIndex = 6;
        	this.chx_savePW.Text = "Sauvegarder ?";
        	this.chx_savePW.UseVisualStyleBackColor = true;
        	this.chx_savePW.CheckedChanged += new System.EventHandler(this.chx_savePW_CheckedChanged);
        	// 
        	// rtb_log
        	// 
        	this.rtb_log.Location = new System.Drawing.Point(12, 192);
        	this.rtb_log.Name = "rtb_log";
        	this.rtb_log.ReadOnly = true;
        	this.rtb_log.Size = new System.Drawing.Size(552, 257);
        	this.rtb_log.TabIndex = 15;
        	this.rtb_log.Text = "";
        	// 
        	// btn_aot
        	// 
        	this.btn_aot.Image = ((System.Drawing.Image)(resources.GetObject("unpined")));
        	this.btn_aot.Location = new System.Drawing.Point(12, 134);
        	this.btn_aot.Name = "btn_aot";
        	this.btn_aot.Size = new System.Drawing.Size(23, 23);
        	this.btn_aot.TabIndex = 16;
        	this.btn_aot.UseVisualStyleBackColor = true;
        	this.btn_aot.Click += new System.EventHandler(this.Btn_aotClick);
        	// 
        	// btn_optimize
        	// 
        	this.btn_optimize.Location = new System.Drawing.Point(100, 134);
        	this.btn_optimize.Name = "btn_optimize";
        	this.btn_optimize.Size = new System.Drawing.Size(140, 23);
        	this.btn_optimize.TabIndex = 17;
        	this.btn_optimize.Text = "Réindexer (TRES LONG)";
        	this.btn_optimize.UseVisualStyleBackColor = true;
        	this.btn_optimize.Click += new System.EventHandler(this.Btn_optimizeClick);
        	// 
        	// frm_main
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(576, 461);
        	this.Controls.Add(this.btn_optimize);
        	this.Controls.Add(this.btn_aot);
        	this.Controls.Add(this.rtb_log);
        	this.Controls.Add(this.chx_savePW);
        	this.Controls.Add(this.btn_listInstance);
        	this.Controls.Add(this.chx_trusted);
        	this.Controls.Add(this.cbx_dblist);
        	this.Controls.Add(this.lbl_db);
        	this.Controls.Add(this.btn_restore);
        	this.Controls.Add(this.btn_browse);
        	this.Controls.Add(this.btn_refresh);
        	this.Controls.Add(this.txt_file);
        	this.Controls.Add(this.lbl_passwd);
        	this.Controls.Add(this.btn_deconnect);
        	this.Controls.Add(this.btn_purge);
        	this.Controls.Add(this.lbl_login);
        	this.Controls.Add(this.txt_passwd);
        	this.Controls.Add(this.txt_login);
        	this.Controls.Add(this.txt_server);
        	this.Controls.Add(this.lbl_server);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "frm_main";
        	this.Text = "Backup et vidage";
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_main_FormClosing);
        	this.mnu_dlList.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.Button btn_optimize;
        private System.Windows.Forms.Button btn_aot;
        private System.Windows.Forms.RichTextBox rtb_log;

        #endregion

        private System.Windows.Forms.Label lbl_server;
        private System.Windows.Forms.TextBox txt_server;
        private System.Windows.Forms.TextBox txt_login;
        private System.Windows.Forms.TextBox txt_passwd;
        private System.Windows.Forms.Label lbl_login;
        private System.Windows.Forms.Label lbl_passwd;
        private System.Windows.Forms.Button btn_purge;
        private System.Windows.Forms.Button btn_deconnect;
        private System.Windows.Forms.TextBox txt_file;
        private System.Windows.Forms.Button btn_browse;
        private System.Windows.Forms.Button btn_restore;
        private System.Windows.Forms.Label lbl_db;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.ComboBox cbx_dblist;
        private System.Windows.Forms.CheckBox chx_trusted;
        private System.Windows.Forms.OpenFileDialog ofd_backup;
        private System.Windows.Forms.Button btn_listInstance;
        private System.Windows.Forms.ContextMenuStrip mnu_dlList;
        private System.Windows.Forms.ToolStripMenuItem patientezToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bgd_loading;
        private System.Windows.Forms.CheckBox chx_savePW;
    }
}

