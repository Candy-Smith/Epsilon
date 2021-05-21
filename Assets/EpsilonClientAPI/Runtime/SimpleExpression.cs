namespace EpsilonServer.EpsilonClientAPI
{
    public class SimpleExpression
    {
        public bool not;
        public string name;
        public object value;

        public SimpleExpression(string name, object value) {
            this.name = name;
            this.value = value;
            this.not = false;
        }

        public void setNot(bool v) {
            not = v;
        }

        public bool isNot() {
            return not;
        }

        public string toJson() {
            return "{name:"+name+",value:"+MyJsonHelper.getValue(value)+(not?",is_not:true":"")+"}";
        }
    }
}
