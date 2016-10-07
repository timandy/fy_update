using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Microsoft.Windows.Forms;
using Update.Config;
using Update.Controls;
using Update.Core;
using Update.Properties;
using Update.Util;

namespace Update
{
    /// <summary>
    /// 主窗口
    /// </summary>
    public partial class FrmMain : BaseForm
    {
        private const int TIMER_INTERVAL = 100;     //进度刷新间隔
        private int m_Percentage;                   //进度百分比
        private Timer m_Timer = new Timer();        //进度刷新定时器
        private Updater m_Updater = new Updater();  //更新器
        //控件
        private UIImage imgBackground;
        private UILabel lblHeader;
        private UILine lnHeader;
        private UILabel lblFooter;
        private UILine lnFooter;
        private UILabel lblLog;
        private UIProgress prgPercentage;
        private UIButton btnRetry;
        private UIButton btnCancel;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
            this.InitUI();
            this.InitLogic();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitUI()
        {
            this.SuspendLayout();
            //
            this.imgBackground = new UIImage();
            this.imgBackground.Name = "imgBackground";
            this.imgBackground.Dock = DockStyle.Fill;
            this.imgBackground.AddFrame(Resources.background0);
            this.imgBackground.AddFrame(Resources.background1);
            this.imgBackground.AddFrame(Resources.background2);
            this.imgBackground.AddFrame(Resources.background3);
            this.imgBackground.AddFrame(Resources.background4);
            this.imgBackground.AddFrame(Resources.background5);
            //
            this.lblHeader = new UILabel();
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Dock = DockStyle.Top;
            this.lblHeader.Height = 75;
            this.lblHeader.Padding = new Padding(60, 0, 0, 10);
            this.lblHeader.Font = new Font("微软雅黑", 23F);
            this.lblHeader.ForeColor = Color.FromArgb(200, 255, 255, 255);
            this.lblHeader.Text = this.Text;
            this.lblHeader.TextAlign = ContentAlignment.BottomLeft;
            this.lblHeader.TextRenderingHint = TextRenderingHint.AntiAlias;
            //
            this.lnHeader = new UILine();
            this.lnHeader.Name = "lnHeader";
            this.lnHeader.Dock = DockStyle.Top;
            this.lnHeader.Height = 2;
            this.lnHeader.LineWidth = 2;
            this.lnHeader.LineColor = Color.FromArgb(40, 255, 255, 255);
            this.lnHeader.LineDashStyle = DashStyle.Dash;
            this.lnHeader.Padding = new Padding(1, 0, 1, 0);
            //
            this.lblFooter = new UILabel();
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Dock = DockStyle.Bottom;
            this.lblFooter.Height = 75;
            //
            this.lnFooter = new UILine();
            this.lnFooter.Name = "lnFooter";
            this.lnFooter.Dock = DockStyle.Bottom;
            this.lnFooter.Height = 2;
            this.lnFooter.LineWidth = 2;
            this.lnFooter.LineColor = Color.FromArgb(40, 255, 255, 255);
            this.lnFooter.LineDashStyle = DashStyle.Dash;
            this.lnFooter.Padding = new Padding(1, 0, 1, 0);
            //
            this.lblLog = new UILabel();
            this.lblLog.Name = "lblLog";
            this.lblLog.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.lblLog.Location = new Point(85, 232);
            this.lblLog.Size = new Size(830, 40);
            this.lblLog.Padding = new System.Windows.Forms.Padding(4);
            this.lblLog.Font = new Font("微软雅黑", 15F);
            this.lblLog.TextAlign = ContentAlignment.MiddleLeft;
            this.lblLog.ForeColor = Color.FromArgb(200, 255, 255, 255);
            this.lblLog.TextRenderingHint = TextRenderingHint.AntiAlias;
            //
            this.prgPercentage = new UIProgress();
            this.prgPercentage.Name = "prgPercentage";
            this.prgPercentage.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.prgPercentage.Location = new Point(85, 278);
            this.prgPercentage.Size = new Size(830, 10);
            this.prgPercentage.BackColor = Color.FromArgb(20, 255, 255, 255);
            this.prgPercentage.BorderColor = Color.FromArgb(200, 255, 255, 255);
            this.prgPercentage.ProgressColor = Color.FromArgb(150, 255, 255, 255);
            //
            this.btnRetry = new UIButton();
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnRetry.Location = new Point(804, 455);
            this.btnRetry.Size = new Size(70, 30);
            this.btnRetry.BackColor = Color.Transparent;
            this.btnRetry.HoveredBackColor = Color.FromArgb(20, 255, 255, 255);
            this.btnRetry.PressedBackColor = Color.FromArgb(50, 255, 255, 255);
            this.btnRetry.Font = new Font("微软雅黑", 12F);
            this.btnRetry.ForeColor = Color.FromArgb(200, 255, 255, 255);
            this.btnRetry.Text = "重试";
            this.btnRetry.TextRenderingHint = TextRenderingHint.AntiAlias;
            this.btnRetry.Image = Resources.retry;
            this.btnRetry.ImageSize = new Size(16, 16);
            this.btnRetry.Visible = false;
            //
            this.btnCancel = new UIButton();
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancel.Location = new Point(880, 455);
            this.btnCancel.Size = new Size(70, 30);
            this.btnCancel.BackColor = Color.Transparent;
            this.btnCancel.HoveredBackColor = Color.FromArgb(20, 255, 255, 255);
            this.btnCancel.PressedBackColor = Color.FromArgb(50, 255, 255, 255);
            this.btnCancel.Font = new Font("微软雅黑", 12F);
            this.btnCancel.ForeColor = Color.FromArgb(200, 255, 255, 255);
            this.btnCancel.Text = "取消";
            this.btnCancel.TextRenderingHint = TextRenderingHint.AntiAlias;
            this.btnCancel.Image = Resources.cancel;
            this.btnCancel.ImageSize = new Size(16, 16);
            //
            this.UIControls.Add(this.imgBackground);
            this.UIControls.Add(this.lblHeader);
            this.UIControls.Add(this.lnHeader);
            this.UIControls.Add(this.lblFooter);
            this.UIControls.Add(this.lnFooter);
            this.UIControls.Add(this.lblLog);
            this.UIControls.Add(this.prgPercentage);
            this.UIControls.Add(this.btnRetry);
            this.UIControls.Add(this.btnCancel);
            //
            this.ResumeLayout(false);
        }

