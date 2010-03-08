#region Using Directives

using System;
using System.Diagnostics;
using System.Reflection;
using log4net;

#endregion

namespace Tools
{
    /// <summary>
    /// Provides a method to shell execute a process
    /// </summary>
    public static class ExecuteProcess
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Starts a process
        /// </summary>
        /// <param name="cmd">Commandline</param>
        /// <param name="args">Arguments</param>
        /// <param name="show">If true console window is shown</param>
        /// <param name="ShellExecute"></param>
        /// <param name="waitForExit">If true, thread will wait for process to exit</param>
        /// <returns></returns>
        public static Process Start(string cmd, string args, bool show, bool ShellExecute, bool waitForExit)
        {
            var psi = new ProcessStartInfo(cmd, args);
            var p = new Process();
            try
            {
                psi.WindowStyle = show ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden; 
                
                psi.UseShellExecute = ShellExecute;

                p.StartInfo = psi;
                p.Start();

                if (waitForExit)
                {
                    p.WaitForExit();
                }


                return p;
            }
            catch (Exception excep)
            {
                log.Error("Unexpected", excep);
                return null;
            }
        }
    }
}