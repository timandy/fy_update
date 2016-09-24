using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WfyUpdate.Model
{
    /// <summary>
    /// 更新包集合
    /// </summary>
    public class UpdatePackageCollection : KeyedCollection<Version, UpdatePackage>
    {
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override Version GetKeyForItem(UpdatePackage item)
        {
            return item.From;
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, UpdatePackage item)
        {
            try
            {
                base.InsertItem(index, item);
            }
            catch
            {
                throw new Exception(string.Format("更新包版本冲突 {0}。", item.From));
            }
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="range"></param>
        public void AddRange(IEnumerable<UpdatePackage> range)
        {
            foreach (UpdatePackage item in range)
                base.Add(item);
        }

        /// <summary>
        /// 获取指定主键元素
        /// /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryGetValue(Version key, out UpdatePackage item)
        {
            return base.Dictionary.TryGetValue(key, out item);
        }
    }
}
