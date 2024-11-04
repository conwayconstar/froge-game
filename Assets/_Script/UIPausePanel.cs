using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIPausePanel : MonoBehaviour
{
    [SerializeField] private GameObject music_On, music_Off;
    [SerializeField] private GameObject sound_On, sound_Off;
    [SerializeField] private Button[] all_Buttons;

    [SerializeField] private RectTransform rect_Container;
    [SerializeField] private float flt_AnimationTime = 0.5f;
    private void OnEnable ( )
    {
        rect_Container.transform.localScale = Vector3.zero;
        rect_Container.DOScale ( Vector3.one, flt_AnimationTime ).SetEase ( Ease.OutBack ).OnComplete ( ( ) => { Time.timeScale = 0; } );
        SetMusic ( );
        SetSounds ( );
    }

    private void SetMusic ( )
    {
        if( DataManager.instance.isMusic )
        {
            music_On.SetActive ( true );
            music_Off.SetActive ( false );
        }
        else
        {
            music_On.SetActive ( false );
            music_Off.SetActive ( true );
        }
    }

    private void SetSounds ( )
    {
        if( DataManager.instance.isSound )
        {
            sound_On.SetActive ( true );
            sound_Off.SetActive ( false );
        }
        else
        {
            sound_On.SetActive ( false );
            sound_Off.SetActive ( true );
        }
    }

    private void ButtonsInterection ( bool value )
    {
        for( int i = 0; i < all_Buttons.Length; i++ )
        {
            all_Buttons[i].interactable = value;
        }
    }


    public void OnClikOn_MusicBtn ( )
    {
        AudioManager.instance.PlayBtnClick_SFX ( );
        DataManager.instance.SetMusicData ( !DataManager.instance.isMusic );
        SetMusic ( );
    }

    public void OnClickOn_SoundBtn ( )
    {
        AudioManager.instance.PlayBtnClick_SFX ( );
        DataManager.instance.SetSoundData ( !DataManager.instance.isSound );
        SetSounds ( );
    }



    public void OnClick_Continue()
    {
        AudioManager.instance.PlayBtnClick_SFX ( );
        Time.timeScale= 1.0f;
        ButtonsInterection ( false );

        rect_Container.DOScale ( Vector3.zero, flt_AnimationTime ).SetEase ( Ease.InBack ).OnComplete ( ( ) => {
            this.gameObject.SetActive ( false );
            ButtonsInterection ( true );
            Uimanager.instance.ui_GameScreen.gameObject.SetActive ( true );
        } );
    }
       

    public void OnClick_Quit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene ( 1 );
    }
}
