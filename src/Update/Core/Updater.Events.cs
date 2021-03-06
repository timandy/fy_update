﻿using Update.Core.Events;

namespace Update.Core
{
    partial class Updater
    {
        //错误
        private static readonly object EVENT_ERROR = new object();
        /// <summary>
        /// 错误事件
        /// </summary>
        public event ErrorEventHandler Error
        {
            add
            {
                this.Events.AddHandler(EVENT_ERROR, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_ERROR, value);
            }
        }
        /// <summary>
        /// 触发错误事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnError(ErrorEventArgs e)
        {
            ErrorEventHandler handler = this.Events[EVENT_ERROR] as ErrorEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //通知
        private static readonly object EVENT_NOTIFY = new object();
        /// <summary>
        /// 通知事件
        /// </summary>
        public event NotifyEventHandler Notify
        {
            add
            {
                this.Events.AddHandler(EVENT_NOTIFY, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_NOTIFY, value);
            }
        }
        /// <summary>
        /// 触发通知事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnNotify(NotifyEventArgs e)
        {
            NotifyEventHandler handler = this.Events[EVENT_NOTIFY] as NotifyEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //进度
        private static readonly object EVENT_PROGRESS = new object();
        /// <summary>
        /// 进度事件
        /// </summary>
        public event ProgressEventHandler Progress
        {
            add
            {
                this.Events.AddHandler(EVENT_PROGRESS, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_PROGRESS, value);
            }
        }
        /// <summary>
        /// 触发进度事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnProgress(ProgressEventArgs e)
        {
            ProgressEventHandler handler = this.Events[EVENT_PROGRESS] as ProgressEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //检查开始
        private static readonly object EVENT_CHECK_STARTED = new object();
        /// <summary>
        /// 开始检查
        /// </summary>
        public event CheckStartedEventHandler CheckStarted
        {
            add
            {
                this.Events.AddHandler(EVENT_CHECK_STARTED, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_CHECK_STARTED, value);
            }
        }
        /// <summary>
        /// 触发检查开始事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnCheckStarted(CheckStartedEventArgs e)
        {
            CheckStartedEventHandler handler = this.Events[EVENT_CHECK_STARTED] as CheckStartedEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //检查完成
        private static readonly object EVENT_CHECK_COMPLETED = new object();
        /// <summary>
        /// 检查完成事件
        /// </summary>
        public event CheckCompletedEventHandler CheckCompleted
        {
            add
            {
                this.Events.AddHandler(EVENT_CHECK_COMPLETED, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_CHECK_COMPLETED, value);
            }
        }
        /// <summary>
        /// 触发检查完成事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnCheckCompleted(CheckCompletedEventArgs e)
        {
            CheckCompletedEventHandler handler = this.Events[EVENT_CHECK_COMPLETED] as CheckCompletedEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //更新开始
        private static readonly object EVENT_UPDATE_STARTED = new object();
        /// <summary>
        /// 开始下载
        /// </summary>
        public event UpdateStartedEventHandler UpdateStarted
        {
            add
            {
                this.Events.AddHandler(EVENT_UPDATE_STARTED, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_UPDATE_STARTED, value);
            }
        }
        /// <summary>
        /// 触发更新开始事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnUpdateStarted(UpdateStartedEventArgs e)
        {
            UpdateStartedEventHandler handler = this.Events[EVENT_UPDATE_STARTED] as UpdateStartedEventHandler;
            if (handler != null)
                handler(this, e);
        }

        //更新完成
        private static readonly object EVENT_UPDATE_COMPLETED = new object();
        /// <summary>
        /// 更新完成事件
        /// </summary>
        public event UpdateCompletedEventHandler UpdateCompleted
        {
            add
            {
                this.Events.AddHandler(EVENT_UPDATE_COMPLETED, value);
            }
            remove
            {
                this.Events.RemoveHandler(EVENT_UPDATE_COMPLETED, value);
            }
        }
        /// <summary>
        /// 触发更新完成事件
        /// </summary>
        /// <param name="e">数据</param>
        protected virtual void OnUpdateCompleted(UpdateCompletedEventArgs e)
        {
            UpdateCompletedEventHandler handler = this.Events[EVENT_UPDATE_COMPLETED] as UpdateCompletedEventHandler;
            if (handler != null)
                handler(this, e);
        }
    }
}
