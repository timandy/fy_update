using System;
using System.IO;

namespace Update.Util
{
    /// <summary>
    /// 文件路径工具
    /// </summary>
    public static class FilePathUtil
    {
        //基目录,格式:C:\WorkDir
        private static readonly string m_BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 获取绝对路径所在目录
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>所在目录</returns>
        public static string GetDirectoryName(string absolutePath)
        {
            return Path.GetDirectoryName(absolutePath);
        }

        /// <summary>
        /// 获取文件的绝对路径(相对于当前基目录)
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns>绝对路径</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            return GetAbsolutePath(relativePath, m_BaseDirectory);
        }

        /// <summary>
        /// 获取文件的绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="basePath">基路径</param>
        /// <returns>绝对路径</returns>
        public static string GetAbsolutePath(string relativePath, string basePath)
        {
            return Path.Combine(basePath, relativePath).Replace('/', '\\');
        }
    }
}
