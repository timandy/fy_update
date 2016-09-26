using System;
using System.ComponentModel;

namespace WfyUpdate.Net.Events
{
    /// <summary>
    /// 解压数据完成事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DecompressDataCompletedEventHandler(object sender, DecompressDataCompletedEventArgs e);

    /// <summary>
    /// 解压数据完成事件数据
    /// </summary>
    public class DecompressDataCompletedEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="error">异常</param>
        /// <param name="cancelled">是否取消操作</param>
        /// <param name="userState">用户数据</param>
        public DecompressDataCompletedEventArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }
    }
}
