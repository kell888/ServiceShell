using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Cjwdev.WindowsApi;

namespace ServiceShell
{
    /// <summary> 
    /// Command 的摘要说明。 
    /// </summary> 
    public class Command
    {
        private Process proc = null;
        private bool singleton = false;
        private System.Threading.Thread thr = null;

        public System.Threading.Thread THR
        {
            get
            {
                return thr;
            }
        }

        public bool Singleton
        {
            get
            {
                return singleton;
            }
            set
            {
                singleton = value;
            }
        }

        /// <summary> 
        /// 构造方法 
        /// <param name="singleton">程序类型是否为单件模式（默认false，允许多实例同时运行）</param>
        /// </summary> 
        public Command(bool singleton = false)
        {
            proc = new Process();
            this.singleton = singleton;
        }

        /// <summary> 
        /// 执行CMD语句 
        /// </summary> 
        /// <param name="cmdStr">要执行的CMD命令行字符串</param> 
        public string RunCmd(string cmdStr = null)
        {
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (cmdStr != null && cmdStr.Length > 0)
            {
                proc.StandardInput.WriteLine(cmdStr);
            }
            StringBuilder outStr = new StringBuilder();
            string tmptStr = proc.StandardOutput.ReadLine();
            while (tmptStr != "")
            {
                outStr.Append(outStr);
                tmptStr = proc.StandardOutput.ReadLine();
            }
            proc.Close();
            return outStr.ToString();
        }

        /// <summary> 
        /// 打开程序并执行命令 
        /// </summary> 
        /// <param name="programFile">程序完全路径（比如：exe、cmd、bat等文件）</param> 
        /// <param name="arg">要执行的命令</param> 
        public string RunProgram(string programFile, string arg = null)
        {
            if (singleton)
            {
                Process[] procs = Process.GetProcesses();
                foreach (Process p in procs)
                {
                    string fullPath = GetMainModuleFilepath(p.Id);
                    if (fullPath != null && fullPath.Equals(programFile, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Logs.Log("", "当前系统中已经运行着一个同一路径下的程序实例=>" + programFile);
                        return "当前系统中已经运行着一个同一路径下的程序实例=>" + programFile;
                    }
                }
            }
            int procId = OpenForm(programFile);
            //proc.StartInfo.CreateNoWindow = true;
            //proc.StartInfo.FileName = programFile;
            //proc.StartInfo.UseShellExecute = false;
            //proc.StartInfo.RedirectStandardError = true;
            //proc.StartInfo.RedirectStandardInput = true;
            //proc.StartInfo.RedirectStandardOutput = true;
            //proc.Start();
            //if (arg != null && arg.Length > 0)
            //{
            //    proc.StandardInput.WriteLine(arg);
            //}
            StringBuilder outStr = new StringBuilder();
            //string tmptStr = proc.StandardOutput.ReadLine();
            //while (tmptStr != "")
            //{
            //    outStr.Append(outStr);
            //    tmptStr = proc.StandardOutput.ReadLine();
            //}
            /*System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                delegate
                {
                    while (true)
                    {
                        if (System.IO.File.Exists("EmergencyStop.txt"))
                        {
                            string[] lines = System.IO.File.ReadAllLines("EmergencyStop.txt", Encoding.UTF8);
                            if (lines != null && lines.Length > 0 && lines[0].Trim() == "1")
                            {
                                try
                                {
                                    Logs.Log("", "强迫退出程序...");
                                    Process p = Process.GetProcessById(procId);
                                    if (p != null)
                                        p.Kill();
                                }
                                catch (Exception e)
                                {
                                    Logs.Log("", "强迫退出程序出错：" + e.ToString());
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(1000);//1秒钟轮询一次
                    }
                }));*/
            thr = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Check));
            thr.IsBackground = true;
            thr.Start(procId);
            return outStr.ToString();
        }
        private void Check(object o)
        {
            int procId = (int)o;
            while (true)
            {
                string dir = System.AppDomain.CurrentDomain.BaseDirectory;// System.IO.Directory.GetParent(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).FullName + "\\";
                if (System.IO.File.Exists(dir + "EmergencyStop.txt"))
                {
                    string[] lines = System.IO.File.ReadAllLines(dir + "EmergencyStop.txt", Encoding.UTF8);
                    if (lines != null && lines.Length > 0 && lines[0].Trim() == "1")
                    {
                        try
                        {
                            Process p = Process.GetProcessById(procId);
                            if (p != null && !p.HasExited)
                                p.Kill();
                        }
                        catch// (Exception e)
                        {
                            //Logs.Log("", "强迫退出程序出错：" + e.ToString());
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);//1秒钟轮询一次
            }
        }

        private int OpenForm(string filePath)
        {
            try
            {
                string appStartPath = filePath;
                IntPtr userTokenHandle = IntPtr.Zero;
                ApiDefinitions.WTSQueryUserToken(ApiDefinitions.WTSGetActiveConsoleSessionId(), ref userTokenHandle);

                ApiDefinitions.PROCESS_INFORMATION procInfo = new ApiDefinitions.PROCESS_INFORMATION();
                ApiDefinitions.STARTUPINFO startInfo = new ApiDefinitions.STARTUPINFO();
                startInfo.cb = (uint)System.Runtime.InteropServices.Marshal.SizeOf(startInfo);

                ApiDefinitions.CreateProcessAsUser(
                    userTokenHandle,
                    appStartPath,
                    "",
                    IntPtr.Zero,
                    IntPtr.Zero,
                    false,
                    0,
                    IntPtr.Zero,
                    null,
                    ref startInfo,
                    out procInfo);

                if (userTokenHandle != IntPtr.Zero)
                    ApiDefinitions.CloseHandle(userTokenHandle);

                int processId = (int)procInfo.dwProcessId;
                return processId;
            }
            catch (Exception ex)
            {
                Logs.Log("", "调用Cjwdev.WindowsApi时出错：" + ex.ToString());
            }
            return 0;
        }
        private static string GetMainModuleFilepath(int processId)
        {
            string wmiQueryString = "SELECT ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
            using (var searcher = new System.Management.ManagementObjectSearcher(wmiQueryString))
            {
                using (var results = searcher.Get())
                {
                    if (results.Count > 0)
                    {
                        System.Collections.IEnumerator ie = results.GetEnumerator();
                        if (ie.MoveNext())
                        {
                            System.Management.ManagementObject mo = (System.Management.ManagementObject)ie.Current;
                            if (mo != null)
                            {
                                return (string)mo["ExecutablePath"];
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}