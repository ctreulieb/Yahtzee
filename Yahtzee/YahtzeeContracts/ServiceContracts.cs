using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace YahtzeeContracts
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
    public interface IGame
    {


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
        [OperationContract(IsOneWay = true)]
        void ready(int id);


        /// <summary>
        /// will take the value of dice the client rolled and display it to all other users.
        /// </summary>
        /// <param name="dice"> int[5]  that contains the values of the dice</param>
        [OperationContract(IsOneWay = true)]
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
}
