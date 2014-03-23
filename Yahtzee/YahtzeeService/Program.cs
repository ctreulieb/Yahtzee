using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using YahtzeeLibrary;

namespace YahtzeeService
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                ServiceHost servHost = new ServiceHost(typeof(Game));

                Console.WriteLine("Yahtzee game service started, Press <Enter> to quit");
                Console.ReadKey();

                servHost.Close();
            }catch(Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
