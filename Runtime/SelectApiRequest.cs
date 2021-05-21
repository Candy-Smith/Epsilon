using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class SelectApiRequest
    {
        public string table;
        public List<string> fields;
        public WhereExpression whereExpression;

        public SelectApiRequest(string table) {
            this.table = table;
            this.fields = new List<string>();
            whereExpression = new WhereExpression();
        }

        public void addField(string field) {
            fields.Add(field);
        }

        public void addFields(params string[] fields) {
            this.fields.AddRange(fields);
        }

        public SelectApiRequest where(string name, object value) {
            whereExpression.addOr(new SimpleExpression(name, value));
            return this;
        }

        public SelectApiRequest whereOr(string name, object value) {
            whereExpression.addOr(new SimpleExpression(name, value));
            return this;
        }

        public SelectApiRequest whereAnd(string name, object value) {
            whereExpression.addAnd(new SimpleExpression(name, value));
            return this;
        }

        public string toJson() {
            bool isFields = fields != null && fields.Count > 0;
            List<OrExpression> orExpressions =  whereExpression.getOrExpressions();
            bool isCondition = orExpressions != null && orExpressions.Count > 0 && orExpressions[0].Count() > 0;
            string result = "{table:'"+table+"'"+(isFields?",fields:["+System.String.Join(",", fields)+"]":"");
            if(isCondition) {
                result += ",condition:"+whereExpression.toJson();
            }
            result += "}";

            return result;
        }
    }
}
