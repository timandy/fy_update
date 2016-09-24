using System;

namespace WfyUpdate.Update.Event
{
    /// <summary>
    /// 更新完成事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UpdatedEventHandler(object sender, UpdatedEventArgs e);

    /// <summary>
    /// 更新完成事件数据
    /// </summary>
    public class UpdatedEventArgs : EventArgs
    {
    }
}
