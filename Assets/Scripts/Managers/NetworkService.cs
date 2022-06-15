using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService
{
    private const string jsonApi =
         "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&appid=2482348cb6cb754ad86ceb3f7935aca9";
    private const string webImage =
        "https://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";


    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError)
            { Debug.LogError($"network problem: {request.error}"); }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            { Debug.LogError($"response error: {request.responseCode}"); }
            else { callback(request.downloadHandler.text); }
        }
    }

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        return CallAPI(jsonApi, callback);
    }

    public IEnumerator DownloadImage (Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        yield return request.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(request));
    }
}
