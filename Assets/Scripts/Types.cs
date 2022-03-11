using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Text;
using System.Threading.Tasks;

namespace RollaBall
{
    internal class Types
    {
        public class Levels
        {
            private const int maxLevels = 3;
            public List<string> levels = new List<string>();

            public Levels()
            {
                foreach (int level in Enumerable.Range(0, maxLevels + 1).ToList())
                {
                    levels.Add(level.ToString());
                }
            }
        }

        public class Level
        {
            public int difficulty;
            public int maxCount;
            public int minCount;

            public Level()
            {
                ChangeDifficulty(0);
            }

            public Level(int alevel)
            {
                ChangeDifficulty(alevel);
            }

            public void ChangeDifficulty(int toLevel)
            {
                this.difficulty = toLevel;
                switch (toLevel)
                {
                    case 0: this.maxCount = 12; this.minCount = 8; break;
                    case 1: this.maxCount = 20; this.minCount = 15; break;
                    case 2: this.maxCount = 28; this.minCount = 20; break;
                    case 3: this.maxCount = 45; this.minCount = 30; break;
                    default: this.maxCount = 12; this.minCount = 8; break;
                }

            }
        }

    }
}
