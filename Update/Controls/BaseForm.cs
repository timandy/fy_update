using System.ComponentModel;
using System.Windows.Forms;

namespace Update.Controls
{
    /// <summary>
    /// 屏蔽 Alt + F4 窗口
    /// </summary>
    public class BaseForm : Form
    {
        private bool m_UserClosing;

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="owner">父窗口</param>
        public virtual void ShowCore(IWin32Window owner)
        {
            this.Show(owner);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public virtual void CloseCore()
        {
            this.m_UserClosing = true;
            this.Close();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="e">数据</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !this.m_UserClosing;
            this.m_UserClosing = false;
        }
    }
}
