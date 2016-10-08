using System;
using System.Collections.Generic;
using System.Linq;

namespace Update.Core.Entities
{
    /// <summary>
    /// 更新包信息
    /// </summary>
    public class Packages
    {
        private const char TAIL_SPLIT_UNDERLINE = '_';                  //末尾分隔符
        private const char TAIL_SPLIT_DOT = '.';                        //末尾分割符
        private static readonly char[] VERSION_SPLIT_CHARS = { '_' };   //版本分割符
        private const string VERSION_SPLIT_STRING = "to";               //版本分割字符串

        /// <summary>
        /// 增量更新包集合
        /// </summary>
        public DiffPackageCollection DiffPackages { get; private set; }

        /// <summary>
        /// 全量更新包信息
        /// </summary>
        public FullPackageCollection FullPackages { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text">服务端配置文件内容</param>
        public Packages(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            //解析
            List<DiffPackage> diffPackages = new List<DiffPackage>();
            List<FullPackage> fullPackages = new List<FullPackage>();
            string[] fileList = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string fileName in fileList)
            {
                //删除扩展名
                string shortName = StripTail(fileName.Trim());
                //分割文件名
                List<string> columns = shortName.Split(VERSION_SPLIT_CHARS, StringSplitOptions.RemoveEmptyEntries)
                    .Where(col => !col.Equals(VERSION_SPLIT_STRING, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                //转换
                try
                {
                    switch (columns.Count)
                    {
                        case 0:
                            break;
                        case 1:
                            fullPackages.Add(new FullPackage { To = new Version(columns[0]), FileName = fileName });
                            break;
                        default:
                            diffPackages.Add(new DiffPackage { From = new Version(columns[0]), To = new Version(columns[1]), FileName = fileName });
                            break;
                    }
                }
                catch
                {
                    throw new Exception(string.Format("更新包配置错误 {0}。", fileName));
                }
            }
            //赋值
            this.DiffPackages = new DiffPackageCollection(diffPackages);
            this.FullPackages = new FullPackageCollection(fullPackages);
        }

        /// <summary>
        /// 是否有可用更新
        /// </summary>
        /// <param name="current">当前版本</param>
        /// <returns>有可用更新返回true,否则返回false</returns>
        public bool HasAvailable(Version current)
        {
            return this.FullPackages.Any(package => package.To > current) || this.DiffPackages.Contains(current);
        }

        /// <summary>
        /// 获取可用更新包集合
        /// </summary>
        /// <param name="current">当前版本</param>
        /// <returns>更新包集合</returns>
        public PackageCollection GetAvailables(Version current)
        {
            PackageCollection packages = new PackageCollection();
            FullPackage maxFullPackage = this.FullPackages.Max();
            if (maxFullPackage != null && maxFullPackage.To > current)
            {
                packages.Add(maxFullPackage);
                current = maxFullPackage.To;
            }
            DiffPackage diffPackage;
            while (this.DiffPackages.TryGetValue(current, out diffPackage))
            {
                packages.Add(diffPackage);
                current = diffPackage.To;
            }
            return packages;
        }

        /// <summary>
        /// 去掉文件名的末尾(最后一个 '.' 或 '_' 后的字符串,如果为非 unit 则去掉
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>处理后的字符串</returns>
        private static string StripTail(string fileName)
        {
            uint temp;
            int start;
            int end = fileName.Length;
            for (int index = fileName.Length - 1; index >= 0; index--)
            {
                switch (fileName[index])
                {
                    case TAIL_SPLIT_DOT:
                    case TAIL_SPLIT_UNDERLINE:
                        if (uint.TryParse(fileName.Substring(start = index + 1, end - start), out temp))
                            return fileName.Substring(0, end);
                        else
                            end = index;
                        break;
                }
            }
            return string.Empty;
        }
    }
}
