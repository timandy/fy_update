using System;

namespace Update.Core.Events
{
    /// <summary>
    /// 更新完成事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UpdateCompletedEventHandler(object sender, UpdateCompletedEventArgs e);

    /// <summary>
    /// 更新完成事件数据
    /// </summary>
    public class UpdateCompletedEventArgs : EventArgs
    {
    }
}
