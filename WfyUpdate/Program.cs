#if !DEBUG
using System;
using System.Diagnostics;
#endif
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WfyUpdate.Config;
using WfyUpdate.Properties;
using WfyUpdate.Util;

namespace WfyUpdate
{
    class Program
    {
        //入口
        static void Main(string[] args)
        {
            HostConfig.ExecutablePath = args == null || args.Length < 1 || string.IsNullOrEmpty(args[0]) ? FilePathUtil.GetAbsolutePath("Wfy_Sale.exe") : args[0];
#if !DEBUG
            //必须从临时目录启动
            if (!AppConfig.ExecutablePath.Equals(AppConfig.ExpectExecutablePath, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    File.Copy(AppConfig.ExecutablePath, AppConfig.ExpectExecutablePath, true);
                    AppRunner.Start(AppConfig.ExpectExecutablePath, HostConfig.ExecutablePath);
                }
                catch (Exception e)
                {
                    MessageBoxUtil.Error(e.Message);
                }
                return;
            }
            //结束目标程序所在文件夹的进程
            foreach (Process process in Process.GetProcesses())
            {
                if (PathRelationUtil.IsParent(HostConfig.ExecutableDirectory, process.ProcessName))
                    process.Kill();
            }
#endif
            ExtractAssembly();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        //提取程序集
        static void ExtractAssembly()
        {
            Dictionary<string, byte[]> map = new Dictionary<string, byte[]>();
            string workFolder = AppConfig.ExecutableDirectory;
            map.Add(workFolder + "SharpCompress.dll", Resources.SharpCompress);
            foreach (var entry in map)
            {
                try { File.WriteAllBytes(entry.Key, entry.Value); }
                catch { }
            }
        }
    }
}
