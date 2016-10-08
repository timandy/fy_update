using System;

namespace Update.Core.Events
{
    /// <summary>
    /// 检查完成事件类型
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">数据</param>
    public delegate void CheckCompletedEventHandler(object sender, CheckCompletedEventArgs e);

    /// <summary>
    /// 检查完成事件数据
    /// </summary>
    public class CheckCompletedEventArgs : EventArgs
    {
        private bool m_Uptodate;
        /// <summary>
        /// 获取是否已最新
        /// </summary>
        public bool Uptodate
        {
            get
            {
                return this.m_Uptodate;
            }
        }

        private bool m_Handled;
        /// <summary>
        /// 获取或设置是否已处理.如果设置为 true,则终止更新
        /// </summary>
        public bool Handled
        {
            get
            {
                return this.m_Handled;
            }
            set
            {
                this.m_Handled = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uptodate">是否已最新</param>
        public CheckCompletedEventArgs(bool uptodate)
        {
            this.m_Uptodate = uptodate;
        }
    }
}
