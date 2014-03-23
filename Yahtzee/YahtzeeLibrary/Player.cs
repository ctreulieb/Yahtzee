using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeeLibrary
{
    class Player
    {
        public int playerID { get; private set; }
        public int Score { get; private set; }


        public Player(int ID)
        {
            this.playerID = ID;
        }
    }
}
