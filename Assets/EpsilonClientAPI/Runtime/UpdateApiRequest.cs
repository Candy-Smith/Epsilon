using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class UpdateApiRequest
    {
        public string table;
        public Dictionary<string, object> setValues;
        public WhereExpression whereExpression;

        public UpdateApiRequest(string table) {
            this.table = table;
            setValues = new Dictionary<string, object>();
            whereExpression = new WhereExpression();
        }

        public UpdateApiRequest set(string name, object value) {
            setValues.Add(name, value);
            return this;
        }

        public UpdateApiRequest where(string name, object value) {
            whereExpression.addOr(new SimpleExpression(name, value));
            return this;
        }

        public UpdateApiRequest whereOr(string name, object value) {
            whereExpression.addOr(new SimpleExpression(name, value));
            return this;
        }

        public UpdateApiRequest whereAnd(string name, object value) {
            whereExpression.addAnd(new SimpleExpression(name, value));
            return this;
        }

        public string toJson() {
            string result = "{table:'"+table+"',"+"set:[";

            int i=-1;
            foreach(KeyValuePair<string, object> setEntry in setValues) {
                result += "{name:'"+setEntry.Key+"',value:"+MyJsonHelper.getValue(setEntry.Value)+"}";
                if(++i<setValues.Count-1) result += ",";
            }

            result += "]";

            List<OrExpression> orExpressions =  whereExpression.getOrExpressions();
            bool isCondition = orExpressions != null && orExpressions.Count > 0 && orExpressions[0].Count() > 0;
            if(isCondition) {
                result += ",condition:"+whereExpression.toJson();
            }
            result += "}";

            return result;
        }
    }
}
