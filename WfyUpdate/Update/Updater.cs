using System.Collections.Generic;
using WfyUpdate.Dispose;
using WfyUpdate.Net;
using WfyUpdate.Update.Entities;
using WfyUpdate.Update.Events;

namespace WfyUpdate.Update
{
    /// <summary>
    /// 更新器 by 许崇雷
    /// </summary>
    public partial class Updater : Disposable, IUpdater
    {
        private static readonly char[] PERIOD = { '.', '。' };      //句号
        private static readonly string PACKAGES = "packages.txt";   //服务端配置文件名
        private WebClientEx m_WebClient;                            //客户端
        private IEnumerator<IPackage> m_Avaliables;                 //更新枚举器

        /// <summary>
        /// 构造函数
        /// </summary>
        public Updater()
        {
            this.m_WebClient = new WebClientEx { Encoding = System.Text.Encoding.UTF8 };
            this.m_WebClient.KillProgressChanged += (sender, e) => this.OnProgress(new ProgressEventArgs(e.ProgressPercentage));
            this.m_WebClient.KillProcessCompleted += (sender, e) => this.KillCompleted(e);
            this.m_WebClient.DownloadProgressChanged += (sender, e) => this.OnProgress(new ProgressEventArgs(e.ProgressPercentage));
            this.m_WebClient.DownloadStringCompleted += (sender, e) => this.CheckCompleted(e);
            this.m_WebClient.DownloadDataCompleted += (sender, e) => this.DownloadCompleted(e);
            this.m_WebClient.DecompressProgressChanged += (sender, e) => this.OnProgress(new ProgressEventArgs(e.ProgressPercentage));
            this.m_WebClient.DecompressDataCompleted += (sender, e) => this.DecompressCompleted(e);
        }
    }
}
