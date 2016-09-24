using System;

namespace WfyUpdate.Model
{
    /// <summary>
    /// 更新包
    /// </summary>
    public class UpdatePackage : IComparable<UpdatePackage>
    {
        /// <summary>
        /// 旧版本版本
        /// </summary>
        public Version From { get; set; }

        /// <summary>
        /// 新版本
        /// </summary>
        public Version To { get; set; }

        /// <summary>
        /// 是否全量更新包
        /// </summary>
        public bool Full { get; set; }

        /// <summary>
        /// 更新包文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">要比较的更新包</param>
        /// <returns>如果较新返回正数,较旧返回负数,相等返回 0</returns>
        public int CompareTo(UpdatePackage other)
        {
            return this.From.CompareTo(other.From);
        }
    }
}
