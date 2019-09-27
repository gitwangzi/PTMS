/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 47a04892-48b7-4539-8b89-4d99f458cbd8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: BindingUtil
/////          Class Version: v1.0.0.0
/////            Create Time: 9/24/2013 2:07:37 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/24/2013 2:07:37 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Converts
{
    public class BindingUtil
    {
        #region DataContextPiggyBack attached property

        /// <summary>
        /// DataContextPiggyBack Attached Dependency Property, used as a mechanism for exposing
        /// DataContext changed events
        /// </summary>
        public static readonly DependencyProperty DataContextPiggyBackProperty =
            DependencyProperty.RegisterAttached("DataContextPiggyBack", typeof(object), typeof(BindingUtil),
                new PropertyMetadata(null, new PropertyChangedCallback(OnDataContextPiggyBackChanged)));

        public static object GetDataContextPiggyBack(DependencyObject d)
        {
            return (object)d.GetValue(DataContextPiggyBackProperty);
        }

        public static void SetDataContextPiggyBack(DependencyObject d, object value)
        {
            d.SetValue(DataContextPiggyBackProperty, value);
        }

        /// <summary>
        /// Handles changes to the DataContextPiggyBack property.
        /// </summary>
        private static void OnDataContextPiggyBackChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement targetElement = d as FrameworkElement;

            // whenever the targeElement DataContext is changed, copy the updated property
            // value to our MultiBinding.
            MultiBindings relay = GetMultiBindings(targetElement);
            relay.SetDataContext(targetElement.DataContext);
        }

        #endregion

        #region MultiBindings attached property

        public static MultiBindings GetMultiBindings(DependencyObject obj)
        {
            return (MultiBindings)obj.GetValue(MultiBindingsProperty);
        }

        public static void SetMultiBindings(DependencyObject obj, MultiBindings value)
        {
            obj.SetValue(MultiBindingsProperty, value);
        }

        public static readonly DependencyProperty MultiBindingsProperty =
            DependencyProperty.RegisterAttached("MultiBindings",
            typeof(MultiBindings), typeof(BindingUtil), new PropertyMetadata(null, OnMultiBindingsChanged));



        /// <summary>
        /// Invoked when the MultiBinding property is set on a framework element
        /// </summary>
        private static void OnMultiBindingsChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement targetElement = depObj as FrameworkElement;

            // bind the target elements DataContext, to our DataContextPiggyBack property
            // this allows us to get property changed events when the targetElement
            // DataContext changes
            targetElement.SetBinding(DataContextPiggyBackProperty, new Binding());

            MultiBindings bindings = GetMultiBindings(targetElement);

            bindings.Initialize(targetElement);
        }

        #endregion
    }
}
