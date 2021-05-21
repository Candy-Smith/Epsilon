using System.Collections.Generic;

namespace EpsilonServer.EpsilonClientAPI
{
    public class InsertApiRequest
    {
        public string table;
        public List<string> fields;
        public List<List<object>> values;

        public InsertApiRequest(string table) {
            this.table = table;
            this.fields = new List<string>();
            this.values = new List<List<object>>();
        }

        public InsertApiRequest addField(string field) {
            this.fields.Add(field);
            return this;
        }

        public InsertApiRequest addRow(params object[] rowValues) {
            this.values.Add(new List<object>(rowValues));
            return this;
        }

        public string toJson() {
            //return JsonUtility.ToJson(this); -- List of List cannot be serialized !!!!!
            bool isFields = fields != null && fields.Count > 0;
            string jsonValues = "";
            foreach(List<object> row in values) {
                jsonValues += "[";
            
                for(int i=0;i<row.Count;i++) {
                    jsonValues += MyJsonHelper.getValue(row[i]);
                    if(i<row.Count-1) jsonValues += ",";
                }

                jsonValues += "]";
            }

            return "{table:'"+table+"',"+(isFields?"fields:["+System.String.Join(",", fields)+"],":"")+"values:["+jsonValues+"]}";
        }

    
    }
}
