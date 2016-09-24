using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using WfyUpdate.Config;
using WfyUpdate.Model;
using WfyUpdate.Update.Event;
using WfyUpdate.Util;

namespace WfyUpdate.Update
{
    /// <summary>
    /// 更新器 by 许崇雷
    /// 默认字符串编码为 UTF-8
    /// </summary>
    [ToolboxItem(false)]
    public sealed class Updater : WebClient, IUpdater
    {
        private static readonly char[] PERIOD = { '.', '。' };//句号
        private static readonly string PACKAGES = "packages.txt";//服务端配置文件名
        private IEnumerator<UpdatePackage> m_AvaliableEnumerator;//更新枚举器

        #region 事件

        //错误
        private static readonly object EVENT_ERROR = new object();
        /// <summary>
        /// 错误时间
        /// </summary>
        public event ErrorEventHandler Error
        {
            add
            {
                this.Events.AddHandler(EVENT_ERROR, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_ERROR, value);
            }
        }
        private void OnError(ErrorEventArgs e)
        {
            ErrorEventHandler handler = this.Events[EVENT_ERROR] as ErrorEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //通知
        private static readonly object EVENT_NOTIFY = new object();
        /// <summary>
        /// 通知事件
        /// </summary>
        public event NotifyEventHandler Notify
        {
            add
            {
                this.Events.AddHandler(EVENT_NOTIFY, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_NOTIFY, value);
            }
        }
        private void OnNotify(NotifyEventArgs e)
        {
            NotifyEventHandler handler = this.Events[EVENT_NOTIFY] as NotifyEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //开始更新
        private static readonly object EVENT_UPDATING = new object();
        /// <summary>
        /// 开始下载
        /// </summary>
        public event UpdatedEventHandler Updating
        {
            add
            {
                this.Events.AddHandler(EVENT_UPDATING, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_UPDATING, value);
            }
        }
        private void OnUpdating(UpdatingEventArgs e)
        {
            UpdatingEventHandler handler = this.Events[EVENT_UPDATING] as UpdatingEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //更新完成
        private static readonly object EVENT_UPDATED = new object();
        /// <summary>
        /// 下载完成
        /// </summary>
        public event UpdatedEventHandler Updated
        {
            add
            {
                this.Events.AddHandler(EVENT_UPDATED, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_UPDATED, value);
            }
        }
        private void OnUpdated(UpdatedEventArgs e)
        {
            UpdatedEventHandler handler = this.Events[EVENT_UPDATED] as UpdatedEventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion


        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Updater()
        {
            this.Encoding = System.Text.Encoding.UTF8;
        }

        #endregion


        #region 接口方法

        /// <summary>
        /// 检查是否有可用更新
        /// </summary>
        /// <returns>有更新返回 true,否则返回 false</returns>
        public bool CheckUpdate()
        {
            this.DisposeResource();
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
                packagesStr = this.DownloadString(HostConfig.UpdateUrl + PACKAGES);
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
        /// 开始异步更新
        /// </summary>
        public void StartUpdate()
        {
            this.BeginCheck();
        }

        /// <summary>
        /// 停止异步更新
        /// </summary>
        public void StopUpdate()
        {
            this.CancelAsync();
        }

        #endregion


        #region 实现方法

        //异步开始检查
        private void BeginCheck()
        {
            this.DisposeResource();
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
            this.DownloadStringAsync(new Uri(HostConfig.UpdateUrl + PACKAGES));
        }

        //异步检查结束
        private void EndCheck(DownloadStringCompletedEventArgs e)
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
            this.m_AvaliableEnumerator = avaliables.GetEnumerator();
            //开始下载
            this.OnUpdating(new UpdatingEventArgs(avaliables));
            this.BeginDownload();
        }

        //异步开始下载
        private void BeginDownload()
        {
            //验证
            if (this.m_AvaliableEnumerator == null)
            {
                this.OnError(new ErrorEventArgs("必须先检查更新。"));
                return;
            }
            //下一个
            if (!this.m_AvaliableEnumerator.MoveNext())
            {
                this.OnNotify(new NotifyEventArgs("更新完成。"));
                AppRunner.Start(HostConfig.ExecutablePath);
                this.OnUpdated(new UpdatedEventArgs());
                return;
            }
            UpdatePackage package = this.m_AvaliableEnumerator.Current;
            this.OnNotify(new NotifyEventArgs("正在下载 {0}。", package.FileName));
            this.DownloadDataAsync(new Uri(HostConfig.UpdateUrl + package.FileName), package);
        }

        //异步下载结束
        private void EndDownload(DownloadDataCompletedEventArgs e)
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
                CompressUtil.Decompress(e.Result, HostConfig.ExecutableDirectory);
            }
            catch (Exception exp)
            {
                this.OnError(new ErrorEventArgs("解压 {0} 失败:{1}。", package.FileName, exp.Message.TrimEnd(PERIOD)));
                return;
            }
            //继续下一个
            this.BeginDownload();
        }

        //释放资源
        private void DisposeResource()
        {
            if (this.m_AvaliableEnumerator != null)
            {
                this.m_AvaliableEnumerator.Dispose();
                this.m_AvaliableEnumerator = null;
            }
        }

        #endregion


        #region 重写方法

        //重写
        protected override void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
        {
            base.OnDownloadStringCompleted(e);
            this.EndCheck(e);
        }

        //重写
        protected override void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
        {
            base.OnDownloadDataCompleted(e);
            this.EndDownload(e);
        }

        //重写
        protected override void Dispose(bool disposing)
        {
            this.DisposeResource();
            base.Dispose(disposing);
        }

        #endregion
    }
}
