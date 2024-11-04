using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusLeaf : MonoBehaviour
{
    [SerializeField] private float flt_Motionspeed;

    private bool isGameStatus;
    private void Start ( )
    {
        GameManager.instance.isGameStart += SetStatusOfGame;
    }

    private void OnDisable ( )
    {
        GameManager.instance.isGameStart -= SetStatusOfGame;
    }


    private void SetStatusOfGame ( bool _gameStatus )
    {
        this.isGameStatus = _gameStatus;
    }
    private void Update ( )
    {
        if( !isGameStatus )
        {
            return;
        }

        transform.Translate ( Vector3.down * flt_Motionspeed * Time.deltaTime, Space.World );

    }
    public void SetLeafData ( float flt_motionSpeed )
    {
        flt_Motionspeed = flt_motionSpeed;
        isGameStatus = true;
    }
      
}
      
     
