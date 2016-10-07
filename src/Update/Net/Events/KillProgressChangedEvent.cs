using System.ComponentModel;

namespace Update.Net.Events
{
    /// <summary>
    /// 结束进程进度报告事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void KillProgressChangedEventHandler(object sender, KillProgressChangedEventArgs e);

    /// <summary>
    /// 结束进程进度报告事件数据
    /// </summary>
    public class KillProgressChangedEventArgs : ProgressChangedEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="progressPercentage">进度百分比</param>
        /// <param name="userState">用户数据</param>
        public KillProgressChangedEventArgs(int progressPercentage, object userState)
            : base(progressPercentage, userState)
        {
        }
    }
}
