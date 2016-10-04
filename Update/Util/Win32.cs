using System;
using System.Runtime.InteropServices;

namespace Update.Util
{
    /// <summary>
    /// Win32 工具
    /// </summary>
    public static class Win32
    {
        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
        /// </summary>
        private const int WM_SYSCOMMAND = 0x0112;

        /// <summary>
        /// Moves the window.
        /// </summary>
        private const int SC_MOVE = 0xF010;

        /// <summary>
        /// In a title bar.
        /// </summary>
        private const int HTCAPTION = 2;

        /// <summary>
        /// <para>功能:</para>
        /// <para>该函数从当前线程中的窗口释放鼠标捕获，并恢复通常的鼠标输入处理。</para>
        /// <para>捕获鼠标的窗口接收所有的鼠标输入（无论光标的位置在哪里），除非点击鼠标键时，光标热点在另一个线程的窗口中。</para>
        /// <para>.</para>
        /// <para>备注:</para>
        /// <para>应用程序在调用函数SetCaPture之后调用此函数。</para>
        /// </summary>
        /// <returns>如果函数调用成功，返回非零值；如果函数调用失败，返回值是零。</returns>
        [DllImport("user32.DLL")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReleaseCapture();

        /// <summary>
        /// <para>功能:</para>
        /// <para>该函数将指定的消息发送到一个或多个窗口。此函数为指定的窗口调用窗口程序，直到窗口程序处理完消息再返回。</para>
        /// <para>而函数PostMessage不同，将一个消息寄送到一个线程的消息队列后立即返回。</para>
        /// <para>.</para>
        /// <para>备注:</para>
        /// <para>需要用HWND_BROADCAST通信的应用程序应当使用函数RegisterWindowMessage来为应用程序间的通信取得一个唯一的消息。</para>
        /// <para>如果指定的窗口是由正在调用的线程创建的，则窗口程序立即作为子程序调用。如果指定的窗口是由不同线程创建的，则系统切换到该线程并调用恰当的窗口程序。</para>
        /// <para>线程间的消息只有在线程执行消息检索代码时才被处理。发送线程被阻塞直到接收线程处理完消息为止。</para>
        /// </summary>
        /// <param name="hWnd">其窗口程序将接收消息的窗口的句柄。如果此参数为HWND_BROADCAST，则消息将被发送到系统中所有顶层窗口，包括无效或不可见的非自身拥有的窗口、被覆盖的窗口和弹出式窗口，但消息不被发送到子窗口。</param>
        /// <param name="Msg">指定被发送的消息。</param>
        /// <param name="wParam">指定附加的消息指定信息。</param>
        /// <param name="lParam">指定附加的消息指定信息。</param>
        /// <returns>返回值指定消息处理的结果，依赖于所发送的消息。</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// 开始拖动窗口
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        public static void BeginDrag(IntPtr hWnd)
        {
            ReleaseCapture();
            SendMessage(hWnd, WM_SYSCOMMAND, SC_MOVE | HTCAPTION, 0);
        }
    }
}
