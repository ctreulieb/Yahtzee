using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeeLibrary
{
    class Categorie
    {
        private int score;
        public bool isScored { get; private set; }

        public int getScore() { return score; }
        public virtual void setScore(List<int> dice) { }
    }

    class Aces : Categorie
    {
        public override void setScore(List<int> dice)
        {

            //score = 10;
            //base.setScore(dice);
        }
    }
    class Twos : Categorie
    {

    }
    class Threes : Categorie
    {

    }
    class Fours : Categorie
    {

    }
    class Fives : Categorie
    {

    }
    class Sixes : Categorie
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

    class ThreeOfAKind : Categorie { }
    class FourOfAKind : Categorie { }
    class FullHouse : Categorie { }

}
