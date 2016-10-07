namespace Update.Core
{
    /// <summary>
    /// 更新器接口
    /// </summary>
    public interface IUpdater
    {
        /// <summary>
        /// 检查是否有可用更新
        /// </summary>
        /// <returns>有更新返回 true,否则返回 false</returns>
        bool CheckUpdate();

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
