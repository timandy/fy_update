using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace WfyUpdate.Net
{
    partial class WebClientEx
    {
        private KillProcessStartDelegate m_KillProcessStart;            //异步结束进程委托
        private SendOrPostCallback m_ReportKillProgressChanged;         //异步结束进程报告回调
        private SendOrPostCallback m_KillProcessOperationCompleted;     //异步结束进程完成回调


        #region 事件

        //结束进程进度
        private static readonly object EVENT_KILL_PROGRESS_CHANGED = new object();
        /// <summary>
        /// 结束进程进度事件
        /// </summary>
        public event KillProgressChangedEventHandler KillProgressChanged
        {
            add { this.Events.AddHandler(EVENT_KILL_PROGRESS_CHANGED, value); }
            remove { this.Events.RemoveHandler(EVENT_KILL_PROGRESS_CHANGED, value); }
        }
        /// <summary>
        /// 触发结束进程进度事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnKillProgressChanged(KillProgressChangedEventArgs e)
        {
            KillProgressChangedEventHandler handler = this.Events[EVENT_KILL_PROGRESS_CHANGED] as KillProgressChangedEventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 触发结束进程进度事件
        /// </summary>
        /// <param name="state">数据</param>
        private void ReportKillProgressChanged(object state)
        {
            this.OnKillProgressChanged(state as KillProgressChangedEventArgs);
        }

        //结束进程完成
        private static readonly object EVENT_KILL_PROCESS_COMPLETED = new object();
        /// <summary>
        /// 结束进程完成事件
        /// </summary>
        public event KillProcessCompletedEventHandler KillProcessCompleted
        {
            add { this.Events.AddHandler(EVENT_KILL_PROCESS_COMPLETED, value); }
            remove { this.Events.RemoveHandler(EVENT_KILL_PROCESS_COMPLETED, value); }
        }
        /// <summary>
        /// 触发结束进程完成事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnKillProcessCompleted(KillProcessCompletedEventArgs e)
        {
            KillProcessCompletedEventHandler handler = this.Events[EVENT_KILL_PROCESS_COMPLETED] as KillProcessCompletedEventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 触发结束进程完成事件
        /// </summary>
        /// <param name="state">数据</param>
        private void KillProcessOperationCompleted(object state)
        {
            this.OnKillProcessCompleted(state as KillProcessCompletedEventArgs);
        }

        #endregion


        #region 执行回调

        /// <summary>
        /// 执行结束进度报告回调
        /// </summary>
        /// <param name="progress">进度数据</param>
        /// <param name="asyncOp">异步生存期</param>
        private void PostKillProgressChanged(ProgressData progress, AsyncOperation asyncOp)
        {
            if (asyncOp == null)
                return;
            int progressPercentage = progress.ToComplete < 0L ? 0 : (progress.ToComplete == 0L ? 100 : (int)(100L * progress.Completed / progress.ToComplete));
            KillProgressChangedEventArgs eventArgs = new KillProgressChangedEventArgs(progressPercentage, asyncOp.UserSuppliedState);
            this.InvokeOperation(asyncOp, this.m_ReportKillProgressChanged, eventArgs);
        }

        /// <summary>
        /// 执行结束进程完成回调
        /// </summary>
        /// <param name="error">错误</param>
        /// <param name="cancelled">是否取消</param>
        /// <param name="asyncOp">异步生存期</param>
        private void KillProcessAsyncCallback(Exception error, bool cancelled, AsyncOperation asyncOp)
        {
            KillProcessCompletedEventArgs eventArgs = new KillProcessCompletedEventArgs(error, cancelled, asyncOp.UserSuppliedState);
            this.InvokeOperationCompleted(asyncOp, this.m_KillProcessOperationCompleted, eventArgs);
        }

        #endregion


        #region 工作方法

        /// <summary>
        /// 结束进程操作
        /// </summary>
        /// <param name="e">参数</param>
        private void KillProcessWork(KillProcessStartArgs e)
        {
            Exception error = null;
            bool cancelled = false;
            try
            {
                Process[] processes = Process.GetProcesses();
                this.m_Progress.ToComplete = processes.Length;
                foreach (Process process in processes)
                {
                    if (this.m_Cancelled)
                    {
                        cancelled = true;
                        return;
                    }
                    try
                    {
                        if (process.MainModule.FileName.StartsWith(e.Directory, StringComparison.CurrentCultureIgnoreCase))//进程文件在指定目录内
                            process.Kill();
                    }
                    catch
                    {
                    }
                    this.m_Progress.Completed++;
                    this.PostKillProgressChanged(this.m_Progress, this.m_AsyncOp);
                }
            }
            catch (Exception exp)
            {
                error = exp;
            }
            finally
            {
                this.KillProcessAsyncCallback(error, cancelled, this.m_AsyncOp);
            }
        }

        #endregion


        #region 公共方法

        /// <summary>
        /// 开始异步结束进程
        /// </summary>
        /// <param name="directory">进程所在目录</param>
        /// <param name="userToken">用户数据</param>
        public virtual void KillProcessAsync(string directory, object userToken = null)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");
            this.InitAsync();
            this.ClearState();
            AsyncOperation operation = AsyncOperationManager.CreateOperation(userToken);
            this.m_AsyncOp = operation;
            try
            {
                this.m_KillProcessStart.BeginInvoke(new KillProcessStartArgs(operation, directory), null, null);
            }
            catch (Exception error)
            {
                if (error is ThreadAbortException || error is StackOverflowException || error is OutOfMemoryException)
                    throw;
                this.KillProcessAsyncCallback(error, false, operation);
            }
        }

        #endregion
    }
}
