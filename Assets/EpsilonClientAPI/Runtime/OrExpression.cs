using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class OrExpression
    {
        public List<SimpleExpression> andExpressions;

        public OrExpression() {
            andExpressions = new List<SimpleExpression>();
        }

        public void addExpression(SimpleExpression simpleExpression) {
            andExpressions.Add(simpleExpression);
        }

        public int Count() {
            return andExpressions.Count;
        }

        public string toJson() {
            string result = "[";
            for(int i=0;i<andExpressions.Count;i++) {
                result += andExpressions[i].toJson();
                if(i<andExpressions.Count-1) result += ",";
            }
            result += "]";

            return result;
        }
    }
}
