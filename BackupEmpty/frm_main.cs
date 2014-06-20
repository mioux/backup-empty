using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Data.Sql;
using System.Threading;

namespace BackupEmpty
{
    /// <summary>
    /// Formulaire
    /// </summary>

    public partial class frm_main : Form
    {
        /// <summary>
        /// variable de connexion
        /// </summary>

        private SqlConnection con = null;

        /// <summary>
        /// Constructeur
        /// </summary>

        public frm_main()
        {
            InitializeComponent();

            txt_file.Text = Properties.Settings.Default.File;
            txt_login.Text = Properties.Settings.Default.Login;
            txt_passwd.Text = Properties.Settings.Default.Password;
            txt_server.Text = Properties.Settings.Default.Server;
            chx_trusted.Checked = Properties.Settings.Default.TrustedConnection;

            /*Thread sqlMenu = new Thread(CreateSqlInstanceMenu);
            sqlMenu.Start();*/
            CreateSqlInstanceMenu();
        }

        /// <summary>
        /// Création de la liste des menus SQL
        /// </summary>

        private void CreateSqlInstanceMenu()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable dbList = instance.GetDataSources();

            mnu_dlList.Items.Clear();

            foreach (DataRow curServer in dbList.Rows)
            {
                string name = "";
                if (curServer["ServerName"] != null && curServer["ServerName"] != DBNull.Value)
                    name = curServer["ServerName"].ToString();
                if (curServer["InstanceName"] != null && curServer["InstanceName"] != DBNull.Value && curServer["InstanceName"].ToString() != string.Empty)
                    name += "\\" + curServer["InstanceName"].ToString();

                mnu_dlList.Items.Add(name, Properties.Resources.sql, menuClick);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void menuClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
                txt_server.Text = ((ToolStripMenuItem)sender).Text;
        }

        /// <summary>
        /// Passage en mode trusted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void chx_trusted_CheckedChanged(object sender, EventArgs e)
        {
            txt_login.Enabled = txt_passwd.Enabled = !chx_trusted.Checked;
            Properties.Settings.Default.TrustedConnection = chx_trusted.Checked;
        }

        /// <summary>
        /// Construction d'une connectionstring
        /// </summary>
        /// <returns></returns>

        private string BuildCS()
        {
            string cnLogin = "Server={0};Database=master;User Id={1};Password={2};Connection Timeout=30";
            string cnTrusted = "Server={0};Database=master;Trusted_Connection=True;Connection Timeout=30";
            string data = chx_trusted.Checked ? cnTrusted : cnLogin;

            return string.Format(data, txt_server.Text, txt_login.Text, txt_passwd.Text);
        }

        /// <summary>
        /// Tester la connexion
        /// </summary>
        /// <returns></returns>

        private bool TestCS()
        {
            bool data = false;

            con = new SqlConnection(BuildCS());
            try
            {
                con.Open();
                data = true;
            }
            catch (Exception exp)
            {
                data = false;
#if DEBUG
                MessageBox.Show(exp.Message);
#endif
            }
            finally
            {
                con.Close();
            }

            return data;
        }

        /// <summary>
        /// Liste des bases de données
        /// </summary>
        /// <returns></returns>

