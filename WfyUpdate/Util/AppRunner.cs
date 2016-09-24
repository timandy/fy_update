using System.Diagnostics;

namespace WfyUpdate.Util
{
    /// <summary>
    /// 程序启动工具
    /// </summary>
    public static class AppRunner
    {
        /// <summary>
        /// 运行指定程序,可指定命令行参数
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static bool Start(string fileName, string arguments = null)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            return process.Start();
        }
    }
}
