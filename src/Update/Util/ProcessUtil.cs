using System.Diagnostics;
using System.Linq;

namespace Update.Util
{
    /// <summary>
    /// 进程工具
    /// </summary>
    public static class ProcessUtil
    {
        /// <summary>
        /// 运行指定程序,可指定命令行参数
        /// </summary>
        /// <param name="fileName">要运行的程序路径</param>
        /// <param name="args">命令行参数数组</param>
        /// <returns>如果启动了进程资源，则为 true；如果没有启动新的进程资源（例如，如果重用了现有进程），则为 false。</returns>
        public static bool Start(string fileName, params string[] args)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            if (args != null)
                process.StartInfo.Arguments = string.Join(" ", args.Select(arg => "\"" + arg + "\""));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            return process.Start();
        }
    }
}
