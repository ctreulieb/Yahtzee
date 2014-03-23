using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace YahtzeeLibrary
{
    public class Category
    {
        private int score;
        public bool isScored { get; private set; }

        public int getScore() { return score; }
        public virtual void setScore(List<int> dice) { }
    }

    public class Aces : Category { }
    public class Twos : Category { }
    public class Threes : Category { }
    public class Fours : Category { }
    public class Fives : Category { }
    public class Sixes : Category { }

    

    public class UpperSection 
    {
        public Aces aces { get; private set; }
        public Twos twos { get; private set; }
        public Threes threes { get; private set; }
        public Fours fours { get; private set; }
        public Fives fives { get; private set; }
        public Sixes sixes { get; private set; }

        public bool hasBonus()
        {
            if (getScore() >= 63)
                return true;
            return false;
        }

        public int getScore()
        {
            return 0;
        } 
    }


    public class ThreeOfAKind : Category { }
    public class FourOfAKind : Category { }
    public class FullHouse : Category { }
    public class SmStraight : Category { }
    public class LgStraight : Category { }
    public class Yahtzee : Category { }
    public class Chance : Category { }


    public class LowerSection
    {
        public ThreeOfAKind threeOfAKind { get; private set; }
        public FourOfAKind fourOfAKind { get; private set; }
        public FullHouse fullHouse { get; private set; }
        public SmStraight smStraight { get; private set; }
        public LgStraight lgStraight { get; private set; }
        public Yahtzee yahtzee { get; private set; }

        public int getScore()
        {
            return 0;
        } 
    }
}
