// Authors: Tyler Garrow, Craig Treulieb
// Date: 07/04/2014
// File: MainWindow.xaml.cs
// Purpose: Cointains the logic for the Yahtzee game window.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ServiceModel;
using YahtzeeContracts;



namespace YahtzeeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml, implements ICallBack
    /// to handle callbacks from the server.
    /// </summary>
        [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
        UseSynchronizationContext = false)]
    public partial class MainWindow : Window, ICallBack
    {
        private IGame game = null;
        private Player thisClientsPlayer = null;
        private int playerID = 0;
        private int numRolls = 0;

        //roll should set this after a roll 
        private int[] dice = { 1, 1, 2, 3, 4 };
        //stand in hard values for testing

        /// <summary>
        /// Initilizes The Window.
        /// looks for and connects to the game server, if server returns player id 0 the game is full and client will.
        /// Will update ui to display what player id this Client.
        /// Will toggle all DieCheckboxes to unchecked and disabled so users can't hold dice before game begins.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try {
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "Game");

                game = channel.CreateChannel();

                playerID = game.joinGame();

                disableButtons();

                switch (playerID)
                {
                    case 0 :
                        MessageBox.Show("To Many Players Already in game Sorry");
                        this.Close();
                        break;
                    case 1:
                        lp1head.Background = System.Windows.Media.Brushes.PowderBlue;
                        break;
                    case 2:
                        lp2head.Background = System.Windows.Media.Brushes.PowderBlue;
                        break;
                    case 3:
                        lp3head.Background = System.Windows.Media.Brushes.PowderBlue;
                        break;
                    case 4:
                        lp4head.Background = System.Windows.Media.Brushes.PowderBlue;
                        break;
                }
                toggleDieCheckboxEnable(false);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message); 
            }
        }

        /// <summary>
        /// Sends message to client
        /// </summary>
        /// <param name="message">The message to be sent to the client</param>
        private delegate void sendMessageDelegate(string message);        
        public void sendMessage(string message)
        {
            try 
            {
                if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
                {
                    MessageBox.Show(message);
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new sendMessageDelegate(sendMessage), message);
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>
        /// Updates GUI based on gameState passed to it containing every player's scores so far as well as the dice
        /// </summary>
        /// <param name="gameState">GameState object containing the scores and dice</param>
        private delegate void ClientUpdateDelegate(GameState gameState);
        public void UpdateGui(GameState gameState)
        {
            try {
                if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
                {
                    //fill in player sheets
                    foreach(Player p in gameState.players) 
                    {
                        switch(p.playerID)
                        {
                            case 1 :
                                fillPlayerOneSheet(p);
                                break;
                            case 2 :
                                fillPlayerTwoSheet(p);
                                break;
                            case 3 :
                                fillPlayerThreeSheet(p);
                                break;
                            case 4 :
                                fillPlayerFourSheet(p);
                                break;
                        }
                    }
                    
                    if(gameState.turnID == playerID)
                    {
                        numRolls = 0;
                        btnRoll.IsEnabled = true;
                        cbDie1.IsChecked = false;
                        cbDie2.IsChecked = false;
                        cbDie3.IsChecked = false;
                        cbDie4.IsChecked = false;
                        cbDie5.IsChecked = false;

                        for(int i =0; i < gameState.players.Length; ++i) 
                        {
                            if(gameState.players[i].playerID == playerID)
                                thisClientsPlayer = gameState.players[i];                                
                        }
                    }
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(UpdateGui), gameState);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        /// <summary>
        /// fills in player one's scores
        /// </summary>
        /// <param name="player">player object containing the scores</param>
        private void fillPlayerOneSheet(Player player) 
        {
            lp1Aces.Content = player.upperSection.aces.getScore();
            lp1Twos.Content = player.upperSection.twos.getScore();
            lp1Threes.Content = player.upperSection.threes.getScore();
            lp1Fours.Content = player.upperSection.fours.getScore();
            lp1Fives.Content = player.upperSection.fives.getScore();
            lp1Sixes.Content = player.upperSection.sixes.getScore();
            lp1Subtotal.Content = player.upperSection.getSubTotal();
            int bonus = 0;
            if(player.upperSection.hasBonus()) 
            {
                bonus += 50;
            }
            lp1Bonus.Content = bonus;
            lp1UpperTotal.Content = player.upperSection.getTotal();

            lp1ThreeOfAKind.Content = player.lowerSection.threeOfAKind.getScore();
            lp1FourOfAKind.Content = player.lowerSection.fourOfAKind.getScore();
            lp1FullHouse.Content = player.lowerSection.fullHouse.getScore();
            lp1SmallStraight.Content = player.lowerSection.smStraight.getScore();
            lp1LargeStraight.Content = player.lowerSection.lgStraight.getScore();
            lp1Yahtzee.Content = player.lowerSection.yahtzee.getScore();
            lp1Chance.Content = player.lowerSection.chance.getScore();
            lp1LowerTotal.Content = player.lowerSection.getTotal();
            lp1GrandTotal.Content = player.getGrandTotal();
        }

        /// <summary>
        /// fills in player two's scores
        /// </summary>
        /// <param name="player">player object containing the scores</param>
        private void fillPlayerTwoSheet(Player player)
        {
            lp2Aces.Content = player.upperSection.aces.getScore();
            lp2Twos.Content = player.upperSection.twos.getScore();
            lp2Threes.Content = player.upperSection.threes.getScore();
            lp2Fours.Content = player.upperSection.fours.getScore();
            lp2Fives.Content = player.upperSection.fives.getScore();
            lp2Sixes.Content = player.upperSection.sixes.getScore();
            lp2Subtotal.Content = player.upperSection.getSubTotal();

            int bonus = 0;
            if (player.upperSection.hasBonus())
            {
                bonus += 50;
            }
            lp2Bonus.Content = bonus;
            lp2UpperTotal.Content = player.upperSection.getTotal();

            lp2ThreeOfAKind.Content = player.lowerSection.threeOfAKind.getScore();
            lp2FourOfAKind.Content = player.lowerSection.fourOfAKind.getScore();
            lp2FullHouse.Content = player.lowerSection.fullHouse.getScore();
            lp2SmallStraight.Content = player.lowerSection.smStraight.getScore();
            lp2LargeStraight.Content = player.lowerSection.lgStraight.getScore();
            lp2Yahtzee.Content = player.lowerSection.yahtzee.getScore();
            lp2Chance.Content = player.lowerSection.chance.getScore();
            lp2LowerTotal.Content = player.lowerSection.getTotal();
            lp2GrandTotal.Content = player.getGrandTotal();
        }

        /// <summary>
        /// fills in player three's scores
        /// </summary>
        /// <param name="player">player object containing the scores</param>
        private void fillPlayerThreeSheet(Player player)
        {
            lp3Aces.Content = player.upperSection.aces.getScore();
            lp3Twos.Content = player.upperSection.twos.getScore();
            lp3Threes.Content = player.upperSection.threes.getScore();
            lp3Fours.Content = player.upperSection.fours.getScore();
            lp3Fives.Content = player.upperSection.fives.getScore();
            lp3Sixes.Content = player.upperSection.sixes.getScore();
            lp3Subtotal.Content = player.upperSection.getSubTotal();

            int bonus = 0;
            if (player.upperSection.hasBonus())
            {
                bonus += 50;
            }
            lp3Bonus.Content = bonus;
            lp3UpperTotal.Content = player.upperSection.getTotal();

            lp3ThreeOfAKind.Content = player.lowerSection.threeOfAKind.getScore();
            lp3FourOfAKind.Content = player.lowerSection.fourOfAKind.getScore();
            lp3FullHouse.Content = player.lowerSection.fullHouse.getScore();
            lp3SmallStraight.Content = player.lowerSection.smStraight.getScore();
            lp3LargeStraight.Content = player.lowerSection.lgStraight.getScore();
            lp3Yahtzee.Content = player.lowerSection.yahtzee.getScore();
            lp3Chance.Content = player.lowerSection.chance.getScore();
            lp3LowerTotal.Content = player.lowerSection.getTotal();
            lp3GrandTotal.Content = player.getGrandTotal();
        }

        /// <summary>
        /// fills in player four's scores
        /// </summary>
        /// <param name="player">player object containing the scores</param>
        private void fillPlayerFourSheet(Player player)
        {
            lp4Aces.Content = player.upperSection.aces.getScore();
            lp4Twos.Content = player.upperSection.twos.getScore();
            lp4Threes.Content = player.upperSection.threes.getScore();
            lp4Fours.Content = player.upperSection.fours.getScore();
            lp4Fives.Content = player.upperSection.fives.getScore();
            lp4Sixes.Content = player.upperSection.sixes.getScore();
            lp4Subtotal.Content = player.upperSection.getSubTotal();

            int bonus = 0;
            if (player.upperSection.hasBonus())
            {
                bonus += 50;
            }
            lp4Bonus.Content = bonus;
            lp4UpperTotal.Content = player.upperSection.getTotal();

            lp4ThreeOfAKind.Content = player.lowerSection.threeOfAKind.getScore();
            lp4FourOfAKind.Content = player.lowerSection.fourOfAKind.getScore();
            lp4FullHouse.Content = player.lowerSection.fullHouse.getScore();
            lp4SmallStraight.Content = player.lowerSection.smStraight.getScore();
            lp4LargeStraight.Content = player.lowerSection.lgStraight.getScore();
            lp4Yahtzee.Content = player.lowerSection.yahtzee.getScore();
            lp4Chance.Content = player.lowerSection.chance.getScore();
            lp4LowerTotal.Content = player.lowerSection.getTotal();
            lp4GrandTotal.Content = player.getGrandTotal();
        }

        /// <summary>
        /// enables the die comboboxes and scoring buttons that haven't already been scored yet
        /// </summary>
        /// <param name="player">Player object containing information on what categories have been scored already</param>
        private void enableButtons(Player player) 
        {

            btnAces.IsEnabled = !player.upperSection.aces.isScored;
            btnTwos.IsEnabled = !player.upperSection.twos.isScored;
            btnThrees.IsEnabled = !player.upperSection.threes.isScored;
            btnFours.IsEnabled = !player.upperSection.fours.isScored;
            btnFives.IsEnabled = !player.upperSection.fives.isScored;
            btnSixes.IsEnabled = !player.upperSection.sixes.isScored;

            btnThreeOfAKind.IsEnabled = !player.lowerSection.threeOfAKind.isScored;
            btnFourOfAKind.IsEnabled = !player.lowerSection.fourOfAKind.isScored;
            btnFullHouse.IsEnabled = !player.lowerSection.fullHouse.isScored;
            btnSmallStraight.IsEnabled = !player.lowerSection.smStraight.isScored;
            btnLargeStraight.IsEnabled = !player.lowerSection.lgStraight.isScored;
            btnYahtzee.IsEnabled = !player.lowerSection.yahtzee.isScored;
            btnChance.IsEnabled = !player.lowerSection.chance.isScored;

            cbDie1.IsEnabled = true;
            cbDie2.IsEnabled = true;
            cbDie3.IsEnabled = true;
            cbDie4.IsEnabled = true;
            cbDie5.IsEnabled = true;
        }
        
        /// <summary>
        /// Disables all buttons and combo boxes
        /// </summary>
        private void disableButtons()
        {
            btnAces.IsEnabled =  false;
            btnTwos.IsEnabled =  false;
            btnThrees.IsEnabled = false;
            btnFours.IsEnabled = false;
            btnFives.IsEnabled = false;
            btnSixes.IsEnabled = false;

            btnThreeOfAKind.IsEnabled = false;
            btnFourOfAKind.IsEnabled = false;
            btnFullHouse.IsEnabled = false;
            btnSmallStraight.IsEnabled = false;
            btnLargeStraight.IsEnabled = false;
            btnYahtzee.IsEnabled = false;
            btnChance.IsEnabled = false;
            btnRoll.IsEnabled = false;

            cbDie1.IsEnabled = false;
            cbDie2.IsEnabled = false;
            cbDie3.IsEnabled = false;
            cbDie4.IsEnabled = false;
            cbDie5.IsEnabled = false;
        }

        /// <summary>
        /// handles what needs to happen when closing the client
        /// </summary>
        private void closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try {
                if (playerID != 0 && game != null)
                    game.leaveGame(playerID);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// sets the enabled status of all the check boxes to the value passed in
        /// </summary>
        private void toggleDieCheckboxEnable(bool value) 
        {
            cbDie1.IsEnabled = value;
            cbDie2.IsEnabled = value;
            cbDie3.IsEnabled = value;
            cbDie4.IsEnabled = value;
            cbDie5.IsEnabled = value;
        }
        
        /// <summary>
        /// updates the display for the dice
        /// </summary>
        private void diplayDice() 
        {
            var uriSource = new Uri("img/die" + dice[0].ToString() + ".png", UriKind.Relative);
            iDie1.Source = new BitmapImage(uriSource);

            uriSource = new Uri("img/die" + dice[1].ToString() + ".png", UriKind.Relative);
            iDie2.Source = new BitmapImage(uriSource);

            uriSource = new Uri("img/die" + dice[2].ToString() + ".png", UriKind.Relative);
            iDie3.Source = new BitmapImage(uriSource);

            uriSource = new Uri("img/die" + dice[3].ToString() + ".png", UriKind.Relative);
            iDie4.Source = new BitmapImage(uriSource);

            uriSource = new Uri("img/die" + dice[4].ToString() + ".png", UriKind.Relative);
            iDie5.Source = new BitmapImage(uriSource);
        }        

        /// <summary>
        /// recieves the updated dice values and calls a method to display them
        /// </summary>
        /// <param name="dice">int array representing the dice</param>
        private delegate void ClientDiceUpdate(int[] dice);
        public void diceUpdated(int[] dice)
        {
            try {
                if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
                {
                    this.dice = dice;
                    diplayDice();
                } else 
                {
                    this.Dispatcher.BeginInvoke(new ClientDiceUpdate(diceUpdated), dice);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                 
        }

        //Button handlers
        private void btnOnes_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                game.scoreAces(playerID, dice);
                disableButtons();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTwos_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                game.scoreTwos(playerID, dice);
                disableButtons();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThrees_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreThrees(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFours_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreFours(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFives_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreFives(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSixes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreSixes(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThreeOfAKind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreThreeOfAKind(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFourOfAKind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreFourOfAKind(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFullHouse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreFullHouse(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSmallStraight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreSMStraight(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLargeStraight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreLGStraight(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnYahtzee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreYahtzee(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreChance(playerID, dice);
                disableButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            if (++numRolls == 3) {
                btnRoll.IsEnabled = false;
                toggleDieCheckboxEnable(false);
            }   
            if (numRolls == 1) {
                enableButtons(thisClientsPlayer);
                toggleDieCheckboxEnable(true);
            }

            Random random = new Random();           

            if(!(bool)cbDie1.IsChecked) 
                dice[0] = random.Next(1, 7);

            if(!(bool)cbDie2.IsChecked)
                dice[1] = random.Next(1, 7);

            if (!(bool)cbDie3.IsChecked)
                dice[2] = random.Next(1, 7);
            

            if (!(bool)cbDie4.IsChecked)
                dice[3] = random.Next(1, 7);

            if (!(bool)cbDie5.IsChecked)
                dice[4] = random.Next(1, 7);

            try {
                game.updateDice(dice);    
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnReady_Click(object sender, RoutedEventArgs e)
        {
            try {
                btnReady.IsEnabled = false;
                game.ready(playerID);
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void die1Click(object sender, RoutedEventArgs e)
        {
            if(numRolls != 0)
                cbDie1.IsChecked = !cbDie1.IsChecked;
        }

        private void die2Click(object sender, RoutedEventArgs e)
        {
            if (numRolls != 0)
                cbDie2.IsChecked = !cbDie2.IsChecked;
        }

        private void die3Click(object sender, RoutedEventArgs e)
        {
            if (numRolls != 0)
                cbDie3.IsChecked = !cbDie3.IsChecked;
        }

        private void die4Click(object sender, RoutedEventArgs e)
        {
            if (numRolls != 0)
                cbDie4.IsChecked = !cbDie4.IsChecked;
        }

        private void dick5Click(object sender, RoutedEventArgs e)
        {
            if (numRolls != 0)
                cbDie5.IsChecked = !cbDie5.IsChecked;
        }
    }
}
