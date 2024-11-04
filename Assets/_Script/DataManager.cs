using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [field: SerializeField] public bool isMusic { get; private set; }
    [field: SerializeField] public bool isSound { get; private set; }
    [field: SerializeField] public int bestScore { get; private set; }
    [field: SerializeField] public bool canShowAd { get; private set; }
   



    private void Awake ( )
    {
        instance = this;
    }

    private void Start ( )
    {
        SetData ( );
    }


    private void SetData ( )
    {

        if( PlayerPrefs.HasKey ( DataKeys.key_Music ) )
        {
            LoadDataFromPlayerPrefs ( );
        }
        else
        {
            SaveDataInPlayerPrefs ( );
        }

        AudioManager.instance.SetMusicData ( );
    }

    private void SaveDataInPlayerPrefs ( )
    {
        PlayerPrefs.SetInt ( DataKeys.key_Score, 0 );

        canShowAd = true;
        
       

        if( isMusic )
        {
            PlayerPrefs.SetInt ( DataKeys.key_Music, 1 );
        }
        else
        {
            PlayerPrefs.SetInt ( DataKeys.key_Music, 0 );
        }

        if( isSound )
        {
            PlayerPrefs.SetInt ( DataKeys.key_Sound, 1 );
        }
        else
        {
            PlayerPrefs.SetInt ( DataKeys.key_Sound, 0 );
        }

    }

    private void LoadDataFromPlayerPrefs ( )
    {

        if( PlayerPrefs.GetInt ( DataKeys.SHOW_AD_KEY, 1) == 1 )
        {
            canShowAd = true;
            
        }
        else
        {
            canShowAd = false;
        }


        if( PlayerPrefs.GetInt ( DataKeys.key_Music ) == 0 )
        {
            isMusic = false;
        }
        else
        {
            isMusic = true;
        }

        if( PlayerPrefs.GetInt ( DataKeys.key_Sound ) == 0 )
        {
            isSound = false;
        }
        else
        {
            isSound = true;
        }

        bestScore = PlayerPrefs.GetInt ( DataKeys.key_Score );
    }

    public void NoAdPurchase ( )
    {
        canShowAd = false;
        PlayerPrefs.SetInt ( DataKeys.SHOW_AD_KEY, 0 );
    }

    public void SetHighScore(int score)
    {
        if( score > bestScore )
        {
            bestScore = score;
            PlayerPrefs.SetInt ( DataKeys.key_Score, score );
        }
    }

    public void SetSoundData ( bool value )
    {
        isSound = value;

        if( value )
        {
            PlayerPrefs.SetInt ( DataKeys.key_Sound, 1 );
        }
        else
        {
            PlayerPrefs.SetInt ( DataKeys.key_Sound, 0 );
        }
       
    }

    public void SetMusicData ( bool value )
    {
        isMusic = value;

        if( value )
        {
            PlayerPrefs.SetInt ( DataKeys.key_Music, 1 );
        }
        else
        {
            PlayerPrefs.SetInt ( DataKeys.key_Music, 0 );
        }

        AudioManager.instance.SetMusicData ( );
    }
}
