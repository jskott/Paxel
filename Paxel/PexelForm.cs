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
using System.Data.OleDb;
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
        private TableByName m_tableByName = new TableByName();
        enum ViewType
        {
            OGP,
            SFP,
            DFR,
            FP
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

        private bool ValidConnection(OleDbConnection connection)
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
                    else if (ColumnExistInTable("Grid", "OGP"))
                    {
                        sb.AppendLine("    OGP.Grid,");
                    }
                    sb.AppendLine("    RAD_OGP.StandShutter2,");
                    sb.AppendLine("    RAD_OGP.StandShutter1");
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
                case ViewType.DFR:
                    sb.AppendLine("SELECT");
                    sb.AppendLine("OGP.Name,");
                    sb.AppendLine("FPSet.Name,");
                    sb.AppendLine("ID_DoseLevel,");
                    sb.AppendLine("kvauto,");
                    sb.AppendLine("OK1.Value,");
                    sb.AppendLine("CharacteristicCurve,");
                    sb.AppendLine("OK2.Value,");
                    sb.AppendLine("Focus.Name,");
                    sb.AppendLine("MaxPulseWidth,");
                    sb.AppendLine("BlackeningCorrection,");
                    if (ColumnExistInTable("Grid", "DFR_OGP"))
                    {
                        sb.AppendLine("Grid,");
                    }
                    else if (ColumnExistInTable("Grid", "OGP"))
                    {
                        sb.AppendLine("O2.Grid,");
                    }

                    sb.AppendLine("CollimationSizeY,");
                    sb.AppendLine("CollimationSizeX,");
                    sb.AppendLine("FilterType.Name,");
                    sb.AppendLine("SingleShot,");
                    sb.AppendLine("FixedFrameRate,");
                    sb.AppendLine("AR1.Value,");
                    sb.AppendLine("AR2.Value,");
                    sb.AppendLine("AR3.Value,");
                    sb.AppendLine("Autowindowing,");
                    sb.AppendLine("WidthFactor,");
                    sb.AppendLine("CenterShift,");
                    sb.AppendLine("Bandwidth,");
                    sb.AppendLine("Center,");
                    sb.AppendLine("Width,");
                    sb.AppendLine("SpatialFrequencyParameter.Name");
                    sb.AppendLine("FROM((((((((((DFR_OGP");
                    sb.AppendLine("left join OGP ON OGP.ID = DFR_OGP.ID)");
                    sb.AppendLine("left join FPSet ON FPSet.ID = OGP.ID_FPSet)");
                    sb.AppendLine("left join OGP_kV AS OK1 ON OK1.ID = OGP.ID_kV)");
                    sb.AppendLine("left join OGP_kV AS OK2 ON OK2.ID = DFR_OGP.ID_DoseReduction)");
                    sb.AppendLine("left join Focus ON Focus.ID = OGP.ID_Focus)");
                    sb.AppendLine("left join OGP AS O2 ON O2.ID = DFR_OGP.ID)");
                    sb.AppendLine("left join FilterType ON FilterType.ID = OGP.ID_FilterType)");
                    sb.AppendLine("left join AcquisitionRate AS AR1 ON AR1.ID = DFR_OGP.ID_AcquisitionRate1)");
                    sb.AppendLine("left join AcquisitionRate AS AR2 ON AR2.ID = DFR_OGP.ID_AcquisitionRate2)");
                    sb.AppendLine("left join AcquisitionRate AS AR3 ON AR3.ID = DFR_OGP.ID_AcquisitionRate3)");
                    sb.AppendLine("left join SpatialFrequencyParameter ON SpatialFrequencyParameter.ID = OGP.ID_ImaSpatialFreqParam");
                    break;
                case ViewType.FP:
                    sb.AppendLine("SELECT");
                    sb.AppendLine("FPSet.Name,");
                    sb.AppendLine("FluoroMode,");
                    sb.AppendLine("FrameRate.Value,");
                    sb.AppendLine("DoseLevel_FP.Value,");
                    sb.AppendLine("DoseRateIndex,");
                    sb.AppendLine("FluoroCurve.Name,");
                    sb.AppendLine("FluoroFilterAuto,");
                    sb.AppendLine("FilterType.Name,");
                    sb.AppendLine("NoiseReduction.Value,");
                    sb.AppendLine("ID_ImaSpatialFreqParam,");
                    sb.AppendLine("WindowCenter,");
                    sb.AppendLine("WindowWidth,");
                    sb.AppendLine("Autowindowing,");
                    sb.AppendLine("WidthFactor,");
                    sb.AppendLine("CenterShift,");
                    sb.AppendLine("Bandwidth,");
                    sb.AppendLine("Default");
                    sb.AppendLine("FROM(((((FluoroProgram");
                    sb.AppendLine("inner join FPSet ON FPSet.ID = FluoroProgram.ID_FPSet)");
                    sb.AppendLine("inner join DoseLevel_FP ON DoseLevel_FP.ID = FluoroProgram.ID_DoseLevel)");
                    sb.AppendLine("inner join FluoroCurve ON FluoroCurve.ID = FluoroProgram.ID_FluoroCurve)");
                    sb.AppendLine("inner join FilterType ON FilterType.ID = FluoroProgram.ID_FilterType)");
                    sb.AppendLine("inner join NoiseReduction ON NoiseReduction.ID = FluoroProgram.ID_NoiseReduction)");
                    sb.AppendLine("inner join FrameRate ON FrameRate.ID = FluoroProgram.ID_FrameRate");
                    break;

            }


            return sb.ToString();
        }

        private void PopulateTablesByColumn(OleDbConnection connection)
        {
            m_tableByColumn = new TableByColumn();
            using (DataTable tableschema = connection.GetSchema("COLUMNS"))
            {
                // first column name
                foreach (DataRow row in tableschema.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    string columnName = row["COLUMN_NAME"].ToString();


                    if (!m_tableByColumn.ContainsKey(columnName))
                    {
                        m_tableByColumn[columnName] = new StringSet();
                    }

                    m_tableByColumn[columnName].Add(tableName);

                    Table table = null;
                    if (!m_tableByName.TryGetValue(tableName, out table))
                    {
                        table = new Table();
                        table.Name = tableName;
                        m_tableByName[tableName] = table;
                    }

                    table.Columns.Add(columnName);
                }
            }

            PopulateForeignKeys(connection);

        }
        // Retrieve the list of a table's foreign keys.
        private void PopulateForeignKeys(OleDbConnection connection)
        {
            String[] restrictions = new string[] { null };
            DataTable schema;
            // Open the schema information for the foreign keys.
            schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, restrictions);
            // Enumerate the table's foreign keys.
            foreach (DataRow row in schema.Rows)
            {
                string fkTableName = row["FK_TABLE_NAME"].ToString();
                string fkColumnName = row["FK_COLUMN_NAME"].ToString();
                string pkTableName = row["PK_TABLE_NAME"].ToString();
                string pkColumnName = row["PK_COLUMN_NAME"].ToString();

                Relationship relationship = new Relationship();

                relationship.ForeignKey = fkColumnName;
                Table table = null;
                if (m_tableByName.TryGetValue(fkTableName, out table))
                {
                    relationship.ForeignTable = table;
                }
                if (m_tableByName.TryGetValue(pkTableName, out table))
                {
                    relationship.PrimaryTable = table;
                }
                relationship.PrimaryKey = pkColumnName;

                if (relationship.ForeignTable != null && relationship.PrimaryTable != null)
                {
                    relationship.ForeignTable.Relations.Add(relationship);
                }

                //Console.WriteLine(row["FK_TABLE_NAME"].ToString() + ":" + row["FK_COLUMN_NAME"].ToString() + " --> " + row["PK_TABLE_NAME"].ToString() + ":" + row["PK_COLUMN_NAME"].ToString() + " " + row["FK_NAME"].ToString());
            }

            /*
            foreach (Table table in m_tableByName.Values)
            {
                Console.WriteLine(table.Name);

                foreach (string column in table.FlatColumns())
                {
                    Console.WriteLine("\t" + column);
                }
            }
            */
        }

        private bool ColumnExistInTable(string column, string table)
        {
            return m_tableByColumn.Exists(column, table);
        }
        private void TransformData(OleDbConnection connection)
        {
            if (ValidConnection(connection))
            {
                PopulateTablesByColumn(connection);
                OleDbCommand odbcCommand = new OleDbCommand(GetSqlCommand(), connection);

                try
                {
                    OleDbDataReader reader = odbcCommand.ExecuteReader();

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
            switch (m_viewType)
            {
                case ViewType.OGP:
                    m_organProgramsLabel.Text = "Organ Parameters";
                    m_exportToExcelLabel.Text = "Export Organ Parameters to Excel";
                    break;
                case ViewType.SFP:
                    m_organProgramsLabel.Text = "Spatial Frequency Parameters";
                    m_exportToExcelLabel.Text = "Export Spatial Frequency Parameters to Excel";
                    break;
                case ViewType.DFR:
                    m_organProgramsLabel.Text = "Digital Flouro Radiography";
                    m_exportToExcelLabel.Text = "Export Digital Flouro Radiography to Excel";
                    break;
                case ViewType.FP:
                    m_organProgramsLabel.Text = "Flouro Program";
                    m_exportToExcelLabel.Text = "Export Flouro Program to Excel";
                    break;
            }
        }
        private void InitialPopulate()
        {
            SetupLabels();

            organParameterToolStripMenuItem.Checked = m_viewType == ViewType.OGP;
            spatialFrequencyParameterToolStripMenuItem.Checked = m_viewType == ViewType.SFP;
            digitalFlouroRadiographyToolStripMenuItem.Checked = m_viewType == ViewType.DFR;
            flouroProgramToolStripMenuItem.Checked = m_viewType == ViewType.FP;

            SetupColumns(m_mainListView);
            SetupColumns(m_exportToExcelList);

            string mdbPath = Properties.Settings.Default.LastMDBPath;

            if (mdbPath.Length > 0)
            {
                if (ConnectToMDBFile(mdbPath))
                {
                    SuspendRedraw(m_mainListView);
                    Populate();
                    m_mainListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    m_mainListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    ResumeRedraw(m_mainListView);
                }
                else
                {
                    Properties.Settings.Default.LastMDBPath = "";
                    Properties.Settings.Default.Save();
                }
            }

        }

        private const int WM_SETREDRAW = 0x000B;

        public static void SuspendRedraw(Control control)
        {
            Message msgSuspendUpdate = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero,
                IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        public static void ResumeRedraw(Control control)
        {
            // Create a C "true" boolean as an IntPtr
            IntPtr wparam = new IntPtr(1);
            Message msgResumeUpdate = Message.Create(control.Handle, WM_SETREDRAW, wparam,
                IntPtr.Zero);

            NativeWindow window = NativeWindow.FromHandle(control.Handle);
            window.DefWndProc(ref msgResumeUpdate);

            control.Invalidate();
        }
        private void Populate()
        {
            m_visibleRows.Clear();
            foreach (PexDataRow row in m_pexTable)
            {
                if (SatisfyFilter(row))
                {
                    m_visibleRows.Add(row);
                }
            }

            m_mainListView.VirtualListSize = m_visibleRows.Count;

        }

        private bool ConnectToMDBFile(string path)
        {
            bool ret = false;
            if (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=");
                sb.Append(path);

                string connectString = sb.ToString();


                OleDbConnection connection = new OleDbConnection();

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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


            return ret;
        }
        private void SetupColumns(ListView listView)
        {
            listView.Columns.Clear();

            switch (m_viewType)
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
                    m_sorter = new PexTableComparer();
                    break;
                case ViewType.SFP:
                    listView.Columns.Add("Namn");
                    listView.Columns.Add("DV");
                    listView.Columns.Add("EK");
                    listView.Columns.Add("EG");
                    listView.Columns.Add("HK");
                    listView.Columns.Add("HG");
                    m_sorter = new PexTableComparer();
                    break;
                case ViewType.DFR:
                    listView.Columns.Add("Namn");
                    listView.Columns.Add("Flouro");
                    listView.Columns.Add("Dos per puls");
                    listView.Columns.Add("Auto kV");
                    listView.Columns.Add("kV");
                    listView.Columns.Add("C-Curve");
                    listView.Columns.Add("Dos Reduction");
                    listView.Columns.Add("Focus");
                    listView.Columns.Add("Max Pulse Width");
                    listView.Columns.Add("BC");
                    listView.Columns.Add("Raster");
                    listView.Columns.Add("Höjd");
                    listView.Columns.Add("Bredd");
                    listView.Columns.Add("Cufilter");
                    listView.Columns.Add("Single");
                    listView.Columns.Add("FixedFrameRate");
                    listView.Columns.Add("FR1");
                    listView.Columns.Add("FR2");
                    listView.Columns.Add("FR3");
                    listView.Columns.Add("Autowindowing");
                    listView.Columns.Add("WF");
                    listView.Columns.Add("CS");
                    listView.Columns.Add("Bandwidth");
                    listView.Columns.Add("WC");
                    listView.Columns.Add("WW");
                    listView.Columns.Add("SFP");
                    m_sorter = new PexTableComparer();
                    break;
                case ViewType.FP:
                    listView.Columns.Add("Namn");
                    listView.Columns.Add("Mode");
                    listView.Columns.Add("P/S");
                    listView.Columns.Add("Dose Level");
                    listView.Columns.Add("Dose Rate Index");
                    listView.Columns.Add("Flouro Curve");
                    listView.Columns.Add("Flouro Filter Auto");
                    listView.Columns.Add("K Factor");
                    listView.Columns.Add("SFP");
                    listView.Columns.Add("WC");
                    listView.Columns.Add("WW");
                    listView.Columns.Add("Auto W");
                    listView.Columns.Add("WF");
                    listView.Columns.Add("CS");
                    listView.Columns.Add("Bandwidth");
                    m_sorter = new PexTableComparer();
                    m_sorter.UpdateSortColumn(1, true);
                    m_sorter.UpdateSortColumn(0, true);
                    break;
            }
        }
        private void OnFormLoad(object sender, EventArgs e)
        {
            List<int> initialSortColumn = new List<int>();
            initialSortColumn.Add(0);
            initialSortColumn.Add(1);

            m_mainListView.ListViewItemSorter = new ListViewSorter(initialSortColumn);
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

            for (int i = 0; i < m_exportToExcelList.Columns.Count; i++)
            {
                ColumnHeader columnHeader = m_exportToExcelList.Columns[i];

                xlWorkSheet.Cells[1, i + 1] = columnHeader.Text;
            }

            for (int i = 0; i < m_exportToExcelList.Items.Count; i++)
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


        private void OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            ColumnSorter column = m_sorter.CurrentSortColumn();
               
            if (column.Column == e.Column)
            {
                column.Ascending = !column.Ascending;
            }
            else
            {
                m_sorter.UpdateSortColumn(e.Column, true);
            }


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
            SuspendRedraw(m_exportToExcelList);
            foreach (int index in m_mainListView.SelectedIndices)
            {
                ListViewItem newItem = ListItemFromPexRow(m_visibleRows[index]);

                m_exportToExcelList.Items.Add(newItem);
            }

            if (m_exportToExcelList.Items.Count == 1)
            {
                m_exportToExcelList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                m_exportToExcelList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            ResumeRedraw(m_exportToExcelList);
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
            if (e.KeyCode == Keys.Up)
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

        private void digitalFlouroRadiographyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_viewType = ViewType.DFR;
            InitialPopulate();
        }
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialPopulate();
        }

        private void columnConfiguratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColumnSetupForm columnSetupForm = new ColumnSetupForm(m_tableByName);

            columnSetupForm.ShowDialog();
        }

        private void flouroProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_viewType = ViewType.FP;
            InitialPopulate();
        }
    }
}
