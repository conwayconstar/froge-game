using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeeManager : MonoBehaviour {

    [SerializeField] private BeeMotion pf_Bee;
    [SerializeField] private int holdingProbabilty = 10;
    [SerializeField] private int spawnProbablity = 10;
    [SerializeField] private float flt_MinHoldingTime;
    [SerializeField] private float flt_maxHoldingTime;

    [SerializeField] private float flt_MinactvatedTime;
    [SerializeField] private float flt_MaxActavtetionTime;
    [SerializeField] private float flt_Beefirerate;
    [SerializeField] private float flt_CurrentTime;

    [SerializeField] private float flt_XPosition;
    [SerializeField] private float flt_MinYPostion;
    [SerializeField] private float flt_maxYPostion;

    private bool isGameStart;

    private void Start() {


        GameManager.instance.isGameStart += SetStausofGame;
    }

   

    private void OnDisable() {
        GameManager.instance.isGameStart -= SetStausofGame;
    }

    private void SetStausofGame(bool _GameStatus) {
        isGameStart = _GameStatus;
    }

    private void Update() {
        if (!isGameStart) {
            return;
        }

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime >= flt_Beefirerate) {
            flt_CurrentTime = 0;
            SpawnBee();
        }

        

       
    }

    private void SpawnBee() {


        int SpawnIndex = Random.Range(0, 100);
        if (SpawnIndex <= spawnProbablity) {
           BeeMotion Current_Bee =  Instantiate(pf_Bee, new Vector3(flt_XPosition, 
                                                Random.Range(flt_MinYPostion, flt_maxYPostion), 0), transform.rotation);

            int HoldIndex = Random.Range(0, 100);
            if (HoldIndex <= holdingProbabilty) {


                Current_Bee.SetBeeData(Random.Range(flt_MinHoldingTime,flt_maxHoldingTime),
                    Random.Range(flt_MinactvatedTime,flt_MaxActavtetionTime)); 
            }
            else {
                Current_Bee.SetBeeData(0, Random.Range(flt_MinactvatedTime, flt_MaxActavtetionTime));
            }
        }
    }
}
