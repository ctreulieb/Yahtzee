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

        [OperationContract(IsOneWay = true)]
        void startTurn(GameState gameState);
    }

    [ServiceContract(CallbackContract = typeof(ICallBack))]
    public interface IGame {

        [OperationContract]
        int joinGame();

        [OperationContract]
        void leaveGame(int Id);

        [OperationContract(IsOneWay=true)]
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
        public int joinGame()
        {
            ICallBack cb = OperationContext.Current.GetCallbackChannel<ICallBack>();
            players.Add(new Player(playerID, cb));
            Console.WriteLine("Player {0} has joined the game!", playerID);
            return playerID++;
        }

        private void updateAllClients(){
            foreach(Player p in players) {
                p.callBack.UpdateGui( new GameState (players, dice));
            }
        }

        public void leaveGame(int Id)
        {
            var playerToRemove = players.Single(p => p.playerID == Id);
            players.Remove(playerToRemove);
            Console.WriteLine("Player {0} has left the game!" , Id);
        }


        public void endTurn(int player, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreAces(int playerID, int[] dice)
        {
            var playerToScore = players.Single(p => p.playerID == playerID);
            playerToScore.upperSection.aces.setScore(dice);
            Console.WriteLine("Player {0} has scored {1} in Aces!", playerID, playerToScore.upperSection.aces.getScore());
        }

        public void scoreTwos(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreThrees(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreFours(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreFives(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreSixes(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreThreeOfAKind(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreFourOfAKind(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreFullHouse(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreSMStraight(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreLGStraight(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreYahtzee(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }

        public void scoreChance(int playerID, int[] dice)
        {
            throw new NotImplementedException();
        }
    }
}
