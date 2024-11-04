using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIPanelHomeScreen : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_HighScore;
    [SerializeField] private RectTransform[] all_Buttons;

    [SerializeField] private float flt_AnimationTime = 0.5f;


    private void OnEnable ( )
    {
        EnableAnimation ( );


       

        txt_HighScore.text = "Best : " + DataManager.instance.bestScore;
    }

    private void EnableAnimation()
    {
        for( int i = 0; i < all_Buttons.Length; i++ )
        {
            all_Buttons[i].transform.localScale = Vector3.zero;
        }

        for( int i = 0; i < all_Buttons.Length; i++ )
        {
            all_Buttons[i].DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack );
        }
    }

  

    public void OnClick_PlayBtn() {

        AudioManager.instance.PlayBtnClick_SFX ( );
        this.gameObject.SetActive(false);
        GameManager.instance.startGame();
    }
    public void OnClick_Settings()
    {
        AudioManager.instance.PlayBtnClick_SFX ( );
        Uimanager.instance.ui_SettingPanel.gameObject.SetActive( true );

    }
       
    public void OnClick_NoAds()
    {
        AudioManager.instance.PlayBtnClick_SFX ( );
        AdsManager.Instance.DestroyBannerAd ( );
        DataManager.instance.NoAdPurchase ( );
    }
    
}

