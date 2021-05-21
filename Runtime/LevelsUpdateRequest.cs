using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class LevelsUpdateRequest
    {
        public class Level {
            public int level;
            public int score;
            public int stars;

            public Level(int level, int score, int stars) {
                this.level = level;
                this.score = score;
                this.stars = stars;
            }
        }

        private List<Level> levels = new List<Level>();

        public LevelsUpdateRequest addLevel(Level lvl) {
            levels.Add(lvl);
            return this;
        }

        public LevelsUpdateRequest addLevel(int level, int stars, int score)
        {
            levels.Add(new Level(level, score, stars));
            return this;
        }

        public string toJson() {
            string result = "[";

            for(int i=0;i<levels.Count;i++) {
                Level level = levels[i];
                result += "{level:"+level.level+",score:"+level.score+", stars:"+level.stars+"}";
                if(i<levels.Count-1) result += ",";
            }

            result += "]";

            return result;
        }
    }
}
