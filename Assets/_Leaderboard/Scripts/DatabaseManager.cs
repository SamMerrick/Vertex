using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    private static string databaseLocation = "scores";

    void OnEnable()
    {
        GameController.GameEnd += CheckIfHiScore;
    }

    void OnDisable()
    {
        GameController.GameEnd -= CheckIfHiScore;
    }

    public void CheckIfHiScore()
    {
        int hiScore = PlayerPrefs.GetInt("hiScore", 0);
        int score = Stats.gameStats["Destroyed"];

        if(hiScore < score)
        {
            PlayerPrefs.SetInt("hiScore", score);
            SaveScoreToDatabase();
        }
    }

    public static void UpdateNameInDatabase(string newName)
    {
        PlayerPrefs.SetString("playerName", newName);
        SaveScoreToDatabase();
    }

    public static void SaveScoreToDatabase()
    {
        string playerName = PlayerPrefs.GetString("playerName", "Anonymous");

        string id = AuthController.UID;
        int score = PlayerPrefs.GetInt("hiScore");

        Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "name", playerName },
                { "score", score },
            };

        DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;

        // Get location / UserID and set values
        db.Child(databaseLocation).Child(id).SetValueAsync(data);
    }

    public static void RemoveScoreFromDatabase()
    {
        string id = AuthController.UID;

        DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;

        db.Child(databaseLocation).Child(id).RemoveValueAsync();
    }
}

