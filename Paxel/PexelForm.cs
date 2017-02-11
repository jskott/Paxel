using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Pexel
{
    public partial class PexelForm : Form
    {
        private PexTable m_pexTable = new PexTable();

        public PexelForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Log files (*.mdb)|*.mdb|All files (*.*)|*.*";
            dlg.FilterIndex = 1;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = dlg.FileName;

                if(ConnectToMDBFile(path))
                {
                    Populate();
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSelectItemToExcelExport();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelectItemFromExcelExport();
        }

        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void OnFilterTextChanged(object sender, EventArgs e)
        {
            Populate();
        }

        private bool ValidConnection(OdbcConnection connection)
        {
            return connection != null && connection.State == ConnectionState.Open;
        }

        private string GetSqlCommand()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("    OGP.Name,");
            sb.AppendLine("    FPSet.Name,");
            sb.AppendLine("    Technique.Value,");
            sb.AppendLine("    OGP_kV.Value,");
            sb.AppendLine("    RADOGP_mAs.Value,");
            sb.AppendLine("    RADOGP_ms.Value,");
            sb.AppendLine("    Dose_Rad.Dose,");
            sb.AppendLine("    Focus.Name,");
            sb.AppendLine("    FilterType.Name,");
            sb.AppendLine("    ImageAmplification.Value,");
            sb.AppendLine("    RAD_OGP.ImageAutoamplification,");
            sb.AppendLine("    GradationParameter.Name,");
            sb.AppendLine("    SpatialFrequencyParameter.Name,");
            sb.AppendLine("    RAD_OGP.ImageWinCenter,");
            sb.AppendLine("    RAD_OGP.ImageWinWidth,");
            sb.AppendLine("    RAD_OGP.ImageWinAutowindowing,");
            sb.AppendLine("    OGP.Grid");
            sb.AppendLine("FROM(((((((((((OGP");
            sb.AppendLine("left join FPSet ON FPSet.ID = OGP.ID_FPSet)");
            sb.AppendLine("left join RAD_OGP ON RAD_OGP.ID = OGP.ID)");
            sb.AppendLine("left join Technique ON RAD_OGP.ID_Technique = Technique.ID)");
            sb.AppendLine("left join OGP_kV ON OGP.ID_kV = OGP_kV.ID)");
            sb.AppendLine("left join RADOGP_mAs ON RAD_OGP.ID_mAs = RADOGP_mAs.ID)");
            sb.AppendLine("left join RADOGP_ms ON RAD_OGP.ID_ms = RADOGP_ms.ID)");
            sb.AppendLine("left join Dose_Rad ON RAD_OGP.ID_Dose = Dose_Rad.ID)");
            sb.AppendLine("left join Focus ON OGP.ID_Focus = Focus.ID)");
            sb.AppendLine("left join FilterType ON OGP.ID_FilterType = FilterType.ID)");
            sb.AppendLine("left join ImageAmplification ON RAD_OGP.ID_ImageAmplification = ImageAmplification.ID)");
            sb.AppendLine("left join GradationParameter ON RAD_OGP.ID_ImageGradation = GradationParameter.IDs)");
            sb.AppendLine("left join SpatialFrequencyParameter ON OGP.ID_ImaSpatialFreqParam = SpatialFrequencyParameter.ID");

            return sb.ToString();
        }
        private void TransformData(OdbcConnection connection)
        {
            if(ValidConnection(connection))
            {

                OdbcCommand odbcCommand = new OdbcCommand(GetSqlCommand(), connection);

                try
                {
                    OdbcDataReader reader = odbcCommand.ExecuteReader();

                    m_pexTable = new PexTable();

                    while (reader.Read())
                    {
                        PexDataRow row = new PexDataRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string value = reader[i].ToString();

                            PexItem item = new PexItem();

                            item.DisplayName = value;
                            item.Key = value.ToLower();

                            row.Add(item);
                        }
                        m_pexTable.Add(row);
                    }

                    // Call Close when done reading.
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private bool SatisfyFilter(PexDataRow row)
        {
            string filter = m_filterEdit.Text.ToLower();

            bool ret = filter.Length == 0;

            if (!ret)
            {

                foreach (PexItem value in row)
                {
                    if (value.Key.Contains(filter))
                    {
                        ret = true;
                        break;

                    }
                }
            }
            return ret;
        }
        private void Populate()
        {
            m_mainListView.Items.Clear();

            foreach(PexDataRow row in m_pexTable)
            {
                if(SatisfyFilter(row))
                {
                    ListViewItem listItem = new ListViewItem();
                    bool first = true;
                    foreach (PexItem item in row)
                    {
                        if (first)
                        {
                            listItem.Text = item.DisplayName;
                            first = false;
                        }
                        else
                        {
                            listItem.SubItems.Add(item.DisplayName);
                        }
                    }

                    m_mainListView.Items.Add(listItem);
                }
            }

        }
        private void OpenLastMDBIfPossible()
        {
            string mdbPath = Properties.Settings.Default.LastMDBPath;

            if (mdbPath.Length > 0)
            {
                if (ConnectToMDBFile(mdbPath))
                {
                    Populate();
                }
                else
                {
                    Properties.Settings.Default.LastMDBPath = "";
                    Properties.Settings.Default.Save();
                }
            }
        }
        private bool ConnectToMDBFile(string path)
        {
            bool ret = false;
            if(File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"Driver={Microsoft Access Driver (*.mdb)}; Dbq=");
                sb.Append(path);
                sb.Append(@";Uid=Admin;Pwd=;");

                string connectString = sb.ToString();


                OdbcConnection connection = new OdbcConnection();

                connection.ConnectionString = connectString;

                try
                {
                    connection.Open();
                    TransformData(connection);
                    connection.Close();

                    Properties.Settings.Default.LastMDBPath = path;
                    Properties.Settings.Default.Save();
                    Text = "Pexcel - PEX to Excel - " + path;

                    ret = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


            return ret;
        }
        private void SetupColumns(ListView listView)
        {
            listView.Columns.Add("Namn");
            listView.Columns.Add("Flouro");
            listView.Columns.Add("Punkt");
            listView.Columns.Add("kV");
            listView.Columns.Add("mAs");
            listView.Columns.Add("ms");
            listView.Columns.Add("Dos");
            listView.Columns.Add("Fokus");
            listView.Columns.Add("Cu");
            listView.Columns.Add("Amp");
            listView.Columns.Add("Amp auto");
            listView.Columns.Add("LUT");
            listView.Columns.Add("SFP");
            listView.Columns.Add("WC");
            listView.Columns.Add("WW");
            listView.Columns.Add("Auto");
            listView.Columns.Add("Raster");
            listView.Columns.Add("Höjd");
            listView.Columns.Add("Bredd");
        }
        private void OnFormLoad(object sender, EventArgs e)
        {
            m_mainListView.ListViewItemSorter = new ListViewSorter();
            SetupColumns(m_mainListView);
            SetupColumns(m_exportToExcelList);
            OpenLastMDBIfPossible();
        }

        private void ExportToExcel()
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }


            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            for(int i = 0; i < m_exportToExcelList.Columns.Count; i++)
            {
                ColumnHeader columnHeader = m_exportToExcelList.Columns[i];

                xlWorkSheet.Cells[1, i + 1] = columnHeader.Text;
            }

            for(int i = 0; i < m_exportToExcelList.Items.Count; i++)
            {
                int row = i + 2;
                ListViewItem item = m_exportToExcelList.Items[i];

                for (int j = 0; j < item.SubItems.Count; j++)
                {
                    int column = j + 1;
                    xlWorkSheet.Cells[row, column] = item.SubItems[j].Text;

                }
            }

            xlApp.Visible = true;
            /*            
            xlWorkBook.SaveAs("d:\\csharp-Excel.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            */

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }

    private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
        }

        class PexItem
        {
            public string DisplayName { get; set; }
            public string Key { get; set; }
        }

        class PexDataRow : List<PexItem>
        {}

        class PexTable : List<PexDataRow>
        { }

        private void OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListViewSorter sorter = m_mainListView.ListViewItemSorter as ListViewSorter;

            if (sorter.m_column == e.Column)
            {
                sorter.m_ascending = !sorter.m_ascending;
            }
            else
            {
                sorter.m_ascending = true;
            }

            sorter.m_column = e.Column;

            m_mainListView.Sort();
        }

        private void OnGetVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {

        }

        public class FlickerFreeListView : ListView
        {
            public FlickerFreeListView()
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            }
            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                //SetWindowTheme(this.Handle, "explorer", null);
            }
        }

        private void AddSelectItemToExcelExport()
        {
            foreach(ListViewItem item in m_mainListView.SelectedItems)
            {
                ListViewItem newItem = item.Clone() as ListViewItem;

                m_exportToExcelList.Items.Add(newItem);
            }

            m_exportToExcelList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            m_exportToExcelList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }
        private void RemoveSelectItemFromExcelExport()
        {
            foreach (ListViewItem eachItem in m_exportToExcelList.SelectedItems)
            {
                m_exportToExcelList.Items.Remove(eachItem);
            }
        }
        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddSelectItemToExcelExport();
        }

        private void OnExportToExcelDoubleClick(object sender, MouseEventArgs e)
        {
            RemoveSelectItemFromExcelExport();
        }

        private void MoveSelection(bool up)
        {

        }
        private void OnFilterKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                MoveSelection(true);
            }
            if (e.KeyCode == Keys.Down)
            {
                MoveSelection(false);
            }
        }
    }
}
