using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;
using UnityEngine.Networking;

namespace EpsilonServer.EpsilonClientAPI
{
    [ExecuteAlways]
    public class EpsilonClientAPI : MonoBehaviour
    {
        public static EpsilonClientAPI THIS;

        private void Awake()
        {
            if (THIS == null) THIS = this;
            else if (THIS != this) Destroy(gameObject);
            serverSettings = Resources.Load<ServerSettings>("Scriptable/ServerSettings");
        }

        private void Start()
        {
            Awake();
        }

        private static string _http = "https://candy-smith.info:3636";//"http://192.168.1.142:3636";
        private ServerSettings serverSettings;

        public void LoginWithFacebook( Action<UnityWebRequest> result)
        {
            SendSimpleRequiest("/player/authorization", null, "", result);
        }
        
        public void SendSimpleRequiest(string uri, EpsilonRequest request_, string body, Action<UnityWebRequest> result = null)
        {
            string url = _http+uri;
            url += request_?.extraURL;
            Debug.Log("SendSimpleRequest : " + url);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            string hash = GetHash(uri);
            if(Application.isPlaying)
            {
                StartCoroutine(SendDataCor( url, bodyRaw, hash, result));
            }
        }

        public static void SendEditorRequiest(string uri, string body, Action<UnityWebRequest> result = null)
        {
#if UNITY_EDITOR
            string url = _http+uri;
            Debug.Log("SendEditorRequiest : " + url);
            // url += request_?.extraURL;
            byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
            EditorCoroutineUtility.StartCoroutineOwnerless(SendDataCor( url, bodyRaw, null, result));
#endif
        }

        public void SendApi(string group, string command, string jsonRequest, Action<UnityWebRequest> result=null)
        {
            string uri = "/"+group+"/"+command;
            string url = _http + uri;
            Debug.Log("Send API : " + url);
            var hash = GetHash(uri);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            StartCoroutine(SendDataCor( url, bodyRaw, hash, result));
        }

        public void SendBodyRequest(string command, EpsilonRequest request, Action<UnityWebRequest> result=null)
        {
            string uri = "/api/"+command;
            uri += request.extraURL;
            if (request.specialURL != String.Empty) uri = request.specialURL;
            string url = _http + uri;
            string bodyJsonString;
            // request.attributes.Add("PlayerID", NetworkManager.UserID);
            if (!request.singleLineBody)
            {
                bodyJsonString = "[";
                foreach (var line in request.attributes)
                {
                    bodyJsonString += GetJSONBodyString(line);
                    if (!line.Equals(request.attributes.Last())) bodyJsonString += ",";
                }

                bodyJsonString += "]";
            }
            else bodyJsonString = request.attributes.First().Key;
            var hash = GetHash(uri);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            Debug.Log("send url: "+url);
            Debug.Log("send body " +bodyJsonString);
            StartCoroutine(SendDataCor( url, bodyRaw, hash, result));
        }

        private static string GetJSONBodyString(KeyValuePair<string, object> line)
        {
            if(line.Value.ToString().Contains("["))
                return "{name:\""+line.Key + "\",value:" + line.Value + "}";
            else
                return "{name:\""+line.Key + "\",value:\"" + line.Value + "\"}";
        }

        static IEnumerator SendDataCor(string url, byte[] bodyRaw, string hash, Action<UnityWebRequest> result)
        {
            var request = UnityWebRequest(url, bodyRaw, hash);
            yield return request.SendWebRequest();
            Debug.Log("receive url: "+request.url);
            if (request.uploadHandler != null) Debug.Log("receive body " +Encoding.UTF8.GetString(request.uploadHandler.data));
            // Debug.Log("hash: "+hash);
            if (request.isNetworkError || request.isHttpError)
            {
                 Debug.Log(request.downloadHandler.text /*+ " ||| " + request.url + " ||| "*/ + request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text /*+ " ||| " + request.url + " ||| "*/);
            }

            result?.Invoke(request);
        }

        private static UnityWebRequest UnityWebRequest(string url, byte[] bodyRaw, string hash="")
        {
            var request = new UnityWebRequest(url, "POST");
            request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            if (hash != null)
            {
                request.SetRequestHeader("Authorization", hash);
                var userID = PlayerPrefs.GetString("UserID");
                if (userID != null) request.SetRequestHeader("Player_id", userID);
                Debug.Log("header : Player_id " + request.GetRequestHeader("Player_id"));
            }
            if(bodyRaw.Length != 0)
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.chunkedTransfer = false;
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        private string GetHash(string uri)
        {
            var gameHash = Hash(serverSettings.APISecret,serverSettings.APIKey);
            var requestHash = Hash(gameHash, gameHash.Substring(0, 8) + uri);
            return requestHash +":"+ gameHash;
        }

        string Hash(string key, string data) {
            HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.ASCII.GetBytes(data)));
        }
    }

    class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
    {
        // Encoded RSAPublicKey
        private static string PUB_KEY = "mypublickey";
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
//            X509Certificate2 certificate = new X509Certificate2(certificateData);
//            string pk = certificate.GetPublicKeyString();
//            if (pk.ToLower().Equals(PUB_KEY.ToLower()))
//                return true;
//            return false;
        }
    }

    
}
