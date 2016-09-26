using System.Windows.Forms;

namespace WfyUpdate.Util
{
    /// <summary>
    /// 对话框工具
    /// </summary>
    public static class MessageBoxUtil
    {
        //主窗口
        private static Form MainForm
        {
            get
            {
                FormCollection openForms = Application.OpenForms;
                return openForms.Count == 0 ? null : openForms[0];
            }
        }

        /// <summary>
        /// 显示提示对话框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DialogResult Info(string text)
        {
            return MessageBox.Show(MainForm, text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示警告对话框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DialogResult Warn(string text)
        {
            return MessageBox.Show(MainForm, text, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 显示错误对话框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DialogResult Error(string text)
        {
            return MessageBox.Show(MainForm, text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DialogResult Confirm(string text)
        {
            return MessageBox.Show(MainForm, text, "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
    }
}
