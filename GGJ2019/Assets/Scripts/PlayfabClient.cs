using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ScoreBoardList = System.Collections.Generic.List<GameController.ScoreBoardEntry>;

public class PlayfabClient : MonoBehaviour
{
    private bool isLoggedIn = false;

    public void Start()
    {
        Login();
    }

    /// Requests

    private void Login() {
        var request = new LoginWithCustomIDRequest { CustomId = GameController.UserID, CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnNetworkOperationFailure);
    }

    public void SubmitScore(int scoreValue, UnityAction<bool> callback = null) { 
        var updates = new Dictionary<string, string> {
            {Globals.PLAYFAB_SCORE_KEY, scoreValue.ToString()}
        };
        var request = new UpdateUserDataRequest{ 
            Data = updates,
            Permission = UserDataPermission.Public
        };        
        PlayFabClientAPI.UpdateUserData(request, result => {            
            if (callback != null) {
                callback(true);
            }
        }, result => {
            OnNetworkOperationFailure(result);
            if (callback != null) {
                callback(false);
            }
        });
    }
    
    public void GetScoreLeaderboard(UnityAction<ScoreBoardList> callback) {
        // TODO
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
