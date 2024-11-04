using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMotion : MonoBehaviour {

    [SerializeField] private float flt_MotionSpeed;
    private SpriteRenderer sr;
    private bool isGameStart = false;



  


    private void OnDisable() {
        GameManager.instance.isGameStart -= SetStatusofGame;
    }

    private void SetStatusofGame(bool _GameStatus) {
        isGameStart = _GameStatus;
    }

    private void Start() {
        
        GameManager.instance.isGameStart += SetStatusofGame;
        sr = GetComponent<SpriteRenderer>();

        //float flt_CameraHeight = Camera.main.orthographicSize * 2;
        //float aspectRatio = (float)Screen.width / Screen.height;
        //float flt_CameraWidth = aspectRatio * flt_CameraHeight;

        //transform.localScale = new Vector3( flt_CameraHeight / sr.bounds.size.y, flt_CameraWidth / sr.bounds.size.x, 1);


    }



    private void Update() {

        if (!isGameStart) {
            return;
        }

      

        sr.material.mainTextureOffset += Vector2.down * flt_MotionSpeed * Time.deltaTime;
    }
}
