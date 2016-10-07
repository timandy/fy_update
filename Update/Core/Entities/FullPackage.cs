using System;

namespace Update.Core.Entities
{
    /// <summary>
    /// 全量更新包
    /// </summary>
    public class FullPackage : IPackage, IComparable<FullPackage>
    {
        /// <summary>
        /// 更新包文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 目标版本
        /// </summary>
        public Version To { get; set; }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">要比较的更新包</param>
        /// <returns>如果较新返回正数,较旧返回负数,相等返回 0</returns>
        public int CompareTo(FullPackage other)
        {
            return this.To.CompareTo(other.To);
        }
    }
}
