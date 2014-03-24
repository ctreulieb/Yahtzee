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
                    throw new NotImplementedException();
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

        private delegate void StartTurnDelegate(GameState gameState);
        public void startTurn(GameState gameState)
        {
            try
            {
                if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(startTurn), gameState);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (playerID != 0 && game != null)
                game.leaveGame(playerID);
        }

        private void btnOnes_Click(object sender, RoutedEventArgs e)
        {
            game.scoreAces(playerID, dice);
        }


        
    }
}