        /// <summary>
        /// 初始化逻辑
        /// </summary>
        private void InitLogic()
        {
            //定时器
            this.m_Timer.Interval = TIMER_INTERVAL;
            this.m_Timer.Tick += (sender, e) => this.prgPercentage.Percentage = this.m_Percentage;
            //更新器
            this.m_Updater.Notify += (sender, e) => this.lblLog.Text = e.Info;
            this.m_Updater.Progress += (sender, e) => this.m_Percentage = e.ProgressPercentage;
            this.m_Updater.UpdateCompleted += (sender, e) =>
            {
                ProcessUtil.Start(HostConfig.ExecutablePath);
                this.CloseCore();
            };
            this.m_Updater.Error += (sender, e) =>
            {
                this.lblLog.Text = e.Error.Message;
                this.btnRetry.Visible = true;
                this.m_Timer.Stop();
            };
            //界面
            this.lblHeader.MouseDown += (sender, e) => Microsoft.Win32.Util.BeginDrag(this.Handle);
            this.lblFooter.MouseDown += (sender, e) => Microsoft.Win32.Util.BeginDrag(this.Handle);
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
            AppUtil.Idle(() =>
            {
                this.m_Timer.Start();
                this.m_Updater.StartUpdate();
            });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">释放托管资源为true,否则为false</param>
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
