using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace WfyUpdate.Net
{
    /// <summary>
    /// 异步操作网络客户端 by 许崇雷
    /// </summary>
    [ToolboxItem(false)]
    public partial class WebClientEx : WebClient
    {
        private bool m_InitAsync;           //是否已初始化异步操作
        private bool m_Cancelled;           //是否取消异步操作
        private int m_CallNesting;          //如果大于 0 表示有异步工作正在进行
        private AsyncOperation m_AsyncOp;   //异步操作生存期
        private ProgressData m_Progress;    //异步操作进度数据


        #region 属性

        /// <summary>
        /// 获取是否正在进行异步操作
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
            this.m_KillProcessStart = new KillProcessStartDelegate(this.KillProcessWork);
            this.m_ReportKillProgressChanged = new SendOrPostCallback(this.ReportKillProgressChanged);
            this.m_KillProcessOperationCompleted = new SendOrPostCallback(this.KillProcessOperationCompleted);
            this.m_DecompressDataStart = new DecompressDataStartDelegate(this.DecompressDataWork);
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
                throw new NotSupportedException("无效的操作，操作占用中。");
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


        #region 执行回调

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

        #endregion


        #region 公共方法

        /// <summary>
        /// 取消异步操作
        /// </summary>
        public new virtual void CancelAsync()
        {
            this.m_Cancelled = true;
            base.CancelAsync();
        }

        #endregion
    }
}
