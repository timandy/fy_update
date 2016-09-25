using System.ComponentModel;

namespace WfyUpdate.Net
{
    /// <summary>
    /// 解压工作委托
    /// </summary>
    /// <param name="e">参数</param>
    public delegate void DecompressStartDelegate(DecompressStartArgs e);

    /// <summary>
    /// 解压工作参数
    /// </summary>
    public class DecompressStartArgs
    {
        /// <summary>
        /// 异步操作生存期
        /// </summary>
        public AsyncOperation Operation { get; internal set; }

        /// <summary>
        /// 要解压的数据
        /// </summary>
        public byte[] Data { get; internal set; }

        /// <summary>
        /// 最后一个解压的文件
        /// </summary>
        public string LastEntry { get; internal set; }

        /// <summary>
        /// 要解压到的目录
        /// </summary>
        public string DestinationDirectory { get; internal set; }
    }
}
