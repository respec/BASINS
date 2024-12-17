
namespace D4EMDownloadDebugger
{
    using System;
    using System.IO;
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                usage();
                return;
            }
            bool silentconsole = false;
            var specfilepath = "";
            var exefilepath = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "/s" || args[i] == "/S")
                {
                    silentconsole = true;
                }
                if (args[i] == "/f" || args[i] == "/F")
                {
                    if (i + 1 < args.Length && string.IsNullOrEmpty(specfilepath))
                    {
                        specfilepath = args[i + 1];
                    }
                }
                if (args[i] == "/e" || args[i] == "/E")
                {
                    if (i + 1 < args.Length && string.IsNullOrEmpty(exefilepath))
                    {
                        exefilepath = args[i + 1];
                    }
                }
            }
            if (string.IsNullOrEmpty(specfilepath))
            {
                Console.WriteLine("Must provide data download spec file path.");
                return;
            }
            if (string.IsNullOrEmpty(exefilepath) || !File.Exists(exefilepath))
            {
                Console.WriteLine("Must provide D4EMDownload.exe file path.");
                return;
            }
            Execute(specfilepath, exefilepath);
        }

        static void usage()
        {
            Console.WriteLine("Toolbox Datadownload Debugger:\n\n");
            Console.WriteLine("D4EMDownloadDebugger [/S] /F SpecFileFullPath /E D4EMDownloadExeFullPath\n");
            Console.WriteLine("[/S]                   Silent mode switch. Verbose if not specified.\n");
            Console.WriteLine("/F SpecFileFullPath    Must specify download spec file full path.\n");
            Console.WriteLine("/E D4EMDownloadExeFullPath    Must specify D4EMDownload.exe full path.\n");
        }

        public static string Execute(string aQueryFilePath, string aD4EMDownloadExe)
        {
            var lResult = "";
            try
            {
                //string lQueryFilename As String = GetTemporaryFileName("DataDownloadQuery", ".txt")
                string lResultsFilename = Path.Combine(Path.GetDirectoryName(aQueryFilePath), "DataDownloadResults.txt");
                //Logger.Dbg("Writing Data Download Query to " & lQueryFilename & ", requesting results in " & lResultsFilename)
                //SaveFileString(lQueryFilename, aQuery)
                string lArgs = @"""" + lResultsFilename + @"""" + " " + @"""" + aQueryFilePath + @"""";
                LaunchProgram(aD4EMDownloadExe, Path.GetDirectoryName(aQueryFilePath), lArgs);
                if (File.Exists(lResultsFilename))
                {
                    return File.ReadAllText(lResultsFilename);
                }
                else
                {
                    return "<error>Download did not complete, result status Not found." + "</error>";
                }
            }
            catch (Exception lEx)
            {
                lResult = lEx.InnerException.Message;
            }
            return lResult;
        }

        static int LaunchProgram(string aExeName, string aWorkingDirectory, string aArguments = "", bool aWait = true)
        {
            int lExitCode = 0;
        try
                {

            var lProcess = new System.Diagnostics.Process();
                lProcess.StartInfo.FileName = aExeName;
                lProcess.StartInfo.WorkingDirectory = aWorkingDirectory;
                lProcess.StartInfo.CreateNoWindow = true;
                lProcess.StartInfo.UseShellExecute = false;
                if (!string.IsNullOrEmpty(aArguments))
                    lProcess.StartInfo.Arguments = aArguments;
                lProcess.StartInfo.RedirectStandardOutput = true;
                //AddHandler lProcess.OutputDataReceived, AddressOf MessageHandler
                lProcess.StartInfo.RedirectStandardError = true;
                //AddHandler lProcess.ErrorDataReceived, AddressOf MessageHandler

            lProcess.Start();
                lProcess.BeginErrorReadLine();
                lProcess.BeginOutputReadLine();
            if (aWait)
                {

//KeepWaiting:
                try
                    {
                        lProcess.WaitForExit();
                        lExitCode = lProcess.ExitCode;

                    }
                catch (Exception lWaitError)
                    {
                        Console.WriteLine(lWaitError.Message);

                    }
                    //If Not lProcess.HasExited Then GoTo KeepWaiting
                    Console.WriteLine("LaunchProgram: " + aExeName + ": Exit code " + lExitCode);
                }
            }
        catch (ApplicationException lEx)
            {

                Console.WriteLine("LaunchProgram: " + aExeName + ": Exception: " + lEx.Message);
                lExitCode = -1;
            }
            return lExitCode;
    }

    }
}
