using UnityEngine;

namespace EpsilonServer.EpsilonClientAPI
{
    [CreateAssetMenu(fileName = "ServerSettings", menuName = "ServerSettings", order = 0)]
    public class ServerSettings : ScriptableObject
    {
        public string APIKey;
        public string APISecret;
    }
}