using System;
using System.Windows.Forms;

namespace WfyUpdate.Util
{
    /// <summary>
    /// 应用工具
    /// </summary>
    public static class AppUtil
    {
        /// <summary>
        /// 空闲时执行指定方法
        /// </summary>
        /// <param name="action">方法</param>
        public static void Idle(Action action)
        {
            EventHandler handler = null;
            handler = (sender, e) =>
            {
                Application.Idle -= handler;
                action();
            };
            Application.Idle += handler;
        }
    }
}
