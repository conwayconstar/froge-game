using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] private AudioSource audioSource_BG;
    [SerializeField] private AudioSource audioSource_BtnClick;
    [SerializeField] private AudioSource audioSource_JumpStart;
    [SerializeField] private AudioSource audioSource_JumpEnd;
    [SerializeField] private AudioSource audioSource_Die;
    [SerializeField] private AudioSource audioSource_EatFly;
    [SerializeField] private AudioSource audioSource_LeafClick;
    [SerializeField] private AudioSource audioSource_LeafDestroy;

 
    private void Awake()
    {
        instance = this;
    }


    public void PlayBtnClick_SFX()
    {
        if (!DataManager.instance.isSound)
        {
            return;
        }

        audioSource_BtnClick.Play();
    }

    public void PlayJumpStart_SFX()
    {
        if (!DataManager.instance.isSound)
        {
            return;
        }

        audioSource_JumpStart.Play();
    }
    public void PlayDie_SFX()
    {
        if (!DataManager.instance.isSound)
        {
            return;
        }

        audioSource_Die.Play();
    }

    public void PlayEatFly_SFX()
    {
        if (!DataManager.instance.isSound)
        {
            return;
        }
        audioSource_EatFly.Play();
    }

    public void PlayLeafClick_SFX()
    {
        if (!DataManager.instance.isSound)
        {
            return;
        }
        audioSource_LeafClick.Play();
    }
    public void PlayLeafDestroy_SFX()
    {
        if (!DataManager.instance.isSound)
        {
            return;
        }
        audioSource_LeafDestroy.Play();
    }

    public void PlayJumpEnd_SFX()
    {
        if (!DataManager.instance.isSound)
        {
            return;
        }
      
        audioSource_JumpEnd.Play();
    }

    public void SetMusicData()
    {
        if (DataManager.instance.isMusic)
        {
            audioSource_BG.Play();
        }
        else
        {
            audioSource_BG.Stop();
        }
    }
}

