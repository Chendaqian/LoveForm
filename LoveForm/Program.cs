using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace LoveFormsApp
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 使用一个主控制器来管理所有窗口的生命周期
            // 这样可以确保所有UI操作都在同一个UI线程上执行
            Application.Run(new MainForm());
        }
    }

    /// <summary>
    /// 这是一个隐藏的主窗口，作为所有子窗口的控制器
    /// </summary>
    public class MainForm : Form
    {
        private readonly List<Form> _childForms = new List<Form>();
        private readonly int _maxChildForms; // 最大子窗口数量（从配置读取）

        #region 数据源
        private static readonly string[] strarr =
        {
            "继续前进", "你是最可爱的", "保持热情", "相信奇迹",
            "温柔待人", "也被温柔以待", "心之所向", "素履以往",
            "生活明朗", "万物可爱", "人间值得", "未来可期","天冷加衣",
            "愿你被世界温柔以待", "每一天都值得珍惜", "做自己的太阳",
            "不负时光", "勇往直前", "静待花开", "一切美好都会到来" ,"我永远支持你", "做你自己就好", "生活很美好", "珍惜当下",
            "阳光总在风雨后", "你是被爱的", "每一天都是新的开始", "勇敢一点",
            "别放弃", "你很特别", "世界因你而美丽", "保持初心",
            "简单就是幸福", "记得想我", "你值得最好的", "慢慢变好",
            "未来可期", "平安喜乐", "照顾好自己", "你是重要的",
            "永远相信美好", "保持可爱", "今天也要努力呀", "别太在意",
            "放松心情", "你笑起来真好看", "一切都会如愿", "保持善良",
            "感恩遇见", "岁月静好", "愿你快乐", "不忘初心","心有梦想，步履不停", "你是独一无二的星辰",
            "热情如火，照亮前路", "相信自己，奇迹就在不远处", "温柔以待，世界亦会温柔相待",
            "被温柔包围，幸福常伴", "心之所向，素履以往", "生活明朗，未来可期", "万物可爱，人间值得",
            "天冷加衣，温暖常在", "愿你被世界温柔以待", "每一天都值得珍惜，每一刻都充满希望",
            "做自己的太阳，照亮前行的路", "不负时光，勇往直前", "静待花开，美好终会到来", "一切美好都会到来，只需耐心等待",
            "我永远支持你，无论何时何地", "做你自己就好，最真实的你最美", "生活很美好，用心感受每一刻",
            "珍惜当下，把握每一个瞬间", "阳光总在风雨后，坚持就是胜利", "你是被爱的", "永远不要忘记这一点",
            "每一天都是新的开始，充满无限可能", "勇敢一点", "迈出那一步", "别放弃", "坚持到底就是胜利", "你很特别", "独一无二的存在",
            "世界因你而美丽，你的存在让世界更精彩", "保持初心，不忘初心，方得始终", "简单就是幸福", "平凡中见真章",
            "记得想我", "我也在想你", "你值得最好的", "不要轻易妥协", "慢慢变好", "每一步都在进步", "未来可期", "美好的明天等着你",
            "平安喜乐", "愿你一生幸福安康", "照顾好自己", "健康最重要", "你是重要的", "你的存在不可或缺", "永远相信美好", "美好总会如期而至",
            "保持可爱", "让生活充满乐趣", "今天也要努力呀", "每一天都是新的挑战", "别太在意", "放松心情", "一切都会好起来", "放松心情", "享受每一个当下",
            "你笑起来真好看", "笑容是最美的风景", "一切都会如愿", "只要你不放弃", "保持善良", "善良的人会有好运", "感恩遇见", "感谢生命中的每一个人",
            "岁月静好", "现世安稳", "愿你快乐", "每一天都充满欢笑", "不忘初心", "砥砺前行", "心中有梦", "脚下有路",
            "温暖如春", "愿你被世界温柔以待","梦想在远方", "脚步不停歇", "你是独一无二的光芒",
            "热情如炬", "照亮前行的路", "相信自己", "奇迹就在前方", "温柔待人", "世界亦会温柔以待", "被温柔包围", "幸福常伴身边",
            "心之所向", "无问西东", "生活明媚", "未来充满希望", "世间万物", "皆有可爱之处", "天冷加衣", "温暖常伴",
            "愿你被世界温柔相待", "每一刻都值得珍惜", "每一天都充满希望", "做自己的太阳", "照亮前进的路", "不负时光", "勇往直前",
            "静待花开", "美好终将到来", "一切美好都会到来", "只需耐心等待", "我永远支持你", "无论何时何地",
            "做你自己就好", "最真实的你最美", "生活很美好", "用心感受每一刻", "珍惜当下", "把握每一个瞬间",
            "阳光总在风雨后", "坚持就是胜利", "你是被爱的", "永远不要忘记这一点", "每一天都是新的开始", "充满无限可能",
            "勇敢一点", "迈出那一步", "别放弃", "坚持到底就是胜利", "你很特别", "独一无二的存在",
            "世界因你而美丽", "你的存在让世界更精彩", "保持初心", "不忘初心", "方得始终", "简单就是幸福", "平凡中见真章",
            "记得想我", "我也在想你", "你值得最好的", "不要轻易妥协", "慢慢变好", "每一步都在进步", "未来可期", "美好的明天等着你",
            "平安喜乐", "愿你一生幸福安康", "照顾好自己", "健康最重要", "你是重要的", "你的存在不可或缺",
            "永远相信美好", "美好总会如期而至", "保持可爱", "让生活充满乐趣", "今天也要努力呀", "每一天都是新的挑战",
            "别太在意", "放松心情", "一切都会好起来", "放松心情", "享受每一个当下", "你笑起来真好看", "笑容是最美的风景",
            "一切都会如愿", "只要你不放弃", "保持善良", "善良的人会有好运", "感恩遇见", "感谢生命中的每一个人",
            "岁月静好", "现世安稳", "愿你快乐", "每一天都充满欢笑", "不忘初心", "砥砺前行", "心中有梦", "脚下有路",
            "温暖如春", "愿你被世界温柔以待", "追梦的路上", "永不言弃", "你是夜空中最亮的星", "热情似火", "照亮黑暗",
            "相信奇迹", "它就在前方", "温柔待人", "世界更加美好", "被温柔包围", "幸福常在", "心之所向", "无问西东",
            "生活明媚", "未来可期", "世间万物", "皆有可爱之处", "天冷加衣", "温暖相伴", "愿你被世界温柔相待",
            "每一刻都值得珍惜", "每一天都充满希望", "做自己的太阳", "照亮前行的路", "不负韶华", "勇往直前",
            "静待花开", "美好终会到来", "一切美好都会到来", "只需耐心等待", "我永远支持你", "无论何时何地",
            "做你自己就好", "最真实的你最美", "生活很美好", "用心感受每一刻", "珍惜当下", "把握每一个瞬间",
            "阳光总在风雨后", "坚持就是胜利", "你是被爱的", "永远不要忘记这一点", "每一天都是新的开始", "充满无限可能",
            "勇敢一点", "迈出那一步", "别放弃", "坚持到底就是胜利", "你很特别", "独一无二的存在",
            "世界因你而美丽", "你的存在让世界更精彩", "保持初心", "不忘初心", "方得始终", "简单就是幸福", "平凡中见真章",
            "记得想我", "我也在想你", "你值得最好的", "不要轻易妥协", "慢慢变好", "每一步都在进步", "未来可期", "美好的明天等着你",
            "平安喜乐", "愿你一生幸福安康", "照顾好自己", "健康最重要", "你是重要的", "你的存在不可或缺",
            "永远相信美好", "美好总会如期而至", "保持可爱", "让生活充满乐趣", "今天也要努力呀", "每一天都是新的挑战",
            "别太在意", "放松心情", "一切都会好起来", "放松心情", "享受每一个当下", "你笑起来真好看", "笑容是最美的风景",
            "一切都会如愿", "只要你不放弃", "保持善良", "善良的人会有好运", "感恩遇见", "感谢生命中的每一个人",
            "岁月静好", "现世安稳", "愿你快乐", "每一天都充满欢笑", "不忘初心", "砥砺前行", "心中有梦", "脚下有路",
            "温暖如春", "愿你被世界温柔以待"
        };
        #endregion

        public MainForm()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            const int defaultMax = 80; // 默认最大子窗口数量
            string configValue = ConfigurationManager.AppSettings["MaxChildForms"]; // 从App.config读取
            if (!int.TryParse(configValue, out int parsedValue))
            {
                parsedValue = defaultMax; // 解析失败使用默认值
            }
            _maxChildForms = parsedValue; // 保存最大子窗口数量
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await CreateChildForms();
        }

        private async Task CreateChildForms()
        {
            for (int i = 0; i < strarr.Length; i++)
            {
                if (_childForms.Count >= _maxChildForms) // 达到最大窗口数量则退出
                {
                    Application.Exit();
                    return;
                }
                // 使用Task.Delay进行非阻塞延时
                await Task.Delay(200);

                // 在UI线程上创建和显示新窗口
                ColorForm form = new ColorForm(strarr[i], i);
                form.Show();
                _childForms.Add(form);
            }
        }
    }

    public partial class ColorForm : Form
    {
        private static readonly Random _rand = new Random();
        // 更新后的颜色列表，颜色更深一些
        private static readonly List<Color> _freshColors = new List<Color>
        {
            Color.FromArgb(173, 216, 230), // 亮蓝色
            Color.FromArgb(144, 238, 144), // 亮绿色
            Color.FromArgb(255, 218, 185), // 亮橙色 (桃色)
            Color.FromArgb(255, 192, 203), // 亮粉色
            Color.FromArgb(221, 160, 221), // 亮紫色 (紫丁香色)
            Color.FromArgb(175, 238, 238), // 亮青色 (苍白绿松石色)
            Color.FromArgb(250, 128, 114)  // 亮红色 (鲑鱼色)
        };

        public ColorForm(string text, int index)
        {
            //this.Text = $"窗口 {index + 1}";
            this.Size = new Size(300, 100);
            this.StartPosition = FormStartPosition.Manual;
            this.FormBorderStyle = FormBorderStyle.None; // 移除窗体边框和标题栏
            this.ShowInTaskbar = false;
            // 设置随机背景色
            this.BackColor = _freshColors[_rand.Next(_freshColors.Count)];

            // 获取屏幕工作区域的大小
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            // 生成随机位置，确保窗口完整显示在屏幕内
            int x = _rand.Next(workingArea.Left, workingArea.Right - this.Width);
            int y = _rand.Next(workingArea.Top, workingArea.Bottom - this.Height);
            this.Location = new Point(x, y);

            Label label = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134),
                BackColor = Color.Transparent // 使标签背景透明
            };
            this.Controls.Add(label);
        }

        // 重写OnPaint事件来绘制边框
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // 绘制一个1像素宽的黑色边框
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
    }
}