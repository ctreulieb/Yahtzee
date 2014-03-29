using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

/*
 * remove end turn contract, turn will en dafter scoring 
 * add player id argument to all scoring
 */

namespace YahtzeeLibrary
{
    /// <summary>
    /// Interface to be implemented by the Client to enable the server to talk to the client
    /// </summary>
    public interface ICallBack
    {
        /// <summary>
        ///  implemented by the client.
        ///  will be passed with the information needed for the user to update the ui with the
        ///  current score and which players turn it is.
        /// </summary>
        /// <param name="gameState">
        /// Class containing
        /// - Array of Players for there scores
        /// - current dice values
        /// - id of the current player whos turn it is
        /// </param>
        [OperationContract(IsOneWay = true)]
        void UpdateGui(GameState gameState);

        /// <summary>
        /// implemented by the client.
        /// will give the the current dice state so they can update the UI.
        /// </summary>
        /// <param name="dice">int[5] containing the values of the current dice state</param>
        [OperationContract(IsOneWay = true)]
        void diceUpdated(int[] dice);
    }

    /// <summary>
    /// IGame service Interface, defines Contracts needed to be implemented for a client to use this serivce.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(ICallBack))]
    public interface IGame {


        /// <summary>
        /// Will register the client with call backs and add them to the game if there is less 
        /// than 4 players already in the game.
        /// </summary>
        /// <returns>int - contains player ID of client registering with the game.
        /// If 0 is returned game is full.
        /// </returns>
        [OperationContract]
        int joinGame();


        /// <summary>
        /// Will tell the service that the client specified is ready to start the game, then will check to see if all other clients
        /// are also ready, if this the case the game begins.
        /// </summary>
        /// <param name="id">The id of the client wanting to tell the service they are ready to start the game.</param>
        [OperationContract(IsOneWay=true)]
        void ready(int id);


        /// <summary>
        /// will take the value of dice the client rolled and display it to all other users.
        /// </summary>
        /// <param name="dice"> int[5]  that contains the values of the dice</param>
        [OperationContract(IsOneWay=true)]
        void updateDice(int[] dice);

        /// <summary>
        /// will remove a player from the game.
        /// </summary>
        /// <param name="Id"> int containg the id of the user that is leaving the game</param>
        [OperationContract]
        void leaveGame(int Id);

        [OperationContract]
        void scoreAces(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreTwos(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreThrees(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreFours(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreFives(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreSixes(int playerID, int[] dice);


        [OperationContract(IsOneWay = true)]
        void scoreThreeOfAKind(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreFourOfAKind(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreFullHouse(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreSMStraight(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreLGStraight(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreYahtzee(int playerID, int[] dice);

        [OperationContract(IsOneWay = true)]
        void scoreChance(int playerID, int[] dice);
    }
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        private List<Player> players = new List<Player>();
        private int playerID = 1;
        private int[] dice = new int[6];
        private int currentTurn = 1;
        public int joinGame()
        {
            //no more than 4 players
            if(playerID > 4) {
                return 0;
            }

            ICallBack cb = OperationContext.Current.GetCallbackChannel<ICallBack>();
            players.Add(new Player(playerID, cb));
            Console.WriteLine("Player {0} has joined the game!", playerID);
            return playerID++;
        }

        private void nextTurn() {
            if (++currentTurn > players.Count)
                currentTurn = 1;
        }

        private void updateAllClients(){
            foreach(Player p in players) {
                p.callBack.UpdateGui(new GameState(players.ToArray(), dice, currentTurn));
            }
        }

        public void leaveGame(int Id)
        {
            var playerToRemove = players.Single(p => p.playerID == Id);
            players.Remove(playerToRemove);
            Console.WriteLine("Player {0} has left the game!" , Id);
        }


        public void scoreAces(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.aces.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Aces!", playerID, playerToScore.upperSection.aces.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients();
        }

        public void scoreTwos(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.twos.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Twos!", playerID, playerToScore.upperSection.twos.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients();
        }

        public void scoreThrees(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.threes.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Threes!", playerID, playerToScore.upperSection.threes.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreFours(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.fours.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Fours!", playerID, playerToScore.upperSection.fours.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreFives(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.fives.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Fives!", playerID, playerToScore.upperSection.fives.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients();
        }

        public void scoreSixes(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.sixes.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Sixes!", playerID, playerToScore.upperSection.sixes.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreThreeOfAKind(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.threeOfAKind.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Three of a Kind!", playerID, playerToScore.lowerSection.threeOfAKind.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreFourOfAKind(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.fourOfAKind.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Four of a Kind!", playerID, playerToScore.lowerSection.fourOfAKind.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreFullHouse(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.fullHouse.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Fullhouse!", playerID, playerToScore.lowerSection.fullHouse.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreSMStraight(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.smStraight.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Small Straight!", playerID, playerToScore.lowerSection.smStraight.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreLGStraight(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.lgStraight.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Large Straight!", playerID, playerToScore.lowerSection.lgStraight.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }

        public void scoreYahtzee(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.yahtzee.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Yahtzee!", playerID, playerToScore.lowerSection.yahtzee.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients();  
        }

        public void scoreChance(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.chance.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Chance!", playerID, playerToScore.lowerSection.chance.getScore());
            this.dice = dice;
            nextTurn();
            updateAllClients(); 
        }


        public void updateDice(int[] dice)
        {
            this.dice = dice;
            foreach(Player p in players) {
                p.callBack.diceUpdated(this.dice);
            }
        }


        public void ready(int id)
        {
            var readyPlayer = players.Single(p => p.playerID == id);
            readyPlayer.ready = true;

            //check to see if all players are ready.
            //if all players are ready game will start
            bool allPlayersReady = true;
            foreach(Player p in players) 
            {
                if (!p.ready)
                    allPlayersReady = false;
            }

            if(allPlayersReady)
            {
                updateAllClients();
            }
        }
    }
}
