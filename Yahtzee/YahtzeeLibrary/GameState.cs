using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace YahtzeeLibrary
{
    [DataContract]
    public class GameState
    {
        [DataMember]
        public List<Player> players { get; private set; }

        [DataMember]
        public int[] dice { get; private set; }

        public GameState(List<Player> p, int[] d)
        {
            dice = d;
            players = p;
        }
    }
}
