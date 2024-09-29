/*
 * @author: S 2024/9/27 17:07:54
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsCtrlHub.tools
{
    /// <summary>
    /// 进程管理
    /// </summary>
    public class ProcessKit
    {
        /// <summary>
        /// 批量创建进程
        /// </summary>
        /// <param name="processes">进程列表</param>
        /// <param name="processCount">创建数量</param>
        /// <param name="consoleTitlePrefix">控制台标题</param>
        /// <param name="filePath">启动路径</param>
        public void MultiProcessCreate(List<Process> processes, int processCount, string consoleTitlePrefix, string filePath)
        {
            for (int i = 0; i < processCount; i++)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = filePath;
                var guid = Guid.NewGuid().ToString().Substring(0, 8);
                processStartInfo.Arguments = $"{consoleTitlePrefix}_{guid}";

                Process process = new Process();
                process.EnableRaisingEvents = true;
                process.StartInfo = processStartInfo;
                process.Exited += new EventHandler(Process_Exited);
                process.Start();

                processes.Add(process);
            }
        }

        /// <summary>
        /// 批量关闭进程
        /// </summary>
        /// <param name="processes">进程列表</param>
        /// <param name="processCount">关闭数量</param>
        public void MultiProcessClose(List<Process> processes, int processCount)
        {
            try
            {
                var partProcesses = processes.Take(processCount).ToList();
                foreach (var partItem in partProcesses)
                {
                    partItem.Kill();

                    processes.Remove(partItem);
                }
            }
            catch (Exception e) when (e is Win32Exception || e is FileNotFoundException)
            {
                MessageBox.Show($"The following exception was raised: {e.Message}");
            }
        }

        /// <summary>
        /// 进程全部显示
        /// </summary>
        /// <param name="processes">进程列表</param>
        public void ProcessShowALL(List<Process> processes)
        {
            foreach (var item in processes)
            {
                var consoleTitle = item.StartInfo.Arguments;
                ConsoleCtrl.ShowConsole(consoleTitle);
            }
        }

        /// <summary>
        /// 进程全部隐藏
        /// </summary>
        /// <param name="processes">进程列表</param>
        public void ProcessHideALL(List<Process> processes)
        {
            foreach (var item in processes)
            {
                var consoleTitle = item.StartInfo.Arguments;
                ConsoleCtrl.HideConsole(consoleTitle);
            }
        }

        /// <summary>
        /// 进程退出
        /// </summary>
        private void Process_Exited(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 根据进程名，精确查找进程，并创建返回
        /// </summary>
        /// <param name="strProcName"></param>
        /// <returns></returns>
        public List<Process> GetProcessByName(string strProcName, List<string> arguements)
        {
            try
            {
                var processes = Process.GetProcessesByName(strProcName);
                for (int i = 0; i < processes.Length; i++)
                {
                    processes[i].StartInfo.Arguments = arguements[i];
                }
                return processes.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "获取进程失败");
                return null;
            }
        }
    }
}
