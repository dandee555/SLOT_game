using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System;
using System.Text;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    #region Private Fields

    private const string GATEWAY_URL = @"https://pas2-game-rd-lb.sayyogames.com:61337/api/unityexam/getroll";

    #endregion

    #region Public Helper Methods

    [Button("Get Roll Data")]
    public void GetRollData()
    {
        StartCoroutine(SendRequest());
    }

    #endregion

    #region Private Helper Methods

    private IEnumerator SendRequest()
    {
        PostData postData = new()
        {
            METHOD = "spin",
            PARAMS = "test"
        };

        string jsonPostData = JsonUtility.ToJson(postData);
        byte[] bodyRaw      = Encoding.UTF8.GetBytes(jsonPostData);

        using UnityWebRequest request = new(GATEWAY_URL, "POST");

        request.uploadHandler   = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if(ConnectionError() || ProtocalError())
        {
            Debug.LogError($"error : {request.error}");
        }
        else
        {
            string responseJson = request.downloadHandler.text;
            Debug.Log($"response : {responseJson}");
        }


        bool ConnectionError() => request.result == UnityWebRequest.Result.ConnectionError;
        bool ProtocalError()   => request.result == UnityWebRequest.Result.ProtocolError;
    }

    #endregion
}

[Serializable]
public class PostData
{
    public string METHOD;
    public string PARAMS;
}
