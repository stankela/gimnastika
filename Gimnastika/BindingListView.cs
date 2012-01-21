using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Gimnastika
{
    public class BindingListView<T> : BindingList<T>, IBindingListView, ICancelAddNew
    {
        private bool m_Sorted = false;
        private PropertyDescriptor m_SortProperty = null;
        private ListSortDirection m_SortDirection = ListSortDirection.Ascending;
        private ListSortDescriptionCollection m_SortDescriptions = new ListSortDescriptionCollection();

        private bool m_Filtered = false;
        private string m_FilterString = null;
        string[] filterCriteria;
        PropertyDescriptor[] filterPropDescs;

        private List<T> m_OriginalCollection = new List<T>();

        bool updateOriginal = true;

        public BindingListView()
            : base()
        {

        }

        public BindingListView(List<T> list)
            : base(list)
        {

        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            // Simple iteration:
            for (int i = 0; i < Count; i++)
            {
                T item = this[i];
                if (property.GetValue(item).Equals(key))
                {
                    return i;
                }
            }
            return -1; // Not found

            // Using List.FindIndex:
            //Predicate<T> pred = delegate(T item)
            //{
            //   if (property.GetValue(item).Equals(key))
            //      return true;
            //   else
            //      return false;
            //};
            //List<T> list = Items as List<T>;
            //if (list == null)
            //   return -1;
            //return list.FindIndex(pred);
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return m_Sorted; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return m_SortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return m_SortProperty; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            m_SortProperty = property;
            m_SortDirection = direction;
            SortComparer<T> comparer = new SortComparer<T>(property, direction);
            ApplySortInternal(comparer);
        }

        private void ApplySortInternal(SortComparer<T> comparer)
        {
            // The check for the list being populated is in case the list is sorted
            // several times in succession
            if (m_OriginalCollection.Count == 0)
            {
                m_OriginalCollection.AddRange(this);
            }
            List<T> listRef = this.Items as List<T>;
            if (listRef != null)
            {
                listRef.Sort(comparer);
                m_Sorted = true;

                // Fires ListChanged event through a call to the base class
                // OnListChanged method, indicating that the list has changed.
                // The Reset type of change is most appropriate, since potentially
                // every item in the list has been moved around.
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        protected override void RemoveSortCore()
        {
            if (m_Sorted)
            {
                updateOriginal = false;
                Clear();
                foreach (T item in m_OriginalCollection)
                {
                    Add(item);
                }
                updateOriginal = true;

                m_SortProperty = null;
                m_SortDescriptions = null;
                m_Sorted = false;

                if (m_Filtered)
                {
                    // reapply filter
                    (this as IBindingListView).Filter = (this as IBindingListView).Filter;
                }
                else
                {
                    m_OriginalCollection.Clear();
                }
            }
        }


        #region IBindingListView Members

        ListSortDescriptionCollection IBindingListView.SortDescriptions
        {
            get
            {
                return m_SortDescriptions;
            }
        }

        bool IBindingListView.SupportsAdvancedSorting
        {
            get
            {
                return true;
            }
        }

        void IBindingListView.ApplySort(ListSortDescriptionCollection sorts)
        {
            m_SortProperty = null;
            m_SortDescriptions = sorts;
            SortComparer<T> comparer = new SortComparer<T>(sorts);
            ApplySortInternal(comparer);
        }

        bool IBindingListView.SupportsFiltering
        {
            get
            {
                return true;
            }
        }

        // Filtering is based on the string representation of the property value, and
        // uses quotes to delimit the value that is being filtered. Filters can be
        // concatenated using AND. For example:
        //     listView.Filter = "ProductName='Deep fryer fat'";
        string IBindingListView.Filter
        {
            get
            {
                return m_FilterString;
            }
            set
            {
                string oldFilterString = m_FilterString;
                m_FilterString = value;
                try
                {
                    UpdateFilter();
                    m_Filtered = true;
                }
                catch (Exception)
                {
                    m_FilterString = oldFilterString;
                }
            }
        }

        void IBindingListView.RemoveFilter()
        {
            if (m_Filtered)
            {
                updateOriginal = false;
                Clear();
                foreach (T item in m_OriginalCollection)
                {
                    Add(item);
                }
                updateOriginal = true;

                m_FilterString = null;
                m_Filtered = false;

                if (m_Sorted)
                {
                    if (m_SortProperty != null) // Simple sort 
                    {
                        ApplySortCore(m_SortProperty, m_SortDirection);
                    }
                    else
                    {
                        (this as IBindingListView).ApplySort(m_SortDescriptions);
                    }
                }
                else
                {
                    m_OriginalCollection.Clear();
                }
            }
        }

        #endregion

        protected virtual void UpdateFilter()
        {
            parseFilterString();

            if (m_OriginalCollection.Count == 0)
            {
                m_OriginalCollection.AddRange(this);
            }

            updateOriginal = false;
            Clear();
            foreach (T item in m_OriginalCollection)
            {
                if (passesFilter(item))
                {
                    Add(item);
                }
            }
            updateOriginal = true;

            if (m_Sorted)
            {
                if (m_SortProperty != null) // Simple sort 
                {
                    ApplySortCore(m_SortProperty, m_SortDirection);
                }
                else
                {
                    (this as IBindingListView).ApplySort(m_SortDescriptions);
                }
            }
        }

        private void parseFilterString()
        {
            string[] filters = m_FilterString.Split(new string[] { " AND " },
                StringSplitOptions.RemoveEmptyEntries);
            filterPropDescs = new PropertyDescriptor[filters.Length];
            filterCriteria = new string[filters.Length];

            for (int i = 0; i < filters.Length; i++)
            {
                string filter = filters[i];
                int equalsPos = filter.IndexOf('=');
                string propName = filter.Substring(0, equalsPos).Trim();
                string criterion = filter.Substring(equalsPos + 1, filter.Length - equalsPos - 1).Trim();
                if (criterion.IndexOf("'") != -1 || criterion.IndexOf('#') != -1)
                {
                    // strip leading and trailing quotes/pragmas
                    criterion = criterion.Substring(1, criterion.Length - 2);
                }
                filterPropDescs[i] = TypeDescriptor.GetProperties(typeof(T))[propName];
                filterCriteria[i] = criterion;

                // Check whether filter criteria can be converted to property type.
                // This can throw FormatException
                TypeDescriptor.GetConverter(filterPropDescs[i].PropertyType).
                    ConvertFrom(filterCriteria[i]);
            }
        }

        private bool passesFilter(object item)
        {
            for (int i = 0; i < filterPropDescs.Length; i++)
            {
                TypeConverter converter =
                    TypeDescriptor.GetConverter(filterPropDescs[i].PropertyType);

                // can throw FormatException
                object value = converter.ConvertFrom(filterCriteria[i]);

                if (!filterPropDescs[i].GetValue(item).Equals(value))
                {
                    return false;
                }
            }
            return true;
        }

        // When implementing sorting and filtering, you need to intercept the additions
        // and removals to make sure that the same changes get made to the original
        // (unsorted and unfiltered) collection as well as the primary collection.
        // To do this, you need to override InsertItem, RemoveItem and ClearItems
        // methods from the BindingList<T> base class. You also have to worry about
        // transacted additions to the collection, so you will need overrides of EndNew
        // and CancelNew, as well as transacted removals from the unsorted collection.
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            if ((m_Sorted || m_Filtered) && updateOriginal)
            {
                m_OriginalCollection.Add(item);
            }
        }

        protected override void RemoveItem(int index)
        {
            T item = Items[index];
            base.RemoveItem(index);
            if ((m_Sorted || m_Filtered) && updateOriginal)
            {
                m_OriginalCollection.Remove(item);
            }
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            if ((m_Sorted || m_Filtered) && updateOriginal)
            {
                m_OriginalCollection.Clear();
            }
        }

        public void EndNew()
        {
            // vidi komentar ispred metoda InsertItem
            throw new Exception("The method or operation is not implemented.");
        }

        public void CancelNew()
        {
            // vidi komentar ispred metoda InsertItem
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
