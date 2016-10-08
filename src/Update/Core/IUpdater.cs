namespace Update.Core
{
    /// <summary>
    /// 更新器接口
    /// </summary>
    public interface IUpdater
    {
        /// <summary>
        /// 开始异步更新
        /// </summary>
        void StartUpdate();

        /// <summary>
        /// 停止异步更新
        /// </summary>
        void StopUpdate();
    }
}
