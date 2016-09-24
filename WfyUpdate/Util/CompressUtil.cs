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
        /// <param name="buffer"></param>
        /// <param name="targetFolder"></param>
        public static void Decompress(byte[] buffer, string targetFolder)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (IArchive archive = ArchiveFactory.Open(stream))
                {
                    foreach (var entry in archive.Entries)
                    {
                        entry.WriteToDirectory(targetFolder, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
        }
    }
}
