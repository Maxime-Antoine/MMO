using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public string ApiUrl;
    public InputField LoginInput;
    public InputField PasswordInput;
    public Button LoginButton;
    public Text LoginErrorText;

    public void Start()
    {
        LoginButton.onClick.AddListener(_Login);
    }

    private void _Login()
    {
        if (LoginInput.text != null && PasswordInput.text != null)
        {
            var login = LoginInput.text;
            var password = PasswordInput.text;
            var url = ApiUrl + "/login";

            var credentials = new LoginCredentials { login = login, pwd = password };
            var jsonCredentials = JsonUtility.ToJson(credentials);

            var res = RestClient.Post(url, jsonCredentials);

            var result = JsonUtility.FromJson<LoginResult>(res);

            if (result != null && result.result.ToUpper() == "OK")
            {
                ApplicationModel.PlayerId = result.playerId;
                ApplicationModel.PlayerName = result.login;
                SceneManager.LoadScene("mainScene", LoadSceneMode.Single);
            }
            else
            {
                LoginErrorText.text = "Incorrect credentials";
            }
        }
    }
}

[Serializable]
internal class LoginCredentials
{
    public string login;
    public string pwd;
}

[Serializable]
internal class LoginResult
{
    public string result;
    public string playerId;
    public string login;
    public string reason;
}