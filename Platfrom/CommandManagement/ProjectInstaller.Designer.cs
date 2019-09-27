namespace Gsafety.PTMS.CommandManagement
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CommandManagenetSPI = new System.ServiceProcess.ServiceProcessInstaller();
            this.CommandManagementSI = new System.ServiceProcess.ServiceInstaller();
            // 
            // CommandManagenetSPI
            // 
            this.CommandManagenetSPI.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.CommandManagenetSPI.Password = null;
            this.CommandManagenetSPI.Username = null;
            this.CommandManagenetSPI.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.CommandManagenetSPI_AfterInstall);
            // 
            // CommandManagementSI
            // 
            this.CommandManagementSI.Description = "PTMS command issued manage, send and reply under management commands issued the c" +
    "ommand and the command and reply sent out.";
            this.CommandManagementSI.DisplayName = "PTMS Command Management";
            this.CommandManagementSI.ServiceName = "PTMS Command Management";
            this.CommandManagementSI.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.CommandManagenetSPI,
            this.CommandManagementSI});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller CommandManagenetSPI;
        private System.ServiceProcess.ServiceInstaller CommandManagementSI;
    }
}