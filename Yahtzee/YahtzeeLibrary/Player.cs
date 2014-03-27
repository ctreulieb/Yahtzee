using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace YahtzeeLibrary
{
   [DataContract]
   public class Player
    {
        [DataMember]
        public int playerID { get; private set; }

        public ICallBack callBack { get; private set; }

        public bool ready { get; set; } 

        [DataMember]
        public UpperSection upperSection;
        [DataMember]
        public LowerSection lowerSection;

        public Player(int ID, ICallBack  callBack)
        {
            this.upperSection = new UpperSection();
            this.lowerSection = new LowerSection();
            this.playerID = ID;
            this.callBack = callBack;
        }

        public int getGrandTotal() {
            return upperSection.getTotal() + lowerSection.getTotal();
        }
    }
}
