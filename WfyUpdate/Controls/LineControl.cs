using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WfyUpdate.Layout;

namespace WfyUpdate.Controls
{
    /// <summary>
    /// 直线控件
    /// </summary>
    public class LineControl : Control
    {
        #region 控件属性

        /// <summary>
        /// 获取或设置内边距
        /// </summary>
        [Category("布局"), DefaultValue(typeof(Padding), "5, 5, 5, 5")]
        public new Padding Padding
        {
            get
            {
                return base.Padding;
            }
            set
            {
                base.Padding = value;
            }
        }

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


        #region 文本属性

        private string m_Text = "";
        /// <summary>
        /// 获取或设置文本
        /// </summary>
        [Category("外观"), DefaultValue("")]
        public new string Text
        {
            get
            {
                return this.m_Text ?? string.Empty;
            }
            set
            {
                if (value != this.m_Text)
                {
                    this.m_Text = value;
                    this.Refresh();
                }
            }
        }

        private ContentAlignment m_TextAlign = ContentAlignment.MiddleLeft;
        /// <summary>
        /// 获取或设置文本对其方式
        /// </summary>
        [Category("外观"), DefaultValue(ContentAlignment.MiddleLeft)]
        public virtual ContentAlignment TextAlign
        {
            get
            {
                return this.m_TextAlign;
            }
            set
            {
                if (value != this.m_TextAlign)
                {
                    this.m_TextAlign = value;
                    this.Invalidate();
                }
            }
        }

        private SmoothingMode m_TextSmoothingMode = SmoothingMode.None;
        /// <summary>
        /// 获取或设置文本抗锯齿模式
        /// </summary>
        [Category("外观"), DefaultValue(SmoothingMode.None)]
        public virtual SmoothingMode TextSmoothingMode
        {
            get
            {
                return this.m_TextSmoothingMode;
            }
            set
            {
                if (value != this.m_TextSmoothingMode)
                {
                    this.m_TextSmoothingMode = value;
                    this.Invalidate();
                }
            }
        }

        #endregion


        #region 直线属性

        private bool m_LineHorizontal = true;
        /// <summary>
        /// 获取或设置是否水平线.true 表示水平线,false 表示垂直线
        /// </summary>
        [Category("直线"), DefaultValue(true)]
        public virtual bool LineHorizontal
        {
            get
            {
                return this.m_LineHorizontal;
            }
            set
            {
                if (value != this.m_LineHorizontal)
                {
                    this.m_LineHorizontal = value;
                    this.Invalidate();
                }
            }
        }

        private Padding m_LinePadding = new Padding(0);
        /// <summary>
        /// 获取或设置直线边距
        /// </summary>
        [Category("直线"), DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public virtual Padding LinePadding
        {
            get
            {
                return this.m_LinePadding;
            }
            set
            {
                if (value != this.m_LinePadding)
                {
                    this.m_LinePadding = value;
                    this.Invalidate();
                }
            }
        }

        private Color m_LineColor = Color.FromArgb(200, 200, 200);
        /// <summary>
        /// 获取或设置直线颜色
        /// </summary>
        [Category("直线"), DefaultValue(typeof(Color), "200, 200, 200")]
        public virtual Color LineColor
        {
            get
            {
                return this.m_LineColor;
            }
            set
            {
                if (value != this.m_LineColor)
                {
                    this.m_LineColor = value;
                    this.Invalidate();
                }
            }
        }

        private int m_LineWidth = 1;
        /// <summary>
        /// 获取或设置直线宽度
        /// </summary>
        [Category("直线"), DefaultValue(1)]
        public virtual int LineWidth
        {
            get
            {
                return this.m_LineWidth;
            }
            set
            {
                if (value != this.m_LineWidth)
                {
                    this.m_LineWidth = value;
                    this.Invalidate();
                }
            }
        }

        private DockStyle m_LineDock = DockStyle.None;
        /// <summary>
        /// 获取或设置直线停靠位置
        /// </summary>
        [Category("直线"), DefaultValue(DockStyle.None)]
        public virtual DockStyle LineDock
        {
            get
            {
                return this.m_LineDock;
            }
            set
            {
                if (value != this.m_LineDock)
                {
                    this.m_LineDock = value;
                    this.Invalidate();
                }
            }
        }

        #endregion


        #region 构造

        /// <summary>
        /// 构造函数
        /// </summary>
        public LineControl()
        {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            base.Padding = new Padding(5);
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
            //直线
            Graphics g = e.Graphics;
            Rectangle rcClient = this.ClientRectangle;
            if (this.LineColor.A > 0)
            {
                Padding padLine = this.LinePadding;
                Rectangle rcLine = new Rectangle(rcClient.Left + padLine.Left, rcClient.Top + padLine.Top, rcClient.Width - padLine.Horizontal, rcClient.Height - padLine.Vertical);
                Point begin, end;
                if (this.LineHorizontal)
                {
                    int y;
                    switch (this.LineDock)
                    {
                        case DockStyle.Top:
                            y = rcLine.Y + 1;
                            break;
                        case DockStyle.Bottom:
                            y = rcLine.Bottom - this.LineWidth + 1;
                            break;
                        default:
                            y = (rcLine.Y + rcLine.Bottom - this.LineWidth) / 2;
                            break;
                    }
                    begin = new Point(rcLine.X, y);
                    end = new Point(rcLine.Right, y);
                }
                else
                {
                    int x;
                    switch (this.LineDock)
                    {
                        case DockStyle.Left:
                            x = rcLine.X + 1;
                            break;
                        case DockStyle.Right:
                            x = rcLine.Right - this.LineWidth + 1;
                            break;
                        default:
                            x = (rcLine.X + rcLine.Right - this.LineWidth) / 2;
                            break;
                    }
                    begin = new Point(x, rcLine.Y);
                    end = new Point(x, rcLine.Bottom);
                }
                using (Pen pen = new Pen(this.LineColor, this.LineWidth))
                {
                    g.DrawLine(pen, begin, end);
                }
            }
            //文本
            if (this.Text.Length > 0)
            {
                Padding padText = this.Padding;
                Rectangle rcText = new Rectangle(rcClient.Left + padText.Left, rcClient.Top + padText.Top, rcClient.Width - padText.Horizontal, rcClient.Height - padText.Vertical);
                using (Brush brush = new SolidBrush(this.ForeColor))
                {
                    using (StringFormat format = LayoutOptions.GetStringFormat(this.TextAlign, this.RightToLeft))
                    {
                        using (new SmoothingModeGraphics(g, this.TextSmoothingMode))
                        {
                            g.DrawString(this.Text, this.Font, brush, rcText, format);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
