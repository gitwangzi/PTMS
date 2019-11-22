using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Gsafety.Common.Controls
{
    public class NodeTypeTemplateSelector : ContentControl
    {
        //protected override void OnContentChanged(object oldContent, object newContent)
        //{
        //    base.OnContentChanged(oldContent, newContent);

        //    var typeName = newContent.GetType().FullName;

        //    foreach (DictionaryEntry resource in Resources)
        //    {
        //        var names = resource.Key.ToString().Split('_').Take(2).ToArray();
        //        if (typeName.Contains(names[0]) && typeName.Contains(names[1]))
        //        {
        //            this.ContentTemplate = resource.Value as DataTemplate;
        //            break;
        //        }
        //    }
        //}

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            var type = newContent.GetType();
            var typeName = type.Name;

            if (type.IsGenericType == false)
            {
                this.ContentTemplate = Resources[typeName] as DataTemplate;
                //foreach (DictionaryEntry resource in Resources)
                //{
                //    if (string.Equals(typeName, resource.Key.ToString(), StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        this.ContentTemplate = resource.Value as DataTemplate;
                //        break;
                //    }
                //}
            }
            else
            {
                foreach (DictionaryEntry resource in Resources)
                {
                    var names = resource.Key.ToString().Split('_').Take(2).ToArray();
                    if (typeName.Contains(names[0]) && typeName.Contains(names[1]))
                    {
                        this.ContentTemplate = resource.Value as DataTemplate;
                        break;
                    }
                }
            }

        }
    }
}
