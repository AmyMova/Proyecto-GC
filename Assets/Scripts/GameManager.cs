using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState {
        Gameplay,
        Pause,
        Level_up
    }

    public GameState state;
    public GameState previousState;

    public GameObject playerObject;

    public bool choosingUpgrade;

    [Header("Screens")]
    public GameObject levelUpScreen;

    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public TextMeshProUGUI stopwatchDisplay;

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DisableScreen();
    }

    void Update() {
        switch(state) {
            case GameState.Gameplay:
                UpdateStopwatch();
                break;
            case GameState.Level_up:
                if(!choosingUpgrade) {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    public void ChangeState(GameState newState) {
        state = newState;
    }

    void DisableScreen() {
        levelUpScreen.SetActive(false);
    }

    public void Pause() {
        if(state != GameState.Pause) {
            previousState = state;
            ChangeState(GameState.Pause);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame() {
        if(state == GameState.Pause) {
            ChangeState(previousState);
            Time.timeScale = 1f;
        }
    }

    void UpdateStopwatch() {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        if(stopwatchTime >= timeLimit) {
            
        }
    }

    void UpdateStopwatchDisplay() {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp() {
        ChangeState(GameState.Level_up);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp() {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
