using System.ComponentModel;

namespace WfyUpdate.Net.Events
{
    /// <summary>
    /// 解压工作委托
    /// </summary>
    /// <param name="e">参数</param>
    public delegate void DecompressDataStartDelegate(DecompressDataStartArgs e);

    /// <summary>
    /// 解压工作参数
    /// </summary>
    public class DecompressDataStartArgs
    {
        /// <summary>
        /// 异步操作生存期
        /// </summary>
        public AsyncOperation Operation { get; private set; }

        /// <summary>
        /// 要解压的数据
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 最后一个解压的文件
        /// </summary>
        public string LastEntry { get; private set; }

        /// <summary>
        /// 要解压到的目录
        /// </summary>
        public string DestinationDirectory { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="asyncOp">异步操作生存期</param>
        /// <param name="data">要解压的数据</param>
        /// <param name="lastEntry">最后一个解压的文件</param>
        /// <param name="destinationDirectory">要解压到的目录</param>
        public DecompressDataStartArgs(AsyncOperation asyncOp, byte[] data, string lastEntry, string destinationDirectory)
        {
            this.Operation = asyncOp;
            this.Data = data;
            this.LastEntry = lastEntry;
            this.DestinationDirectory = destinationDirectory;
        }
    }
}
