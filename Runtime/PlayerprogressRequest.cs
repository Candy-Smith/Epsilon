using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class PlayerprogressRequest
    {
        private List<string> facebookIds;
        private int level;

        public PlayerprogressRequest(int level, List<string> facebookIds) {
            this.level = level;
            this.facebookIds = facebookIds;
        }

        public PlayerprogressRequest(int level, params string[] facebookIds) {
            this.facebookIds = new List<string>();
            this.level = level;
            this.facebookIds.AddRange(facebookIds);
        }

        public PlayerprogressRequest addFbId(string fbId) {
            facebookIds.Add(fbId);
            return this;
        }

        public PlayerprogressRequest setLevel(int level)
        {
            this.level = level;
            return this;
        }

        public string toJson() {
            string result = "{\"level\":"+level+",\"f_ids\":[";

            for(int i=0;i<facebookIds.Count;i++) {
                string fbId = facebookIds[i];
                result += "\""+facebookIds[i]+"\"";
                if(i<facebookIds.Count-1) result += ",";
            }

            result += "]}";

            return result;
        }
    }
}
