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
                    this.Refresh();
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
            Rectangle rcProgress = new Rectangle(rcClient.Left + 1, rcClient.Top + 1, (int)((rcClient.Width - 2) * this.Percentage / 100m), rcClient.Height - 2);
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

        #endregion
    }
}
