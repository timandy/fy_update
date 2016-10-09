using Update.Config;

namespace Update.Util
{
    /// <summary>
    /// 更新工具类
    /// </summary>
    public static class UpdateUtil
    {
        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns>true 表示已是最新版本,否则表示用户取消或发生错误或发现新版本</returns>
        public static bool CheckUpdate()
        {
            //声明
            bool completed;
            bool uptodate;
            //检查更新
            using (FrmUpdate dialog = new FrmUpdate { CheckMode = true })
            {
                dialog.ShowDialog();
                completed = dialog.CheckCompleted;
                uptodate = dialog.Uptodate;
            }
            //用户取消或发生错误
            if (!completed)
                return false;
            //发现新版本
            if (!uptodate)
                ProcessUtil.Start(AppConfig.AssemblyPath, HostConfig.ExecutablePath);
            return uptodate;
        }
    }
}
