using UnityEngine;

public class LotusLeafManager : MonoBehaviour
{
    [SerializeField] private LotusLeaf[] all_LotusLeaf;

    [SerializeField] private float flt_CurrentTime;   //Current Time 
    [SerializeField] private float flt_minSpawnTime;   // minimum Spawn Time To Spwn Leaf
    [SerializeField] private float flt_maxSpawnTime;   // Maximum Spawn Time To Spwn Leaf
    private float flt_SpawnTime;      // Current Time To Spawn Leaf

    [SerializeField] private float flt_MinSpeed;
    [SerializeField] private float flt_MaxSpeed;

    private bool isStartGame;

    private void OnDisable ( )
    {
        GameManager.instance.isGameStart += SetStatusOfGame;
    }

    private void SetStatusOfGame ( bool _GameStatus )
    {
        isStartGame = _GameStatus;
    }

    private void Start ( )
    {

        GameManager.instance.isGameStart += SetStatusOfGame;
       
        // Set random Spawn Time
        flt_SpawnTime = Random.Range ( flt_minSpawnTime, flt_maxSpawnTime );
        flt_CurrentTime = flt_SpawnTime;
      
    }
    private void Update ( )
    {
        if( !isStartGame )
        {
            return;
        }

        TimeHandler ( );
    }

    private void TimeHandler ( )
    {
        flt_CurrentTime += Time.deltaTime;
        if( flt_CurrentTime >= flt_SpawnTime )
        {
            SpawnLotusLeaf ( );
            flt_SpawnTime = Random.Range ( flt_minSpawnTime, flt_maxSpawnTime );
            flt_CurrentTime = 0;
        }
    }

    private void SpawnLotusLeaf ( )
    {
        float scaleValue = Random.Range ( 0.2f, 0.4f );
        int spawnIndex = Random.Range ( 0, all_LotusLeaf.Length );
        float flt_Speed = Random.Range ( flt_MinSpeed, flt_MaxSpeed);

        LotusLeaf current = Instantiate ( all_LotusLeaf[spawnIndex], GetRandomPostion ( ), transform.rotation, transform );
        current.SetLeafData ( flt_Speed );
        current.transform.localScale = new Vector3 ( scaleValue, scaleValue, scaleValue );
    }

    private Vector3 GetRandomPostion ( )
    {
        float flt_CameraHeight = Camera.main.orthographicSize * 2;
        float flt_aspectRatio = ( float )Screen.width / Screen.height;
        float flt_CameraWidth = flt_aspectRatio * flt_CameraHeight;

        Vector3 postion = Vector3.zero;
        bool isSpawn = false;
        while( !isSpawn )
        {
            postion = new Vector3 ( Random.Range ( -flt_CameraWidth + 8, flt_CameraWidth - 8 ), flt_CameraHeight / 2 + 5, 0 );
            isSpawn = true;
        }
        return postion;
    }
}


