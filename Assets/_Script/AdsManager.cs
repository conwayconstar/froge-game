using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private BannerView bannerView = null;
    private InterstitialAd interstitialAd = null;
    private RewardedAd rewardedAd = null;

    public string str_BannerID;
    public string str_InterstitialID;
    public string str_RewardID;
    public bool isTestMode;


    public bool shouldBeRewarded = false;
    private void Awake ( )
    {
        if( FindObjectsOfType ( GetType ( ) ).Length > 1 )
        {
            Destroy ( gameObject );
        }

        if( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy ( gameObject );
        }
        DontDestroyOnLoad ( this.gameObject );
    }

    private void Start ( )
    {
        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        MobileAds.Initialize ( initStatus =>
        {

            AdsInitializeComplete ( );
        } );
    }

  
    private void AdsInitializeComplete ( )
    {
        LoadRewardAd ( );
        LoadInterstitialAndBannerAd ( );
    }

    public void LoadInterstitialAndBannerAd ( )
    {
        //LoadAndShowBannerAd();
        LoadInterstitialAd ( );
    }
     
    public void PurchasedNoAds ( )
    {
        if( bannerView != null )
        {
            bannerView.Hide ( );
            bannerView.Destroy ( );
            print ( "hide banner vire" );
        }
        else
        {
            print ( "Cant find banner view" );
        }
    }
    public void DestroyBannerAd()
    {
        bannerView.Destroy ( );
    }

    public void LoadAndShowBannerAd ( )
    {
        // Clean up banner ad before creating a new one.
        if( bannerView != null )
        {
            bannerView.Destroy ( );
            bannerView = null;
        }

        string adUnitId = "";

        if( !isTestMode )
        {
            adUnitId = str_BannerID;
        }
        else
        {
            adUnitId = "ca-app-pub-3940256099942544/6300978111";
        }

        // Create a banner
        AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth ( AdSize.FullWidth );
        bannerView = new BannerView ( adUnitId, adSize, AdPosition.Bottom );

        var adRequest = new AdRequest ( );
        bannerView.LoadAd ( adRequest );
    }

    private void LoadInterstitialAd ( )
    {
        // Clean up the old ad before loading a new one.
        if( interstitialAd != null )
        {
            interstitialAd.Destroy ( );
            interstitialAd = null;
        }

        string adUnitId = "";

        if( !isTestMode )
        {
            adUnitId = str_InterstitialID;
        }
        else
        {
            adUnitId = "ca-app-pub-3940256099942544/1033173712";
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest ( );

        InterstitialAd.Load ( adUnitId, adRequest,
        ( InterstitialAd ad, LoadAdError error ) =>
        {
            // if error is not null, the load request failed.
            if( error != null || ad == null )
            {
                return;
            }


            interstitialAd = ad;
            interstitialAd.OnAdFullScreenContentClosed += ( ) =>
            {
                HandleInterstitialClosed ( );
            };
        } );
    }

    public void ShowInterstitialAd ( )
    {
        if( interstitialAd.CanShowAd ( ) )
        {
            interstitialAd.Show ( );
        }
        else
        {
            LoadInterstitialAd ( );
        }



    }

    private void HandleInterstitialClosed ( )
    {

        LoadInterstitialAd ( );
    }

    private void LoadRewardAd ( )
    {
        // Clean up the old ad before loading a new one.
        if( rewardedAd != null )
        {
            rewardedAd.Destroy ( );
            rewardedAd = null;
        }
        Debug.Log ( "Reward Ad Loaded" );

        string adUnitId = "";

        if( !isTestMode )
        {
            adUnitId = str_RewardID;
        }
        else
        {
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest ( );

        // send the request to load the ad.
        RewardedAd.Load ( adUnitId, adRequest,
            ( RewardedAd ad, LoadAdError error ) =>
            {
                // if error is not null, the load request failed.
                if( error != null || ad == null )
                {
                    return;
                }

                rewardedAd = ad;

            } );
    }


    public void ShowRewardedAd ( )
    {
        shouldBeRewarded = false;
        Debug.Log ( "Show Reward" );
        rewardedAd.Show ( ( Reward reward ) =>
        {
            GiveReward ( );
        } );
    }

 
    public bool IsRewardAdReady ( )
    {
        if( rewardedAd != null && rewardedAd.CanShowAd ( ) )
        {
            return true;
        }

        LoadRewardAd ( );
        return false;
    }

    private void GiveReward ( )
    {
        GameManager.instance.RewiveGame ( );
        LoadRewardAd ( );
    }
}


