﻿using Gsafety.PTMS.Installation;
using Jounce.Core.ViewModel;
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

namespace Gsafety.Ant.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.DevGPSManageVm)]
    public class InstallDevGPSManageViewModel : BaseInformation.ViewModels.DevGpsMangeViewModel
    {
        public InstallDevGPSManageViewModel()
        {
            AddVisibility = (Visibility)converter.Convert("03-01-06-03-02", null, "03-01-06-03-02", null);
            RaisePropertyChanged(() => AddVisibility);

            ImportVisibility = Visibility.Collapsed;
            RaisePropertyChanged(() => ImportVisibility);
            DownloadTemplateVisibility = Visibility.Collapsed;
            RaisePropertyChanged(() => DownloadTemplateVisibility);
            ExportVisibility = Visibility.Collapsed;
            RaisePropertyChanged(() => ExportVisibility);

            EditVisibility = (Visibility)converter.Convert("03-01-06-03-03", null, "03-01-06-03-03", null);
            RaisePropertyChanged(() => EditVisibility);

            ViewVisibility = (Visibility)converter.Convert("03-01-06-03-01", null, "03-01-06-03-01", null);
            RaisePropertyChanged(() => ViewVisibility);

            DeleteVisibility = (Visibility)converter.Convert("03-01-06-03-04", null, "03-01-06-03-04", null);
            RaisePropertyChanged(() => DeleteVisibility);
        }
    }
}
