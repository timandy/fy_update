using System.Windows.Forms;
using WfyUpdate.Config;
using WfyUpdate.Controls;
using WfyUpdate.Update;
using WfyUpdate.Util;

namespace WfyUpdate
{
    //主窗口
    public partial class FrmMain : BaseForm
    {
        private const int TIMER_INTERVAL = 100;     //进度刷新间隔
        private bool m_Loaded;                      //是否已加载
        private int m_Percentage;                   //进度百分比
        private Timer m_Timer = new Timer();        //进度刷新定时器
        private Updater m_Updater = new Updater();  //更新器

        //构造函数
        public FrmMain()
        {
            InitializeComponent();
            this.Init();
        }

        //初始化
        private void Init()
        {
            //定时器
            this.m_Timer.Interval = TIMER_INTERVAL;
            this.m_Timer.Tick += (sender, e) => this.progress.Percentage = this.m_Percentage;
            //更新器
            this.m_Updater.Notify += (sender, e) => this.lblLog.Text = e.Info;
            this.m_Updater.Progress += (sender, e) => this.m_Percentage = e.ProgressPercentage;
            this.m_Updater.UpdateCompleted += (sender, e) =>
            {
                AppRunner.Start(HostConfig.ExecutablePath);
                this.CloseCore();
            };
            this.m_Updater.Error += (sender, e) =>
            {
                this.lblLog.Text = e.Error.Message;
                this.btnRetry.Visible = true;
                this.m_Timer.Stop();
            };
            //界面
            this.lblHeader.MouseDown += (sender, e) => Win32.BeginDrag(this.Handle);
            this.lblFooter.MouseDown += (sender, e) => Win32.BeginDrag(this.Handle);
            this.btnRetry.Click += (sender, e) =>
            {
                this.btnRetry.Visible = false;
                this.m_Timer.Start();
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
                this.m_Timer.Start();
                this.m_Updater.StartUpdate();
            };
        }

        //释放资源
        protected override void Dispose(bool disposing)
        {
            if (this.m_Timer != null)
            {
                this.m_Timer.Dispose();
                this.m_Timer = null;
            }
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
