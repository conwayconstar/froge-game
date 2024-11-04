using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;


public class GameOverScreenUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI txt_Score;
    [SerializeField] private TextMeshProUGUI txt_BestScore;
    [SerializeField] private TextMeshProUGUI txt_TotalKill;

    [SerializeField] private RectTransform rect_Container;
    [SerializeField] private RectTransform rect_Score;
    [SerializeField] private RectTransform rect_HighScore;
    [SerializeField] private RectTransform rect_Kill;
    [SerializeField] private RectTransform rect_HomeBtn;

    [SerializeField] private float flt_AnimationTime = 0.5f;


    private void OnEnable ( )
    {
        EnableAnimation ( );

        txt_BestScore.text = DataManager.instance.bestScore.ToString ( );
        txt_TotalKill.text = GameManager.instance.totalKill.ToString ( );
    }
    public void SetScoreTextValue(int score)
    {
        txt_Score.text = score.ToString();
    }
    private void EnableAnimation ( )
    {
        rect_Container.transform.localScale = Vector3.zero;
        rect_Score.transform.localScale = Vector3.zero;
        rect_HighScore.transform.localScale = Vector3.zero;
        rect_Kill.transform.localScale = Vector3.zero;
        rect_HomeBtn.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence ( );

        seq.Append ( rect_Container.DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack ) ).
            Append ( rect_Score.DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack ) ).
            Append ( rect_HighScore.DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack ) ).
            Append ( rect_Kill.DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack ) ).
            Append ( rect_HomeBtn.DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack ) ).
            AppendCallback ( ( ) => { ShowAd ( ); } );

    }

    private void ShowAd ( )
    {
        if( DataManager.instance.canShowAd )
        {
            AdsManager.Instance.ShowInterstitialAd ( );
        }
    }

    public void OnClick_Home() {
        AudioManager.instance.PlayBtnClick_SFX ( );
        SceneManager.LoadScene(1);
    }

}
