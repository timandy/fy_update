using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using SharpCompress.Archive;
using SharpCompress.Common;

namespace WfyUpdate.Net
{
    /// <summary>
    /// 支持异步解压的客户端 by 许崇雷
    /// </summary>
    [ToolboxItem(false)]
    public class WebClientEx : WebClient
    {
        private bool m_InitAsync;                                       //是否已初始化异步操作
        private bool m_Cancelled;                                       //是否取消异步操作
        private int m_CallNesting;                                      //如果大于 0 表示有异步工作正在进行
        private AsyncOperation m_AsyncOp;                               //异步操作生存期
        private ProgressData m_Progress;                                //异步操作进度数据
        private DecompressStartDelegate m_DecompressStart;              //异步解压委托
        private SendOrPostCallback m_ReportDecompressProgressChanged;   //解压进度报告回调
        private SendOrPostCallback m_DecompressDataOperationCompleted;  //解压操作完成回调

        #region 属性

        /// <summary>
        /// 是否正在进行异步操作
        /// </summary>
        public new virtual bool IsBusy
        {
            get
            {
                return base.IsBusy || this.m_AsyncOp != null;
            }
        }

        #endregion


        #region 状态

        /// <summary>
        /// 初始化异步操作
        /// </summary>
        private void InitAsync()
        {
            if (this.m_InitAsync)
                return;
            this.m_Progress = new ProgressData();
            this.m_DecompressStart = new DecompressStartDelegate(this.DecompressWork);
            this.m_ReportDecompressProgressChanged = new SendOrPostCallback(this.ReportDecompressProgressChanged);
            this.m_DecompressDataOperationCompleted = new SendOrPostCallback(this.DecompressDataOperationCompleted);
            this.m_InitAsync = true;
        }

        /// <summary>
        /// 清空状态
        /// </summary>
        private void ClearState()
        {
            if (this.AnotherCallInProgress(Interlocked.Increment(ref this.m_CallNesting)))
            {
                this.CompleteState();
                throw new NotSupportedException("无效的操作，IO 正忙。");
            }
            this.m_Cancelled = false;
            if (this.m_Progress != null)
                this.m_Progress.Reset();
        }

        /// <summary>
        /// 完成状态
        /// </summary>
        private void CompleteState()
        {
            Interlocked.Decrement(ref m_CallNesting);
        }

        /// <summary>
        /// 是否有其他异步操作正在进行
        /// </summary>
        /// <param name="callNesting">计数</param>
        /// <returns>有返回true,否则返回false</returns>
        private bool AnotherCallInProgress(int callNesting)
        {
            return callNesting > 1;
        }

