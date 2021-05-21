using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class BoostsUpdateRequest
    {
        public class Boost
        {
            public string name;
            public int count;

            public Boost(string name, int count)
            {
                this.name = name;
                this.count = count;
            }
        }

        private List<Boost> boosts = new List<Boost>();

        public BoostsUpdateRequest addBoost(Boost boost)
        {
            boosts.Add(boost);
            return this;
        }

        public string toJson()
        {
            string result = "[";

            for (int i = 0; i < boosts.Count; i++)
            {
                Boost boost = boosts[i];
                result += "{name:\"" + boost.name + "\",count:" + boost.count + "}";
                if (i < boosts.Count - 1) result += ",";
            }

            result += "]";

            return result;
        }
    }
}