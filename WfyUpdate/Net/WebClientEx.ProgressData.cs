namespace WfyUpdate.Net
{
    partial class WebClientEx
    {
        /// <summary>
        /// 进度数据
        /// </summary>
        private class ProgressData
        {
            /// <summary>
            /// 已完成字节数
            /// </summary>
            internal long Completed = 0;

            /// <summary>
            /// 所有要处理的字节数
            /// </summary>
            internal long ToComplete = -1;

            /// <summary>
            /// 重置计数
            /// </summary>
            internal void Reset()
            {
                this.Completed = 0;
                this.ToComplete = -1;
            }
        }
    }
}
