using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pexel
{
    class PexelStorage
    {
        private View m_view = new View();

        public PexelStorage(string mainTable)
        {
            m_view = CreateView(mainTable);
        }

        public class View
        {
            public string MainTable { set; get; }
            public List<Column> Columns { set; get; }
            public List<Table> Tables { set; get; }
        }
        public class Table
        {
            public string DisplayName { set; get; }
            public string Name { set; get; }
            public string ColumnID { set; get; }
            public string MainTableColumnID { set; get; }
        }
        public class Column
        {
            public string DisplayName { set; get; }
            public string TableName { set; get; }
            public string Name { set; get; }
        }

        private View CreateView(string mainTable)
        {
            View view = new View();

            view.MainTable = mainTable;
            view.Tables = new List<Table>();
            view.Columns = new List<Column>();

            return view;
        }

        public void AddColumn(string displayName, string name, string tableName)
        {
            Column column = new Column();

            column.DisplayName = displayName;
            column.Name = name;
            column.TableName = tableName;

            m_view.Columns.Add(column);
        }
        public void AddTable(string displayName, string name, string columnID, string mainTableColumnID)
        {
            Table table = new Table();

            table.DisplayName = displayName;
            table.Name = name;
            table.ColumnID = columnID;
            table.MainTableColumnID = mainTableColumnID;

            m_view.Tables.Add(table);
        }
    }
}
