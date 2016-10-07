using System;
using System.IO;
using System.Windows.Forms;
using Update.Util;

namespace Update.Config
{
    /// <summary>
    /// 当前配置
    /// </summary>
    public static class AppConfig
    {
        private static string m_Temp;
        /// <summary>
        /// 获取系统临时目录 格式:C:\Users\Administrator\AppData\Local\Temp\
        /// </summary>
        public static string Temp
        {
            get
            {
                return m_Temp ?? (m_Temp = Path.GetTempPath());
            }
        }

        private static string m_ExecutablePath;
        /// <summary>
        /// 获取当前运行的exe的路径 格式:D:\xx系统\update.exe
        /// </summary>
        public static string ExecutablePath
        {
            get
            {
                return m_ExecutablePath ?? (m_ExecutablePath = Application.ExecutablePath);
            }
        }

        private static string m_ExecutableDirectory;
        /// <summary>
        /// 获取当前运行的exe的目录 格式:D:\xx系统\
        /// </summary>
        public static string ExecutableDirectory
        {
            get
            {
                return m_ExecutableDirectory ?? (m_ExecutableDirectory = AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        private static string m_FileName;
        /// <summary>
        /// 获取当前运行的exe的名称 格式:update.exe
        /// </summary>
        public static string FileName
        {
            get
            {
                return m_FileName ?? (m_FileName = Path.GetFileName(ExecutablePath));
            }
        }

        private static string m_ExpectExecutablePath;
        /// <summary>
        /// 获取期望启动路径 格式:C:\Users\Administrator\AppData\Local\Temp\update.exe
        /// </summary>
        public static string ExpectExecutablePath
        {
            get
            {
                return m_ExpectExecutablePath ?? (m_ExpectExecutablePath = FilePathUtil.GetAbsolutePath(FileName, Temp));
            }
        }
    }
}