using System;

namespace Update.Core.Entities
{
    /// <summary>
    /// 增量跟新包
    /// </summary>
    public class DiffPackage : IPackage, IComparable<DiffPackage>
    {
        /// <summary>
        /// 更新包文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 起始版本
        /// </summary>
        public Version From { get; set; }

        /// <summary>
        /// 目标版本
        /// </summary>
        public Version To { get; set; }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">要比较的更新包</param>
        /// <returns>如果较新返回正数,较旧返回负数,相等返回 0</returns>
        public int CompareTo(DiffPackage other)
        {
            return this.From.CompareTo(other.From);
        }
    }
}
