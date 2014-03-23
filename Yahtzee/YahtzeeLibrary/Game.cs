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

    }

    [ServiceContract(CallbackContract = typeof(ICallBack))]
    public interface IGame {

    }
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        private List<Player> players = new List<Player>();
    }
}
