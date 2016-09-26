using System;
using System.ComponentModel;

namespace WfyUpdate.Net.Events
{
    /// <summary>
    /// 结束进程完成事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void KillProcessCompletedEventHandler(object sender, KillProcessCompletedEventArgs e);

    /// <summary>
    /// 结束进程完成事件数据
    /// </summary>
    public class KillProcessCompletedEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="error">异常</param>
        /// <param name="cancelled">是否取消操作</param>
        /// <param name="userState">用户数据</param>
        public KillProcessCompletedEventArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }
    }
}
