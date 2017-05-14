using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pexel
{
    public partial class ColumnSetupForm : Form
    {
        TableByName m_tables = null;
        public ColumnSetupForm(TableByName tables)
        {
            InitializeComponent();
            m_tables = tables;

            PopulateTables();
            PopulateAvailableColumns();
            UpdateControls();
        }

        private void PopulateTables()
        {
            bool hasItems = false;
            foreach(string tableName in m_tables.Keys)
            {
                hasItems = true;
                m_tableComboBox.Items.Add(tableName);
            }

            if(hasItems)
            {
                m_tableComboBox.SelectedIndex = 0;
            }
        }

        private void PopulateAvailableColumns()
        {
            Table table = GetSelectedTable();
            m_availableColumns.Items.Clear();
            m_selectedColumnsList.Items.Clear();
            if (table != null)
            {
                List<string> columns = table.FlatColumns();
                columns.Sort();
                foreach (string column in columns)
                {
                    ListViewItem item = m_availableColumns.Items.Add(column);
                    item.Tag = column;
                }
            }
        }
        private Table GetSelectedTable()
        {
            Table table = null;
            int index = m_tableComboBox.SelectedIndex;

            if (index != -1)
            {
                string tableName = m_tableComboBox.Items[index].ToString();
                m_tables.TryGetValue(tableName, out table);
            }

            return table;
        }
        private List<string> GetSelectColumns()
        {
            List<string> columns = new List<string>();

            foreach(ListViewItem item in m_selectedColumnsList.Items)
            {
                columns.Add(item.Tag.ToString());
            }

            return columns;
        }
        private void AddAvailableColumn()
        {
            foreach(ListViewItem item in m_availableColumns.SelectedItems)
            {
                ListViewItem selectedColumnItem = m_selectedColumnsList.Items.Add(item.Text);
                selectedColumnItem.Tag = item.Tag;
                m_availableColumns.Items.Remove(item);
            }
        }
        private void RemoveSelectedColumn()
        {
            foreach (ListViewItem item in m_selectedColumnsList.SelectedItems)
            {
                ListViewItem availableColumnItem = m_availableColumns.Items.Add(item.Text);
                availableColumnItem.Tag = item.Tag;
                m_selectedColumnsList.Items.Remove(item);
            }
        }

        private void m_okButton_Click(object sender, EventArgs e)
        {
            Console.Write(SQLFromColumns.Generate(GetSelectedTable(), GetSelectColumns()));
            DialogResult = DialogResult.OK;
            //Close();
        }

        private void m_cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void m_addColumnButton_Click(object sender, EventArgs e)
        {
            AddAvailableColumn();
        }

        private void m_removeColumnButton_Click(object sender, EventArgs e)
        {
            RemoveSelectedColumn();
        }

        private void OnAvailableColumnsDblClicked(object sender, EventArgs e)
        {
            AddAvailableColumn();

        }

        private void OnSelectedColumnDblClicked(object sender, EventArgs e)
        {
            RemoveSelectedColumn();
        }

        private void OnTableComboSelectionChanged(object sender, EventArgs e)
        {
            PopulateAvailableColumns();
        }

        private void OnConfigurationNameChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
        private bool ValidName(string name)
        {
            bool ret = false;
            if(name.Length > 0)
            {
                ret = true;
            }

            return ret;
        }
        private void UpdateControls()
        {
            m_okButton.Enabled = ValidName(m_configurationNameTextBox.Text);
            m_addColumnButton.Enabled = m_availableColumns.SelectedItems.Count > 0;
            m_removeColumnButton.Enabled = m_selectedColumnsList.SelectedItems.Count > 0;
        }

        private void OnAvailableColumnsSelectionChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void OnSelectedColumnsSelectionChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void OnAfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if(e.Label == null || e.Label.Length == 0)
            {
                ListViewItem item = m_availableColumns.Items[e.Item];
                item.Text = item.Tag.ToString();
                e.CancelEdit = true;
            }
        }
    }
}
