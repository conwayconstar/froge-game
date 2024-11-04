using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    [field : SerializeField]public PlayerFrogg frogg { get; set; }

    [SerializeField] private int Score;
    [SerializeField] private int currentLevelUpdateAmmount;
    [SerializeField] private int increasedScoreAmmount;
    [field: SerializeField] public int totalKill { get; set; }
    public Action<int> UpdateScore { get; set; }
    public Action<bool> isGameStart { get; set; }
    public Action updateLevel { get; set; }

    public bool hasPlayerRiwived;
    private bool isPlayerTakingRewive = false;
    public bool isPlayerAlive = true;
   

    private void Awake() {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start() {
        DataSetUpInGame();
        Uimanager.instance.ui_HomeScreen.gameObject.SetActive(true);

        if( DataManager.instance.canShowAd )
        {
            AdsManager.Instance.LoadAndShowBannerAd ( );
        }
    }



    public void DataSetUpInGame() {
        Uimanager.instance.ui_HomeScreen.gameObject.SetActive(true);
        isGameStart?.Invoke(false);
        UpdateScore?.Invoke(0);
    }


    public void startGame() {


        Uimanager.instance.ui_GameScreen.gameObject.SetActive(true);
        isGameStart?.Invoke(true);
        UpdateScore?.Invoke(Score);
    }

    public void RewiveGame ( )
    {
        LeafManager.instance.DestroyAllEnemy ( );
        frogg.RewiveFrog ( );
        isPlayerAlive = true;
        hasPlayerRiwived = true;
        isGameStart?.Invoke ( true );
    }
 
    public void GameOver() {

        isGameStart?.Invoke ( false );

        if( isPlayerAlive )
        {
            AudioManager.instance.PlayDie_SFX ( );
            isPlayerAlive = false;
        }
           
      
        if( !isPlayerTakingRewive )
        {
            isPlayerTakingRewive = true;
            Uimanager.instance.ui_RewivePanel.gameObject.SetActive ( true );
        }

        else
        {
            
            if( hasPlayerRiwived )
            {
                ActivateGameOver ( );
            }
            else
            {
                ActivateGameOver ( );
            }
        }
    }
 
    private void ActivateGameOver()
    {
        //AudioManager.instance.PlayDie_SFX ( );
        Uimanager.instance.ui_GameScreen.gameObject.SetActive ( false );
        Uimanager.instance.ui_GameOver.gameObject.SetActive ( true );
        Uimanager.instance.ui_GameOver.SetScoreTextValue ( Score );
        //Invoke ( nameof ( ShowAd ), 0.3f );
    }
 
    public void IncresedScore(int _Score) {
        Score += _Score;
        UpdateScore?.Invoke(Score);
       
        DataManager.instance.SetHighScore(Score);
      
        if (Score >= currentLevelUpdateAmmount) {
            updateLevel?.Invoke();
            currentLevelUpdateAmmount += increasedScoreAmmount;
        }


    }

   
}
