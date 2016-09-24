using System.Collections.Generic;
using System.Net;
using WfyUpdate.Dispose;
using WfyUpdate.Model;
using WfyUpdate.Update.Event;

namespace WfyUpdate.Update
{
    /// <summary>
    /// 更新器 by 许崇雷
    /// </summary>
    public partial class Updater : Disposable, IUpdater
    {
        private static readonly char[] PERIOD = { '.', '。' };       //句号
        private static readonly string PACKAGES = "packages.txt";   //服务端配置文件名
        private WebClient m_WebClient;                              //客户端
        private IEnumerator<UpdatePackage> m_Avaliables;            //更新枚举器

        /// <summary>
        /// 构造函数
        /// </summary>
        public Updater()
        {
            this.m_WebClient = new WebClient();
            this.m_WebClient.Encoding = System.Text.Encoding.UTF8;
            this.m_WebClient.DownloadStringCompleted += (sender, e) => this.EndCheck(e);
            this.m_WebClient.DownloadDataCompleted += (sender, e) => this.EndDownload(e);
            this.m_WebClient.DownloadProgressChanged += (sender, e) => this.OnProgress(new ProgressEventArgs(e.ProgressPercentage));
        }
    }
}
