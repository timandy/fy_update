namespace Update.Core
{
    partial class Updater
    {
        /// <summary>
        /// 开始异步更新
        /// </summary>
        public void StartUpdate()
        {
            this.CheckDisposed();
            this.ClientCheckAsync();
        }

        /// <summary>
        /// 停止异步更新
        /// </summary>
        public void StopUpdate()
        {
            this.CheckDisposed();
            this.m_Client.CancelAsync();
        }
    }
}
