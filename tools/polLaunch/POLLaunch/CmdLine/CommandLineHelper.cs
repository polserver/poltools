using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Security.Permissions;

//http://www.codeproject.com/KB/string/CommandLineHelper.aspx
namespace AndrewTweddle.Tools.Utilities.CommandLine
{
    [SecurityPermissionAttribute(SecurityAction.LinkDemand, Unrestricted=true)]
    public static class CommandLineHelper
    {
        private delegate string StringDelegate();

        public static string Run(string fileName, string arguments, 
            out string errorMessage)
        {
            errorMessage = "";
            Process cmdLineProcess = new Process();
			using (cmdLineProcess)
			{
				cmdLineProcess.StartInfo.FileName = fileName;
				cmdLineProcess.StartInfo.Arguments = arguments;
				cmdLineProcess.StartInfo.UseShellExecute = false;
				cmdLineProcess.StartInfo.CreateNoWindow = true;
				cmdLineProcess.StartInfo.RedirectStandardOutput = true;
				cmdLineProcess.StartInfo.RedirectStandardError = true;

				try
				{
					cmdLineProcess.Start();
				}
				catch
				{
					/* Note: arguments aren't also shown in the 
                     * exception as they might contain privileged 
                     * information (such as passwords).
                     */
					throw new CommandLineException(String.Format(
						"Could not start command line process: {0}",
						fileName));
				}
				return ReadProcessOutput(cmdLineProcess, ref errorMessage, fileName);
			}
        }

        private static string ReadProcessOutput(Process cmdLineProcess, 
            ref string errorMessage, string fileName)
        {
            StringDelegate outputStreamAsyncReader
                = new StringDelegate(cmdLineProcess.StandardOutput.ReadToEnd);
            StringDelegate errorStreamAsyncReader
                = new StringDelegate(cmdLineProcess.StandardError.ReadToEnd);

            IAsyncResult outAR 
                = outputStreamAsyncReader.BeginInvoke(null, null);
            IAsyncResult errAR = errorStreamAsyncReader.BeginInvoke(null, null);

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                /* WaitHandle.WaitAll fails on single-threaded 
                 * apartments. PolloutputStreamAsyncReader for completion instead:
                 */
                while (!(outAR.IsCompleted && errAR.IsCompleted))
                {
                    /* Check again every 10 milliseconds: */
                    Thread.Sleep(10);
                }
            }
            else
            {
                WaitHandle[] arWaitHandles = new WaitHandle[2];
                arWaitHandles[0] = outAR.AsyncWaitHandle;
                arWaitHandles[1] = errAR.AsyncWaitHandle;

                if (!WaitHandle.WaitAll(arWaitHandles))
                {
                    throw new CommandLineException(
                        String.Format("Command line aborted: {0}", fileName));
                    /* Note: arguments aren't also shown in the 
                     * exception as they might contain privileged 
                     * information (such as passwords).
                     */
                }
            }

            string results = outputStreamAsyncReader.EndInvoke(outAR);
            errorMessage = errorStreamAsyncReader.EndInvoke(errAR);

            /* At this point the process should surely have exited,
             * since both the error and output streams have been fully read.
             * To be paranoid, let's check anyway...
             */
            if (!cmdLineProcess.HasExited)
            {
                cmdLineProcess.WaitForExit();
            }

            return results;
        }

        public static string Run(string fileName, string arguments)
        {
            string result;
            string errorMsg = String.Empty;
            
            result = Run(fileName, arguments, out errorMsg);

            if (errorMsg.Length > 0)
                throw new CommandLineException(errorMsg);

            return result;
        }

        public static string Run(string fileName)
        {
            return Run(fileName, "");
        }
    }
}
