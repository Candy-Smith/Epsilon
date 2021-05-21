using System;
using System.Collections.Generic;
using UnityEngine;

namespace EpsilonServer.EpsilonClientAPI.EpsilonLives
{
    public class EpsilonLifesManager
    {
        /// <summary>
        /// Create Life request....
        /// </summary>
        public static void AskLife(List<string> lifeAskTo, Action<bool> callback)
        {
            var friendFbIdsJSON = ListToJsonString(lifeAskTo);
            new EpsilonRequest().Special("/game/create_life_request").SetAttribute(friendFbIdsJSON).Get(
                (response) =>
                {
                    if (!response.isNetworkError && !response.downloadHandler.text.Contains("Error") &&
                        !response.downloadHandler.text.Contains("Error"))
                    {
                        callback.Invoke(true);
                    }
                    else
                    {
                        Debug.LogError("life request not created...");
                    }
                });
        }

        public static void ConfirmLifeRequest(List<string> confirmLifeRequestList, Action<bool> callback)
        {
            var confirmLifeRequestListJson = ListToJsonString(confirmLifeRequestList);
            new EpsilonRequest().Special("/game/confirm_life_request").SetAttribute(confirmLifeRequestListJson).Get(
                (response) =>
                {
                    if (!response.isNetworkError && !response.downloadHandler.text.Contains("Error") &&
                        !response.downloadHandler.text.Contains("Error"))
                    {
                        callback.Invoke(true);
                    }
                    else
                    {
                        Debug.LogError("life request not created...");
                    }
                });
        }
        
        /*public void DenyLifeRequest(string requestId)
        {
            new EpsilonRequest().Special("/game/deny_life_request").SetAttribute("life_request_id=" + requestId).Get(
                (response) =>
                {
                    if (!response.isNetworkError)
                    {
                        var downloadHandlerText = response.downloadHandler.text;

                        ResultObject result;
                        result = JsonUtility.FromJson<ResultObject>(downloadHandlerText);
                        if (result != null)
                        {
                            Debug.Log(" :::: DenyLifeRequest Result success :::: " + result);
                        }
                        else
                        {
                            Debug.LogError(" :::: DenyLifeRequest Result is NULL :::: ");
                        }
                    }
                });
        }*/

        public static void SendLifeToFriend(List<string> sendLifeList)
        {
            var sendLifeListJSON = ListToJsonString(sendLifeList);
            new EpsilonRequest().Special("/game/send_life").SetAttribute(sendLifeListJSON).Get(
                (response) =>
                {
                    if (!response.isNetworkError && !response.downloadHandler.text.Contains("Error") &&
                        !response.downloadHandler.text.Contains("Error"))
                    {
                        Debug.Log("life request successfully created...");
                    }
                    else
                    {
                        Debug.LogError("life request not created...");
                    }
                });
        }

        public void LifeRequests(Action<LifeRequestResults> callback)
        {
            // if (!NetworkManager.THIS.IsLoggedIn)
            // {
            //     Debug.LogError("Please Facebook login first...");
            //     return;
            // }
            
            new EpsilonRequest().Special("/game/life_requests").Get(
                (response) =>
                {
                    if (!response.isNetworkError && !response.downloadHandler.text.Contains("Error") &&
                        !response.downloadHandler.text.Contains("Error"))
                    {
                        LifeRequestResults result =
                            JsonUtility.FromJson<LifeRequestResults>(response.downloadHandler.text);

                        if (result != null)
                        {
                            callback.Invoke(result);
                        }
                    }
                    else
                    {
                        Debug.Log("Error Retrieving life requests: " + response.downloadHandler.text);
                    }
                });
        }


        /// <summary>
        /// Accept Life will be removed from the tables.
        /// </summary>
        public static void AcceptLife(List<string> acceptLifeRequestList, Action<int> callback)
        {
            var acceptLifeRequestListJson = ListToJsonString(acceptLifeRequestList);
            new EpsilonRequest().Special("/game/accept_life").SetAttribute(acceptLifeRequestListJson).Get(
                (response) =>
                {
                    if (!response.isNetworkError && !response.downloadHandler.text.Contains("Error") &&
                        !response.downloadHandler.text.Contains("Error"))
                    {
                        callback(acceptLifeRequestList.Count);
                    }
                    else
                    {
                        Debug.LogError("life request not created...");
                    }
                });
        }


        private static string ListToJsonString(List<string> strList)
        {
            return ToJson(strList);
        }


        private static string ToJson(List<string> data)
        {
            string result = "[";

            for (int i = 0; i < data.Count; i++)
            {
                result += "\"" + data[i] + "\"";
                if (i < data.Count - 1) result += ",";
            }

            result += "]";

            return result;
        }
    }


    [Serializable]
    public class RequestsForLife
    {
        public string request_id;
        public string player_facebook_id;
    }

    [Serializable]
    public class ResponseOfLife
    {
        public string from_facebook_id;
        public string request_id;
    }

    [Serializable]
    public class LifeRequestResults
    {
        public List<RequestsForLife> requests_for_you_lives = new List<RequestsForLife>();
        public List<ResponseOfLife> your_approved_lives = new List<ResponseOfLife>();
    }

}
