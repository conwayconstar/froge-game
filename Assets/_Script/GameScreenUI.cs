using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;



public class GameScreenUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Scoretext;
    [SerializeField] private RectTransform[] all_Rect;

    [SerializeField] private float flt_AnimationTime = 0.3f;



    private void OnEnable() {

        for( int i = 0; i < all_Rect.Length; i++ )
        {
            all_Rect[i].DOAnchorPosY ( 0f, flt_AnimationTime ); 
        }


        GameManager.instance.UpdateScore += SetMyScore;
    }

    private void OnDisable() {
        GameManager.instance.UpdateScore -= SetMyScore;
    }

    private void SetMyScore(int score) {
        txt_Scoretext.text = /*"Score :" +*/ score.ToString();
    }
    public void OnClick_Pause()
    {
        AudioManager.instance.PlayBtnClick_SFX ( );
        this.gameObject.SetActive ( false );
        Uimanager.instance.ui_PausePanel.gameObject.SetActive( true );
        //Time.timeScale = 0;
    }
}


