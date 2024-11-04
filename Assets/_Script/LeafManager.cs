using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LeafManager : MonoBehaviour {



    public static LeafManager instance;

    [SerializeField] private LeafMotion pf_Leaf;   // Prefab Leaf
    //[SerializeField] private LotusLeaf[] pf_LotusLeaf;   // Prefab LotusLeaf
    [SerializeField] private RuntimeAnimatorController[] all_Animators;   // ANIMATOR LEAF
    [SerializeField] private Sprite[] all_Sprite;   // ANIMATOR LEAF
    [SerializeField] private float flt_CurrentTime;   //Current Time 
    [SerializeField] private float flt_minSpawnTime;   // minimum Spawn Time To Spwn Leaf
    [SerializeField] private float flt_maxSpawnTime;   // Maximum Spawn Time To Spwn Leaf
    private float flt_SpawnTime;      // Current Time To Spawn Leaf
    //private float flt_SpawnTime;      // Current Time To Spawn Leaf


    [Header("Speed Handler")]
    [SerializeField] private float flt_MinSpeed;
    [SerializeField] private float flt_DiffBetweenMinspeedAndMaxSpeed;
    [SerializeField] private float flt_IncresedSpeedAmountOfLevelUpgradeTime;
    [SerializeField] private float flt_MaxspeedAmmountofLevelUpgradeTime;



    [Header("Spawn Percenatge  Data")]
    [SerializeField]private float flt_SpawnPercentage; // Prob To Spawn EnemyFrog
    [SerializeField] private float flt_IncreasedAmmountofPercentage;
    [SerializeField]private float flt_maxSpwnPercentage;


    private bool isStartGame;
    private bool isEnemySpawn;
    private float flt_TimeToSpawnEnemy = 20;

    private float flt_CurrentTimeToSpawn = 0;
    private bool isCalculating = true;

    private void Awake() {
        instance = this; 
    }


    private void TimeHandlerForSpawnEnemy ( )
    {
        if( !isCalculating )
            return;

        flt_CurrentTimeToSpawn += Time.deltaTime;
        if( flt_CurrentTimeToSpawn >= flt_TimeToSpawnEnemy )
        {
            isEnemySpawn = true;
            isCalculating = false;
        }
    }

    private void OnDisable() {
        GameManager.instance.isGameStart += SetStatusOfGame;
    }

    private void SetStatusOfGame(bool _GameStatus) {
        isStartGame = _GameStatus;
    }

    private void Start() {

        GameManager.instance.isGameStart += SetStatusOfGame;
        GameManager.instance.updateLevel += LevelUpgrade;
        // Set random Spawn Time
        flt_SpawnTime = Random.Range(flt_minSpawnTime, flt_maxSpawnTime);
        flt_CurrentTime = flt_SpawnTime;
        isEnemySpawn = false;
        //StartCoroutine(Delay_ToSpawnEnemy());
    }

    //private IEnumerator Delay_ToSpawnEnemy() {
    //    yield return new WaitForSeconds(flt_TimeToSpawnEnemy);
    //    isEnemySpawn = true;
    //}

    private void LevelUpgrade() {

        // Percentage Increased
        flt_SpawnPercentage += flt_IncreasedAmmountofPercentage;

        if (flt_SpawnPercentage >= flt_maxSpwnPercentage) {
            flt_SpawnPercentage = flt_maxSpwnPercentage;
        }


        // Speed handler

        flt_MinSpeed += flt_IncresedSpeedAmountOfLevelUpgradeTime;
        if (flt_MinSpeed >= flt_MaxspeedAmmountofLevelUpgradeTime) {
            flt_MinSpeed = flt_MaxspeedAmmountofLevelUpgradeTime;
        }
    }

    private void Update() {

        if (!isStartGame) {
            return;
        }

        TimeHandlerForSpawnEnemy ( );
        TimeHandler ();
      
    }



    /// <summary>
    /// calculate Of Time and Spawn Leaf
    /// </summary>
    private void TimeHandler() {

        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime >= flt_SpawnTime) {

            //SpawnLotusLeaf();
            //SpawnLotusLeaf();
            //SpawnLotusLeaf();
            SpawnLeaf();
            flt_SpawnTime = Random.Range(flt_minSpawnTime, flt_maxSpawnTime);
            flt_CurrentTime = 0;
        }
    }


    /// <summary>
    /// Spawn Leaf 
    /// Set Enemy Frogg For As Per Probabilty
    /// </summary>
    private void SpawnLeaf() {


        int spawnRange = Random.Range ( 1, 4 );

        for( int i = 0; i < spawnRange; i++ )
        {
            int animIndex = Random.Range ( 0, all_Animators.Length );

            LeafMotion current = Instantiate ( pf_Leaf, GetRandomPostion ( ), transform.rotation, transform );
            current.anim.runtimeAnimatorController = all_Animators[animIndex];
            current.changingSprite.sprite = all_Sprite[animIndex];

            int index = Random.Range ( 0, 100 );

            float flt_Speed = Random.Range ( flt_MinSpeed, flt_MinSpeed + flt_DiffBetweenMinspeedAndMaxSpeed );
            if( index <= flt_SpawnPercentage )
            {
                // Spawn Enemy Forgg
                if( isEnemySpawn )
                {
                    Debug.Log ( "SpawnEnemy" );
                    current.SetLeafData ( true, flt_Speed );
                }
                else
                {
                    current.SetLeafData ( false, flt_Speed );
                }


            }
            else
            {

                current.SetLeafData ( false, flt_Speed );
            }
        }

        // Spawn Leaf
       

        // Caluclate Probability
       

    }


    /// <summary>
    /// get Random Spawn Postion
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPostion() {

        float flt_CameraHeight = Camera.main.orthographicSize * 2;
        float flt_aspectRatio = ( float )Screen.width / Screen.height;
        float flt_CameraWidth = flt_aspectRatio * flt_CameraHeight;

        Vector3 postion = Vector3.zero;
        bool isSpawn = false;
        while (!isSpawn) {

            postion = new Vector3 ( Random.Range ( -flt_CameraWidth + 8, flt_CameraWidth - 8 ), flt_CameraHeight / 2 + 3, 0 );
            isSpawn = true;

            //Collider2D[] all_Collider = Physics2D.OverlapCircleAll(postion, 1);

            //isSpawn = (all_Collider.Length == 0);
           
        }


     
        return postion ;
    }

    public void DestroyAllEnemy ( )
    {
        foreach( Transform childTransform in transform )
        {
            EnemyFrogg enemy = childTransform.GetComponentInChildren<EnemyFrogg> ( );
            if( enemy != null ) Destroy ( enemy.gameObject );
          

        }
    }
}
