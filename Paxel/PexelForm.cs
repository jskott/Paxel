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
using System.Data.SQLite;

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
        private NameByTypeAndIndex m_nameByTypeAndIndex = new NameByTypeAndIndex();

        public PexelForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Database file (*.mdb;*.sqlite)|*.mdb;*.sqlite|All files (*.*)|*.*";
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
        private bool ValidConnection(SQLiteConnection connection)
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
                    if (ColumnExistInTable("ImageAmplification", "RAD_OGP"))
                    {
                        sb.AppendLine("    ImageAmplification,");
                    }
                    else if (ColumnExistInTable("Grid", "OGP"))
                    {
                        sb.AppendLine("    ImageAmplification.Value,");
                    }
                    sb.AppendLine("    RAD_OGP.ImageAutoamplification,");
                    sb.AppendLine("    GradationParameter.Name,");
                    sb.AppendLine("    SpatialFrequencyParameter.Name,");
                    sb.AppendLine("    EXI_Parameter.Name,");
                    sb.AppendLine("    RAD_OGP.ImageWinAutowindowing,");
                    sb.AppendLine("    ImageWinWidthFactor,");
                    sb.AppendLine("    ImageWinCenterShift,");
                    sb.AppendLine("    RAD_OGP.ImageWinCenter,");
                    sb.AppendLine("    RAD_OGP.ImageWinWidth,");
                    sb.AppendLine("    OGP.ViewFlip1,");
                    sb.AppendLine("    OGP.ViewFlip2,");
                    sb.AppendLine("    ViewRotate,");

                    if (ColumnExistInTable("StandGrid", "RAD_OGP"))
                    {
                        sb.AppendLine("    RAD_OGP.StandGrid,");
                    }
                    else if (ColumnExistInTable("Grid", "OGP"))
                    {
                        sb.AppendLine("    OGP.Grid,");
                    }
                    sb.AppendLine("    RAD_OGP.StandShutter2,");
                    sb.AppendLine("    RAD_OGP.StandShutter1,");
                    sb.AppendLine("    StandPosition.Name");
                    sb.AppendLine("FROM((((((((((((");
                    if (!ColumnExistInTable("ImageAmplification", "RAD_OGP"))
                    {
                        sb.Append("(");
                    }
                    sb.AppendLine("OGP");
                    sb.AppendLine("left join FPSet ON FPSet.ID = OGP.ID_FPSet)");
                    sb.AppendLine("left join RAD_OGP ON RAD_OGP.ID = OGP.ID)");
                    sb.AppendLine("left join Technique ON RAD_OGP.ID_Technique = Technique.ID)");
                    sb.AppendLine("left join OGP_kV ON OGP.ID_kV = OGP_kV.ID)");
                    sb.AppendLine("left join RADOGP_mAs ON RAD_OGP.ID_mAs = RADOGP_mAs.ID)");
                    sb.AppendLine("left join RADOGP_ms ON RAD_OGP.ID_ms = RADOGP_ms.ID)");
                    sb.AppendLine("left join Dose_Rad ON RAD_OGP.ID_Dose = Dose_Rad.ID)");
                    sb.AppendLine("left join Focus ON OGP.ID_Focus = Focus.ID)");
                    sb.AppendLine("left join FilterType ON OGP.ID_FilterType = FilterType.ID)");
                    if (!ColumnExistInTable("ImageAmplification", "RAD_OGP"))
                    {
                        sb.AppendLine("left join ImageAmplification ON RAD_OGP.ID_ImageAmplification = ImageAmplification.ID)");
                    }
                    sb.AppendLine("left join GradationParameter ON RAD_OGP.ID_ImageGradation = GradationParameter.IDs)");
                    sb.AppendLine("left join SpatialFrequencyParameter ON OGP.ID_ImaSpatialFreqParam = SpatialFrequencyParameter.ID)");
                    sb.AppendLine("left join EXI_Parameter ON RAD_OGP.ID_EXI_Parameter = EXI_Parameter.ID)");
                    sb.AppendLine("left join StandPosition ON OGP.ID_StandPosition = StandPosition.ID");
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
                    sb.AppendLine("SpatialFrequencyParameter.Name,");
                    sb.AppendLine("WindowCenter,");
                    sb.AppendLine("WindowWidth,");
                    sb.AppendLine("Autowindowing,");
                    sb.AppendLine("WidthFactor,");
                    sb.AppendLine("CenterShift,");
                    sb.AppendLine("Bandwidth,");
                    sb.AppendLine("Default");
                    sb.AppendLine("FROM((((((FluoroProgram");
                    sb.AppendLine("left join FPSet ON FPSet.ID = FluoroProgram.ID_FPSet)");
                    sb.AppendLine("left join DoseLevel_FP ON DoseLevel_FP.ID = FluoroProgram.ID_DoseLevel)");
                    sb.AppendLine("left join FluoroCurve ON FluoroCurve.ID = FluoroProgram.ID_FluoroCurve)");
                    sb.AppendLine("left join FilterType ON FilterType.ID = FluoroProgram.ID_FilterType)");
                    sb.AppendLine("left join NoiseReduction ON NoiseReduction.ID = FluoroProgram.ID_NoiseReduction)");
                    sb.AppendLine("left join FrameRate ON FrameRate.ID = FluoroProgram.ID_FrameRate)");
                    sb.AppendLine("left join SpatialFrequencyParameter ON FluoroProgram.ID_ImaSpatialFreqParam = SpatialFrequencyParameter.ID");
                    break;
                case ViewType.SP:
                    sb.AppendLine("SELECT");
                    sb.AppendLine("Name,");
                    sb.AppendLine("Identifier");
                    sb.AppendLine("FROM StandPosition");
                    break;


            }


            return sb.ToString();
        }

        private void PopulateTablesByColumn(SQLiteConnection connection)
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

            //PopulateForeignKeys(connection);

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
        private void PopulateForeignKeys(SQLiteConnection connection)
        {
            String[] restrictions = new string[] { null };
            DataTable schema;
            // Open the schema information for the foreign keys.
            schema = connection.GetSchema("ForeignKeys", restrictions);
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
            }
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
            }
        }

        private bool ColumnExistInTable(string column, string table)
        {
            return m_tableByColumn.Exists(column, table);
        }
        private object TransformRotation(object value)
        {
            int i = Convert.ToInt32(value);
            switch (i)
            {
                case 1:
                    value = 0;
                    break;
                case 2:
                    value = 90;
                    break;
                case 3:
                    value = 180;
                    break;
                case 4:
                    value = 270;
                    break;

            }

            return value;
        }
        object DefaultTransformer(object value)
        {
            return value;
        }
        private object TransformMAS(object value)
        {
            int i = Convert.ToInt32(value);
            value = i / 100.0;

            return value;
        }
        private object TransformKV(object value)
        {
            int i = Convert.ToInt32(value);
            value = i / 10;

            return value;
        }
        private object TransformIfApplicable(object value, int index)
        {
            if(value != DBNull.Value)
            { 
                if(value.GetType() == typeof(short))
                {
                    value = Convert.ToInt32(value);
                }
                Func<object, object> func = m_nameByTypeAndIndex[m_viewType][index];

                value = func(value);
            }

            return value;
        }
        private void TransformData(SQLiteConnection connection)
        {
            if (ValidConnection(connection))
            {
                PopulateTablesByColumn(connection);
                SQLiteCommand sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = GetSqlCommand();

                try
                {

                    SQLiteDataReader reader = sqliteCommand.ExecuteReader();

                    m_pexTable = new PexTable();

                    while (reader.Read())
                    {
                        PexDataRow row = new PexDataRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object value = TransformIfApplicable(reader[i], i);
                            string stringValue = value.ToString();

                            PexItem item = new PexItem();

                            item.DisplayName = stringValue.ToString();
                            item.FilterKey = stringValue.ToLower();
                            item.RawData = value;

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
                            object value = TransformIfApplicable(reader[i], i);
                            string stringValue = value.ToString();

                            PexItem item = new PexItem();

                            item.DisplayName = stringValue.ToString();
                            item.FilterKey = stringValue.ToLower();
                            item.RawData = value;

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
                    if (regex.IsMatch(value.FilterKey))
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
                    listItem.SubItems[0].Tag = item;
                    first = false;
                }
                else
                {
                    ListViewItem.ListViewSubItem subItem = listItem.SubItems.Add(item.DisplayName);

                    subItem.Tag = item;
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
                case ViewType.SP:
                    m_organProgramsLabel.Text = "Stand Position";
                    m_exportToExcelLabel.Text = "Export Stand Position to Excel";
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
            standPositionToolStripMenuItem.Checked = m_viewType == ViewType.SP;

            SetupColumns(m_mainListView);
            SetupColumns(m_exportToExcelList);

            string path = Properties.Settings.Default.LastMDBPath;

            if (path.Length > 0)
            {
                string extension = Path.GetExtension(path);

                bool ok = false;

                if(extension == ".mdb")
                {
                    ok = ConnectToMDBFile(path);
                }
                else if(extension == ".sqlite")
                {
                    ok = ConnectToSQLiteFile(path);
                }
                if (ok)
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
        private bool ConnectToSQLiteFile(string path)
        {
            bool ret = false;
            if (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"Data Source=");
                sb.Append(path);

                string connectString = sb.ToString();


                SQLiteConnection connection = new SQLiteConnection(connectString);


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

        void AddColumn(ViewType viewType, ListView listView, string name, Func<object, object> transformFunc = null)
        {
            if(!m_nameByTypeAndIndex.ContainsKey(viewType))
            {
                m_nameByTypeAndIndex[viewType] = new Dictionary<int, Func<object, object>>();
            }

            int index = listView.Columns.Count;
            m_nameByTypeAndIndex[viewType][index] = transformFunc == null ? DefaultTransformer : transformFunc;
            listView.Columns.Add(name);
        }
        private void SetupColumns(ListView listView)
        {
            listView.Columns.Clear();

            switch (m_viewType)
            {
                case ViewType.OGP:
                    AddColumn(m_viewType, listView, "Namn");
                    AddColumn(m_viewType, listView, "Flouro");
                    AddColumn(m_viewType, listView, "Punkt");
                    AddColumn(m_viewType, listView, "kV", TransformKV);
                    AddColumn(m_viewType, listView, "mAs", TransformMAS);
                    AddColumn(m_viewType, listView, "ms");
                    AddColumn(m_viewType, listView, "Dos");
                    AddColumn(m_viewType, listView, "Fokus");
                    AddColumn(m_viewType, listView, "Cu");
                    AddColumn(m_viewType, listView, "Amp");
                    AddColumn(m_viewType, listView, "Amp auto");
                    AddColumn(m_viewType, listView, "LUT");
                    AddColumn(m_viewType, listView, "SFP");
                    AddColumn(m_viewType, listView, "EXI");

                    AddColumn(m_viewType, listView, "Auto");

                    AddColumn(m_viewType, listView, "WF");
                    AddColumn(m_viewType, listView, "CS");

                    AddColumn(m_viewType, listView, "WC");
                    AddColumn(m_viewType, listView, "WW");

                    AddColumn(m_viewType, listView, "Vertikal");
                    AddColumn(m_viewType, listView, "Horisontell");
                    AddColumn(m_viewType, listView, "Rotation", TransformRotation);

                    AddColumn(m_viewType, listView, "Raster");
                    AddColumn(m_viewType, listView, "Höjd");
                    AddColumn(m_viewType, listView, "Bredd");
                    AddColumn(m_viewType, listView, "Plats");



                    m_sorter = new PexTableComparer();
                    break;
                case ViewType.SFP:
                    AddColumn(m_viewType, listView, "Namn");
                    AddColumn(m_viewType, listView, "DV");
                    AddColumn(m_viewType, listView, "EK");
                    AddColumn(m_viewType, listView, "EG");
                    AddColumn(m_viewType, listView, "HK");
                    AddColumn(m_viewType, listView, "HG");
                    m_sorter = new PexTableComparer();
                    break;
                case ViewType.DFR:
                    AddColumn(m_viewType, listView, "Namn");
                    AddColumn(m_viewType, listView, "Flouro");
                    AddColumn(m_viewType, listView, "Dos per puls");
                    AddColumn(m_viewType, listView, "Auto kV");
                    AddColumn(m_viewType, listView, "kV", TransformRotation);
                    AddColumn(m_viewType, listView, "C-Curve");
                    AddColumn(m_viewType, listView, "Dos Reduction");
                    AddColumn(m_viewType, listView, "Focus");
                    AddColumn(m_viewType, listView, "Max Pulse Width");
                    AddColumn(m_viewType, listView, "BC");
                    AddColumn(m_viewType, listView, "Raster");
                    AddColumn(m_viewType, listView, "Höjd");
                    AddColumn(m_viewType, listView, "Bredd");
                    AddColumn(m_viewType, listView, "Cufilter");
                    AddColumn(m_viewType, listView, "Single");
                    AddColumn(m_viewType, listView, "FixedFrameRate");
                    AddColumn(m_viewType, listView, "FR1");
                    AddColumn(m_viewType, listView, "FR2");
                    AddColumn(m_viewType, listView, "FR3");
                    AddColumn(m_viewType, listView, "Autowindowing");
                    AddColumn(m_viewType, listView, "WF");
                    AddColumn(m_viewType, listView, "CS");
                    AddColumn(m_viewType, listView, "Bandwidth");
                    AddColumn(m_viewType, listView, "WC");
                    AddColumn(m_viewType, listView, "WW");
                    AddColumn(m_viewType, listView, "SFP");
                    m_sorter = new PexTableComparer();
                    break;
                case ViewType.FP:
                    AddColumn(m_viewType, listView, "Namn");
                    AddColumn(m_viewType, listView, "Mode");
                    AddColumn(m_viewType, listView, "P/S");
                    AddColumn(m_viewType, listView, "Dose Level");
                    AddColumn(m_viewType, listView, "Dose Rate Index");
                    AddColumn(m_viewType, listView, "Flouro Curve");
                    AddColumn(m_viewType, listView, "Flouro Filter Auto");
                    AddColumn(m_viewType, listView, "Cu");
                    AddColumn(m_viewType, listView, "K Factor");
                    AddColumn(m_viewType, listView, "SFP");
                    AddColumn(m_viewType, listView, "WC");
                    AddColumn(m_viewType, listView, "WW");
                    AddColumn(m_viewType, listView, "Auto W");
                    AddColumn(m_viewType, listView, "WF");
                    AddColumn(m_viewType, listView, "CS");
                    AddColumn(m_viewType, listView, "Bandwidth");
                    AddColumn(m_viewType, listView, "Default");
                    m_sorter = new PexTableComparer();
                    m_sorter.UpdateSortColumn(1, true);
                    m_sorter.UpdateSortColumn(0, true);
                    break;
                case ViewType.SP:
                    AddColumn(m_viewType, listView, "Namn");
                    AddColumn(m_viewType, listView, "Identifier");

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
                    PexItem pexItem = item.SubItems[j].Tag as PexItem;

                    xlWorkSheet.Cells[row, column] = pexItem != null ? pexItem.RawData : "";

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

        private void standPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_viewType = ViewType.SP;
            InitialPopulate();
        }
    }
}
