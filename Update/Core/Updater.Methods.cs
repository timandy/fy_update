using System;
using System.Net;
using Update.Config;
using Update.Core.Entities;
using Update.Core.Events;
using Update.Net.Events;

namespace Update.Core
{
    partial class Updater
    {
        /// <summary>
        /// 同步检查更新
        /// </summary>
        /// <returns>有可用更新返回true,否则为false</returns>
        protected virtual bool Check()
        {
            this.DisposeAvaliables();
            HostConfig.Refresh();
            if (!System.IO.File.Exists(HostConfig.ExecutablePath))
                throw new Exception("要更新的程序不存在。");
            if (!System.IO.File.Exists(HostConfig.ExecutableConfigPath))
                throw new Exception("要更新的程序的配置文件不存在。");
            if (HostConfig.UpdateUrl == null)
                throw new Exception("没有配置更新地址。");
            string text;
            try
            {
                text = this.m_WebClient.DownloadString(HostConfig.UpdateUrl + PACKAGES);
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("下载更新信息失败:{0}。", exp.Message.TrimEnd(PERIOD)));
            }
            Packages packages;
            try
            {
                packages = new Packages(text);
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("解析更新信息失败:{0}。", exp.Message.TrimEnd(PERIOD)));
            }
            return packages.HasAvailable(HostConfig.CurrentVersion);
        }

        /// <summary>
        /// 异步检查开始
        /// </summary>
        protected virtual void CheckAsync()
        {
            this.DisposeAvaliables();
            HostConfig.Refresh();
            if (!System.IO.File.Exists(HostConfig.ExecutablePath))
            {
                this.OnError(new ErrorEventArgs("要更新的程序不存在。"));
                return;
            }
            if (!System.IO.File.Exists(HostConfig.ExecutableConfigPath))
            {
                this.OnError(new ErrorEventArgs("要更新的程序的配置文件不存在。"));
                return;
            }
            if (HostConfig.UpdateUrl == null)
            {
                this.OnError(new ErrorEventArgs("没有配置更新地址。"));
                return;
            }
            this.OnNotify(new NotifyEventArgs("正在下载更新信息。"));
            this.m_WebClient.DownloadStringAsync(new Uri(HostConfig.UpdateUrl + PACKAGES));
        }

        /// <summary>
        /// 异步检查完成
        /// </summary>
        /// <param name="e">结果</param>
        protected virtual void CheckCompleted(DownloadStringCompletedEventArgs e)
        {
            //用户取消
            if (e.Cancelled)
            {
                this.OnNotify(new NotifyEventArgs("已取消更新。"));
                return;
            }
            //出错
            if (e.Error != null)
            {
                this.OnError(new ErrorEventArgs("下载更新信息失败:{0}。", e.Error.Message.TrimEnd(PERIOD)));
                return;
            }
            //解析
            Packages packages;
            try
            {
                packages = new Packages(e.Result);
            }
            catch (Exception exp)
            {
                this.OnError(new ErrorEventArgs("解析更新信息失败:{0}。", exp.Message.TrimEnd(PERIOD)));
                return;
            }
            //可用更新
            PackageCollection avaliables = packages.GetAvailables(HostConfig.CurrentVersion);
            if (avaliables.Count < 1)
            {
                this.OnNotify(new NotifyEventArgs("已是最新版本。"));
                return;
            }
            this.m_Avaliables = avaliables.GetEnumerator();
            //开始更新
            this.OnUpdateStarted(new UpdateStartedEventArgs(avaliables));
            this.KillAsync();
        }

        /// <summary>
        /// 异步结束进程开始
        /// </summary>
        protected virtual void KillAsync()
        {
            //结束进程
            this.OnNotify(new NotifyEventArgs("正在结束占用进程。"));
            this.m_WebClient.KillProcessAsync(HostConfig.ExecutableDirectory);
        }

        /// <summary>
        /// 异步结束进程完成
        /// </summary>
        /// <param name="e">结果</param>
        protected virtual void KillCompleted(KillProcessCompletedEventArgs e)
        {
            //用户取消
            if (e.Cancelled)
            {
                this.OnNotify(new NotifyEventArgs("已取消更新。"));
                return;
            }
            //出错
            if (e.Error != null)
            {
                this.OnError(new ErrorEventArgs("结束占用进程失败:{0}。", e.Error.Message.TrimEnd(PERIOD)));
                return;
            }
            //开始下载
            this.DownloadAsync();
        }

        /// <summary>
        /// 异步下载开始
        /// </summary>
        protected virtual void DownloadAsync()
        {
            //验证
            if (this.m_Avaliables == null)
            {
                this.OnError(new ErrorEventArgs("必须先检查更新。"));
                return;
            }
            //下一个
            if (!this.m_Avaliables.MoveNext())
            {
                this.OnNotify(new NotifyEventArgs("更新完成。"));
                this.OnUpdateCompleted(new UpdateCompletedEventArgs());
                return;
            }
            IPackage package = this.m_Avaliables.Current;
            this.OnNotify(new NotifyEventArgs("正在下载 {0}。", package.FileName));
            this.m_WebClient.DownloadDataAsync(new Uri(HostConfig.UpdateUrl + package.FileName), package);
        }

        /// <summary>
        /// 异步下载完成
        /// </summary>
        /// <param name="e">结果</param>
        protected virtual void DownloadCompleted(DownloadDataCompletedEventArgs e)
        {
            //验证
            IPackage package = e.UserState as IPackage;
            if (package == null)
            {
                this.OnError(new ErrorEventArgs("无效的下载操作。"));
                return;
            }
            //用户取消
            if (e.Cancelled)
            {
                this.OnNotify(new NotifyEventArgs("已取消更新。"));
                return;
            }
            //出错
            if (e.Error != null)
            {
                this.OnError(new ErrorEventArgs("下载 {0} 失败:{1}。", package.FileName, e.Error.Message.TrimEnd(PERIOD)));
                return;
            }
            this.DecompressAsync(e.Result, package);
        }

        /// <summary>
        /// 异步解压开始
        /// </summary>
        /// <param name="data">要解压的数据</param>
        protected virtual void DecompressAsync(byte[] data, IPackage package)
        {
            //解压
            this.OnNotify(new NotifyEventArgs("正在解压 {0}。", package.FileName));
            this.m_WebClient.DecompressDataAsync(data, HostConfig.ExecutableName, HostConfig.ExecutableDirectory, package);
        }

        /// <summary>
        /// 异步解压完成
        /// </summary>
        /// <param name="e">结果</param>
        protected virtual void DecompressCompleted(DecompressDataCompletedEventArgs e)
        {
            //验证
            IPackage package = e.UserState as IPackage;
            if (package == null)
            {
                this.OnError(new ErrorEventArgs("无效的解压操作。"));
                return;
            }
            //用户取消
            if (e.Cancelled)
            {
                this.OnNotify(new NotifyEventArgs("已取消更新。"));
                return;
            }
            //出错
            if (e.Error != null)
            {
                this.OnError(new ErrorEventArgs("解压 {0} 失败:{1}。", package.FileName, e.Error.Message.TrimEnd(PERIOD)));
                return;
            }
            //继续下一个
            this.DownloadAsync();
        }
    }
}
