using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.Generic;

namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    [ExportAsViewModel(ManagerName.VideoDetailVM)]
    public class VideoDetailVM : BaseViewModel
    {
        public IActionCommand GoBackCommand { get; set; }

        public string DownLoader { get; set; }
        public string DownLoadTime { get; set; }
        public string MDVRId { get; set; }
        public string SubType { get; set; }
        public string VehicleID { get; set; }
        public string ChannelId { get; set; }
        public string VideoStartTime { get; set; }
        public string VideoEndtime { get; set; }
        public string VideoFileName { get; set; }

        public VideoDetailVM()
        {
            GoBackCommand = new ActionCommand<object>(obj => Back());
        }

        private void Back()
        {
            EventAggregator.Publish(new ViewNavigationArgs("VideoDowmLoadlogView", new Dictionary<string, object>() { { "action", "return" } }));
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0)
            {
                var DownLoader = viewParameters["Player"];
                this.DownLoader = DownLoader != null ? DownLoader.ToString() : string.Empty;

                var DownLoadTime = viewParameters["ActionTime"];
                this.DownLoadTime = DownLoadTime != null ? DownLoadTime.ToString() : string.Empty;

                var MDVRId = viewParameters["MDVRId"];
                this.MDVRId = MDVRId != null ? MDVRId.ToString() : string.Empty;

                var SubType = viewParameters["SubType"];
                this.SubType = SubType != null ? SubType.ToString() : string.Empty;

                var VehicleID = viewParameters["VehicleID"];
                this.VehicleID = VehicleID != null ? VehicleID.ToString() : string.Empty;

                var ChannelId = viewParameters["ChannelId"];
                this.ChannelId = ChannelId != null ? ChannelId.ToString() : string.Empty;

                var VideoStartTime = viewParameters["VideoStartTime"];
                this.VideoStartTime = VideoStartTime != null ? VideoStartTime.ToString() : string.Empty;

                var VideoEndtime = viewParameters["VideoEndtime"];
                this.VideoEndtime = VideoEndtime != null ? VideoEndtime.ToString() : string.Empty;

                var VideoFileName = viewParameters["VideoFileName"];
                this.VideoEndtime = VideoEndtime != null ? VideoEndtime.ToString() : string.Empty;

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(new string[] { "DownLoader", "DownLoadTime", "MDVRId", "SubType", "VehicleID", "ChannelId", "VideoStartTime", "VideoEndtime", "VideoFileName" }));
            }
        }
    }
}
