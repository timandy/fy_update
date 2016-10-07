using System;

namespace Update.Core.Entities
{
    /// <summary>
    /// 更新包接口
    /// </summary>
    public interface IPackage
    {
        /// <summary>
        /// 更新包文件名
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// 目标版本
        /// </summary>
        Version To { get; set; }
    }
}
