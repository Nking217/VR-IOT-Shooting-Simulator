using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using UnityEngine.SocialPlatforms.Impl;
using Firebase;
using Firebase.Auth;
using System;

public class Auth : MonoBehaviour
{
    //Firebase variables
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    // current screen state
    public static bool isLogin = true;

    // screen states
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    // UI Elements
    public TMP_InputField emailInput;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button Connect;

    // Error UI Elements
    public GameObject error_popup;
    public TMP_Text error_text;

    // login error messages
    const String LOGIN_FAILED_ERROR_HEB = "ההתחברות נכשלה";
    const String LOGIN_MISSING_EMAIL_ERROR_HEB = "אימייל חסר";
    const String LOGIN_MISSING_PASSWORD_ERROR_HEB = "סיסמא חסרה";
    const String LOGIN_WRONG_PASSWORD_ERROR_HEB = "סיסמא שגויה";
    const String LOGIN_INVALID_EMAIL_ERROR_HEB = "אימייל לא תקין";
    const String LOGIN_ACCOUNT_NOT_EXISTS_ERROR_HEB = "משתמש לא קיים";

    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect.onClick.AddListener(() =>
        {
            if (isLogin)
                StartCoroutine(Login(usernameInput.text, passwordInput.text));
            else
                StartCoroutine(Register(usernameInput.text, passwordInput.text, emailInput.text));
        });
    }

    private IEnumerator Login(string _email, string _password)
    {

        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        
        // Check if there are any errors
        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            
            string message = LOGIN_FAILED_ERROR_HEB;
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = LOGIN_MISSING_EMAIL_ERROR_HEB;
                    break;
                case AuthError.MissingPassword:
                    message = LOGIN_MISSING_PASSWORD_ERROR_HEB;
                    break;
                case AuthError.WrongPassword:
                    message = LOGIN_WRONG_PASSWORD_ERROR_HEB;
                    break;
                case AuthError.InvalidEmail:
                    message = LOGIN_INVALID_EMAIL_ERROR_HEB;
                    break;
                case AuthError.UserNotFound:
                    message = LOGIN_ACCOUNT_NOT_EXISTS_ERROR_HEB;
                    break;
            }

            error_text.text = message;
            error_popup.SetActive(true);
        }
        else
        {
            User = LoginTask.Result;
            Debug.Log("Account connected: " + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName); //Getting accound username
            SceneManager.LoadScene("Map Select");
        }
    }

    private IEnumerator Register(string _username, string _password, string _email)
    {
        if (_username == "" || _email == "" || _password == "")
        {
            error_text.text = "לפחות אחד מהפרטים חסר";
            error_popup.SetActive(true);
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                Debug.LogWarning(message);
            }
            else
            {
                //User has now been created
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        Debug.LogWarning("error");
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        toLogin();
                    }
                }
            }
        }
    }

    public void closeWindow()
    {
        error_popup.SetActive(false);
    }

    public void toRegister()
    {
        isLogin = false;
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    public void toLogin()
    {
        isLogin = true;
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }

    public void whenToUnlockTheButton()
    {
        if(usernameInput.text.Length > 3 && passwordInput.text.Length > 3)
        {
            Connect.interactable = true;
        }
        if (usernameInput.text.Length < 3 || passwordInput.text.Length < 3)
        {
            Connect.interactable = false;
        }
    }
}