using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
namespace Gsafety.Common.Localization
{
    public class StringResourceBase : INotifyPropertyChanged, IDisposable
    {
        private static List<StringResourceBase> srList;

        private static List<StringResourceBase> SrList
        {
            get
            {
                if (srList == null)
                    srList = new List<StringResourceBase>();
                return srList;
            }
        }

        public static void Refresh()
        {
            foreach (var sr in SrList)
                sr.OnPropertyChanged(null);
        }

        ResourceManager manager;
        protected StringResourceBase(ResourceManager manager)
        {
            if (!SrList.Contains(this))
                SrList.Add(this);
            this.manager = manager;
        }

        public ResourceManager Manager
        {
            get
            {
                return this.manager;
            }
        }

        public virtual string this[string name]
        {
            get
            {
                //TODO:here or Converter ??
                if (name.EndsWith(":"))
                    return (GetString(name.TrimEnd(':'), Thread.CurrentThread.CurrentUICulture) ?? String.Empty) + ":";
                return GetString(name, Thread.CurrentThread.CurrentUICulture);
            }

        }

        public virtual string GetString(string name, CultureInfo culture)
        {

            if (this.manager == null)
                return string.Format("[{0}]", name);
            //throw new Exception("ResourceManager can't an NullReference object");
            return manager.GetString(name, culture ?? Thread.CurrentThread.CurrentUICulture);

        }

        public virtual string GetString(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            if (this.manager == null)
                return string.Format("[{0}]", name);
            //throw new Exception("ResourceManager can't an NullReference object");
            string targetstr = manager.GetString(name, Thread.CurrentThread.CurrentUICulture);
            if (targetstr == null) return string.Format("[{0}]", name);
            return targetstr;

        }

        public string GetKey(string value)
        {

            var dic = this.manager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true);
            var temp = dic.GetEnumerator();
            while (temp.MoveNext())
            {
                if (temp.Value.ToString() == value)
                {
                    return temp.Key.ToString();
                }
            }
            return null;
        }

        public void TranslateDataGrid(DataGrid dg)
        {
            try
            {
                foreach (DataGridColumn dgc in dg.Columns)
                {
                    if (dgc.Header == null || dgc.Header.ToString().Equals(string.Empty))
                        continue;
                    dgc.Header = GetString(dgc.Header.ToString());
                }
            }
            catch
            {

            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (SrList.Contains(this))
                SrList.Remove(this);
        }

        #endregion

    }
}
