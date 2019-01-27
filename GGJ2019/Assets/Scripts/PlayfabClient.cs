using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ScoreBoardList = System.Collections.Generic.List<GameController.ScoreBoardEntry>;

using System;

public class PlayfabClient : MonoBehaviour
{
    private bool isLoggedIn = false;

    public void Start()
    {
        Login();
    }

    /// Requests

    private void Login() {
        string id = System.Guid.NewGuid().ToString();
        print(id);
        var request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnNetworkOperationFailure);
    }

    public void UpdateDisplayName(string displayName, UnityAction<bool> callback = null) {
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = displayName
        };        
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, response => {
            if (callback != null) {
                callback(true);
            }
        }, failure => {
            OnNetworkOperationFailure(failure);
            if (callback != null) {
                callback(false);
            }
        });
    }

    public void SubmitScore(int scoreValue, UnityAction<bool> callback = null) { 
        var update = new StatisticUpdate {
            StatisticName = Globals.PLAYFAB_SCORE_KEY,
            Value = scoreValue
        };
        var list = new List<StatisticUpdate>();
        list.Add(update);
        var req = new UpdatePlayerStatisticsRequest();
        req.Statistics = list;        
        PlayFabClientAPI.UpdatePlayerStatistics(req, result => {            
            if (callback != null) {
                callback(true);
            }
        }, failure => {
            OnNetworkOperationFailure(failure);
            if (callback != null) {
                callback(false);
            }
        });
    }
    
    public void GetScoreLeaderboard(UnityAction<ScoreBoardList> callback) {
        var list = new ScoreBoardList();
        var request = new GetLeaderboardRequest
        {
            MaxResultsCount = 10,
            StatisticName = Globals.PLAYFAB_SCORE_KEY
        };
        PlayFabClientAPI.GetLeaderboard(request, result => {
            if (callback != null) {
                foreach (var item in result.Leaderboard) {
                    list.Add(new GameController.ScoreBoardEntry(
                        item.DisplayName, item.StatValue
                    ));
                }
                callback(list);
            }            
        }, error => {
            OnNetworkOperationFailure(error);
            if (callback != null) {                
                callback(list);
            }
        });
    }

    /// Success Responses

    private void OnLoginSuccess(LoginResult result) {
        isLoggedIn = true;
    }

    /// Debug Failure Responses

    private void OnNetworkOperationFailure(PlayFabError error) {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
        Debug.LogError("Error Details: " + error.Error.ToString());
    }

}
