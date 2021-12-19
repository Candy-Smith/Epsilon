using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class MaxPlayerprogressRequest
    {
        private List<string> facebookIds;

        public MaxPlayerprogressRequest(List<string> facebookIds) {
            this.facebookIds = facebookIds;
        }

        public MaxPlayerprogressRequest(params string[] facebookIds) {
            this.facebookIds = new List<string>();
            this.facebookIds.AddRange(facebookIds);
        }

        public MaxPlayerprogressRequest addFbId(string fbId) {
            facebookIds.Add(fbId);
            return this;
        }

        public string toJson() {
            string result = "[";

            for(int i=0;i<facebookIds.Count;i++) {
                string fbId = facebookIds[i];
                result += "\""+facebookIds[i]+"\"";
                if(i<facebookIds.Count-1) result += ",";
            }

            result += "]";

            return result;
        }
    }
}
