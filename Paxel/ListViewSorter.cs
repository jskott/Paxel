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
        public int  m_column = 0;
        public bool m_ascending = true;

        private CaseInsensitiveComparer m_objectCompare;
        public ListViewSorter()
        {
            m_column = 0;

            // Initialize the CaseInsensitiveComparer object
            m_objectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object o1, object o2)
        {
            int compareResult;

            ListViewItem item1 = o1 as ListViewItem;
            ListViewItem item2 = o2 as ListViewItem;

            compareResult = m_objectCompare.Compare(item1.SubItems[m_column].Text, item2.SubItems[m_column].Text);

            return m_ascending ? compareResult : -compareResult;
        }
    }
}
