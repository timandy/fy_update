using System;
using System.IO;
using System.Web;

namespace WfyUpdate.Util
{
    /// <summary>
    /// <para>实现功能:文件路径助手</para>
    /// <para>调用方法:直接通过类名调用静态方法</para>
    /// <para>.</para>
    /// <para>创建人员:许崇雷</para>
    /// <para>创建日期:2011-02-15</para>
    /// <para>创建备注:</para>
    /// <para>.</para>
    /// <para>修改人员:</para>
    /// <para>修改日期:</para>
    /// <para>修改备注:</para>
    /// </summary>
    public static class FilePathUtil
    {
        #region 常量

        #region 静态常量
        #endregion

        #endregion


        #region 属性

        #region 静态属性
        #endregion

        #endregion


        #region 构造

        #region 静态构造
        #endregion

        #endregion


        #region 方法

        #region 静态方法

        /// <summary>
        /// 获取绝对路径所在目录
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>所在目录</returns>
        public static string GetDirectoryName(string absolutePath)
        {
            string directory;
            return FilePathUtil.GetDirectoryName(absolutePath, out directory)
                ? directory
                : string.Empty;
        }

        /// <summary>
        /// 获取绝对路径所在目录
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="directory">所在目录</param>
        /// <returns>成功返回true,否则返回false</returns>
        public static bool GetDirectoryName(string absolutePath, out string directory)
        {
            directory = string.Empty;
            try
            {
                directory = Path.GetDirectoryName(absolutePath);
                return true;
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 获取相对路径所在目录
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="basePath">基路径</param>
        /// <returns>所在目录</returns>
        public static string GetDirectoryName(string relativePath, string basePath)
        {
            string directory;
            return FilePathUtil.GetDirectoryName(relativePath, basePath, out directory)
                ? directory
                : string.Empty;
        }

        /// <summary>
        /// 获取相对路径所在目录
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="basePath">基路径</param>
        /// <param name="directory">所在目录</param>
        /// <returns>成功返回true,否则返回false</returns>
        public static bool GetDirectoryName(string relativePath, string basePath, out string directory)
        {
            directory = string.Empty;
            string absolutePath;
            if (FilePathUtil.GetAbsolutePath(relativePath, basePath, out absolutePath))
            {
                try
                {
                    directory = Path.GetDirectoryName(absolutePath);
                    return true;
                }
                catch { }
            }
            return false;
        }


        /// <summary>
        /// 获取文件的绝对路径,发生异常时不显示错误消息(相对于当前线程基目录)
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns>绝对路径,发生异常时返回String.Empty</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            string absolutePath;
            return FilePathUtil.GetAbsolutePath(relativePath, AppDomain.CurrentDomain.BaseDirectory, out absolutePath)
                ? absolutePath
                : string.Empty;
        }

        /// <summary>
        /// 获取文件的绝对路径,发生异常时不显示错误消息
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="basePath">基路径</param>
        /// <returns>绝对路径,发生异常时返回String.Empty</returns>
        public static string GetAbsolutePath(string relativePath, string basePath)
        {
            string absolutePath;
            return FilePathUtil.GetAbsolutePath(relativePath, basePath, out absolutePath)
                ? absolutePath
                : string.Empty;
        }

        /// <summary>
        /// 获取文件的绝对路径,发生异常时不显示错误消息(相对于当前线程基目录)
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="absolutePath">绝对路径,发生异常时返回String.Empty</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool GetAbsolutePath(string relativePath, out string absolutePath)
        {
            return FilePathUtil.GetAbsolutePath(relativePath, AppDomain.CurrentDomain.BaseDirectory, out absolutePath);
        }

        /// <summary>
        /// 获取文件的绝对路径,发生异常时不显示错误消息
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="basePath">基路径</param>
        /// <param name="absolutePath">绝对路径,发生异常时返回String.Empty</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool GetAbsolutePath(string relativePath, string basePath, out string absolutePath)
        {
            absolutePath = string.Empty;
            try
            {
                absolutePath = Path.Combine(basePath, relativePath).Replace('/', '\\');
                return true;
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 获取文件的相对路径,发生异常时不显示错误消息(基路径为当前应用程序工作目录)
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>相对路径,发生异常时返回String.Empty</returns>
        public static string GetRelativePath(string absolutePath)
        {
            string relativePath;
            return FilePathUtil.GetRelativePath(absolutePath, AppDomain.CurrentDomain.BaseDirectory, out relativePath)
                ? relativePath
                : string.Empty;
        }

        /// <summary>
        /// 获取文件的相对路径,发生异常时不显示错误消息
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="basePath">基路径</param>
        /// <returns>相对路径,发生异常时返回String.Empty</returns>
        public static string GetRelativePath(string absolutePath, string basePath)
        {
            string relativePath;
            return FilePathUtil.GetRelativePath(absolutePath, basePath, out relativePath)
                ? relativePath
                : string.Empty;
        }

        /// <summary>
        /// 获取文件的相对路径,发生异常时不显示错误消息(基路径为当前应用程序工作目录)
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="relativePath">相对路径,发生异常时返回String.Empty</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool GetRelativePath(string absolutePath, out string relativePath)
        {
            return FilePathUtil.GetRelativePath(absolutePath, AppDomain.CurrentDomain.BaseDirectory, out relativePath);
        }

        /// <summary>
        /// 获取文件的相对路径,发生异常时不显示错误消息
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <param name="basePath">基路径</param>
        /// <param name="relativePath">相对路径,发生异常时返回String.Empty</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool GetRelativePath(string absolutePath, string basePath, out string relativePath)
        {
            relativePath = string.Empty;
            try
            {
                relativePath = HttpUtility.UrlDecode(new Uri(basePath).MakeRelativeUri(new Uri(absolutePath)).ToString()).Replace('/', '\\');
                return true;
            }
            catch { }
            return false;
        }

        #endregion

        #endregion
    }
}
