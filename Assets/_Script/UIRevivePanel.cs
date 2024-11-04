using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIRevivePanel : MonoBehaviour
{
    [SerializeField] private Image img_Ring;
    [SerializeField] private TextMeshProUGUI txt_Timer;
    [SerializeField] private RectTransform[] all_Rect;

    [SerializeField] private float flt_AnimationTime = 0.5f;
   
    private float flt_RewiveTime = 6;

    private void OnEnable ( )
    {
        EnableAnimation ( );
    }
    private void EnableAnimation()
    {
        for( int i = 0; i < all_Rect.Length; i++ )
        {
            all_Rect[i].transform.localScale = Vector3.zero;

        }

        for( int i = 0; i < all_Rect.Length; i++ )
        {

            all_Rect[i].DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack );
        }
    }

    private void Update ( )
    {
        flt_RewiveTime -= Time.deltaTime;
        img_Ring.fillAmount = flt_RewiveTime / 6;
        txt_Timer.text = Mathf.FloorToInt ( flt_RewiveTime ).ToString ( );
        if( flt_RewiveTime <= 0 )
        {
            this.gameObject.SetActive ( false );
            //GameManager.instance.isPlayerAlive = false;
            GameManager.instance.GameOver ( );

        }
    }
    public void OnClik_OnReWiveBtn ( )
    {
        AudioManager.instance.PlayBtnClick_SFX ( );
        this.gameObject.SetActive ( false );
        AdsManager.Instance.ShowRewardedAd ( );
        //GameManager.instance.RewiveGame ( );
    }
  
}
