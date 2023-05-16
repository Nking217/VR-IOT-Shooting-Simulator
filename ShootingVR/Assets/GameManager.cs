using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using UnityEngine.SceneManagement;
using Valve.VR;
using System;
using UnityEngine.UI;
using System.Globalization;

public class GameManager : MonoBehaviour
{
    public static float GAME_TIME = 30f;
    private FirebaseFirestore db;

    bool timeIsUp = false;
    float currentTime = 0f;
    float startingTime = GAME_TIME;
    //public int totalHitsnew;

    public TextMeshPro countdownText;
    public TMP_InputField gameTime_input_field;

    public static int totalPoints = 0;
    public static int totalHits = 0; //how many times you hit a target
    public static int centerBodyHits = 0; //how many times you hit the body in the center
    public static int wideBodyHits = 0; //how many times you hit the wide body area
    public static int headShotHits = 0; //how many times you hit the headshot area
    public static int bulletsShot = 0; //Saves how many times you shot a bullet
    
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        currentTime = startingTime;
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
            //await UpdateUsersCoins(totalPoints);
            await setScoreForCurrentGame(bulletsShot, totalHits, headShotHits, wideBodyHits, centerBodyHits); //(int bulletsShot, int totalHits, int headShotHits, int wideBodyHits, int centerBodyHits)
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

    public async Task setScoreForCurrentGame(int bulletsShot, int totalHits, int headShotHits, int wideBodyHits, int centerBodyHits)
    {
        // Get the current user ID
        string displayName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        DocumentReference docRef = db.Collection("users").Document(displayName);

        // Create the "outfit" field with default values if it doesn't exist
        float headShotp = (headShotHits / (float)totalHits)*100;
        float hitAccur = (bulletsShot / (float)totalHits) * 100;
        float wideBody = (wideBodyHits / (float)totalHits) * 100;
        float centerBody = (centerBodyHits / (float)totalHits) * 100;

        Dictionary<string, object> currentGameJson = new Dictionary<string, object>
        {
            { "hitAccuracy", hitAccur },
            { "headShotPercentage", headShotp },
            { "wideBodyShotPercentage", wideBody },
            { "centerBodyShotPercentage", centerBody },
            //{ "gameTime", GAME_TIME },
        };

        var epochStart = System.DateTime.UtcNow;

        // Create the "outfit" field with default values if it doesn't exist
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { epochStart.ToString(), currentGameJson }
        };
        await docRef.SetAsync(data, SetOptions.MergeAll);
        
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
    //Scoring Functions

    //Calculate score percentage
    public float calculateCenterBodyHitPercentage(int bulletsShot, int centerBodyShots)
    {
        return (centerBodyShots / bulletsShot) * 100;
    }
    public float calculateWideBodyHitPercentage(int bulletsShot, int wideBodyHits)
    {
        return (wideBodyHits / bulletsShot) * 100;
    }
    public float calculateHeadShotPercentage(int bulletsShot, int headShotHits)
    {
        return (headShotHits / bulletsShot) * 100;
    }
    public float calculateHitAccuracy(int bulletsShot, int totalHits)
    {
        return (totalHits / bulletsShot) * 100;
    }
    public void updatingGameTime()
    {
        int number;

        // if time is valid, convert game time 
        if (int.TryParse(gameTime_input_field.text, out number))
        {
            Debug.Log("Game time updated to: " + gameTime_input_field.text);
            GAME_TIME = number;
        }
        else
        {
            Debug.Log("Game time you entered is not valid.");
        }
    }

}
