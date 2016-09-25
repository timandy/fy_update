using System.ComponentModel;

namespace WfyUpdate.Net
{
    /// <summary>
    /// 结束进程工作委托
    /// </summary>
    /// <param name="e">参数</param>
    public delegate void KillProcessStartDelegate(KillProcessStartArgs e);

    /// <summary>
    /// 结束进程工作参数
    /// </summary>
    public class KillProcessStartArgs
    {
        /// <summary>
        /// 异步操作生存期
        /// </summary>
        public AsyncOperation Operation { get; private set; }

        /// <summary>
        /// 进程所在目录
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="asyncOp">异步操作生存期</param>
        /// <param name="directory">进程所在目录</param>
        public KillProcessStartArgs(AsyncOperation asyncOp, string directory)
        {
            this.Operation = asyncOp;
            this.Directory = directory;
        }
    }
}
