using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeeLibrary
{
    class Category
    {
        private int score;
        public bool isScored { get; private set; }

        public int getScore() { return score; }
        public virtual void setScore(List<int> dice) { }
    }

    class Aces : Category
    {
        public override void setScore(List<int> dice)
        {

            //score = 10;
            //base.setScore(dice);
        }
    }
    class Twos : Category
    {

    }
    class Threes : Category
    {

    }
    class Fours : Category
    {

    }
    class Fives : Category
    {

    }
    class Sixes : Category
    {

    }

    class UpperSection
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

    class ThreeOfAKind : Category { }
    class FourOfAKind : Category { }
    class FullHouse : Category { }
    class SmStraight : Category { }
    class LgStraight : Category { }
    class Yahtzee : Category { }
}
