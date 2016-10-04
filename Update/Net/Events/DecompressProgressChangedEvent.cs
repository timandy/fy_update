using System.ComponentModel;

namespace Update.Net.Events
{
    /// <summary>
    /// 解压进度报告事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DecompressProgressChangedEventHandler(object sender, DecompressProgressChangedEventArgs e);

    /// <summary>
    /// 解压进度报告事件数据
    /// </summary>
    public class DecompressProgressChangedEventArgs : ProgressChangedEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="progressPercentage">进度百分比</param>
        /// <param name="userState">用户数据</param>
        public DecompressProgressChangedEventArgs(int progressPercentage, object userState)
            : base(progressPercentage, userState)
        {
        }
    }
}
