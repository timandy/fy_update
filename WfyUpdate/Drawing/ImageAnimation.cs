using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WfyUpdate.Drawing
{
    /// <summary>
    /// 图片渐变切换动画
    /// </summary>
    public class ImageAnimation : IDisposable
    {
        internal const int DEFAULT_ANIMATION_SPAN = 3000;       //默认动画执行时间
        internal const int NONE_INDEX = -1;                     //表示空目标索引
        private List<Image> m_Images = new List<Image>();       //原始图片
        private List<byte[]> m_Frames = new List<byte[]>();     //截至帧集合
        private Random m_Rand = new Random();                   //随机器
        private byte[] m_From;                                  //起始帧数据
        private int m_ToIndex = NONE_INDEX;                     //截至帧索引


        #region 属性

        private bool m_Stoped;
        /// <summary>
        /// 获取动画是否已结束
        /// </summary>
        public bool Stoped
        {
            get { return this.m_Stoped; }
        }

        private bool m_Random = true;
        /// <summary>
        /// 获取或设置是否随机图片
        /// </summary>
        public bool Random
        {
            get
            {
                return this.m_Random;
            }
            set
            {
                this.m_Random = value;
            }
        }

        private int m_AnimationSpan = DEFAULT_ANIMATION_SPAN;
        /// <summary>
        /// 获取或设置动画执行时间
        /// </summary>
        public int AnimationSpan
        {
            get
            {
                return this.m_AnimationSpan;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                this.m_AnimationSpan = value;
            }
        }

        private Size m_Size;
        /// <summary>
        /// 获取或设置图片大小
        /// </summary>
        public Size Size
        {
            get { return this.m_Size; }
            set
            {
                if (value != this.m_Size)
                {
                    //暂存
                    Size oldSize = this.m_Size;
                    byte[] oldFrom = this.m_From;
                    //赋值
                    this.m_Size = value;
                    //数据还原
                    this.m_From = new byte[this.m_Size.Width * this.m_Size.Height * 4];//from
                    if (oldFrom != null && this.m_Images.Count > 1)//有多张=>动画=>才会需要还原
                    {
                        int oldWidth = oldSize.Width * 4;
                        int newWidth = this.m_Size.Width * 4;
                        int width = Math.Min(oldWidth, newWidth);
                        int height = Math.Min(oldSize.Height, this.m_Size.Height);
                        for (int i = 0; i < height; i++)
                            Array.Copy(oldFrom, oldWidth * i, this.m_From, newWidth * i, width);
                    }
                    this.RecreateFrames();//to 列表
                    if (this.m_Current != null)//current
                        this.m_Current.Dispose();
                    this.m_Current = new Bitmap(this.m_Size.Width, this.m_Size.Height, PixelFormat.Format32bppArgb);
                    this.m_Stoped = false;//失效,下次调用 current 将获取新的
                }
            }
        }

        private DateTime m_StartTime;
        /// <summary>
        /// 获取动画开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return this.m_StartTime; }
        }

        private Bitmap m_Current;
        /// <summary>
        /// 获取当前显示帧图片
        /// </summary>
        public unsafe Bitmap Current
        {
            get
            {
                //已停止
                if (this.m_Stoped)
                    return this.m_Current;
                //数量判断
                float percent;
                switch (this.m_Images.Count)
                {
                    case 0:
                        using (Graphics g = Graphics.FromImage(this.m_Current))
                            g.Clear(Color.Transparent);
                        this.m_ToIndex = NONE_INDEX;
                        this.m_Stoped = true;
                        return this.m_Current;

                    case 1:
                        this.m_ToIndex = 0;
                        percent = 1f;
                        break;

                    default:
                        if (this.m_ToIndex == NONE_INDEX)
                            this.m_ToIndex = this.NextToIndex();
                        percent = (float)((DateTime.Now - this.m_StartTime).TotalMilliseconds / this.m_AnimationSpan);//百分比
                        break;
                }
                //计算当前时间点图片像素,如果就一副图片直接复制
                byte[] from = this.m_From;
                byte[] to = this.m_Frames[this.m_ToIndex];
                using (LockedBitmapData bmpData = new LockedBitmapData(this.m_Current, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb))
                {
                    int length = bmpData.Stride * this.m_Size.Height;
                    if (percent >= 1)
                    {
                        Marshal.Copy(to, 0, bmpData.Scan0, length);
                        this.m_Stoped = true;
                    }
                    else
                    {
                        byte tmp;
                        byte* ptr = (byte*)bmpData.Scan0;
                        for (int i = 0; i < length; i++)
                            *(ptr++) = (byte)((tmp = from[i]) + (to[i] - tmp) * percent);
                    }
                    return this.m_Current;
                }
            }
        }

        #endregion 属性


        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImageAnimation()
        {
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~ImageAnimation()
        {
            this.Dispose(false);
        }

        #endregion


        #region 动画操作

        /// <summary>
        /// 获取下一个目标帧
        /// </summary>
        /// <returns>下一个目标帧</returns>
        private int NextToIndex()
        {
            //随机
            if (this.m_Random)
            {
                int current = this.m_ToIndex;
                int temp;
                do
                {
                    temp = this.m_Rand.Next(0, this.m_Images.Count);
                } while (temp == current);
                return temp;
            }
            //非随机
            return (this.m_ToIndex >= (this.m_Images.Count - 1)) ? 0 : this.m_ToIndex + 1;
        }

        /// <summary>
        /// 切换下一帧,开始动画
        /// </summary>
        public void Next()
        {
            //数量判断
            switch (this.m_Images.Count)
            {
                case 0:
                    this.m_ToIndex = NONE_INDEX;
                    return;

                case 1:
                    this.m_ToIndex = 0;
                    return;
            }
            //当前快照到from
            byte[] from = this.m_From;
            using (LockedBitmapData bmpData = new LockedBitmapData(this.m_Current, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
            {
                Marshal.Copy(bmpData.Scan0, this.m_From, 0, from.Length);
            }
            //下一帧
            this.m_ToIndex = this.NextToIndex();
            this.m_StartTime = DateTime.Now;
            this.m_Stoped = false;
        }

        #endregion


        #region 集合操作

        /// <summary>
        /// 添加图片到帧集合
        /// </summary>
        /// <param name="image">图片</param>
        private void AddFrame(Image image)
        {
            Rectangle rect = new Rectangle(0, 0, this.m_Size.Width, this.m_Size.Height);
            using (Bitmap bmp = new Bitmap(image, this.m_Size.Width, this.m_Size.Height))
            {
                using (LockedBitmapData bmpData = new LockedBitmapData(bmp, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb))
                {
                    int length = bmpData.Stride * bmpData.Height;
                    byte[] frame = new byte[length];
                    Marshal.Copy(bmpData.Scan0, frame, 0, length);
                    this.m_Frames.Add(frame);
                }
            }
        }

        /// <summary>
        /// 重新创建所有帧,一般在大小改变后调用
        /// </summary>
        private void RecreateFrames()
        {
            this.m_Frames.Clear();
            foreach (Image image in this.m_Images)
                this.AddFrame(image);
        }

        /// <summary>
        /// 添加一个图片
        /// </summary>
        /// <param name="image">图片</param>
        public void Add(Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //添加到原始图
            this.m_Images.Add(image);

            //添加到缓存
            this.AddFrame(image);
        }

        /// <summary>
        /// 删除一个图片
        /// </summary>
        /// <param name="item">图片</param>
        /// <returns>删除成功返回true,否则返回false</returns>
        public bool Remove(Image item)
        {
            int index = this.m_Images.IndexOf(item);
            if (index == -1)
                return false;

            //从原始中移除
            this.m_Images.RemoveAt(index);

            //如果正好移除的为当前，指针后移
            if (this.m_ToIndex == index)
                this.Next();

            //从缓存移除
            this.m_Frames.RemoveAt(index);
            return true;
        }

        #endregion


        #region 释放资源

        /// <summary>
        /// 释放原始图片引用
        /// </summary>
        private void DisposeImages()
        {
            if (this.m_Images != null)
            {
                this.m_Images.Clear();
                this.m_Images = null;
            }
        }

        /// <summary>
        /// 释放帧数据
        /// </summary>
        private void DisposeFrames()
        {
            if (this.m_Frames != null)
            {
                this.m_Frames.Clear();
                this.m_Frames = null;
            }
        }

        /// <summary>
        /// 释放随机数生成器
        /// </summary>
        private void DisposeRand()
        {
            this.m_Rand = null;
        }

        /// <summary>
        /// 释放动画起始数据
        /// </summary>
        private void DisposeFrom()
        {
            this.m_From = null;
        }

        /// <summary>
        /// 释放动画截至数据
        /// </summary>
        private void DisposeTo()
        {
            this.m_ToIndex = 0;
        }

        /// <summary>
        /// 释放动画图片
        /// </summary>
        private void DisposeCurrent()
        {
            if (this.m_Current != null)
            {
                this.m_Current.Dispose();
                this.m_Current = null;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">释放托管资源为true,否则为false</param>
        protected virtual void Dispose(bool disposing)
        {
            this.DisposeImages();
            this.DisposeFrames();
            this.DisposeRand();
            this.DisposeFrom();
            this.DisposeTo();
            this.DisposeCurrent();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
