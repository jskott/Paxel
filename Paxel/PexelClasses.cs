using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pexel
{
    public class PexItem
    {
        public string DisplayName { get; set; }
        public string Key { get; set; }
    }

    public class Relationship
    {
        public string ForeignKey { get; set; }
        public Table ForeignTable { get; set; }
        public string PrimaryKey { get; set; }
        public Table PrimaryTable { get; set; }
    }
    public class Table
    {
        public Table()
        {
            Columns = new List<string>();
            Relations = new List<Relationship>();
        }
        private Relationship RelationFromForeignKey(string foreignKey)
        {
            Relationship ret = null;

            foreach (Relationship relationship in Relations)
            {
                if (relationship.ForeignKey.Equals(foreignKey))
                {
                    ret = relationship;
                    break;
                }
            }

            return ret;
        }
        private Relationship RelationFromPrimaryKey(string primaryKey)
        {
            Relationship ret = null;

            foreach (Relationship relationship in Relations)
            {
                if (relationship.PrimaryKey.Equals(primaryKey))
                {
                    ret = relationship;
                    break;
                }
            }

            return ret;
        }

        public List<string> FlatColumns()
        {
            List<string> allColumns = new List<string>();

            foreach (string column in Columns)
            {
                Relationship relationship = RelationFromForeignKey(column);

                if (relationship == null)
                {
                    allColumns.Add(column);
                }
                else
                {
                    foreach (string relColumn in relationship.PrimaryTable.FlatColumns())
                    {
                        if (!relationship.PrimaryKey.Equals(relColumn))
                        {
                            allColumns.Add(relationship.ForeignKey + ":" + relationship.PrimaryTable.Name + "." + relColumn);
                        }
                    }
                }
            }

            allColumns.Sort();

            return allColumns;
        }
        public List<string> Columns { get; set; }

        public List<Relationship> Relations { get; set; }

        public string Name { get; set; }
    }

    public class TableByName : Dictionary<string, Table>
    { }

    public class PexDataRow : List<PexItem>
    { }

    public class PexTable : List<PexDataRow>
    { }

    public class StringSet : Dictionary<string, int>
    {
        public void Add(string value)
        {
            this[value] = 0;
        }
    }
    public class TableByColumn : Dictionary<string, StringSet>
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

    public class ColumnSorter
    {
        public ColumnSorter(int column, bool ascending = true)
        {
            Column = column;
            Ascending = ascending;
        }
        public int Column { get; set; }
        public bool Ascending { get; set; }
    }
    public class PexTableComparer : IComparer<PexDataRow>
    {
        private List<ColumnSorter> m_columns = new List<ColumnSorter>();

        private CaseInsensitiveComparer m_objectCompare;
        public PexTableComparer()
        {
            m_columns.Add(new ColumnSorter(0));

            // Initialize the CaseInsensitiveComparer object
            m_objectCompare = new CaseInsensitiveComparer();
        }

        public void UpdateSortColumn(int sortColumn, bool descending)
        {
            for(int i = 0; i < m_columns.Count; i++)
            {
                if(m_columns[i].Column == sortColumn)
                {
                    m_columns.RemoveAt(i);
                    break;
                }
            }

            m_columns.Insert(0, new ColumnSorter(sortColumn, descending));
        }
        public ColumnSorter CurrentSortColumn()
        {
            return m_columns[0];
        }
        public int Compare(PexDataRow row1, PexDataRow row2)
        {
            int compareResult = 0;

            foreach (ColumnSorter column in m_columns)
            {
                compareResult = m_objectCompare.Compare(row1[column.Column].DisplayName, row2[column.Column].DisplayName);
                compareResult = column.Ascending ? compareResult : -compareResult;

                if (compareResult != 0)
                {
                    break;
                }
            }

            return compareResult;
        }
    }

    public class SQLFromColumns
    {
        static public string CreateJoinSqlLine(Table table, string[] column)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("inner join ");
            int index1 = column.Count() - 1;
            int index2 = index1 - 1;

            if(index2 > -1)
            {
                string foreignKey = column[index1];
                string primaryKey = column[index2];

                string foreignTable = foreignKey.Split('.')[0];

                sb.Append(foreignTable);
                sb.Append(" ON ");
                sb.Append(foreignTable);
                sb.Append(".ID");
                sb.Append(" = ");
                if (!primaryKey.Contains("."))
                {
                    sb.Append(table.Name);
                    sb.Append(".");
                }
                sb.Append(primaryKey);
            }

            return sb.ToString();
        }

        static public string Generate(Table table, List<string> columns)
        {
            StringBuilder sb = new StringBuilder();
            List<string> rels = new List<string>();

            sb.AppendLine("SELECT");
            int index = 0;
            foreach(string column in columns)
            {
                string sqlRow = column;
                if(column.Contains(":"))
                {
                    string[] splits = column.Split(':');

                    sqlRow = splits.Last();
                    rels.Add(CreateJoinSqlLine(table, splits));
                }

                sb.Append(sqlRow);

                if (index < (columns.Count - 1))
                {
                    sb.Append(",");
                }
                sb.Append(Environment.NewLine);
                index++;

            }

            sb.AppendLine("FROM " + new string('(', rels.Count - 1) + table.Name);
            index = 0;
            foreach(string joins in rels)
            {
                sb.Append(joins);
                if(index < (rels.Count - 1))
                {
                    sb.Append(")");
                }
                sb.Append(Environment.NewLine);
                index++;
            }

            return sb.ToString();
        }
    }

}