        #endregion


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
        /// <param name="e"></param>
        protected virtual void OnDecompressProgressChanged(DecompressProgressChangedEventArgs e)
        {
            DecompressProgressChangedEventHandler handler = this.Events[EVENT_DECOMPRESS_PROGRESS_CHANGED] as DecompressProgressChangedEventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 触发解压进度事件
        /// </summary>
        /// <param name="state"></param>
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
        /// <param name="e"></param>
        protected virtual void OnDecompressDataCompleted(DecompressDataCompletedEventArgs e)
        {
            DecompressDataCompletedEventHandler handler = this.Events[EVENT_DECOMPRESS_DATA_COMPLETED] as DecompressDataCompletedEventHandler;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// 触发解压完成事件
        /// </summary>
        /// <param name="state"></param>
        private void DecompressDataOperationCompleted(object state)
        {
            this.OnDecompressDataCompleted(state as DecompressDataCompletedEventArgs);
        }

        #endregion


        #region 公共接口

        /// <summary>
        /// 开始异步解压
        /// </summary>
        /// <param name="data">要解压的数据</param>
        /// <param name="lastEntry">最后一个解压的文件</param>
        /// <param name="destinationDirectory">要解压到的目录</param>
        /// <param name="userToken">用户数据</param>
        public virtual void DecompressDataAsync(byte[] data, string lastEntry, string destinationDirectory, object userToken)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            this.InitAsync();
            this.ClearState();
            AsyncOperation operation = AsyncOperationManager.CreateOperation(userToken);
            this.m_AsyncOp = operation;
            try
            {
                this.m_DecompressStart.BeginInvoke(new DecompressStartArgs(operation, data, lastEntry, destinationDirectory), null, null);
            }
            catch (Exception error)
            {
                if (error is ThreadAbortException || error is StackOverflowException || error is OutOfMemoryException)
                    throw;
                this.DecompressDataAsyncCallback(error, false, operation);
            }
        }

        /// <summary>
        /// 取消异步操作
        /// </summary>
        public new virtual void CancelAsync()
        {
            this.m_Cancelled = true;
            base.CancelAsync();
        }

        #endregion


        #region 执行回调

        /// <summary>
        /// 执行进度报告回调
        /// </summary>
        /// <param name="progress">进度数据</param>
        /// <param name="asyncOp">异步生存期</param>
        private void PostProgressChanged(ProgressData progress, AsyncOperation asyncOp)
        {
            if (asyncOp == null)
                return;
            int progressPercentage = progress.TotalBytesToComplete < 0L ? 0 : (progress.TotalBytesToComplete == 0L ? 100 : (int)(100L * progress.BytesCompleted / progress.TotalBytesToComplete));
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


        #region 辅助方法

        /// <summary>
        /// 往异步开始线程发送回调
        /// </summary>
        /// <param name="asyncOp">异步生存期</param>
        /// <param name="callback">要发送的委托</param>
        /// <param name="eventArgs">数据</param>
        private void InvokeOperation(AsyncOperation asyncOp, SendOrPostCallback callback, object eventArgs)
        {
            asyncOp.Post(callback, eventArgs);
        }

        /// <summary>
        /// 往异步开始线程发送完成
        /// </summary>
        /// <param name="asyncOp">异步生存期</param>
        /// <param name="callback">要发送的委托</param>
        /// <param name="eventArgs">数据</param>
        private void InvokeOperationCompleted(AsyncOperation asyncOp, SendOrPostCallback callback, AsyncCompletedEventArgs eventArgs)
        {
            if (Interlocked.CompareExchange(ref this.m_AsyncOp, null, asyncOp) == asyncOp)
            {
                this.CompleteState();
                asyncOp.PostOperationCompleted(callback, eventArgs);
            }
        }

        /// <summary>
        /// 解压操作
        /// </summary>
        /// <param name="e">参数</param>
        private void DecompressWork(DecompressStartArgs e)
        {
            Exception error = null;
            bool cancelled = false;
            try
            {
                using (MemoryStream stream = new MemoryStream(e.Data))
                {
                    using (IArchive archive = ArchiveFactory.Open(stream))
                    {
                        this.m_Progress.TotalBytesToComplete = archive.TotalUncompressSize;
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
                            entry.WriteToDirectory(e.DestinationDirectory, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                            this.m_Progress.BytesCompleted += entry.Size;
                            this.PostProgressChanged(this.m_Progress, this.m_AsyncOp);
                        }
                        if (last != null)
                        {
                            if (this.m_Cancelled)
                            {
                                cancelled = true;
                                return;
                            }
                            last.WriteToDirectory(e.DestinationDirectory, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                            this.m_Progress.BytesCompleted += last.Size;
                            this.PostProgressChanged(this.m_Progress, this.m_AsyncOp);
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


        #region 内部类

        /// <summary>
        /// 进度数据
        /// </summary>
        private class ProgressData
        {
            /// <summary>
            /// 已完成字节数
            /// </summary>
            internal long BytesCompleted = 0;
            /// <summary>
            /// 所有要处理的字节数
            /// </summary>
            internal long TotalBytesToComplete = -1;

            /// <summary>
            /// 重置计数
            /// </summary>
            internal void Reset()
            {
                this.BytesCompleted = 0;
                this.TotalBytesToComplete = -1;
            }
        }

        #endregion
    }
}
