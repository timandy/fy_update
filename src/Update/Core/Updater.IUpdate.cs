namespace Update.Core
{
    partial class Updater
    {
        /// <summary>
        /// 检查是否有可用更新
        /// </summary>
        /// <returns>有更新返回 true,否则返回 false</returns>
        public bool CheckUpdate()
        {
            this.CheckDisposed();
            return this.Check();
        }

        /// <summary>
        /// 开始异步更新
        /// </summary>
        public void StartUpdate()
        {
            this.CheckDisposed();
            this.CheckAsync();
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
