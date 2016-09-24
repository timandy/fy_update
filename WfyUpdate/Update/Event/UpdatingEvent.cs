using System;
using WfyUpdate.Model;

namespace WfyUpdate.Update.Event
{
    /// <summary>
    /// 检查完毕,开始更新事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UpdatingEventHandler(object sender, UpdatingEventArgs e);

    /// <summary>
    /// 检查完毕,开始更新事件数据
    /// </summary>
    public class UpdatingEventArgs : EventArgs
    {
        /// <summary>
        /// 要下载的更新包集合
        /// </summary>
        public UpdatePackageCollection Packages { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="packages"></param>
        public UpdatingEventArgs(UpdatePackageCollection packages)
        {
            this.Packages = packages;
        }
    }
}
