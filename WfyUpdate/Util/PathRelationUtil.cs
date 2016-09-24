using System;

namespace WfyUpdate.Util
{
    /// <summary>
    /// 路径关系工具
    /// </summary>
    public class PathRelationUtil
    {
        /// <summary>
        /// 判断是否为父目录
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsParent(string parent, string child)
        {
            return child.StartsWith(parent, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
