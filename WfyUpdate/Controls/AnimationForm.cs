using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WfyUpdate.Drawing;

namespace WfyUpdate.Controls
{
    /// <summary>
    /// 背景渐变窗口
    /// </summary>
    public class AnimationForm : BaseForm
    {
        internal const int DEFAULT_ANIMATION_INTERVAL = 8000;           //默认动画触发时间间隔
        internal const int DEFAULT_FRAME_INTERVAL = 40;                 //默认帧间隔
        private ImageAnimation m_Animation = new ImageAnimation();      //图片动画
        private Timer m_AnimationTimer = new Timer();                   //动画触发定时器
        private Timer m_FrameTimer = new Timer();                       //帧定时器


        #region 属性

        /// <summary>
        /// 获取或设置动画触发时间间隔,毫秒
        /// </summary>
        [Category("动画"), DefaultValue(DEFAULT_ANIMATION_INTERVAL)]
        public int AnimationInterval
        {
            get
            {
                return this.m_AnimationTimer.Interval;
            }
            set
            {
                this.m_AnimationTimer.Interval = value;
            }
        }

        /// <summary>
        /// 获取或设置动画执行时间,毫秒
        /// </summary>
        [Category("动画"), DefaultValue(ImageAnimation.DEFAULT_ANIMATION_SPAN)]
        public int AnimationSpan
        {
            get
            {
                return this.m_Animation.AnimationSpan;
            }
            set
            {
                this.m_Animation.AnimationSpan = value;
            }
        }

        /// <summary>
        /// 获取或设置动画是否随机
        /// </summary>
        [Category("动画"), DefaultValue(ImageAnimation.DEFAULT_ANIMATION_RANDOM)]
        public bool AnimationRandom
        {
            get
            {
                return this.m_Animation.Random;
            }
            set
            {
                this.m_Animation.Random = value;
            }
        }

        #endregion


        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public AnimationForm()
        {
            //设置双缓冲
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            //初始化触发定时器
            this.m_AnimationTimer.Interval = DEFAULT_ANIMATION_INTERVAL;
            this.m_AnimationTimer.Tick += (sender, e) =>
            {
                this.m_FrameTimer.Start();
                this.m_Animation.Next();
            };
            //初始化帧定时器
            this.m_FrameTimer.Interval = DEFAULT_FRAME_INTERVAL;
            this.m_FrameTimer.Tick += (sender, e) =>
            {
                this.Invalidate();
                if (this.m_Animation.Stoped)
                    this.m_FrameTimer.Stop();
            };
            //启动触发定时器
            this.m_AnimationTimer.Start();
        }

        #endregion


        #region 公共方法

        /// <summary>
        /// 添加帧
        /// </summary>
        /// <param name="image">图片</param>
        public void AddFrame(Image image)
        {
            this.m_Animation.Add(image);
        }

        #endregion


        #region 重写方法

        /// <summary>
        /// 重写加载
        /// </summary>
        /// <param name="e">数据</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.m_Animation.Size = this.Size;
        }

        /// <summary>
        /// 重写大小改变
        /// </summary>
        /// <param name="e">数据</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.m_Animation.Size = this.Size;
        }

        /// <summary>
        /// 重写渲染
        /// </summary>
        /// <param name="e">数据</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(this.m_Animation.Current, Point.Empty);
        }

        #endregion


        #region 释放资源

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">释放托管资源为true,否则为false</param>
        protected override void Dispose(bool disposing)
        {
            if (this.m_AnimationTimer != null)
            {
                this.m_AnimationTimer.Dispose();
                this.m_AnimationTimer = null;
            }
            if (this.m_FrameTimer != null)
            {
                this.m_FrameTimer.Dispose();
                this.m_FrameTimer = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
