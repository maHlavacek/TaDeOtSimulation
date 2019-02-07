using System;
using TadeotSimulation.Core;

namespace TadeotSimulation.SimulationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simulation der Kurzpräsentation am Tag der offenen Tür");
            Console.WriteLine("======================================================");
            Controller controller = new Controller();
            controller.Log += Controller_Log;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
            controller.ReadVisitorsFromCsv();
            controller.StartSimulation();

            Console.ReadLine();
        }

        private static void Instance_OneMinuteIsOver(object sender, DateTime time)
        {
            if (time.Minute == 0)
            {
                Console.WriteLine($"--------------------------- {time.Hour:00}:{time.Minute:00} -------------------------------");
            }
        }

        private static void Controller_Log(object sender, string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}
