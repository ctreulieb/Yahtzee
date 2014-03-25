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
using YahtzeeLibrary;

namespace YahtzeeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
        [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
        UseSynchronizationContext = false)]
    public partial class MainWindow : Window, ICallBack
    {
        private IGame game = null;
        private int playerID = 0;

        //roll should set this after a roll 
        private int[] dice = { 1, 1, 2, 3, 4 };
        //stand in hard values for testing

        
        public MainWindow()
        {
            InitializeComponent();
            try {
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "Game");

                game = channel.CreateChannel();

                playerID = game.joinGame();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message); 
            }
        }
        private delegate void ClientUpdateDelegate(GameState gameState);
        public void UpdateGui(GameState gameState)
        {
            try {
                if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
                {
                    //fill in player sheets
                    fillPlayerOneSheet(gameState.players[0]);
                    if(gameState.players.Length >= 2) {
                        fillPlayerTwoSheet(gameState.players[1]);
                    }

                    Player thisClientsPlayer;
                    //dispable buttons
                    for(int i =0; i < gameState.players.Length; ++i) {
                        if(gameState.players[i].playerID == playerID) {
                            thisClientsPlayer = gameState.players[i];
                            disableButtons(thisClientsPlayer);
                            dice = gameState.dice;
                        }
                    }
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(UpdateGui), gameState);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
           
            
        }

        private void fillPlayerOneSheet(Player player) {
            lp1Aces.Content = player.upperSection.aces.getScore();
            lp1Twos.Content = player.upperSection.twos.getScore();
            lp1Threes.Content = player.upperSection.threes.getScore();
            lp1Fours.Content = player.upperSection.fours.getScore();
            lp1Fives.Content = player.upperSection.fives.getScore();
            lp1Sixes.Content = player.upperSection.sixes.getScore();
            lp1Subtotal.Content = player.upperSection.getSubTotal();
            int bonus = 0;
            if(player.upperSection.hasBonus()) {
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

        private void disableButtons(Player player) {

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
        }
        
        private void closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (playerID != 0 && game != null)
                game.leaveGame(playerID);
        }

        private void btnOnes_Click(object sender, RoutedEventArgs e)
        {
            try {
                game.scoreAces(playerID, dice);
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTwos_Click(object sender, RoutedEventArgs e)
        {
            try {
                game.scoreTwos(playerID, dice);
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThrees_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                game.scoreThrees(playerID, dice);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRoll_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
           

            if(!(bool)cbDie1.IsChecked) {
                dice[0] = random.Next(1, 6);
            }

            if(!(bool)cbDie2.IsChecked) {
                dice[1] = random.Next(1, 6);
            }
            if (!(bool)cbDie3.IsChecked)
            {
                dice[2] = random.Next(1, 6);
            }

            if (!(bool)cbDie4.IsChecked)
            {
                dice[3] = random.Next(1, 6);
            }

            if (!(bool)cbDie5.IsChecked)
            {
                dice[4] = random.Next(1, 6);
            }

            diplayDice();
        }
        private void diplayDice() {
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
    }
}
