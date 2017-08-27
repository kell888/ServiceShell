using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Configuration;

namespace ServiceShell
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logs.Log("", "ServiceShell Start...");
            try
            {
                string apps = ConfigurationManager.AppSettings["apps"];
                string[] appList = apps.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string appArg in appList)
                {
                    string file = appArg;
                    string arg = string.Empty;
                    string[] app = appArg.Split("{".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (app.Length > 1)
                    {
                        file = app[0].Trim();
                        arg = app[1].Substring(0, app[1].Length - 1).Trim();
                        string[] parameters = arg.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (parameters.Length > 0)
                        {
                            string exeType = parameters[0];
                            Command cmd = new Command();
                            int type = 0;
                            if (int.TryParse(exeType, out type))
                            {
                                if (type == 0)
                                {
                                    if (parameters.Length > 1)
                                    {
                                        string start = parameters[1];
                                        cmd.RunCmd(file + " " + start);
                                    }
                                    else
                                    {
                                        cmd.RunCmd(file);
                                    }
                                }
                                else
                                {
                                    if (parameters.Length > 1)
                                    {
                                        string start = parameters[1];
                                        cmd.RunProgram(file, start);
                                    }
                                    else
                                    {
                                        cmd.RunProgram(file);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logs.Log("", "ServiceShell Start go wrong:" + e.ToString());
            }
        }

        protected override void OnStop()
        {
            Logs.Log("", "ServiceShell Stop...");
            try
            {
                string apps = ConfigurationManager.AppSettings["apps"];
                string[] appList = apps.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string appArg in appList)
                {
                    string file = appArg;
                    string arg = string.Empty;
                    string[] app = appArg.Split("{".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (app.Length > 1)
                    {
                        file = app[0].Trim();
                        arg = app[1].Substring(0, app[1].Length - 1).Trim();
                        string[] parameters = arg.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (parameters.Length > 0)
                        {
                            string exeType = parameters[0];
                            Command cmd = new Command();
                            int type = 0;
                            if (int.TryParse(exeType, out type))
                            {
                                if (type == 0)
                                {
                                    if (parameters.Length > 2)
                                    {
                                        string stop = parameters[2];
                                        cmd.RunCmd(file + " " + stop);
                                    }
                                    else
                                    {
                                        cmd.RunCmd(file);
                                    }
                                }
                                else
                                {
                                    if (parameters.Length > 2)
                                    {
                                        string stop = parameters[2];
                                        cmd.RunProgram(file, stop);
                                    }
                                    else
                                    {
                                        //cmd.RunProgram(file);如果没有停止参数，这里就不应该再次启动该程序
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logs.Log("", "ServiceShell Stop go wrong:" + e.ToString());
            }
        }
    }
}
