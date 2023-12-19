namespace StressTesting
{
    using System.Diagnostics;
    using CompassWrapper;
    using Microsoft.VisualBasic.Devices;
    using Model;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var malletParameters = new MalletParameters();
            malletParameters.HeadLength = 125.0;
            malletParameters.HeadWidth = 75.0;
            malletParameters.HeadHeight = 75.0;
            malletParameters.HandleHeight = 175.0;
            malletParameters.HandleDiameter = 60.0;
            var compassWrapper = new CompassWrapper();
            var streamWriter = new StreamWriter("log.txt", true);
            var currentProcess = Process.GetCurrentProcess();
            var count = 0;
            while (true)
            {
                const double gigabyteInByte = 0.000000000931322574615478515625;
                MalletBuilder.Build(compassWrapper, malletParameters);
                var computerInfo = new ComputerInfo();
                var usedMemory = (computerInfo.TotalPhysicalMemory
                                  - computerInfo.AvailablePhysicalMemory)
                                 * gigabyteInByte;
                streamWriter.WriteLine(
                    $"{++count}\t{stopWatch.Elapsed:hh\\:mm\\:ss}\t{usedMemory}");
                streamWriter.Flush();
            }
        }
    }
}