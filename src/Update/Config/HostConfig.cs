using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Update.Util;

namespace Update.Config
{
    /// <summary>
    /// 目标配置
    /// </summary>
    public static class HostConfig
    {
        /// <summary>
        /// 默认目标文件名
        /// </summary>
        public const string DEFAULT_NAME = "Wfy_Sale.exe";

        private static string m_ExecutablePath;
        /// <summary>
        /// 获取目标文件路径 格式:D:\xx系统\wfy.exe
        /// </summary>
        public static string ExecutablePath
        {
            get
            {
                return m_ExecutablePath ?? (m_ExecutablePath = AppConfig.ExecutablePath);
            }
        }

        private static string m_ExecutableConfigPath;
        /// <summary>
        /// 获取目标配置文件路径 格式:D:\xx系统\wfy.exe.config
        /// </summary>
        public static string ExecutableConfigPath
        {
            get
            {
                return m_ExecutableConfigPath ?? (m_ExecutableConfigPath = ExecutablePath + ".config");
            }
        }

        private static string m_ExecutableDirectory;
        /// <summary>
        /// 获取目标文件的父文件夹 格式:D:\xx系统\
        /// </summary>
        public static string ExecutableDirectory
        {
            get
            {
                return m_ExecutableDirectory ?? (m_ExecutableDirectory = FilePathUtil.GetDirectoryName(ExecutablePath));
            }
        }

        private static string m_ExecutableName;
        /// <summary>
        /// 获取目标文件的短文件名 格式:wfy.exe
        /// </summary>
        public static string ExecutableName
        {
            get { return m_ExecutableName ?? (m_ExecutableName = Path.GetFileName(ExecutablePath)); }
        }

        private static Version m_CurrentVersion;
        /// <summary>
        /// 获取目标进程文件的文件版本
        /// </summary>
        public static Version CurrentVersion
        {
            get
            {
                return m_CurrentVersion ?? (m_CurrentVersion = new Version(FileVersionInfo.GetVersionInfo(ExecutablePath).FileVersion));
            }
        }

        private static string m_UpdateUrl;
        /// <summary>
        /// 获取目标进程配置文件中的 UpdateUrl 配置项,获取后会自动处理末尾,保证返回时以 '/' 结尾(如果没配置将返回 null)
        /// </summary>
        public static string UpdateUrl
        {
            get
            {
                if (m_UpdateUrl == null)
                {
                    string url = ReadHostConfig("UpdateUrl");
                    if (string.IsNullOrWhiteSpace(url))
                        return null;
                    url = url.Trim();
                    m_UpdateUrl = url.EndsWith("/") ? url : (url + "/");
                }
                return m_UpdateUrl;
            }
        }

        /// <summary>
        /// 初始化目标文件配置
        /// </summary>
        /// <param name="executablePath">目标文件路径</param>
        public static void Init(string executablePath)
        {
            m_ExecutablePath = executablePath;
            m_ExecutableConfigPath = null;
            m_ExecutableDirectory = null;
            m_ExecutableName = null;
            m_CurrentVersion = null;
            m_UpdateUrl = null;
        }

        /// <summary>
        /// 刷新目标程序版本
        /// </summary>
        public static void RefreshVersion()
        {
            m_CurrentVersion = null;
        }

        //读取配置
        private static string ReadHostConfig(string key)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ExecutablePath);
                KeyValueConfigurationCollection appSettings = config.AppSettings.Settings;
                KeyValueConfigurationElement add = appSettings.Cast<KeyValueConfigurationElement>().FirstOrDefault(element => string.Equals(element.Key, key, StringComparison.OrdinalIgnoreCase));
                return add == null ? null : add.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
