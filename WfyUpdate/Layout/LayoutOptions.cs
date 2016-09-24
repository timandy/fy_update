using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WfyUpdate.Layout
{
    /// <summary>
    /// 布局操作类
    /// </summary>
    public static class LayoutOptions
    {
        /// <summary>
        /// 默认文本布局信息.每次获取都将生成新对象,使用完毕需要释放资源.
        /// </summary>
        /// <returns>StringFormat 对象</returns>
        public static StringFormat DefaultStringFormat()
        {
            StringFormat sf = (StringFormat)StringFormat.GenericTypographic.Clone();
            sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            sf.Trimming = StringTrimming.EllipsisCharacter;
            return sf;
        }

        /// <summary>
        /// 获取绘制格式
        /// </summary>
        /// <param name="align">对齐方式</param>
        /// <param name="rtl">左右反转</param>
        /// <returns>绘制格式</returns>
        public static StringFormat GetStringFormat(ContentAlignment align, RightToLeft rtl)
        {
            StringFormat format = DefaultStringFormat();
            switch (align)
            {
                case ContentAlignment.TopLeft:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopRight:
                    format.LineAlignment = StringAlignment.Near;
                    format.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.MiddleLeft:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleCenter:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomLeft:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomCenter:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    format.LineAlignment = StringAlignment.Far;
                    format.Alignment = StringAlignment.Far;
                    break;
                default:
                    break;
            }
            if (rtl == RightToLeft.Yes)
                format.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            format.HotkeyPrefix = HotkeyPrefix.Show;
            return format;
        }
    }
}
