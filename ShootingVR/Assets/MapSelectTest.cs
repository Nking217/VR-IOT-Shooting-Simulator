using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MapSelectTest : MonoBehaviour
{
    public FirebaseAuth auth;
    private FirebaseFirestore db;
    public TextMeshProUGUI usernamefiled;
    public TextMeshProUGUI totalcoinsfiled;
    // Start is called before the first frame update

    public static string Reverse(string Input)
    {
        // Converting string to character array
        char[] charArray = Input.ToCharArray();
        // Declaring an empty string
        string reversedString = String.Empty;
        // Iterating the each character from right to left
        for (int i = charArray.Length - 1; i > -1; i--)
        {
            // Append each character to the reversedstring.
            reversedString += charArray[i];
        }
        // Return the reversed string.
        return reversedString;
    }

    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        int currentCoins = await GetCoins();
        totalcoinsfiled.SetText("You have " + currentCoins + " coins");
        usernamefiled.SetText("Welcome " + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
        Debug.Log("current-coins: " + currentCoins);
        Debug.Log("current-user: " + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
        Debug.Log("Game Score: " + GameManager.totalPoints);
        //await UpdateUsersCoins(15); //Coins update test
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onStartPressed()
    {   
        SceneManager.LoadScene("Shooting Range Test");   
    }

    
    public async Task<int> GetCoins()
    {
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.ContainsField("coins"))
        {
            int coins = snapshot.GetValue<int>("coins");
            return coins;
        }
        else
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "coins", 0 }
            };
            await docRef.SetAsync(data, SetOptions.MergeAll);

            snapshot = await docRef.GetSnapshotAsync();
            int coins = snapshot.GetValue<int>("coins");
            return coins;
        }
    }
}
