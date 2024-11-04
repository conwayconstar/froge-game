using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundHandler : MonoBehaviour
{
    [SerializeField] private Transform[] all_Grounds;
    [SerializeField] private Transform[] all_Paths;
    [SerializeField] private float flt_Speed;
    [SerializeField] private float flt_GroundDistance;
    //[SerializeField] private float flt_PathSpawnOffset;

    private int lastIndex;
    private bool isGameStart = false;

    private void OnDisable ( )
    {
        GameManager.instance.isGameStart -= SetStatusofGame;
    }

    private void SetStatusofGame ( bool _GameStatus )
    {
        isGameStart = _GameStatus;
    }

   
       
    private void Start ( )
    {
        SpawnPathsForAllGrounds ( );
        GameManager.instance.isGameStart += SetStatusofGame;
        lastIndex = all_Grounds.Length - 1;
    }

    private void SpawnPathsForAllGrounds ( )
    {
        foreach( Transform ground in all_Grounds )
        {
            SpawnPath ( ground );
        }
    }

    private void Update ( )
    {
        if( !isGameStart )
        {
            return;
        }

        MoveGroundEdges ( );
    }


    public void MoveGroundEdges ( )
    {
        transform.Translate ( Vector3.up * Time.deltaTime * flt_Speed);

        for( int i = 0; i < all_Grounds.Length; i++ )
        {
            if( all_Grounds[i].position.y > flt_GroundDistance + 40)
            {
                DestroyOldPath ( all_Grounds[i] ); // Destroy old path
                SpawnPath ( all_Grounds[i] ); // Spawn new path
                all_Grounds[i].transform.position = all_Grounds[lastIndex].transform.position +
                                                          new Vector3 ( 0, flt_GroundDistance - 0.8f, 0 );
                lastIndex = i;
                //SpawnPath ( all_Grounds[i] );
            }
        }
    }
       

    private void DestroyOldPath ( Transform ground )
    {
        foreach( Transform child in ground )
        {
            if( child.CompareTag ( "Path" ) )
            {
                Destroy ( child.gameObject );
                break;
            }
        }
    }
 
    private void SpawnPath ( Transform parent )
    {
        int pathIndex = Random.Range ( 0, all_Paths.Length );
        Vector3 spawnPosition = parent.position;
        Instantiate ( all_Paths[pathIndex], spawnPosition, Quaternion.identity, parent );
    }
}

