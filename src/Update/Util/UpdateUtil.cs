using Update.Config;

namespace Update.Util
{
    /// <summary>
    /// 更新工具类
    /// </summary>
    public static class UpdateUtil
    {
        /// <summary>
        /// 检查更新,不允许取消更新
        /// </summary>
        /// <returns>true 表示程序应继续运行,否则程序应退出</returns>
        public static bool CheckUpdate()
        {
            return CheckUpdate(false);
        }

        /// <summary>
        /// 检查更新,可指定是否允许取消更新
        /// </summary>
        /// <param name="allowCancel">是否允许取消更新</param>
        /// <returns>true 表示程序应继续运行,否则程序应退出</returns>
        public static bool CheckUpdate(bool allowCancel)
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
                return allowCancel;
            //发现新版本
            if (!uptodate)
                ProcessUtil.Start(AppConfig.AssemblyPath, HostConfig.ExecutablePath);
            return uptodate;
        }
    }
}
