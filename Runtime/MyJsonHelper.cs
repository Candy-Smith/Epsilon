namespace EpsilonServer.EpsilonClientAPI
{
    public class MyJsonHelper
    {
        public static string getValue(object val) {
            if(val is string) return "'"+val+"'";
            return val.ToString();
        }
    }
}
