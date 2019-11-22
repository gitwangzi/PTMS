using Gsafety.PTMS.BasicPage.ViewModels;
using Gsafety.PTMS.ServiceReference.VedioService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class PhotoMaxWindow : ChildWindow
    {
        PhotoMaxWindowViewModel viewModel;
        public PhotoMaxWindow(Photo photo)
        {
            InitializeComponent();
            UIElement mainPage = Application.Current.RootVisual;;
            this.Width = mainPage.RenderSize.Width;
            this.Height = mainPage.RenderSize.Height - 15;
            viewModel = new PhotoMaxWindowViewModel(photo);
            this.DataContext = viewModel;
        }

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.Close();
            }
        }
    }
}

