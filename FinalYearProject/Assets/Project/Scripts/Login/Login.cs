using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System;

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

    public TextMeshProUGUI debugText;

    public TMP_InputField loginNameInput;
    public TMP_InputField loginPasswordInput;

    public TMP_InputField registerNameInput;
    public TMP_InputField registerPasswordInput;
    public TMP_InputField registerConfirmPasswordInput;

    public GameObject userInfoPrefab;
    public GameObject adminInfoPrefab;
    public Transform contentTransform;
    public TMP_InputField inputField;
    public Button adminButton;

    private void Start()
    {
        list = JsonUtility.FromJson<UserList>(textJSON.text);
        CheckAdmin();
    }

    public void LoginUser()
    {
        if (Verify(loginNameInput.text, loginPasswordInput.text))
        {
            StartCoroutine(DebugText("Login Success", 1.5f, false));
            SceneManager.LoadScene(1);
        }
        else
        {
            StartCoroutine(DebugText("Incorrect Username or Password"));
        }
    }
    
    public void RegisterUser()
    {
        foreach (User user in list.users)
        {
            if (registerNameInput.text.ToLower() == user.username.ToLower())
            {
                StartCoroutine(DebugText("Username has to be unique"));
                return;
            }
        }

        if (registerPasswordInput.text != registerConfirmPasswordInput.text)
        {
            StartCoroutine(DebugText("Passwords do not match"));
            return;
        }

        User newUser = new User();
        newUser.username = registerNameInput.text;
        newUser.password = Encrypt.Encrypts(registerPasswordInput.text);
        newUser.op = false;
        list.users.Add(newUser);

        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(Application.dataPath + "/Project/Resources/LoginCredentials.txt", json);

        StartCoroutine(DebugText("Register Success", 1, false));
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
                    PlayerPrefs.SetInt("IsOperator", Convert.ToInt32(user.op));
                    return true;
                }
            }
        }

        return false;
    }

    IEnumerator DebugText(string text, float time = 2f, bool redColorText = true)
    {
        debugText.text = text;
        debugText.color = redColorText ? Color.red : Color.green;
        Debug.Log(text);
        yield return new WaitForSeconds(time);
        debugText.text = "";
    }

    public void RefreshUserInfo()
    {
        foreach (UserInfo info in FindObjectsOfType<UserInfo>())
            Destroy(info.gameObject);

        foreach (User user in list.users)
        {
            UserInfo info = Instantiate(userInfoPrefab, contentTransform).GetComponent<UserInfo>();
            info.text.text = user.username;
        }
    }

    public void RefreshAdminInfo()
    {
        foreach (AdminInfo info in FindObjectsOfType<AdminInfo>())
            Destroy(info.gameObject);

        foreach (User user in list.users)
        {
            AdminInfo info = Instantiate(adminInfoPrefab, contentTransform).GetComponent<AdminInfo>();
            info.nameText.text = user.username;
        }
    }

    public void DeleteUser(string name)
    {
        User userToDelete = null;
        foreach (User user in list.users)
        {
            if (user.username == name)
            {
                userToDelete = user;
            }
        }
        list.users.Remove(userToDelete);
        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(Application.dataPath + "/Project/Resources/LoginCredentials.txt", json);

        RefreshAdminInfo();
    }

    public void CheckAdmin()
    {
        if (adminButton)
            if (PlayerPrefs.GetInt("IsOperator") == 1)
                adminButton.gameObject.SetActive(true);
    }
}
