using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class LeafMotion : MonoBehaviour {

   
    [SerializeField] private float flt_Motionspeed;
    [SerializeField] private float flt_RotationSpeed;
    private int CurrentTouch = 0;
    private int maxTouch = 2;
    [SerializeField] EnemyFrogg frogg;
    [SerializeField] private SpriteRenderer sr;
    private bool isGameStatus;
    [SerializeField] private ParticleSystem ps_Leaf;
    [SerializeField] private ParticleSystem spriteDestroy;
    [SerializeField] private Rigidbody2D rb;
    public SpriteRenderer changingSprite;
    public Animator anim;



    private void Start() {

        GameManager.instance.isGameStart += SetStatusOfGame;
    }

   

    private void OnDisable() {

        GameManager.instance.isGameStart -= SetStatusOfGame;
    }

    private void SetStatusOfGame(bool _gameStatus) {
        this.isGameStatus = _gameStatus;
    }



    /// <summary>
    /// Set Enemy As Start Time
    /// </summary>
    /// <param name="isEnemySpawn"></param>
    public void SetLeafData(bool isEnemySpawn , float flt_motionSpeed) {
        CurrentTouch = 0;

        // Set Random Speed
        //Debug.Log ( "LeafSpeed" + flt_motionSpeed );
        flt_Motionspeed = flt_motionSpeed;
        isGameStatus = true;

        if (isEnemySpawn) {

          //spawn Enemy Frogg
          EnemyFrogg currentFrogg  =   Instantiate(frogg, transform.position, transform.rotation, transform);
            currentFrogg.parentLeaf = transform;
        }
        
    }

    private void Update() {
        if (!isGameStatus) {
            return;
        }


        // Set As per Movement
        transform.Translate(Vector3.down * flt_Motionspeed * Time.deltaTime, Space.World);
        // Set As Per Rotation
        //transform.Rotate(Vector3.forward * flt_RotationSpeed * flt_RotationSpeed*Time.deltaTime,Space.World);
    }


    /// <summary>
    ///  Handling Count
    ///  If Count >= max Count So Destroyed Leaf
    /// </summary>
    public void TouchHandler() {

        CurrentTouch++;
        if (CurrentTouch >= maxTouch) {

            spriteDestroy.Play ( );
            AudioManager.instance.PlayLeafDestroy_SFX( );
            Destroy ( gameObject, 0.2f );

        }
        else {
          
            changingSprite.gameObject.SetActive(true);
        }
    }


    public void Play_LeafParticle() {
        ps_Leaf.Play();
        anim.SetBool ( "IsJump", true );
        Invoke ( nameof ( ResetLEafAnimation ), 1f );
    }
    private void ResetLEafAnimation()
    {
        anim.SetBool ( "IsJump", false );
    }


    private void OnCollisionEnter2D ( Collision2D collision )
    {

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

}

