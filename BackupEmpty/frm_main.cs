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
using System.Security.Cryptography;
using BackupEmpty.Properties;

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
        /// CHANGEME !!! Clef de chiffrement TripleDES (Longueur 24 exactement)
        /// </summary>
        private static string beKEY = "!+7.Z*~S.;|Te93:X5::J~*D";

        /// <summary>
        /// CHANGEME !!! Vecteur d'initialisation TripleDES (Longueur 8 exactement)
        /// </summary>
        private static string beIV = "z1Tm947j";

        private static List<string> sqlList = new List<string>();

        /// <summary>
        /// Constructeur
        /// </summary>

        public frm_main()
        {
            InitializeComponent();

            txt_file.Text = Settings.Default.File;
            txt_login.Text = Settings.Default.Login;
            txt_passwd.Text = Decypher(Settings.Default.Password);
            txt_server.Text = Settings.Default.Server;
            chx_trusted.Checked = Settings.Default.TrustedConnection;
            chx_savePW.Checked = Settings.Default.SavePW;

            bgd_loading.RunWorkerAsync();
        }

        /// <summary>
        /// Déchiffrer
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        private string Decypher(string p)
        {
            return CD(p, true);
        }

        /// <summary>
        /// Chiffre ou déchiffre
        /// </summary>
        /// <param name="p"></param>

        private string CD(string p, bool isDecypher)
        {
            string CDData = "";

            if (string.IsNullOrEmpty(p))
                return CDData;
            
			try
            {
                TripleDES td = TripleDES.Create();
                td.IV = Encoding.ASCII.GetBytes(beIV);
                td.Key = Encoding.ASCII.GetBytes(beKEY);
                td.Mode = CipherMode.CBC;
                td.Padding = PaddingMode.PKCS7;

                byte[] data = isDecypher ? System.Convert.FromBase64String(p) : Encoding.Unicode.GetBytes(p);

                ICryptoTransform d = isDecypher ? td.CreateDecryptor() : td.CreateEncryptor();
                

                //memory stream for output
                MemoryStream memStream = new MemoryStream();
            
                //setup the cryption - output written to memstream
                CryptoStream cryptStream = new CryptoStream(memStream, d, CryptoStreamMode.Write);

                //write data to cryption engine
                cryptStream.Write(data, 0, data.Length);

                //we are finished
                cryptStream.FlushFinalBlock();

                //get result
                byte[] output = memStream.ToArray();

                //finished with engine, so close the stream
                cryptStream.Close();

                CDData = isDecypher ? Encoding.Unicode.GetString(output) : Convert.ToBase64String(output);
            }
            catch
            {
                CDData = "";
                Settings.Default.Password = "";
#if DEBUG
                MessageBox.Show("Erreur lors du " + (isDecypher ? "dé" : "") + "chiffrement du mot de passe", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            }

            return CDData;
        }

        /// <summary>
        /// Chiffrer
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>

        private string Cypher(string p)
        {
            return CD(p, false);
        }

        /// <summary>
        /// Création de la liste des menus SQL
        /// </summary>

        private void CreateSqlInstanceMenu()
        {
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable dbList = instance.GetDataSources();

            foreach (DataRow curServer in dbList.Rows)
            {
                string name = "";
                if (curServer["ServerName"] != null && curServer["ServerName"] != DBNull.Value)
                    name = curServer["ServerName"].ToString();
                if (curServer["InstanceName"] != null && curServer["InstanceName"] != DBNull.Value && curServer["InstanceName"].ToString() != string.Empty)
                    name += "\\" + curServer["InstanceName"].ToString();

                sqlList.Add(name);
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
            txt_login.Enabled = txt_passwd.Enabled = chx_savePW.Enabled = !chx_trusted.Checked;
            Settings.Default.TrustedConnection = chx_trusted.Checked;
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
            con.InfoMessage += con_InfoMessage;
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

            SqlCommand com = new SqlCommand(@"SELECT	name
FROM 	master..sysdatabases
WHERE 	sid <> 0x01 and        -- Not system
		status & 32    = 0 and -- Not Loading
		status & 64    = 0 and -- Not Pre recovery
		status & 128   = 0 and -- Not Recovering
		status & 256   = 0 and -- Not ""Not recovered""
		status & 512   = 0 and -- Not Offline
		status & 1024  = 0 and -- Not Read only
		status & 32768 = 0     -- Not emergency mode;", con);
            com.CommandTimeout = 86400;

            SqlDataAdapter adapt = new SqlDataAdapter(com);

            try
            {
            	LogExec(com.CommandText);
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

            try
            {
                ExecuteNonQuery("exec sp_msforeachdb 'if ''?'' not in (''master'', ''tempdb'', ''model'', ''msdb'') exec (''alter database ? set recovery simple'')'");

                ExecuteNonQuery("exec sp_msforeachdb 'DBCC SHRINKDATABASE(?, 0)'");
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

            try
            {
                object version = ExecuteScalar("SELECT SERVERPROPERTY('ProductVersion')");
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

            try
            {
            	if (!TestCS())
	            {
	                MessageBox.Show("Erreur lors de la déconnexion des utilisateurs. Vérifier les paramètres de connexion.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
	                this.Enabled = true;
	            }
	            else if (cbx_dblist.SelectedItem.ToString() == string.Empty)
	            {
	                MessageBox.Show("Erreur lors de la déconnexion des utilisateurs. Sélectionnez une base de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
	                this.Enabled = true;
	                return;
	            }
	            else
	            {
	                ExecuteNonQuery(string.Format("alter database {0} set restricted_user with rollback immediate; alter database {0} set multi_user;", cbx_dblist.SelectedValue.ToString()));
	            }
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
            	if (con.State == ConnectionState.Connecting || con.State == ConnectionState.Open)
            	{
            		con.Close();
            	}
            	this.Enabled = true;
            }
            
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
            {
                txt_file.Text = ofd_backup.FileName;
            }
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
            	string curDB = cbx_dblist.SelectedValue.ToString();
            	
                ExecuteNonQuery(string.Format("alter database {0} set restricted_user with rollback immediate", curDB));
                
                DataSet ds = new DataSet();
                con.ChangeDatabase(curDB);
                com.CommandText = "exec sp_helpfile";
                SqlDataAdapter adapter = new SqlDataAdapter(com);
                adapter.Fill(ds);
                con.ChangeDatabase("master");
                
				string sql = string.Format("restore database {0} from disk = '{1}' with replace", curDB, txt_file.Text);
                
                if (ds.Tables.Count > 0)
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                	string curFile = dr["name"].ToString();
                	string curPath = dr["filename"].ToString();
                	
                	sql += string.Format(", MOVE '{0}' TO '{1}'", curFile, curPath);
                }
                
                ExecuteNonQuery(sql);
                
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
            Settings.Default.Server = txt_server.Text;
        }

        /// <summary>
        /// Sauvegarde du login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txt_login_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.Login = txt_login.Text;
        }

        /// <summary>
        /// Modification du mot de passe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txt_passwd_TextChanged(object sender, EventArgs e)
        {
            SavePwd();
        }

        /// <summary>
        /// Sauvegarde du mot de passe
        /// </summary>

        private void SavePwd()
        {
            Settings.Default.Password = chx_savePW.Checked ? Cypher(txt_passwd.Text) : "";
        }

        /// <summary>
        /// Sauvegarde du fichier à restaurer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void txt_file_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.File = txt_file.Text;
        }

        /// <summary>
        /// Fermeture du formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void bgd_loading_DoWork(object sender, DoWorkEventArgs e)
        {
            CreateSqlInstanceMenu();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void bgd_loading_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == false)
            {
                mnu_dlList.Items.Clear();

                foreach (string curSql in sqlList)
                    mnu_dlList.Items.Add(curSql,Resources.sql, menuClick);
            }

        }

        /// <summary>
        /// Modification de la sauvegarde du mot de passe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void chx_savePW_CheckedChanged(object sender, EventArgs e)
        {
            SavePwd();
            Settings.Default.SavePW = chx_savePW.Checked;
        }
        
        /// <summary>
        /// Réception d'un message de la part du serveur SQL.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void con_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
        	rtb_log.Text += e.Message + "\n";
        	rtb_log.Refresh();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        
        private void LogExec (string command)
        {
        	foreach (string line in command.Split('\n'))
        	{
        		rtb_log.Text += "> " + line + "\n";
        	}
        	rtb_log.Refresh();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        
        private int ExecuteNonQuery(string sql)
        {
        	SqlCommand com = new SqlCommand(sql, con);
        	com.CommandTimeout = 86400;
        	bool wasClosed = false;
        	
        	if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed)
        	{
        		wasClosed = true;
        		con.Open();
        	}
        	
        	LogExec(sql);
        	int data = com.ExecuteNonQuery();
        	
        	if (true == wasClosed)
        	{
        		con.Close();
        	}
        	
        	return data;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        
        private object ExecuteScalar(string sql)
        {
        	SqlCommand com = new SqlCommand(sql, con);
        	bool wasClosed = false;
        	if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed)
        	{
        		wasClosed = true;
        		con.Open();
        	}
        	
        	LogExec(sql);
        	object data = com.ExecuteScalar();
        	
        	if (true == wasClosed)
        	{
        		con.Close();
        	}
        	
        	return data;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        void Btn_aotClick(object sender, EventArgs e)
        {
        	this.TopMost = (this.TopMost == false);
        	
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_main));
        	
        	if (this.TopMost == true)
        	{
        		btn_aot.Image = (System.Drawing.Image)(resources.GetObject("pined"));
        	}
        	else
        	{
        		btn_aot.Image = (System.Drawing.Image)(resources.GetObject("unpined"));
        	}
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        void Btn_optimizeClick(object sender, EventArgs e)
        {
        	if (!TestCS())
            {
                MessageBox.Show("Erreur lors de la purge. Vérifier les paramètres de connexion.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }
        	
        	ExecuteNonQuery(@"exec sp_msforeachdb '
use ?
declare @sql_index varchar(4000)

declare c cursor for
select  distinct ''alter index '' + i.name + '' on '' + s.name + ''.'' + t.name + '' rebuild''
from	sys.indexes i inner join
		sys.tables t on t.object_id = i.object_id inner join
		sys.schemas s on t.schema_id = s.schema_id
where	i.name is not null

open c

fetch next from c into @sql_index
while @@fetch_status = 0
begin
	if ''?'' not in (''master'', ''tempdb'', ''model'', ''msdb'')
	begin

		print (''? / '' + @sql_index)
		exec (@sql_index)
		
	end
	fetch next from c into @sql_index
end

close c
deallocate c'");
        }
    }
}
