using Gsafety.PTMS.ServiceReference.VedioService;
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

namespace Gsafety.PTMS.BasicPage.ViewModels
{
    public class PhotoMaxWindowViewModel : BaseViewModel
    {
        public PhotoMaxWindowViewModel(Photo photo)
        {
            this.MPhoto = photo;
        }

        private Photo _photo = new Photo();
        public Photo MPhoto
        {
            get { return _photo; }
            set
            {
                _photo = value;
                RaisePropertyChanged(() => MPhoto);
            }
        }
    }
}
