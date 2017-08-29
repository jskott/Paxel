using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Pexel
{
    class ListViewSorter : IComparer
    {
        public List<int>  m_columns = null;
        public bool       m_ascending = true;

        private CaseInsensitiveComparer m_objectCompare;
        public ListViewSorter(List<int> initialSortColumns)
        {
            m_columns = initialSortColumns;
            // Initialize the CaseInsensitiveComparer object
            m_objectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object o1, object o2)
        {
            int compareResult = 0;

            ListViewItem item1 = o1 as ListViewItem;
            ListViewItem item2 = o2 as ListViewItem;

            foreach (int column in m_columns)
            {
                compareResult = m_objectCompare.Compare(item1.SubItems[column].Text, item2.SubItems[column].Text);

                if(compareResult != 0)
                {
                    break;
                }
            }

            return m_ascending ? compareResult : -compareResult;
        }
    }
}
