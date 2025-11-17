using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace LoveFormsApp
{
    public partial class ColorForm : Form
    {
        private readonly int coun;
        private readonly string[] arr;

        private ManualResetEvent mect;
        private const int VM_NCLBUTTONDOWN_ = 0XA1; //VM_NCLBUTTONDOWN //定义鼠标左键按下
        private const int HTCAPTION_ = 2; // HTCAPTION

        #region Win32 API函数

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();//从当前线程中的窗口释放鼠标捕获（释放被当前线程中某个窗口捕获的光标），并恢复正常的鼠标输入处理

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwdn, int wMsg, int mParam, int lParam);//向指定的窗体发送Windows消息

        #endregion Win32 API函数

        private void setForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();// 释放鼠标捕获
                //发送消息 让系统误以为在标题栏上按下鼠标
                SendMessage(this.Handle, VM_NCLBUTTONDOWN_, HTCAPTION_, 0);
            }
        }

        public ColorForm()
        {
            InitializeComponent();
        }

        public ColorForm(string[] lab, int count, ManualResetEvent manual)
        {
            InitializeComponent();
            // 设置窗体的背景色和透明键
            this.TransparencyKey = Color.Magenta; // 选择一个不常用的颜色作为透明键
            this.BackColor = Color.Magenta; // 将背景色设置为透明键的颜色

            // 移除边框以获得更好的透明效果
            this.FormBorderStyle = FormBorderStyle.None;
            arr = lab;
            coun = count;
            mect = manual;
            SetStyle(
                    ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.ResizeRedraw
                    | ControlStyles.Selectable
                    | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.SupportsTransparentBackColor,
                    true);
            UpdateStyles();
        }

        private readonly Random random1 = new Random();

        private void ColorForm_Load(object sender, EventArgs e)
        {
            // 获取当前屏幕的工作区域
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

            // 生成随机的 X 和 Y 坐标
            Random random = new Random();
            int randomX = random.Next(workingArea.Left, workingArea.Right - this.Width);
            int randomY = random.Next(workingArea.Top, workingArea.Bottom - this.Height);
            // 设置窗体的位置
            this.Location = new Point(randomX, randomY);

            Win32.AnimateWindow(this.Handle, 1000, Win32.AW_CENTER);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int red = random1.Next(0, 256);

            int green = random1.Next(0, 256);

            int blue = random1.Next(0, 256);
            Color randomColor = Color.FromArgb(red, green, blue);

            //this.BackColor = randomColor;
            //label1.Text = arr[random1.Next(0, arr.Length-1)];
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Win32.AnimateWindow(this.Handle, 300, Win32.AW_BLEND | Win32.AW_HIDE | Win32.AW_CENTER);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            mect.Set();
        }
    }

    public class Win32
    {
        public const Int32 AW_HOR_POSITIVE = 0x00000001; // 从左到右打开窗口
        public const Int32 AW_HOR_NEGATIVE = 0x00000002; // 从右到左打开窗口
        public const Int32 AW_VER_POSITIVE = 0x00000004; // 从上到下打开窗口
        public const Int32 AW_VER_NEGATIVE = 0x00000008; // 从下到上打开窗口
        public const Int32 AW_CENTER = 0x00000010; //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展。
        public const Int32 AW_HIDE = 0x00010000; //隐藏窗口，缺省则显示窗口。
        public const Int32 AW_ACTIVATE = 0x00020000; //激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
        public const Int32 AW_SLIDE = 0x00040000; //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
        public const Int32 AW_BLEND = 0x00080000; //使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AnimateWindow(
            IntPtr hwnd, // handle to window
          int dwTime, // duration of animation
          int dwFlags // animation type
          );
    }
}