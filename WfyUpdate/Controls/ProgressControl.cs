using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WfyUpdate.Controls
{
    /// <summary>
    /// 进度条控件
    /// </summary>
    public class ProgressControl : Control
    {
        private const int ANIMATION_INTERVAL = 10;          //定时器间隔(毫秒)
        private const int ANIMATION_SPAN = 200;             //动画时间(毫秒)
        private Timer m_Timer = new Timer();                //动画定时器
        private Animation m_Animation = new Animation();    //动画对象

        #region 控件属性

        /// <summary>
        /// 获取或设置背景色
        /// </summary>
        [Category("外观"), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        #endregion


        #region 进度条属性

        private Color m_ProgressColor = Color.DodgerBlue;
        /// <summary>
        /// 获取或设置进度条颜色
        /// </summary>
        [Category("进度条"), DefaultValue(typeof(Color), "DodgerBlue")]
        public virtual Color ProgressColor
        {
            get
            {
                return this.m_ProgressColor;
            }
            set
            {
                if (value != this.m_ProgressColor)
                {
                    this.m_ProgressColor = value;
                    this.Invalidate();
                }
            }
        }

        private Color m_ProgressColor2 = Color.White;
        /// <summary>
        /// 获取或设置进度条颜色2
        /// </summary>
        [Category("进度条"), DefaultValue(typeof(Color), "White")]
        public virtual Color ProgressColor2
        {
            get
            {
                return this.m_ProgressColor2;
            }
            set
            {
                if (value != this.m_ProgressColor2)
                {
                    this.m_ProgressColor2 = value;
                    this.Invalidate();
                }
            }
        }

        public Color m_BorderColor = Color.DodgerBlue;
        /// <summary>
        /// 获取或设置进度条边框颜色
        /// </summary>
        [Category("进度条"), DefaultValue(typeof(Color), "DodgerBlue")]
        public virtual Color BorderColor
        {
            get
            {
                return this.m_BorderColor;
            }
            set
            {
                if (value != this.m_BorderColor)
                {
                    this.m_BorderColor = value;
                    this.Invalidate();
                }
            }
        }

        private int m_Percentage = 0;
        /// <summary>
        /// 获取或设置进度条颜色
        /// </summary>
        [Category("进度条"), DefaultValue(0)]
        public virtual int Percentage
        {
            get
            {
                return this.m_Percentage;
            }
            set
            {
                if (value != this.m_Percentage)
                {
                    this.m_Percentage = value;
                    this.m_Timer.Start();
                    this.m_Animation.Reset(this.m_Percentage);
                }
            }
        }

        #endregion


        #region 构造

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProgressControl()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            base.BackColor = Color.Transparent;
            this.m_Timer.Interval = ANIMATION_INTERVAL;
            this.m_Timer.Tick += (sender, e) =>
            {
                if (this.m_Animation.Last == this.m_Percentage)
                    this.m_Timer.Stop();
                else
                    this.Invalidate();
            };
        }

        #endregion


        #region 重写

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="e">数据</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rcClient = this.ClientRectangle;
            //进度
            int maxWidth = rcClient.Width - 2;
            int width = (int)(maxWidth * this.m_Animation.Current / 100m);
            Rectangle rcProgress = new Rectangle(rcClient.Left + 1, rcClient.Top + 1, Math.Min(width, maxWidth), rcClient.Height - 2);
            using (Brush brush = new SolidBrush(this.ProgressColor))
            {
                g.FillRectangle(brush, rcProgress);
            }
            //未完成进度
            Rectangle rcBack = new Rectangle(rcProgress.Right, rcProgress.Top, rcClient.Width - 2 - rcProgress.Width, rcProgress.Height);
            using (Brush brush = new SolidBrush(this.ProgressColor2))
            {
                g.FillRectangle(brush, rcBack);
            }
            //边框
            Rectangle rcBorder = new Rectangle(rcClient.Left, rcClient.Top, rcClient.Width - 1, rcClient.Height - 1);
            using (Pen pen = new Pen(this.BorderColor))
            {
                g.DrawRectangle(pen, rcBorder);
            }
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
            this.m_Animation = null;
            base.Dispose(disposing);
        }

        #endregion


        #region 内部类

        /// <summary>
        /// 动画
        /// </summary>
        private class Animation
        {
            private DateTime m_StartTime;
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime StartTime
            {
                get
                {
                    return this.m_StartTime;
                }
            }

            private int m_From;
            /// <summary>
            /// 起始百分比
            /// </summary>
            public int Form
            {
                get
                {
                    return this.m_From;
                }
            }

            private int m_To;
            /// <summary>
            /// 截止百分比
            /// </summary>
            public int To
            {
                get
                {
                    return this.m_To;
                }
            }

            private int m_Last;
            /// <summary>
            /// 获取上次百分比
            /// </summary>
            public int Last
            {
                get
                {
                    return this.m_Last;
                }
            }

            /// <summary>
            /// 当前显示百分比
            /// </summary>
            public int Current
            {
                get
                {
                    if (this.m_From >= this.m_To)
                        return this.m_Last = this.m_To;
                    int current = this.m_From + (this.m_To - this.m_From) * (DateTime.Now - this.m_StartTime).Milliseconds / ANIMATION_SPAN;
                    return this.m_Last = current > this.m_To ? this.m_To : current;
                }
            }

            /// <summary>
            /// 重置动画
            /// </summary>
            /// <param name="from">起始百分比</param>
            /// <param name="to">截止百分比</param>
            public void Reset(int to)
            {
                this.m_From = this.m_Last;
                this.m_To = to;
                this.m_StartTime = DateTime.Now;
            }
        }

        #endregion
    }
}
