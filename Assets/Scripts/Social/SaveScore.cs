﻿using UnityEngine;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class SaveScore : MonoBehaviour {

    DatabaseReference mDatabase;

    private void OnEnable()
    {
        GameController.GameEnd += EndGame;
    }

    private void OnDisable()
    {
        GameController.GameEnd -= EndGame;
    }

    // Use this for initialization
    void Start ()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://star-defender.firebaseio.com/");

        // Get the root reference location of the database.
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void EndGame()
    {
        Firebase.Auth.FirebaseUser user = FirebaseUser.user;

        if (user == null) {
            Debug.Log("There is no signed in user");
            return;
        }

        writeNewScore(new Score
        {
            userID = user.UserId,
            profilePicture = user.PhotoUrl.ToString(),
            name = user.DisplayName,
            score = Stats.gameStats["Destroyed"],
        });
    }

    private void writeNewScore(Score score)
    {
        string json = JsonUtility.ToJson(score);
        mDatabase.Child("scores").Child(score.userID).SetRawJsonValueAsync(json);

        Debug.Log("Wrote new score" + json);
    }

    public class Score
    {
        public string userID;
        public string profilePicture;
        public int score;
        public string name;
    }

}

