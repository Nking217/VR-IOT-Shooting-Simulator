using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;

public class FirebaseHelper : MonoBehaviour
{
    // Reference to the Firebase Firestore
    private FirebaseFirestore db;

    // Start is called before the first frame update
    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        int currentCoins = await GetCoins();
        Debug.Log("current-coins: " + currentCoins);
        //await UpdateUsersCoins(15); //Coins update test
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
