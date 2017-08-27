﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServiceShell
{
    public static class Logs
    {
        private static string logPath = AppDomain.CurrentDomain.BaseDirectory + "Logs";
        public static void Create(string msg, string errPoint = null)
        {
            msg = msg ?? "";
            if (!string.IsNullOrEmpty(errPoint))
                msg += Environment.NewLine + "【出错位置】：" + errPoint;
            DateTime now = DateTime.Now;
            try
            {
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);
                string path = logPath + "\\" + now.ToString("yyyy-MM-dd") + ".log";
                File.AppendAllText(path, "[" + now.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Minutes.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Seconds.ToString().PadLeft(2, '0') + "]" + msg + Environment.NewLine + Environment.NewLine, Encoding.UTF8);
            }
            catch
            {
                if (!Directory.Exists("c:\\Logs"))
                    Directory.CreateDirectory("c:\\Logs");
                string path = "c:\\Logs\\" + now.ToString("yyyy-MM-dd") + ".log";
                File.AppendAllText(path, "[" + now.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Minutes.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Seconds.ToString().PadLeft(2, '0') + "]" + msg + Environment.NewLine + Environment.NewLine, Encoding.UTF8);
            }
        }
        public static void Log(string user, string msg)
        {
            DateTime now = DateTime.Now;
            try
            {
                if (!Directory.Exists(logPath + "\\" + user))
                    Directory.CreateDirectory(logPath + "\\" + user);
                string path = logPath + "\\" + user + "\\" + now.ToString("yyyy-MM-dd") + ".log";
                File.AppendAllText(path, "[" + now.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Minutes.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Seconds.ToString().PadLeft(2, '0') + "]" + msg + Environment.NewLine + Environment.NewLine, Encoding.UTF8);
            }
            catch
            {
                if (!Directory.Exists("c:\\Logs" + user))
                    Directory.CreateDirectory("c:\\Logs\\" + user);
                string path = "c:\\Logs\\" + user + "\\" + now.ToString("yyyy-MM-dd") + ".log";
                File.AppendAllText(path, "[" + now.TimeOfDay.Hours.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Minutes.ToString().PadLeft(2, '0') + ":" + now.TimeOfDay.Seconds.ToString().PadLeft(2, '0') + "]" + msg + Environment.NewLine + Environment.NewLine, Encoding.UTF8);
            }
        }
    }
}