        private DataTable DBList()
        {
            DataTable data = new DataTable();
         
            if (!TestCS())
            {
                MessageBox.Show("Erreur lors de la récupération de la liste des bases de données. Vérifier les paramètres de connexion.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return data;
            }

            SqlCommand com = new SqlCommand(@"SELECT name
FROM master..sysdatabases
WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');", con);
            com.CommandTimeout = 86400;

            SqlDataAdapter adapt = new SqlDataAdapter(com);

            try
            {
                con.Open();
                adapt.Fill(data);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Erreur lors de la récupération de la liste des bases de données. Droits insuffisants.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if DEBUG
                MessageBox.Show(exp.Message);
#endif
            }
            finally
            {
                con.Close();
            }

            return data;
        }

        /// <summary>
        /// Purger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_purge_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            
            if (!TestCS())
            {
                MessageBox.Show("Erreur lors de la purge. Vérifier les paramètres de connexion.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }

            int version = SqlVersion();
            if (version == -1)
            {
                MessageBox.Show("Erreur lors de la purge. Impossible de déterminer la version de SQL Server.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }

            SqlCommand com = new SqlCommand("", con);
            com.CommandTimeout = 86400;

            try
            {
                con.Open();
                if (version > 8)
                {
                    com.CommandText = "exec sp_msforeachdb 'if ''?'' not in (''master'', ''tempdb'', ''model'', ''msdb'') exec (''alter database ? set recovery simple'')'";
                    com.ExecuteNonQuery();

                    com.CommandText = "exec sp_msforeachdb 'DBCC SHRINKDATABASE(?, 0)'";
                    com.ExecuteNonQuery();
                }
                else
                {
                    com.CommandText = "exec sp_msforeachdb 'backup log ? with truncate_only'";
                    com.ExecuteNonQuery();

                    DataTable tbl_db = DBList();

                    foreach (DataRow row in tbl_db.Rows)
                    {
                        DataTable files = new DataTable();
                        com.CommandText = "exec sp_helpfile";
                        SqlDataAdapter adapt = new SqlDataAdapter(com);
                        adapt.Fill(files);

                        foreach (DataRow rowFile in files.Rows)
                        {
                            com.CommandText = string.Format("dbcc shrinkfile({0}, 0)", rowFile["name"].ToString());
                            com.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Erreur lors de la purge. Impossible de purger une base de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if DEBUG
                MessageBox.Show(exp.Message);
#endif
            }
            finally
            {
                con.Close();
            }

            this.Enabled = true;
        }

        /// <summary>
        /// Version SQL
        /// </summary>
        /// <returns></returns>

        private int SqlVersion()
        {
            int data = -1;

            SqlCommand com = new SqlCommand("SELECT SERVERPROPERTY('ProductVersion')", con);
            com.CommandTimeout = 86400;

            try
            {
                con.Open();
                object version = com.ExecuteScalar();
                if (version != null && version.ToString() != "")
                {
                    string tmpVersion = version.ToString().Split('.')[0];

                    if (int.TryParse(tmpVersion, out data) == false)
                        data = -1;
                }
            }
            catch (Exception exp)
            {
                data = -1;
#if DEBUG
                MessageBox.Show(exp.Message);
#endif
            }
            finally
            {
                con.Close();
            }

            return data;
        }

        /// <summary>
        /// Déconnecter tous les utitilsateurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_deconnect_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            if (!TestCS())
            {
                MessageBox.Show("Erreur lors de la déconnexion des utilisateurs. Vérifier les paramètres de connexion.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }
            if (cbx_dblist.SelectedItem.ToString() == string.Empty)
            {
                MessageBox.Show("Erreur lors de la déconnexion des utilisateurs. Sélectionnez une base de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }

            SqlCommand com = new SqlCommand(string.Empty, con);
            com.CommandTimeout = 86400;

            try
            {
                con.Open();
                com.CommandText = string.Format("alter database {0} set restricted_user; alter database {0} set multi_user;", cbx_dblist.SelectedItem.ToString());
                com.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Erreur lors de la déconnexion des utilisateurs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if DEBUG
                MessageBox.Show(exp.Message);
#endif
            }
            finally
            {
                con.Close();
            }

            this.Enabled = true;
        }

        /// <summary>
        /// Raffraichir la liste des bases de données.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            if (!TestCS())
            {
                MessageBox.Show("Erreur lors du raffraichissement des BDD. Vérifier les paramètres de connexion.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }

            cbx_dblist.DataSource = DBList();

            this.Enabled = true;
        }

        /// <summary>
        /// Bouton parcourir (backup)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_browse_Click(object sender, EventArgs e)
        {
            if (ofd_backup.ShowDialog() == DialogResult.OK)
                txt_file.Text = ofd_backup.FileName;
        }

        /// <summary>
        /// Restaurer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_restore_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            if (!TestCS())
            {
                MessageBox.Show("Erreur lors de la restauration de la BDD. Vérifier les paramètres de connexion.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }
            if (cbx_dblist.SelectedItem.ToString() == string.Empty)
            {
                MessageBox.Show("Erreur lors de la restauration de la BDD. Sélectionnez une base de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }
            if (!File.Exists(txt_file.Text))
            {
                MessageBox.Show("Erreur lors de la restauration de la BDD. Le fichier sélectionné n'existe pas.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }

            SqlCommand com = new SqlCommand(string.Empty, con);
            com.CommandTimeout = 86400;

            try
            {
                con.Open();
                com.CommandText = string.Format("alter database {0} set restricted_user", cbx_dblist.SelectedItem.ToString());
                com.ExecuteNonQuery();
                com.CommandText = string.Format("restore database {0} from disk = '{1}' with replace", cbx_dblist.SelectedItem.ToString(), txt_file.Text);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Erreur lors de la restauration de la BDD.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if DEBUG
                MessageBox.Show(exp.Message);
#endif
            }
            finally
            {
                con.Close();
            }

            this.Enabled = true;
        }

        /// <summary>
        /// Affichage de la liste des instances détectées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btn_listInstance_Click(object sender, EventArgs e)
        {
            mnu_dlList.Show(MousePosition);
        }

        /// <summary>
        /// Sauvegarde du serveur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txt_server_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Server = txt_server.Text;
        }

        /// <summary>
        /// Sauvegarde du login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txt_login_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Login = txt_login.Text;
        }

        /// <summary>
        /// Sauvegarde du mot de passe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txt_passwd_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Password = txt_passwd.Text;
        }

        /// <summary>
        /// Sauvegarde du fichier à restaurer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txt_file_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.File = txt_file.Text;
        }

        /// <summary>
        /// Fermeture du formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
