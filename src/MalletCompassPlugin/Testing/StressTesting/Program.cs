namespace StressTesting
{
    using System.Diagnostics;
    using CompassWrapper;
    using Microsoft.VisualBasic.Devices;
    using Model;

    /// <summary>
    /// Класс для нагрузочного тестирования.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа.
        /// </summary>
        /// <param name="args">Аргументы.</param>
        private static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var malletParameters = new MalletParameters(
                HeadType.Rectangle,
                0,
                75,
                75,
                75,
                125,
                175,
                50);
            var compassWrapper = new CompassWrapper();
            var streamWriter = new StreamWriter("log.txt", true);
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