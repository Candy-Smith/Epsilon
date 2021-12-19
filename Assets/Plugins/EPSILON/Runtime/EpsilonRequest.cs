using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace EpsilonServer.EpsilonClientAPI
{
    public class EpsilonRequest
    {
        public string APIKey;
        public string playerID;
        public string table;
        public Dictionary<string, object> attributes;
        public string extraURL="";
        public string specialURL ="";
        public bool singleLineBody;
        public EpsilonRequest()
        {
            attributes = new Dictionary<string, object>();
        }

        public EpsilonRequest SetTable(string table)
        {
            this.table = table;
            return this;
        }

        public EpsilonRequest SetAttribute(string key, string value)
        {
            Debug.Log("Set Attributes : " + key + "     value : " + value);
            attributes.Add(key,value);
            return this;
        }
        public EpsilonRequest SetAttribute(string key)
        {
            Debug.Log("Set Attributes : " + key);
            attributes.Add(key,null);
            this.singleLineBody = true;
            return this;
        }
        public EpsilonRequest SetAttribute(string key, int value)
        {
            Debug.Log("Set Attributes : " + key + "     value : " + value);
            attributes.Add(key,value);
            return this;
        }
        public EpsilonRequest SetAttribute(string key, bool value)
        {
            attributes.Add(key,value);
            return this;
        }
        public EpsilonRequest AddURL(string extraUrl)
        {
            extraURL = extraUrl;
            return this;
        }
        public EpsilonRequest Special(string extraUrl)
        {
            specialURL = extraUrl;
            Debug.Log("Special URL : " + specialURL);
            return this;
        }
        public void Send(Action<UnityWebRequest> result=null)
        {
            EpsilonClientAPI.THIS.SendBodyRequest("insert", this, result);
        }
        public void Get(Action<UnityWebRequest> result=null)
        {
            Debug.Log(EpsilonClientAPI.THIS);
            EpsilonClientAPI.THIS.SendBodyRequest("select", this, result);
        }
        
        public void SendGenerateKey(Action<UnityWebRequest> result=null)
        {
            EpsilonClientAPI.THIS.SendSimpleRequiest("/system/generate_api_key", this, "", result);
        }
        
        public void SendRegisterGame(string body, Action<UnityWebRequest> result = null)
        {
            EpsilonClientAPI.THIS.SendSimpleRequiest("/system/register_game", this, body ,result);
        }
        
    }
    
    public class EpsilonResult
    {
        public bool HasError;
        public string error;
    }
}
