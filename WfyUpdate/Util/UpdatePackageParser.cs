using System;
using System.Collections.Generic;
using System.Linq;
using WfyUpdate.Config;
using WfyUpdate.Model;

namespace WfyUpdate.Util
{
    /// <summary>
    /// 更新包转换器
    /// </summary>
    public static class UpdatePackageParser
    {
        private const string TO = "to";
        private const string FULL = "full";
        private const string ZIP = ".zip";
        private const string RAR = ".rar";
        private const string _7Z = ".7z";

        /// <summary>
        /// 字符串转换为更新包集合(按更新包排序)
        /// </summary>
        /// <param name="packagesStr">服务端配置文件内容</param>
        /// <returns>更新包集合</returns>
        public static UpdatePackageCollection Parse(string packagesStr)
        {
            if (packagesStr == null)
                throw new ArgumentNullException("packagesStr");

            List<UpdatePackage> list = new List<UpdatePackage>();
            string[] fileList = packagesStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string fileName in fileList)
            {
                //排除扩展名
                string trimZip;
                if (fileName.EndsWith(ZIP, StringComparison.OrdinalIgnoreCase))
                    trimZip = fileName.Substring(0, fileName.Length - 4);
                else if (fileName.EndsWith(RAR, StringComparison.OrdinalIgnoreCase))
                    trimZip = fileName.Substring(0, fileName.Length - 4);
                else if (fileName.EndsWith(_7Z, StringComparison.OrdinalIgnoreCase))
                    trimZip = fileName.Substring(0, fileName.Length - 3);
                else
                    trimZip = fileName;
                List<string> columns = trimZip.Split(new char[] { '_', }, StringSplitOptions.RemoveEmptyEntries) //分割
                    .Where(col => !col.Equals(TO, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                //转换
                try
                {
                    UpdatePackage remoteFile = new UpdatePackage
                    {
                        From = new Version(columns[0]),
                        To = new Version(columns[1]),
                        Full = columns.Count > 2 && columns[2].Equals(FULL, StringComparison.OrdinalIgnoreCase),
                        FileName = fileName
                    };
                    list.Add(remoteFile);
                }
                catch
                {
                    throw new Exception(string.Format("更新包配置错误 {0}。", fileName));
                }
            }
            UpdatePackageCollection collection = new UpdatePackageCollection();
            collection.AddRange(list.OrderBy(ver => ver));
            return collection;
        }

        /// <summary>
        /// 获取可用的更新包集合
        /// </summary>
        /// <param name="packages">所有更新包集合</param>
        /// <returns>可用的更新包集合</returns>
        public static UpdatePackageCollection GetAvaliablePackages(UpdatePackageCollection packages)
        {
            if (packages == null)
                throw new ArgumentNullException("packages");

            List<UpdatePackage> list = new List<UpdatePackage>();
            Version from = HostConfig.CurrentVersion;
            UpdatePackage current;
            while (packages.TryGetValue(from, out current))
            {
                if (current.Full)
                    list.Clear();
                list.Add(current);
                from = current.To;
            }
            UpdatePackageCollection collection = new UpdatePackageCollection();
            collection.AddRange(list);
            return collection;
        }
    }
}
