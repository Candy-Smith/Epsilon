using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class WhereExpression
    {
        private List<OrExpression> orExpressions;

        public WhereExpression() {
            orExpressions = new List<OrExpression>();
        }

        public void addOr(SimpleExpression exp) {
            OrExpression newOrExpression = new OrExpression();
            newOrExpression.addExpression(exp);
            orExpressions.Add(newOrExpression);
        }

        public void addAnd(SimpleExpression exp) {
            orExpressions[orExpressions.Count-1].addExpression(exp);
        }

        public List<OrExpression> getOrExpressions() {
            return orExpressions;
        }

        public string toJson() {
            string result = "[";
            for(int i=0;i<orExpressions.Count;i++) {
                result += orExpressions[i].toJson();
                if(i<orExpressions.Count-1) result += ",";
            }
            result += "]";

            return result;
        }
    }
}
