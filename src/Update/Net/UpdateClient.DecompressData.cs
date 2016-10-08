using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using SharpCompress.Archives;
using SharpCompress.Readers;
using Update.Net.Events;

namespace Update.Net
{
    partial class UpdateClient
    {
        private DecompressDataStartDelegate m_DecompressDataStart;          //异步解压委托
        private SendOrPostCallback m_ReportDecompressProgressChanged;       //解压进度报告回调
        private SendOrPostCallback m_DecompressDataOperationCompleted;      //解压操作完成回调


        #region 事件

        //解压进度
        private static readonly object EVENT_DECOMPRESS_PROGRESS_CHANGED = new object();
        /// <summary>
        /// 解压进度事件
        /// </summary>
        public event DecompressProgressChangedEventHandler DecompressProgressChanged
        {
            add { this.Events.AddHandler(EVENT_DECOMPRESS_PROGRESS_CHANGED, value); }
            remove { this.Events.RemoveHandler(EVENT_DECOMPRESS_PROGRESS_CHANGED, value); }
        }
        /// <summary>
        /// 触发解压进度事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnDecompressProgressChanged(DecompressProgressChangedEventArgs e)
        {
            DecompressProgressChangedEventHandler handler = this.Events[EVENT_DECOMPRESS_PROGRESS_CHANGED] as DecompressProgressChangedEventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 触发解压进度事件
        /// </summary>
        /// <param name="state">数据</param>
        private void ReportDecompressProgressChanged(object state)
        {
            this.OnDecompressProgressChanged(state as DecompressProgressChangedEventArgs);
        }

        //解压完成
        private static readonly object EVENT_DECOMPRESS_DATA_COMPLETED = new object();
        /// <summary>
        /// 解压完成事件
        /// </summary>
        public event DecompressDataCompletedEventHandler DecompressDataCompleted
        {
            add { this.Events.AddHandler(EVENT_DECOMPRESS_DATA_COMPLETED, value); }
            remove { this.Events.RemoveHandler(EVENT_DECOMPRESS_DATA_COMPLETED, value); }
        }
        /// <summary>
        /// 触发解压完成事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnDecompressDataCompleted(DecompressDataCompletedEventArgs e)
        {
            DecompressDataCompletedEventHandler handler = this.Events[EVENT_DECOMPRESS_DATA_COMPLETED] as DecompressDataCompletedEventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 触发解压完成事件
        /// </summary>
        /// <param name="state">数据</param>
        private void DecompressDataOperationCompleted(object state)
        {
            this.OnDecompressDataCompleted(state as DecompressDataCompletedEventArgs);
        }

        #endregion


        #region 执行回调

        /// <summary>
        /// 执行解压进度报告回调
        /// </summary>
        /// <param name="progress">进度数据</param>
        /// <param name="asyncOp">异步生存期</param>
        private void PostDecompressProgressChanged(ProgressData progress, AsyncOperation asyncOp)
        {
            if (asyncOp == null)
                return;
            int progressPercentage = progress.ToComplete < 0L ? 0 : (progress.ToComplete == 0L ? 100 : (int)(100L * progress.Completed / progress.ToComplete));
            DecompressProgressChangedEventArgs eventArgs = new DecompressProgressChangedEventArgs(progressPercentage, asyncOp.UserSuppliedState);
            this.InvokeOperation(asyncOp, this.m_ReportDecompressProgressChanged, eventArgs);
        }

        /// <summary>
        /// 执行解压数据完成回调
        /// </summary>
        /// <param name="error">错误</param>
        /// <param name="cancelled">是否取消</param>
        /// <param name="asyncOp">异步生存期</param>
        private void DecompressDataAsyncCallback(Exception error, bool cancelled, AsyncOperation asyncOp)
        {
            DecompressDataCompletedEventArgs eventArgs = new DecompressDataCompletedEventArgs(error, cancelled, asyncOp.UserSuppliedState);
            this.InvokeOperationCompleted(asyncOp, this.m_DecompressDataOperationCompleted, eventArgs);
        }

        #endregion


        #region 工作方法

        /// <summary>
        /// 解压数据操作
        /// </summary>
        /// <param name="e">参数</param>
        private void DecompressDataWork(DecompressDataStartArgs e)
        {
            Exception error = null;
            bool cancelled = false;
            try
            {
                using (MemoryStream stream = new MemoryStream(e.Data))
                {
                    using (IArchive archive = ArchiveFactory.Open(stream))
                    {
                        this.m_Progress.ToComplete = archive.TotalUncompressSize;
                        ExtractionOptions options = new ExtractionOptions { ExtractFullPath = true, Overwrite = true, PreserveFileTime = true };
                        IArchiveEntry last = null;
                        foreach (IArchiveEntry entry in archive.Entries)
                        {
                            if (this.m_Cancelled)
                            {
                                cancelled = true;
                                return;
                            }
                            if (entry.IsDirectory)
                                continue;
                            if (last == null && entry.Key.Equals(e.LastEntry, StringComparison.OrdinalIgnoreCase))
                            {
                                last = entry;
                                continue;
                            }
                            entry.WriteToDirectory(e.DestinationDirectory, options);
                            this.m_Progress.Completed += entry.Size;
                            this.PostDecompressProgressChanged(this.m_Progress, this.m_AsyncOp);
                        }
                        if (last != null)
                        {
                            if (this.m_Cancelled)
                            {
                                cancelled = true;
                                return;
                            }
                            last.WriteToDirectory(e.DestinationDirectory, options);
                            this.m_Progress.Completed += last.Size;
                            this.PostDecompressProgressChanged(this.m_Progress, this.m_AsyncOp);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                error = exp;
            }
            finally
            {
                this.DecompressDataAsyncCallback(error, cancelled, this.m_AsyncOp);
            }
        }

        #endregion


        #region 公共方法

        /// <summary>
        /// 开始异步解压
        /// </summary>
        /// <param name="data">要解压的数据</param>
        /// <param name="lastEntry">最后一个解压的文件</param>
        /// <param name="destinationDirectory">要解压到的目录</param>
        /// <param name="userToken">用户数据</param>
        public virtual void DecompressDataAsync(byte[] data, string lastEntry, string destinationDirectory, object userToken = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.InitAsync();
            this.ClearState();
            AsyncOperation operation = AsyncOperationManager.CreateOperation(userToken);
            this.m_AsyncOp = operation;
            try
            {
                this.m_DecompressDataStart.BeginInvoke(new DecompressDataStartArgs(operation, data, lastEntry, destinationDirectory), null, null);
            }
            catch (Exception error)
            {
                if (error is ThreadAbortException || error is StackOverflowException || error is OutOfMemoryException)
                    throw;
                this.DecompressDataAsyncCallback(error, false, operation);
            }
        }

        #endregion
    }
}
