using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pexel
{
    public partial class ConvertPexelDatabase : Form
    {
        string m_sourcePath = "";

        public ConvertPexelDatabase(string sourcePath)
        {
            m_sourcePath = sourcePath;
            InitializeComponent();
        }

        private void m_browseButton_Click(object sender, EventArgs e)
        {

        }
        private OleDbConnection GetSourceConnection()
        {

            try
            {

                StringBuilder sb = new StringBuilder();

                sb.Append(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=");
                sb.Append(m_sourcePath);

                string connectString = sb.ToString();


                OleDbConnection connection = new OleDbConnection();

                connection.ConnectionString = connectString;

                return connection;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }
        private void Convert(OleDbConnection sourceConnection, SQLiteConnection destinationConnection)
        {

        }
        private void m_convertButton_Click(object sender, EventArgs e)
        {
            OleDbConnection sourceConnection = GetSourceConnection();

            sourceConnection.Open();

            string basePath = @"pex_base_db.sqlite";
            if (File.Exists(basePath))
            {
                string path = m_pathTextBox.Text;


                File.Copy(basePath, path, true);
                StringBuilder sb = new StringBuilder();

                sb.Append(@"Data Source=");
                sb.Append(path);

                string connectString = sb.ToString();

                SQLiteConnection connection = new SQLiteConnection(connectString);

                connection.Open();

                connection.Close();
            }


            sourceConnection.Close();

        }
    }
}
