using System.ComponentModel;
using System.Windows.Forms;

namespace WfyUpdate.Controls
{
    /// <summary>
    /// 按钮控件
    /// </summary>
    public class ButtonControl : Button
    {
        #region 属性

        /// <summary>
        /// 获取或设置按钮控件的平面样式外观
        /// </summary>
        [Category("外观"), DefaultValue(FlatStyle.Flat)]
        public new FlatStyle FlatStyle
        {
            get
            {
                return base.FlatStyle;
            }
            set
            {
                base.FlatStyle = value;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public ButtonControl()
        {
            base.SetStyle(
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.ResizeRedraw |
              ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.Selectable, false);
            base.FlatStyle = FlatStyle.Flat;
        }

        #endregion
    }
}
