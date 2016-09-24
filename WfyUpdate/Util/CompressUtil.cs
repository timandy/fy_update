using System;
using System.IO;
using System.Text;
using SharpCompress.Archive;
using SharpCompress.Common;

namespace WfyUpdate.Util
{
    /// <summary>
    /// 压缩工具
    /// </summary>
    public static class CompressUtil
    {
        /// <summary>
        /// 静态构造
        /// </summary>
        static CompressUtil()
        {
            ArchiveEncoding.Default = Encoding.GetEncoding("GB2312");
        }

        /// <summary>
        /// 解压,支持 7z,zip,rar
        /// </summary>
        /// <param name="buffer">压缩包数据</param>
        /// <param name="destFolder">目标文件夹</param>
        /// <param name="lastEntry">指定最后一个解压的文件</param>
        public static void Decompress(byte[] buffer, string destFolder, string lastEntry)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (IArchive archive = ArchiveFactory.Open(stream))
                {
                    IArchiveEntry last = null;
                    foreach (IArchiveEntry entry in archive.Entries)
                    {
                        if (entry.IsDirectory)
                            continue;
                        if (last == null && entry.Key.Equals(lastEntry, StringComparison.OrdinalIgnoreCase))
                        {
                            last = entry;
                            continue;
                        }
                        entry.WriteToDirectory(destFolder, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                    if (last != null)
                        last.WriteToDirectory(destFolder, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                }
            }
        }
    }
}
