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
        protected int score;
        public bool isScored { get; protected set; }

        public int getScore() { return score; }
        public virtual void setScore(int[] dice) { }
    }

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
    public class Fours : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for (int i = 0; i < 5; ++i)
            {
                if (dice[i] == 4)
                    score += 4;
            }
            isScored = true;
        }
    }
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
    public class Yahtzee : Category {
        public override void setScore(int[] dice)
        {
            bool isYahtzee = true;
            score = 0;
            for(int i = 0; i < 4; ++i)
            {
                if (dice[i] != dice[5])
                    isYahtzee = false;
            }
            if (isYahtzee)
                score = 50;
            isScored = true;
        }
    }
    public class Chance : Category {
        public override void setScore(int[] dice)
        {
            score = 0;
            for (int i = 0; i < 5; ++i)
                score += dice[i];
            isScored = true;
        }
    }
    

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
            return aces.getScore() + twos.getScore() + threes.getScore() + fours.getScore() + fives.getScore() + sixes.getScore();;
        } 
    }

    public class LowerSection
    {
        public ThreeOfAKind threeOfAKind { get; private set; }
        public FourOfAKind fourOfAKind { get; private set; }
        public FullHouse fullHouse { get; private set; }
        public SmStraight smStraight { get; private set; }
        public LgStraight lgStraight { get; private set; }
        public Yahtzee yahtzee { get; private set; }
        public Chance chance { get; private set; }

        public int getScore()
        {
            return 0;
        } 
    }
}
