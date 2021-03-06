﻿namespace Update.Core
{
    partial class Updater
    {
        /// <summary>
        /// 释放更新包枚举器
        /// </summary>
        protected virtual void DisposeAvaliables()
        {
            if (this.m_Avaliables != null)
            {
                this.m_Avaliables.Dispose();
                this.m_Avaliables = null;
            }
        }

        /// <summary>
        /// 释放 WebClient
        /// </summary>
        protected virtual void DisposeClient()
        {
            if (this.m_Client != null)
            {
                this.m_Client.Dispose();
                this.m_Client = null;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">释放托管资源为true,否则为false</param>
        protected override void Dispose(bool disposing)
        {
            this.DisposeAvaliables();
            this.DisposeClient();
        }
    }
}
