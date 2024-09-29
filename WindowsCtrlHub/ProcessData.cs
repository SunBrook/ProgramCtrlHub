/*
 * @author: S 2024/9/27 17:00:58
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace WindowsCtrlHub
{
    /// <summary>
    /// 进程数据
    /// </summary>
    public class ProcessData
    {
        public ProcessData()
        {
            Processes_A1 = new List<Process>();
            Processes_A2 = new List<Process>();
        }

        public const string ProcessName_A1 = "A1";
        public List<Process> Processes_A1 { get; set; }

        public const string ProcessName_A2 = "A2";
        public List<Process> Processes_A2 { get; set; }
    }
}
