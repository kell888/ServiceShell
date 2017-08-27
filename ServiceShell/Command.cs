using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ServiceShell
{
    /// <summary> 
    /// Command 的摘要说明。 
    /// </summary> 
    public class Command
    {
        private Process proc = null;

        /// <summary> 
        /// 构造方法 
        /// </summary> 
        public Command()
        {
            proc = new Process();
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
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = programFile;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            if (arg != null && arg.Length > 0)
            {
                proc.StandardInput.WriteLine(arg);
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
    }
}