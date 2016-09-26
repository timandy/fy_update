using System;

namespace WfyUpdate.Update.Events
{
    /// <summary>
    /// 进度事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

    /// <summary>
    /// 进度数据
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// 百分比
        /// </summary>
        public int ProgressPercentage { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="progressPercentage">进度百分比</param>
        public ProgressEventArgs(int progressPercentage)
        {
            this.ProgressPercentage = progressPercentage;
        }
    }
}
