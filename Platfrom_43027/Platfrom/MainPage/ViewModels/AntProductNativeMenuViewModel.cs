using BaseLib.ViewModels;
using Gsafety.PTMS.MainPage;
using Jounce.Core.ViewModel;

namespace Gsafety.Ant.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.AntProductNativeMenuVm)]
    public class AntProductNativeMenuViewModel : PTMSBaseViewModel
    {
        #region 属性

        private int nativeMenuIndex;

        public int NativeMenuIndex
        {
            get { return this.nativeMenuIndex; }
            set
            {
                this.nativeMenuIndex = value;
                RaisePropertyChanged(() => this.NativeMenuIndex);
            }
        }

        #endregion

        #region 命令


        #endregion


        #region 构造函数

        public AntProductNativeMenuViewModel()
        {
            //默认实时监控
            this.NativeMenuIndex = 3;
        }
        #endregion
    }
}
