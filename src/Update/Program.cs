#if !DEBUG
using System;
using System.IO;
#endif
using System.Windows.Forms;
using Update.Config;
using Update.Util;

namespace Update
{
    class Program
    {
        //入口
        static void Main(string[] args)
        {
#if DEBUG
            HostConfig.ExecutablePath = FilePathUtil.GetAbsolutePath("Test\\" + HostConfig.DefaultName);
#else
            HostConfig.ExecutablePath = args == null || args.Length < 1 || string.IsNullOrEmpty(args[0]) ? FilePathUtil.GetAbsolutePath(HostConfig.DefaultName) : args[0];
            //必须从临时目录启动
            if (!AppConfig.ExecutablePath.Equals(AppConfig.ExpectExecutablePath, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    File.Copy(AppConfig.ExecutablePath, AppConfig.ExpectExecutablePath, true);
                    ProcessUtil.Start(AppConfig.ExpectExecutablePath, HostConfig.ExecutablePath);
                }
                catch (Exception e)
                {
                    MessageBoxUtil.Error(e.Message);
                }
                return;
            }
#endif
            //运行
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmUpdate());
        }
    }
}
