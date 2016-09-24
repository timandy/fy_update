using System.Windows.Forms;
using WfyUpdate.Controls;
using WfyUpdate.Update;
using WfyUpdate.Util;

namespace WfyUpdate
{
    public partial class FrmMain : BaseForm
    {
        private bool m_Loaded;//是否以启动更新
        private Updater m_Updater = new Updater();

        //构造函数
        public FrmMain()
        {
            InitializeComponent();
            this.Init();
        }

        //初始化
        private void Init()
        {
            //updater
            this.m_Updater.DownloadProgressChanged += (sender, e) => this.progress.Percentage = e.ProgressPercentage;
            this.m_Updater.Notify += (sender, e) => this.lblLog.Text = e.Info;
            this.m_Updater.Error += (sender, e) => { this.lblLog.Text = e.Error.Message; this.btnRetry.Visible = true; };
            this.m_Updater.Updated += (sender, e) => this.CloseCore();
            //ui
            this.lblHeader.MouseDown += (sender, e) => Win32.BeginDrag(this.Handle);
            this.lblFooter.MouseDown += (sender, e) => Win32.BeginDrag(this.Handle);
            this.btnRetry.Click += (sender, e) =>
            {
                this.btnRetry.Visible = false;
                this.m_Updater.StartUpdate();
            };
            this.btnCancel.Click += (sender, e) =>
            {
                if (MessageBoxUtil.Confirm("您确定要退出更新？") == DialogResult.OK)
                    this.CloseCore();
            };

            //程序空闲时开始更新
            Application.Idle += (sender, e) =>
            {
                if (this.m_Loaded)
                    return;
                this.m_Loaded = true;
                this.m_Updater.StartUpdate();
            };
        }

        //释放资源
        protected override void Dispose(bool disposing)
        {
            if (this.m_Updater != null)
            {
                this.m_Updater.Dispose();
                this.m_Updater = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
