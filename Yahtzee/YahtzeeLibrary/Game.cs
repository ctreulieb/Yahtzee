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
    public interface ICallBack
    {
        [OperationContract(IsOneWay = true)]
        void UpdateGui(GameState gameState);
    }

    [ServiceContract(CallbackContract = typeof(ICallBack))]
    public interface IGame {

        [OperationContract]
        int joinGame();

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
            ICallBack cb = OperationContext.Current.GetCallbackChannel<ICallBack>();
            players.Add(new Player(playerID, cb));
            Console.WriteLine("Player {0} has joined the game!", playerID);
            return playerID++;
        }

        private void nextTurn() {

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
                updateAllClients();
        }

        public void scoreTwos(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.twos.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Twos!", playerID, playerToScore.upperSection.twos.getScore());
            updateAllClients();
        }

        public void scoreThrees(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.threes.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Threes!", playerID, playerToScore.upperSection.threes.getScore());
            updateAllClients();
        }

        public void scoreFours(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.fours.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Fours!", playerID, playerToScore.upperSection.fours.getScore());
            updateAllClients();
        }

        public void scoreFives(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.fives.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Fives!", playerID, playerToScore.upperSection.fives.getScore());
            updateAllClients();
        }

        public void scoreSixes(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.sixes.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Sixes!", playerID, playerToScore.upperSection.sixes.getScore());
            updateAllClients();
        }

        public void scoreThreeOfAKind(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.threeOfAKind.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Three of a Kind!", playerID, playerToScore.lowerSection.threeOfAKind.getScore());
            updateAllClients();
        }

        public void scoreFourOfAKind(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.fourOfAKind.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Four of a Kind!", playerID, playerToScore.lowerSection.fourOfAKind.getScore());
            updateAllClients();
        }

        public void scoreFullHouse(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.fullHouse.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Fullhouse!", playerID, playerToScore.lowerSection.fullHouse.getScore());
            updateAllClients();
        }

        public void scoreSMStraight(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.smStraight.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Small Straight!", playerID, playerToScore.lowerSection.smStraight.getScore());
            updateAllClients();
        }

        public void scoreLGStraight(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.lgStraight.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Large Straight!", playerID, playerToScore.lowerSection.lgStraight.getScore());
            updateAllClients();
        }

        public void scoreYahtzee(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.yahtzee.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Yahtzee!", playerID, playerToScore.lowerSection.yahtzee.getScore());
            updateAllClients();
        }

        public void scoreChance(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.lowerSection.chance.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Chance!", playerID, playerToScore.lowerSection.chance.getScore());
            updateAllClients();
        }
    }
}
