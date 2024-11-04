using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeeMotion : MonoBehaviour {

    [field: SerializeField] public float GetBeePowerupTime { get; private set; }
    [SerializeField] private float flt_BeeMotion;
    [SerializeField] private float flt_ChangeRotationTime;
    [SerializeField] private float flt_CurrentRotationTime;
    [SerializeField] private bool isRotationStart;
    [SerializeField] private float flt_CurrentRotationActvationTime;
    [SerializeField] private float flt_ToatalRoationTime;
    [SerializeField] private float flt_HodingTime;
    [SerializeField] private bool isHold;
    [SerializeField] private float flt_CurrnetHoldTime;
    private bool isHoldTake;
    private bool isGameStart;




    private void OnEnable() {

        GameManager.instance.isGameStart += SetStatusOfGame;
    }
    private void OnDisable() {
        GameManager.instance.isGameStart -= SetStatusOfGame;
    }

    private void SetStatusOfGame(bool _Gamestatus) {
        this.isGameStart = _Gamestatus;
    }

    public void SetBeeData(float _hodingTime,float flt_ActivationTime) {

        isGameStart = true;
        flt_ToatalRoationTime = flt_ActivationTime;
        this.flt_HodingTime = _hodingTime;
        isHoldTake = false;
        StartCoroutine(Delay_StartRotation());
    }

    private IEnumerator Delay_StartRotation() {

        yield return new WaitForSeconds(1);
        isRotationStart = true;
    }

    private void Update() {
        if (!isGameStart) {
            StopAllCoroutines();
            return;
           
        }

        if (!isHold) {
            transform.Translate(transform.right * flt_BeeMotion * Time.deltaTime, Space.World);
        }

        HoldingHandler();
       
        RotationHandler();
       
    }

    private void HoldingHandler() {
        if (!isHold) {
            return;
        }
        flt_CurrnetHoldTime += Time.deltaTime;
        if (flt_CurrnetHoldTime >= flt_HodingTime) {
            isHold = false;
        }
    }

    private void RotationHandler() {

        if (!isRotationStart) {
            return;
        }
        else if (isHold) {
            return;
        }
        flt_CurrentRotationTime += Time.deltaTime;
        flt_CurrentRotationActvationTime += Time.deltaTime;
        
        if (flt_CurrentRotationTime >= flt_ChangeRotationTime) {

            SetRandomRotation();
            flt_CurrentRotationTime = 0;
        }
        if (flt_CurrentRotationActvationTime >= flt_ToatalRoationTime/2 && !isHoldTake) {
            isHold = true;
            flt_CurrnetHoldTime = 0;
            isHoldTake = true;
           
        }
        if (flt_CurrentRotationActvationTime >= flt_ToatalRoationTime) {
            flt_CurrentRotationActvationTime = 0;
            isRotationStart = false;
            StartCoroutine(Delay_Destroyed());
        }
    }

    private IEnumerator Delay_Destroyed() {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    private void SetRandomRotation() {

        Vector3 RandomDirection = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0).normalized;

        float flt_targetangle = Mathf.Atan2(RandomDirection.y, RandomDirection.x) * Mathf.Rad2Deg;

        //transform.localEulerAngles = new Vector3(0, 0, flt_targetangle);

        //if( flt_targetangle < 0 )
        //{
        //    transform.localScale = Vector3.one;
        //}
        //else
        //{
        //    transform.localScale = new Vector3 ( -1, 1, 1 );
        //}
    }

    public void DestroyedBee() {
        Invoke ( nameof ( DestroyFly ), 0.2f );
    }
    private void DestroyFly()
    {
        StopAllCoroutines ( );
        Destroy ( gameObject );
    }
}
