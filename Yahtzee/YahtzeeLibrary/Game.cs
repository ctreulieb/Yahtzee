// Authors: Tyler Garrow, Craig Treulieb
// Date: 07/04/2014
// File: Game.cs
// Purpose: Contains implementation of IGame contract. Allows for joining, leaving, scoring, and taking turns in a game
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using YahtzeeContracts;
/*
 * 
 * */
namespace YahtzeeLibrary
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        private List<Player> players = new List<Player>();
        private int playerID = 1;
        private int[] dice = new int[6];
        private int currentTurn = 1;

        ~Game() {
            sendMessageToAllClients("Yahzee game service is no longer running you will not be able to complete the game. Sorry");
        }

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

            bool gameOver = true;
            foreach(Player p in players) {
                if (!p.allScored())
                    gameOver = false;
            }

            if(gameOver) {
                var sortedPlayers = players.OrderBy(p => p.getGrandTotal()).ToList();
                sortedPlayers.Reverse();
                if(sortedPlayers.Count == 1)
                    sendMessageToAllClients("GameOver Player " + sortedPlayers[0].playerID + "Won");
                else if(sortedPlayers.Count == 2)
                {
                    if(sortedPlayers[0].getGrandTotal() == sortedPlayers[1].getGrandTotal())
                        sendMessageToAllClients("Game Over! Tie Game!");
                    else
                        sendMessageToAllClients("Game Over! Player " + sortedPlayers[0].playerID + "Won");
                }
                else if(sortedPlayers.Count == 3)
                {
                    if(sortedPlayers[0].getGrandTotal() == sortedPlayers[1].getGrandTotal())
                    {
                        if(sortedPlayers[0].getGrandTotal() == sortedPlayers[2].getGrandTotal())
                        {
                            sendMessageToAllClients("Game Over! Three Way Tie!");
                        }
                        else
                        {
                            sendMessageToAllClients("Game Over! Players " + sortedPlayers[0].playerID + ", and " + sortedPlayers[1].playerID + " Won");
                        }
                    }
                    else
                    {
                        sendMessageToAllClients("Game Over! Player " + sortedPlayers[0].playerID + "Won");
                    }
                }
                else
                {
                    if(sortedPlayers[0].getGrandTotal() == sortedPlayers[1].getGrandTotal())
                    {
                        if(sortedPlayers[0].getGrandTotal() == sortedPlayers[2].getGrandTotal())
                        {
                            if(sortedPlayers[0].getGrandTotal() == sortedPlayers[3].getGrandTotal())
                            {
                                sendMessageToAllClients("Game Over! Four Way Tie! Absolutely Remarkable!");
                            }
                        }
                        else
                        {
                            sendMessageToAllClients("Game Over! Players " + sortedPlayers[0].playerID + ", " + sortedPlayers[1].playerID + ", and " + sortedPlayers[2].playerID + " Won");
                        }
                    }
                    else
                    {
                        sendMessageToAllClients("Game Over! Player " + sortedPlayers[0].playerID + "Won");
                    }
                }
            }
        }

        private void updateAllClients(){
            foreach(Player p in players) {
                p.callBack.UpdateGui(new GameState(players.ToArray(), dice, currentTurn));
            }
        }

        private void sendMessageToAllClients(string message) {
            foreach (Player p in players)
            {
                p.callBack.sendMessage(message);
            }
        }
        public void leaveGame(int Id)
        {
            if(currentTurn == Id) {
                nextTurn();
            }

            var playerToRemove = players.Single(p => p.playerID == Id);
            players.Remove(playerToRemove);
            Console.WriteLine("Player {0} has left the game!" , Id);
            updateAllClients();
            sendMessageToAllClients("Player " + Id + " has left the game");
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
