using System;

namespace WfyUpdate.Update.Event
{
    /// <summary>
    /// 通知事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NotifyEventHandler(object sender, NotifyEventArgs e);

    /// <summary>
    /// 通知事件数据
    /// </summary>
    public class NotifyEventArgs : EventArgs
    {
        /// <summary>
        /// 通知信息
        /// </summary>
        public string Info { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        public NotifyEventArgs(string info)
        {
            this.Info = info;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public NotifyEventArgs(string format, params object[] args)
        {
            this.Info = string.Format(format, args);
        }
    }
}
