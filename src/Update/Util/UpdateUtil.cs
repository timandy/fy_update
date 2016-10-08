using System.Windows.Forms;

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
        /// <returns>true 表示已是最新版本,否则表示用户取消或发生错误或有新版本</returns>
        public static bool CheckUpdate()
        {
            using (FrmUpdate dialog = new FrmUpdate { CheckMode = true })
            {
                dialog.ShowDialog();
                //用户取消或发生错误
                if (!dialog.CheckCompleted)
                    return false;
                //已是最新版本
                if (dialog.Uptodate)
                {
                    return true;
                }
                else
                {
                    ProcessUtil.Start(FilePathUtil.GetAbsolutePath("Update.exe"), Application.ExecutablePath);
                    return false;
                }
            }
        }
    }
}
