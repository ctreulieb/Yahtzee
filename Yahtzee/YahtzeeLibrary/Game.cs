using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace YahtzeeLibrary
{
    public interface ICallBack
    {
        [OperationContract(IsOneWay = true)]
        void UpdateGui(List<Player> players);

        [OperationContract(IsOneWay = true)]
        void startTurn(List<Player> players);
    }

    [ServiceContract(CallbackContract = typeof(ICallBack))]
    public interface IGame {
        [OperationContract]
        int joinGame();

        [OperationContract(IsOneWay = true)]
        void endTurn(int player);

        [OperationContract(IsOneWay = true)]
        void leaveGame(int playerID);

        [OperationContract(IsOneWay=true)]
        void scoreAces(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreTwos(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreThrees(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreFours(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreFives(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreSixes(int playerID);


        [OperationContract(IsOneWay = true)]
        void scoreThreeOfAKind(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreFourOfAKind(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreFullHouse(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreSMStraight(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreLGStraight(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreYahtzee(int playerID);

        [OperationContract(IsOneWay = true)]
        void scoreChance(int playerID);
    }
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        private List<Player> players = new List<Player>();
        private int playerID = 1;
        public int joinGame()
        {
            ICallBack cb = OperationContext.Current.GetCallbackChannel<ICallBack>();
            players.Add(new Player(playerID, cb));

            return playerID++;
        }

        private void updateAllClients(){
            foreach(Player p in players) {
                p.callBack.UpdateGui(players);
            }
        }


        public void scoreAces(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreTwos(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreThrees(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreFours(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreFives(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreSixes(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreThreeOfAKind(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreFourOfAKind(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreFullHouse(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreSMStraight(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreLGStraight(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreYahtzee(int playerID)
        {
            throw new NotImplementedException();
        }

        public void scoreChance(int playerID)
        {
            throw new NotImplementedException();
        }


        public void leaveGame(int playerID)
        {
            var playerToRemove = players.Single(p => p.playerID == playerID);
            players.Remove(playerToRemove);
        }

        public void endTurn(int player)
        {
            throw new NotImplementedException();
        }
    }
}
