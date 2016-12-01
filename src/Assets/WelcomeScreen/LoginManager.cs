using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour {

    public string ApiUrl;
    public InputField LoginInput;
    public InputField PasswordInput;
    public Button LoginButton;
    public Text LoginErrorText;
    public InputField SignupInput;
    public Button SignupButton;
    public Text SignupErrorText;

    public void Start()
    {
        LoginButton.onClick.AddListener(_Login);
        SignupButton.onClick.AddListener(_Signup);
        SignupInput.onValueChanged.AddListener(_CheckNameAvailability);
    }

    private void _Login()
    {
        var login = LoginInput.text;
        var password = PasswordInput.text;
        var url = ApiUrl + "/login";

        var credentials = new LoginCredentials { login = login, pwd = password };
        var jsonCredentials = JsonUtility.ToJson(credentials);

        //workaround: create req as PUT then change as POST to avoid Unity url-encoding POST req payload
        UnityWebRequest req = UnityWebRequest.Put(url, jsonCredentials);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Accept", "application/json");

        var bCredentials = Encoding.UTF8.GetBytes(jsonCredentials);
        var uploadHandler = new UploadHandlerRaw(bCredentials);
        uploadHandler.contentType = "application/json";
        req.downloadHandler = new DownloadHandlerBuffer();

        req.Send();

        var result = JsonUtility.FromJson<LoginResult>(req.downloadHandler.text);

        if (result.result.ToUpper() == "OK")
        {
            ApplicationModel.PlayerId = result.playerId;
            SceneManager.LoadScene("mainScene", LoadSceneMode.Single);
        }
        else
        {
            LoginErrorText.text = "Incorrect credentials";
        }
    }

    private void _Signup()
    { }

    private void _CheckNameAvailability(string name)
    { }
}

[Serializable]
public class LoginCredentials
{
    public string login;
    public string pwd;
}

[Serializable]
public class LoginResult
{
    public string result;
    public string playerId;
    public string reason;
}