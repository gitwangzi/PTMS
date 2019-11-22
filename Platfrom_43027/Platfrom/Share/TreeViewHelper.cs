using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace Gsafety.PTMS.Share
{
    public class TreeViewHelper
    {
        private static Dictionary<WeakReference, bool> registFlagDic = new Dictionary<WeakReference, bool>();

        #region SelectedItem
        public static object GetSelectedItem(DependencyObject obj)
        {
            return (object)obj.GetValue(SelectedItemProperty);
        }
        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new PropertyMetadata(null, SelectedItemChanged));
        private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is TreeView) || e.NewValue == null)
                return;

            var view = obj as TreeView;

            bool isExist = false;
            foreach (var item in registFlagDic)
            {
                if (item.Key.Target == view)
                {
                    isExist = true;
                }
            }

            if (isExist == false)
            {
                view.SelectedItemChanged += (sender, e2) => SetSelectedItem(view, e2.NewValue);
                registFlagDic[new WeakReference(view)] = true;
            }
        }
        #endregion
    }
}
