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
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

namespace Pexel
{
    public partial class PexelForm : Form
    {
        private PexTable m_pexTable = new PexTable();
        private PexTable m_visibleRows = new PexTable();
        private PexTableComparer m_sorter = new PexTableComparer();
        private ViewType m_viewType = ViewType.OGP;
        private TableByColumn m_tableByColumn = null;

        enum ViewType
        {
            OGP,
            SFP
        }

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
                Properties.Settings.Default.LastMDBPath = dlg.FileName;

                InitialPopulate();
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

            switch (m_viewType)
            {
                case ViewType.OGP:
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
                    if (ColumnExistInTable("StandGrid", "RAD_OGP"))
                    {
                        sb.AppendLine("    RAD_OGP.StandGrid,");
                    }
                    else if(ColumnExistInTable("Grid", "OGP"))
                    {
                        sb.AppendLine("    OGP.Grid,");
                    }
                    sb.AppendLine("    RAD_OGP.StandShutter1,");
                    sb.AppendLine("    RAD_OGP.StandShutter2");
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
                    break;
                case ViewType.SFP:
                    sb.AppendLine("SELECT");
                    sb.AppendLine("    SpatialFrequencyParameter.Name,");
                    sb.AppendLine("    DiamondViewID.Name,");
                    sb.AppendLine("    EdgeFilterKernel.Value,");
                    sb.AppendLine("    SpatialFrequencyParameter.EdgeFilterGain,");
                    sb.AppendLine("    HarmonisKernel.Value,");
                    sb.AppendLine("    SpatialFrequencyParameter.HarmonisGain");
                    sb.AppendLine("    FROM((SpatialFrequencyParameter");
                    sb.AppendLine("left join DiamondViewID ON SpatialFrequencyParameter.ID_DiamondViewID = DiamondViewID.ID)");
                    sb.AppendLine("left join EdgeFilterKernel ON SpatialFrequencyParameter.ID_EdgeFilterKernel = EdgeFilterKernel.ID)");
                    sb.AppendLine("left join HarmonisKernel ON SpatialFrequencyParameter.ID_HarmonisKernel = HarmonisKernel.ID");
                    break;
            }
                    

            return sb.ToString();
        }

        private void PopulateTablesByColumn(OdbcConnection connection)
        {
            m_tableByColumn = new TableByColumn();
            using (DataTable tableschema = connection.GetSchema("COLUMNS"))
            {
                // first column name
                foreach (DataRow row in tableschema.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    string columnName = row["COLUMN_NAME"].ToString();

                    if(!m_tableByColumn.ContainsKey(columnName))
                    {
                        m_tableByColumn[columnName] = new StringSet();
                    }

                    m_tableByColumn[columnName].Add(tableName);
                }
            }
        }

