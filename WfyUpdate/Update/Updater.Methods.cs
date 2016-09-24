using System;
using System.ComponentModel;
using System.Net;
using WfyUpdate.Config;
using WfyUpdate.Model;
using WfyUpdate.Update.Event;
using WfyUpdate.Util;

namespace WfyUpdate.Update
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
            string packagesStr;
            try
            {
                packagesStr = this.m_WebClient.DownloadString(HostConfig.UpdateUrl + PACKAGES);
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("下载更新信息失败:{0}。", exp.Message.TrimEnd(PERIOD)));
            }
            UpdatePackageCollection packages;
            try
            {
                packages = UpdatePackageParser.Parse(packagesStr);
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("解析更新信息失败:{0}。", exp.Message.TrimEnd(PERIOD)));
            }
            return packages.Contains(HostConfig.CurrentVersion);
        }

        /// <summary>
        /// 异步检查开始
        /// </summary>
        protected virtual void BeginCheck()
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
        /// 异步检查结束
        /// </summary>
        /// <param name="e">结果</param>
        protected virtual void EndCheck(DownloadStringCompletedEventArgs e)
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
            UpdatePackageCollection packages;
            try
            {
                packages = UpdatePackageParser.Parse(e.Result);
            }
            catch (Exception exp)
            {
                this.OnError(new ErrorEventArgs("解析更新信息失败:{0}。", exp.Message.TrimEnd(PERIOD)));
                return;
            }
            //已最新
            if (!packages.Contains(HostConfig.CurrentVersion))
            {
                this.OnNotify(new NotifyEventArgs("已是最新版本。"));
                return;
            }
            //有新版本
            UpdatePackageCollection avaliables = UpdatePackageParser.GetAvaliablePackages(packages);
            this.m_Avaliables = avaliables.GetEnumerator();
            //开始下载
            this.OnUpdateStarted(new UpdateStartedEventArgs(avaliables));
            this.BeginDownload();
        }

        /// <summary>
        /// 异步下载开始
        /// </summary>
        protected virtual void BeginDownload()
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
            UpdatePackage package = this.m_Avaliables.Current;
            this.OnNotify(new NotifyEventArgs("正在下载 {0}。", package.FileName));
            this.m_WebClient.DownloadDataAsync(new Uri(HostConfig.UpdateUrl + package.FileName), package);
        }

        /// <summary>
        /// 异步下载结束
        /// </summary>
        /// <param name="e">结果</param>
        protected virtual void EndDownload(DownloadDataCompletedEventArgs e)
        {
            //验证
            UpdatePackage package = e.UserState as UpdatePackage;
            if (package == null)
            {
                this.OnError(new ErrorEventArgs("无效的操作。"));
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
            //解压
            try
            {
                this.OnNotify(new NotifyEventArgs("正在解压 {0}。", package.FileName));
                CompressUtil.Decompress(e.Result, HostConfig.ExecutableDirectory, HostConfig.ExecutableName);
            }
            catch (Exception exp)
            {
                this.OnError(new ErrorEventArgs("解压 {0} 失败:{1}。", package.FileName, exp.Message.TrimEnd(PERIOD)));
                return;
            }
            //继续下一个
            this.BeginDownload();
        }

        /// <summary>
        /// 异步解压开始
        /// </summary>
        /// <param name="buffer">要解压的数据</param>
        protected virtual void BeginDecompress(byte[] buffer)
        {
            //TODO
        }

        /// <summary>
        /// 异步解压结束
        /// </summary>
        /// <param name="e">结果</param>
        protected virtual void EndDecompress(AsyncCompletedEventArgs e)
        {
            //TODO
        }
    }
}
