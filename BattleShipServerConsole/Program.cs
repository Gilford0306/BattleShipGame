using BattleShipServerConsole.Classes;
using System.Diagnostics;
using System.Net.Sockets;

namespace BattleShipServer
{
    class Program
    {
        public static Dictionary<string, Socket> loggedNicks = new Dictionary<string, Socket>();
        public static Dictionary<string, Socket> loggedplayingNicks = new Dictionary<string, Socket>();
        public static Dictionary<string, List<string>> enemiesoffers = new Dictionary<string, List<string>>();
        public static List<string> whowhomSentStart = new List<string>();
        public static List<string> whowhomSentGiveUp = new List<string>();
        static void Main(string[] args)
        {

            String thisprocessname = Process.GetCurrentProcess().ProcessName;
            Process[] procs = Process.GetProcesses();
            int count = 0;
            foreach (var p in procs)
            {
                if (p.ProcessName == thisprocessname)
                    count++;
            }
            if (count > 1)
                return;

            SocketListener.Start();
        }
    }

}
