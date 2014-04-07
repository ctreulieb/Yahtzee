// Authors: Tyler Garrow, Craig Treulieb
// Date: 07/04/2014
// File: Program.cs
// Purpose: Main program for the yahtzee server.
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
                // Start the service
                servHost.Open();

                Console.WriteLine("Yahtzee game service started, Enter 'close' to quit service");

                string input = "";
                while (input == Console.ReadLine())
                {
                    if (input == "close") 
                    {
                        servHost.Close();
                    }
                }
            }catch(Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
