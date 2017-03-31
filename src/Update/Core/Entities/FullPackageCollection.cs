using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Update.Core.Entities
{
    /// <summary>
    /// 全量更新包集合
    /// </summary>
    public class FullPackageCollection : KeyedCollection<Version, FullPackage>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FullPackageCollection()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">元素集合</param>
        public FullPackageCollection(IEnumerable<FullPackage> items)
        {
            this.AddRange(items);
        }

        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="item">元素</param>
        /// <returns>主键</returns>
        protected override Version GetKeyForItem(FullPackage item)
        {
            return item.To;
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="item">元素</param>
        protected override void InsertItem(int index, FullPackage item)
        {
            try
            {
                base.InsertItem(index, item);
            }
            catch
            {
                throw new Exception(string.Format("全量更新包冲突 {0}。", item.FileName));
            }
        }

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="range">元素集合</param>
        public void AddRange(IEnumerable<FullPackage> range)
        {
            foreach (FullPackage item in range)
                base.Add(item);
        }

        /// <summary>
        /// 获取指定主键元素
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="item">获取到的元素</param>
        /// <returns>包含返回true,否则返回false</returns>
        public bool TryGetValue(Version key, out FullPackage item)
        {
            var dictionary = this.Dictionary;
            if (dictionary == null)
            {
                var comparer = this.Comparer;
                foreach (var value in this.Items)
                {
                    if (comparer.Equals(this.GetKeyForItem(value), key))
                    {
                        item = value;
                        return true;
                    }
                }
                item = null;
                return false;
            }
            return dictionary.TryGetValue(key, out item);
        }
    }
}
