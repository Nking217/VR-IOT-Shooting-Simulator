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

public class GameManager : MonoBehaviour
{
    private FirebaseFirestore db;

    bool timeIsUp = false;
    float currentTime = 0f;
    float startingTime = 30f;

    public TextMeshPro countdownText;

    public static int totalPoints;
    public static int totalHits; //how many times you hit a target
    public static int centerBodyHits; //how many times you hit the body in the center
    public static int wideBodyHits; //how many times you hit the wide body area
    public static int headShotHits; //how many times you hit the headshot area
    public static int bulletsShot; //Saves how many times you shot a bullet
    
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        currentTime = startingTime;
        totalPoints = 0;
        totalHits = 0;
        wideBodyHits = 0;
        headShotHits = 5;
        centerBodyHits = 0;
        bulletsShot = 5;
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
        Dictionary<string, object> currentGameJson = new Dictionary<string, object>
        {
            { "hitAccuracy", calculateHitAccuracy(bulletsShot, totalHits) },
            { "headShotPercentage", calculateHeadShotPercentage(bulletsShot, headShotHits) },
            { "wideBodyShotPercentage", calculateWideBodyHitPercentage(bulletsShot, wideBodyHits) },
            { "centerBodyShotPercentage", calculateCenterBodyHitPercentage(bulletsShot, centerBodyHits) },
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
    public int calculateCenterBodyHitPercentage(int bulletsShot, int centerBodyShots)
    {
        return (centerBodyShots / bulletsShot) * 100;
    }
    public int calculateWideBodyHitPercentage(int bulletsShot, int wideBodyHits)
    {
        return (wideBodyHits / bulletsShot) * 100;
    }
    public int calculateHeadShotPercentage(int bulletsShot, int headShotHits)
    {
        return (headShotHits / bulletsShot) * 100;
    }
    public int calculateHitAccuracy(int bulletsShot, int totalHits)
    {
        return (totalHits / bulletsShot) * 100;
    }
}
