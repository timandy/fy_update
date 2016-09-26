using System;
using WfyUpdate.Update.Entities;

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
        public PackageCollection Packages { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="packages">更新包集合</param>
        public UpdateStartedEventArgs(PackageCollection packages)
        {
            this.Packages = packages;
        }
    }
}
