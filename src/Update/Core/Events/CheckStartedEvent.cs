using System;

namespace Update.Core.Events
{
    /// <summary>
    /// 检查开始事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CheckStartedEventHandler(object sender, CheckStartedEventArgs e);

    /// <summary>
    /// 检查开始事件数据
    /// </summary>
    public class CheckStartedEventArgs : EventArgs
    {
    }
}
