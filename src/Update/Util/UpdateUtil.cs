namespace Update.Util
{
    /// <summary>
    /// 更新工具类
    /// </summary>
    public static class UpdateUtil
    {
        /// <summary>
        /// 检查更新,可指定是否允许取消更新
        /// </summary>
        /// <param name="allowCancel">是否允许取消更新</param>
        /// <returns>true 表示程序应继续运行,否则程序应退出</returns>
        public static bool CheckUpdate(bool allowCancel = false)
        {
            //检查更新
            using (FrmUpdate dialog = new FrmUpdate())
            {
                return dialog.CheckUpdate(allowCancel);
            }
        }
    }
}
