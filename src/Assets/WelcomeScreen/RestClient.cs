using System.Text;
using UnityEngine.Networking;

public class RestClient
{
    public static string Get(string url)
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        req.SetRequestHeader("Accept", "application/json");
        req.downloadHandler = new DownloadHandlerBuffer();

        var status = req.Send();

        while (!status.isDone) { }//wait until reply

        return req.downloadHandler.text;
    }

    public static string Post(string url, string jsonPayload)
    {
        //workaround: create req as PUT then change as POST to avoid Unity url-encoding POST req payload
        UnityWebRequest req = UnityWebRequest.Put(url, jsonPayload);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Accept", "application/json");

        var bCredentials = Encoding.UTF8.GetBytes(jsonPayload);
        var uploadHandler = new UploadHandlerRaw(bCredentials);
        uploadHandler.contentType = "application/json";
        req.downloadHandler = new DownloadHandlerBuffer();

        var status = req.Send();

        while (!status.isDone) { }//wait until reply

        return req.downloadHandler.text;
    }
}