using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class Login : MonoBehaviour
{
    [System.Serializable]
    public class User
    {
        public string username;
        public string password;
        public bool op;
    }
    [System.Serializable]
    public class UserList
    {
        public List<User> users = new List<User>();
    }
    public TextAsset textJSON;
    public UserList list = new UserList();


    public TMP_InputField loginNameInput;
    public TMP_InputField loginPasswordInput;

    public TMP_InputField registerNameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField registerConfirmPasswordInput;
    public Toggle toggleOp;

    private void Start()
    {
        list = JsonUtility.FromJson<UserList>(textJSON.text);
    }

    public void LoginUser()
    {
        if (Verify(loginNameInput.text, loginPasswordInput.text))
        {
            Debug.Log("Login Success");
            SceneManager.LoadScene(1);
        }
    }
    
    public void RegisterUser()
    {
        foreach (User user in list.users)
        {
            if (registerNameInput.text == user.username)
            {
                Debug.Log("Username has to be unique");
            }
        }

        if (registerPasswordInput.text != registerConfirmPasswordInput.text)
            Debug.Log("Passwords do not match");

        User newUser = new User();
        newUser.username = registerNameInput.text;
        newUser.password = Encrypt.Encrypts(registerPasswordInput.text);
        newUser.op = toggleOp.isOn;
        list.users.Add(newUser);

        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(Application.dataPath + "/Project/Resources/LoginCredentials.txt", json);
    }

    public bool Verify(string username, string password)
    {
        foreach (User user in list.users)
        {
            if (username == user.username)
            {
                if (password == Encrypt.Decrypt(user.password))
                {
                    PlayerPrefs.SetString("Username", username);
                    return true;
                }
            }
        }

        return false;
    }
}
