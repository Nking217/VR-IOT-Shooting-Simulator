using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private FirebaseFirestore db;

    // Start is called before the first frame update
    /*
    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;s
        //await UpdateUsersCoins(15); //Coins update test
    }
    */

    bool timeIsUp = false;
    float currentTime = 0f;
    float startingTime = 30f;
    public TextMeshPro countdownText;
    public static int totalPoints;
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        currentTime = startingTime;
        totalPoints = 0;
        timeIsUp = false;
    }

    // Update is called once per frame
    async void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.SetText(currentTime.ToString());
        if (currentTime <= 0 && !timeIsUp)
        {
            timeIsUp = true;
            await UpdateUsersCoins(totalPoints);
            SceneManager.LoadScene("Map Select");
        }
    }


    //Firebase functions
    public async Task UpdateUsersCoins(int updateAmount)
    {
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);
        int currentCoins = await GetCoins();

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "coins", currentCoins + updateAmount }
        };

        await docRef.SetAsync(updates, SetOptions.MergeAll);
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