        private bool ColumnExistInTable(string column, string table)
        {
            return m_tableByColumn.Exists(column, table);
        }
        private void TransformData(OdbcConnection connection)
        {
            if(ValidConnection(connection))
            {
                PopulateTablesByColumn(connection);
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

                    m_pexTable.Sort(m_sorter);

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
                Regex regex = new Regex(filter);

                foreach (PexItem value in row)
                {
                    if (regex.IsMatch(value.Key))
                    {
                        ret = true;
                        break;

                    }
                }
            }
            return ret;
        }
        ListViewItem ListItemFromPexRow(PexDataRow pexRow)
        {
            ListViewItem listItem = new ListViewItem();
            bool first = true;
            foreach (PexItem item in pexRow)
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
            listItem.SubItems.Add("");
            listItem.SubItems.Add("");

            return listItem;
        }
        private void SetupLabels()
        {
            switch(m_viewType)
            {
                case ViewType.OGP:
                    m_organProgramsLabel.Text = "Organ Parameters";
                    m_exportToExcelLabel.Text = "Export Organ Parameters to Excel";
                    break;
                case ViewType.SFP:
                    m_organProgramsLabel.Text = "Spatial Frequency Parameters";
                    m_exportToExcelLabel.Text = "Export Spatial Frequency Parameters to Excel";
                    break;
            }
        }
        private void InitialPopulate()
        {
            SetupLabels();

            organParameterToolStripMenuItem.Checked = m_viewType == ViewType.OGP;
            spatialFrequencyParameterToolStripMenuItem.Checked = m_viewType == ViewType.SFP;

            SetupColumns(m_mainListView);
            SetupColumns(m_exportToExcelList);

            string mdbPath = Properties.Settings.Default.LastMDBPath;

            if (mdbPath.Length > 0)
            {
                if (ConnectToMDBFile(mdbPath))
                {
                    Populate();
                    m_mainListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    m_mainListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
                else
                {
                    Properties.Settings.Default.LastMDBPath = "";
                    Properties.Settings.Default.Save();
                }
            }

        }
        private void Populate()
        {
            m_visibleRows.Clear();
            foreach(PexDataRow row in m_pexTable)
            {
                if(SatisfyFilter(row))
                {
                    m_visibleRows.Add(row);
                }
            }

            m_mainListView.VirtualListSize = m_visibleRows.Count;

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
            listView.Columns.Clear();

            switch(m_viewType)
            {
                case ViewType.OGP:
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
                    break;
                case ViewType.SFP:
                    listView.Columns.Add("Namn");
                    listView.Columns.Add("DV");
                    listView.Columns.Add("EK");
                    listView.Columns.Add("EG");
                    listView.Columns.Add("HK");
                    listView.Columns.Add("HG");
                    break;
            }           
        }
        private void OnFormLoad(object sender, EventArgs e)
        {
            m_mainListView.ListViewItemSorter = new ListViewSorter();
            InitialPopulate();
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
        {}

        class StringSet : Dictionary<string, int>
        {
            public void Add(string value)
            {
                this[value] = 0;
            }
        }
        class TableByColumn : Dictionary<string, StringSet>
        {
            public bool Exists(string column, string table)
            {
                bool ret = false;

                if (ContainsKey(column))
                {
                    StringSet tables = this[column];

                    if (tables != null)
                    {
                        ret = tables.ContainsKey(table);
                    }
                }
                return ret;
            }
        }

        class PexTableComparer : IComparer<PexDataRow>
        {
            public int m_column = 0;
            public bool m_ascending = true;

            private CaseInsensitiveComparer m_objectCompare;
            public PexTableComparer()
            {
                m_column = 0;

                // Initialize the CaseInsensitiveComparer object
                m_objectCompare = new CaseInsensitiveComparer();
            }

            public int Compare(PexDataRow row1, PexDataRow row2)
            {
                int compareResult;

                compareResult = m_objectCompare.Compare(row1[m_column].DisplayName, row2[m_column].DisplayName);

                return m_ascending ? compareResult : -compareResult;
            }
        }

        private void OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            
            if (m_sorter.m_column == e.Column)
            {
                m_sorter.m_ascending = !m_sorter.m_ascending;
            }
            else
            {
                m_sorter.m_ascending = true;
            }

            m_sorter.m_column = e.Column;

            m_visibleRows.Sort(m_sorter);
            m_pexTable.Sort(m_sorter);
            m_mainListView.Invalidate();
        }

        private void OnGetVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            PexDataRow row = m_visibleRows[e.ItemIndex];
            e.Item = ListItemFromPexRow(row);
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
            }
        }

        private void AddSelectItemToExcelExport()
        {
            foreach(int index in m_mainListView.SelectedIndices)
            {
                ListViewItem newItem = ListItemFromPexRow(m_visibleRows[index]);

                m_exportToExcelList.Items.Add(newItem);
            }

            if(m_exportToExcelList.Items.Count == 1)
            {
                m_exportToExcelList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                m_exportToExcelList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
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

        private void organParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_viewType = ViewType.OGP;

            InitialPopulate();

        }

        private void spatialFrequencyParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_viewType = ViewType.SFP;

            InitialPopulate();

        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialPopulate();
        }
    }
}
