using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace YahtzeeLibrary
{
    [DataContract]
    public class Category
    {
        [DataMember]
        protected int score;

        [DataMember]
        public bool isScored { get; protected set; }

        public int getScore() { return score; }
        public virtual void setScore(int[] dice) { }
    }

    [DataContract]
    public class Aces : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for(int i = 0; i < 5; ++i)
            {
                if (dice[i] == 1)
                    ++score;
            }
            isScored = true;
        }
    }
    [DataContract]
    public class Twos : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (dice[i] == 2)
                    score += 2;
            }
            isScored = true;
        }
    }
    [DataContract]
    public class Threes : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (dice[i] == 3)
                    score += 3;
            }
            isScored = true;
        }
    }
    [DataContract]
    public class Fours : Category {
        public override void setScore(int[] dice)
        {
            isScored = true;
            score = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (dice[i] == 4)
                    score += 4;
            }
        }
    }
    [DataContract]
    public class Fives : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (dice[i] == 5)
                    score += 5;
            }
            isScored = true;
        }
    }
    [DataContract]
    public class Sixes : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (dice[i] == 6)
                    score += 6;
            }
            isScored = true;
        }
    }
    [DataContract]
    public class ThreeOfAKind : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            isScored = true;
            Array.Sort(dice);
            if((dice[0] == dice[1] && dice[1] == dice[2])||(dice[1]==dice[2] && dice[2]==dice[3])||(dice[2]==dice[3] && dice[3]==dice[4]))
            {
                for (int i = 0; i < 5; ++i)
                    score += dice[i];
            }
        }
    }
    [DataContract]
    public class FourOfAKind : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            isScored = true;
            Array.Sort(dice);
            if(dice[1] == dice[2] && dice[2]==dice[3] && (dice[0] == dice[1] || dice[4]==dice[1]))
            {
                for (int i = 0; i < 5; ++i)
                    score += dice[i];
            }
        }
    }
    [DataContract]
    public class FullHouse : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            isScored = true;
            Array.Sort(dice);
            if (dice[0] == dice[1] && dice[3] == dice[4] && dice[0] != dice[3] && (dice[2] == dice[0] || dice[2] == dice[3]))
                score = 25;
        }
    }
    [DataContract]
    public class SmStraight : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            isScored = true;
            Array.Sort(dice);
            if (dice[2] == dice[1] + 1 && dice[3] == dice[2] + 1 && ((dice[1] == dice[0] + 1) || (dice[4] == dice[3] + 1)))
                score = 30;

        }
    }
    [DataContract]
    public class LgStraight : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            isScored = true;
            Array.Sort(dice);
            if (dice[1] == dice[0] + 1 && dice[2] == dice[1] + 1 && dice[3] == dice[2] + 1 && dice[4] == dice[3] + 1)
                score = 40;
            
        }
        
    }
    [DataContract]
    public class Yahtzee : Category {
        public override void setScore(int[] dice)
        {
            bool isYahtzee = true;
            score = 0;
            for(int i = 0; i < 4; ++i)
            {
                if (dice[i] != dice[4])
                    isYahtzee = false;
            }
            if (isYahtzee)
                score = 50;
            isScored = true;
        }
    }
    [DataContract]
    public class Chance : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for (int i = 0; i < 5; ++i)
                score += dice[i];
            isScored = true;
        }
    }

    [DataContract]
    public class UpperSection 
    {
        [DataMember]
        public Aces aces { get; private set; }
        [DataMember]
        public Twos twos { get; private set; }
        [DataMember]
        public Threes threes { get; private set; }
        [DataMember]
        public Fours fours { get; private set; }
        [DataMember]
        public Fives fives { get; private set; }
        [DataMember]
        public Sixes sixes { get; private set; }

        public UpperSection() {
            aces = new Aces();
            twos = new Twos();
            threes = new Threes();
            fours = new Fours();
            fives = new Fives();
            sixes = new Sixes();
        }

        public bool hasBonus()
        {
            if (getSubTotal() >= 63)
                return true;
            return false;
        }

        public int getSubTotal()
        {
            return aces.getScore() + twos.getScore() + threes.getScore() + fours.getScore() + fives.getScore() + sixes.getScore();;
        } 

        public int getTotal() {
            int total = getSubTotal();
            if(hasBonus()) {
                total += 50;
            }
            return total;
        }
    }

    [DataContract]
    public class LowerSection
    {
        [DataMember]
        public ThreeOfAKind threeOfAKind { get; private set; }
        [DataMember]
        public FourOfAKind fourOfAKind { get; private set; }
        [DataMember]
        public FullHouse fullHouse { get; private set; }
        [DataMember]
        public SmStraight smStraight { get; private set; }
        [DataMember]
        public LgStraight lgStraight { get; private set; }
        [DataMember]
        public Yahtzee yahtzee { get; private set; }
        [DataMember]
        public Chance chance { get; private set; }

        public LowerSection() {
            threeOfAKind = new ThreeOfAKind();
            fourOfAKind = new FourOfAKind();
            fullHouse = new FullHouse();
            smStraight = new SmStraight();
            lgStraight = new LgStraight();
            yahtzee = new Yahtzee();
            chance = new Chance();
        }

        public int getTotal()
        {
            return threeOfAKind.getScore() + fourOfAKind.getScore() + fullHouse.getScore() + smStraight.getScore() + lgStraight.getScore() + yahtzee.getScore() + chance.getScore();
        } 
    }
}
