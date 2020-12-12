using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentDiary
{
    static class Program
    {
        public static string FilePath = $@"{Environment.CurrentDirectory}\Student.txt";
        public static List<string> AllClasses = new List<string>() { "1A", "1B", "2A", "2B", "3A", "3B", "4A", "4B" };
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
