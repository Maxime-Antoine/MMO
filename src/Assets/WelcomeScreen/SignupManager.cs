using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public string ApiUrl;
    public Text LoginErrorText;
    public InputField LoginInput;
    public InputField EmailInput;
    public InputField PasswordInput;
    public InputField Password2Input;
    public Text PasswordErrorText;
    public Button SignupButton;

    // Use this for initialization
    private void Start()
    {
        SignupButton.onClick.AddListener(_Signup);
        LoginInput.onEndEdit.AddListener(_CheckNameAvailability);
        Password2Input.onValueChanged.AddListener(_CheckPasswordsMatch);
    }

    private void _Signup()
    {
        _CheckNameAvailability(LoginInput.text); //in case this is the last field updated by user and focus is not lost

        if (PasswordInput.text != null
         && LoginInput.text != null
         && EmailInput.text != null
         && LoginErrorText.text == string.Empty 
         && PasswordErrorText.text == string.Empty)
        {
            var url = ApiUrl + "/signup";
            var signupDetails = new SignupDetails
            {
                login = LoginInput.text,
                pwd = PasswordInput.text,
                email = EmailInput.text
            };

            var jsonSignupDetails = JsonUtility.ToJson(signupDetails);

            var res = RestClient.Post(url, jsonSignupDetails);

            var result = JsonUtility.FromJson<SignupResult>(res);

            if (result != null)
            {
                ApplicationModel.PlayerName = result.login;
                ApplicationModel.PlayerId = result.playerId;
                SceneManager.LoadScene("mainScene", LoadSceneMode.Single);
            }
        }
    }

    private void _CheckNameAvailability(string name)
    {
        var jsonUsedUsername = RestClient.Get(ApiUrl + "/username");
        var usedUsernames = JsonUtility.FromJson<UsedUsernames>(jsonUsedUsername);

        if (usedUsernames != null && usedUsernames.names.Contains(name))
            LoginErrorText.text = "Username not available";
        else
            LoginErrorText.text = string.Empty;
    }

    private void _CheckPasswordsMatch(string pwd2)
    {
        var pwd1 = PasswordInput.text;

        if (pwd1 == pwd2)
            PasswordErrorText.text = string.Empty;
        else
            PasswordErrorText.text = "The passwords don't match";
    }
}

[Serializable]
internal class SignupDetails
{
    public string login;
    public string email;
    public string pwd;
}

[Serializable]
internal class SignupResult
{
    public string login;
    public string playerId;
}

[Serializable]
internal class UsedUsernames
{
    public string[] names;
}