using System;
using WfyUpdate.Model;

namespace WfyUpdate.Update.Events
{
    /// <summary>
    /// 检查完毕,开始更新事件类型
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UpdateStartedEventHandler(object sender, UpdateStartedEventArgs e);

    /// <summary>
    /// 检查完毕,开始更新事件数据
    /// </summary>
    public class UpdateStartedEventArgs : EventArgs
    {
        /// <summary>
        /// 要下载的更新包集合
        /// </summary>
        public UpdatePackageCollection Packages { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="packages"></param>
        public UpdateStartedEventArgs(UpdatePackageCollection packages)
        {
            this.Packages = packages;
        }
    }
}
