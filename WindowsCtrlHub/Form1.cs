using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsCtrlHub.tools;

namespace WindowsCtrlHub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 窗体美化

        //圆角
        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
         (int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        //阴影
        private const int CS_DropSHADOW = 0x20000;
        private const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        //窗体移动
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void Form1_Load(object sender, EventArgs e)
        {
            //圆角
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            //阴影
            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);

            // 按钮提示
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btn_minimize, "最小化到托盘");
            toolTip.SetToolTip(btn_close, "关闭");

            // 加载系统存在的进程
            LoadActiveProcess();
        }

        private void WindoMove()
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        /// <summary>
        /// //窗体移动
        /// </summary>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            WindoMove();
        }

        /// <summary>
        /// 窗体移动
        /// </summary>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            WindoMove();
        }

        /// <summary>
        /// 窗体移动
        /// </summary>
        private void label3_MouseDown(object sender, MouseEventArgs e)
        {
            WindoMove();
        }

        /// <summary>
        /// 窗体移动
        /// </summary>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            WindoMove();
        }

        /// <summary>
        /// 最小化按钮
        /// </summary>
        private void btn_minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        ProcessKit processKit = new ProcessKit();
        ProcessData processData = new ProcessData();

        /// <summary>
        /// 加载当前存在的进程
        /// </summary>
        private void LoadActiveProcess()
        {
            var backFilePath = GetBakFilePath();
            if (!FileKit.Exist(backFilePath))
            {
                return;
            }
            var processDataJson = FileKit.ReadText(backFilePath);
            if (string.IsNullOrEmpty(processDataJson))
            {
                return;
            }

            var processDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(processDataJson);
            var processNames = processDict.Keys;

            if (processNames.Contains(ProcessData.ProcessName_A1))
            {
                processData.Processes_A1 = processKit.GetProcessByName(ProcessData.ProcessName_A1, processDict[ProcessData.ProcessName_A1]);
                Lbl_A1_count.Text = processData.Processes_A1.Count.ToString();
            }
            if (processNames.Contains(ProcessData.ProcessName_A2))
            {
                processData.Processes_A2 = processKit.GetProcessByName(ProcessData.ProcessName_A2, processDict[ProcessData.ProcessName_A2]);
                lbl_A2_count.Text = processData.Processes_A2.Count.ToString();
            }
        }

        private string GetBakFilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProcessData.json");
        }

        #region 最小化到托盘
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                this.Activate();
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        #endregion

        /// <summary>
        /// 关闭事件
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 存储数据
            SaveActiveProcessName();
        }

        /// <summary>
        /// 保存进程名到本地
        /// </summary>
        private void SaveActiveProcessName()
        {
            // 保存配置
            var processDict = new Dictionary<string, List<string>>();
            AddActiveProcessName(processDict, ProcessData.ProcessName_A1, processData.Processes_A1);
            AddActiveProcessName(processDict, ProcessData.ProcessName_A2, processData.Processes_A2);
            var processDataJson = JsonConvert.SerializeObject(processDict);
            FileKit.WriteText(GetBakFilePath(), processDataJson, false);
        }

        private void AddActiveProcessName(Dictionary<string, List<string>> processDict, string name, List<Process> processes)
        {
            if (processes.Count > 0)
            {
                var consoleTitleNames = processes.Select(t => t.StartInfo.Arguments).ToList();
                processDict.Add(name, consoleTitleNames);
            }
        }

        #region A1

        private void btn_A1_create_Click(object sender, EventArgs e)
        {
            if (processData.Processes_A1.Count >= 1)
            {
                MessageBox.Show("最多只能创建一个进程", "A1");
                return;
            }
            var createCount = Convert.ToInt32(input_A1_create_count.Text);
            if (createCount == 0)
            {
                MessageBox.Show("请输入创建进程的数量", "A1");
            }
            else
            {
                string exePath = AppConfigValue.Get($"{ProcessData.ProcessName_A1}.exe.path");
                processKit.MultiProcessCreate(processData.Processes_A1, createCount, ProcessData.ProcessName_A1, exePath);
                Lbl_A1_count.Text = processData.Processes_A1.Count.ToString();
            }
        }

        private void btn_A1_close_Click(object sender, EventArgs e)
        {
            var closeCount = Convert.ToInt32(input_A1_close_count.Text);
            if (closeCount == 0)
            {
                MessageBox.Show("请输入关闭进程的数量", "生产者");
            }
            else
            {
                processKit.MultiProcessClose(processData.Processes_A1, closeCount);
                Lbl_A1_count.Text = processData.Processes_A1.Count.ToString();
            }
        }

        private void btn_A1_close_all_Click(object sender, EventArgs e)
        {
            processKit.MultiProcessClose(processData.Processes_A1, processData.Processes_A1.Count);
            Lbl_A1_count.Text = processData.Processes_A1.Count.ToString();
        }

        private void btn_A1_show_Click(object sender, EventArgs e)
        {
            processKit.ProcessShowALL(processData.Processes_A1);
        }

        private void btn_A1_hidden_Click(object sender, EventArgs e)
        {
            processKit.ProcessHideALL(processData.Processes_A1);
        }


        #endregion

        #region A2

        private void btn_A2_create_Click(object sender, EventArgs e)
        {
            var createCount = Convert.ToInt32(input_A2_create.Text);
            if (createCount == 0)
            {
                MessageBox.Show("请输入创建进程的数量", "A2");
            }
            else
            {
                string exePath = AppConfigValue.Get($"{ProcessData.ProcessName_A2}.exe.path");
                processKit.MultiProcessCreate(processData.Processes_A2, createCount, ProcessData.ProcessName_A2, exePath);
                lbl_A2_count.Text = processData.Processes_A2.Count.ToString();
            }
        }

        private void btn_A2_close_Click(object sender, EventArgs e)
        {
            var closeCount = Convert.ToInt32(input_A2_close.Text);
            if (closeCount == 0)
            {
                MessageBox.Show("请输入关闭进程的数量", "A2");
            }
            else
            {
                processKit.MultiProcessClose(processData.Processes_A2, closeCount);
                lbl_A2_count.Text = processData.Processes_A2.Count.ToString();
            }
        }

        private void btn_A2_close_all_Click(object sender, EventArgs e)
        {
            processKit.MultiProcessClose(processData.Processes_A2, processData.Processes_A2.Count);
            lbl_A2_count.Text = processData.Processes_A2.Count.ToString();
        }

        private void btn_A2_show_Click(object sender, EventArgs e)
        {
            processKit.ProcessShowALL(processData.Processes_A2);
        }

        private void btn_A2_hidden_Click(object sender, EventArgs e)
        {
            processKit.ProcessHideALL(processData.Processes_A2);
        }

        #endregion

        #region 开启全部、关闭全部

        /// <summary>
        /// 初始化开启进程
        /// </summary>
        private void btn_consume_init_create_all_Click(object sender, EventArgs e)
        {
            // 检查是否存在进程，如果存在进程就不能进行初始化操作
            if (processData.Processes_A1.Count > 0 ||
                processData.Processes_A2.Count > 0)
            {
                MessageBox.Show("已经开启过线程，不能执行初始化操作", "初始化创建进程");
            }
            else
            {
                btn_A1_create_Click(btn_A1_create, new EventArgs());
                btn_A2_create_Click(btn_A2_create, new EventArgs());
            }
        }

        /// <summary>
        /// 关闭全部进程
        /// </summary>
        private void btn_process_consume_close_all_Click(object sender, EventArgs e)
        {
            btn_A1_close_all_Click(btn_A1_close_all, new EventArgs());
            btn_A2_close_all_Click(btn_A2_close_all, new EventArgs());
        }

        #endregion
    }
}
