using System;

namespace WfyUpdate.Update.Events
{
    /// <summary>
    /// 错误事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

    /// <summary>
    /// 错误时间数据
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 错误
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="error"></param>
        public ErrorEventArgs(Exception error)
        {
            this.Error = error;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="error"></param>
        public ErrorEventArgs(string error)
        {
            this.Error = new Exception(error);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public ErrorEventArgs(string format, params object[] args)
        {
            this.Error = new Exception(string.Format(format, args));
        }
    }
}
