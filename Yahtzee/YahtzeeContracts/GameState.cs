﻿// Authors: Tyler Garrow, Craig Treulieb
// Date: 07/04/2014
// File: GameState.cs
// Purpose: Contains the GameState class, which holds the game state and tracks the players, dice, and who's turn it is.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace YahtzeeContracts
{
    [DataContract]
    public class GameState
    {
        [DataMember]
        public Player[] players { get; private set; }

        [DataMember]
        public int[] dice { get; private set; }

        [DataMember]
        public int turnID { get; private set; }

        public GameState(Player[] p, int[] d, int t)
        {
            dice = d;
            players = p;
            turnID = t;
        }
    }
}
