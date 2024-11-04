using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class EnemyFrogg : MonoBehaviour {

    private float min_Attack = 2;
    private float max_Attack = 5;
    private float currentAttackTime;
    private float flt_CurrentTime = 0;
    public Transform parentLeaf;
    [SerializeField] private Vector3 movePostion;
    private float targetAngle = 0;
    private float currentAngle = 0;
    [SerializeField] private float flt_MotionSpeed;
    [SerializeField] private float flt_MaxDistance;
    [SerializeField] private bool isAttacking;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem dieEffect;

    // player Find Data
    [SerializeField] private float flt_PlayerPrbabilty = 10;// this is attack Prob
    
    [SerializeField] private float flt_ProbabiltyToJumpWater = 10; // this is water jump prob
   
    private bool isGameStart;
 
    private void OnEnable() {

        GameManager.instance.isGameStart += SetStatusOfGame;
    }

    private void OnDisable() {
        GameManager.instance.isGameStart -= SetStatusOfGame;
    }

    private void SetStatusOfGame(bool _GameStatus) {
        isGameStart = _GameStatus;
    }

    private void Start() {
        isAttacking = true;
        isGameStart = true;
        movePostion = parentLeaf.position;
        currentAttackTime = Random.Range(min_Attack, max_Attack);
    }



    private void Update() {
                                   
        if (!isGameStart) {
            return;
        }
        AttackTimeHandler();
       
        if (parentLeaf != null) {
            movePostion = parentLeaf.position;
        }

       

       

       
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, flt_MotionSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, currentAngle);
        transform.position = Vector3.Lerp(transform.position, movePostion, flt_MotionSpeed * Time.deltaTime);
        
       

    }

    private void AttackTimeHandler() {
        if (!isAttacking) {
            return;
        }
        flt_CurrentTime += Time.deltaTime;
        if (flt_CurrentTime >= currentAttackTime) {
            flt_CurrentTime = 0;
          
          
            FindLeafAndMotion();
            currentAttackTime = Random.Range(min_Attack, max_Attack);
        }
    }

    private void FindLeafAndMotion() {

        LeafMotion playerleaf = null;
        List<LeafMotion> list_Leaf = new List<LeafMotion>();
        Collider2D[] all_Collider = Physics2D.OverlapCircleAll(transform.position,flt_MaxDistance);

       

        if (all_Collider != null) {

            for (int i = 0; i < all_Collider.Length; i++) {

                if (all_Collider[i].transform == parentLeaf) {
                    continue;
                }
                if (all_Collider[i].TryGetComponent<LeafMotion>(out LeafMotion leaf)) {

                   
                   PlayerFrogg player = leaf.GetComponentInChildren<PlayerFrogg>();
                    if (player != null) {
                        playerleaf = leaf;
                        list_Leaf.Add ( leaf );
                    }
                    else {
                        list_Leaf.Add(leaf);
                    }
                }
            }
             
            //Debug.Log(list_Leaf.Count + "LitCount Of Leaf");
            if (list_Leaf.Count == 0) {
                DestoyedHandler();
            }
            else {  

                if (playerleaf != null) {

                    int PlayerJump = Random.Range(0, 100);
                    if (PlayerJump <= flt_PlayerPrbabilty) {
                        StartCoroutine(Delay_OfMove(playerleaf.transform));
                    }
                    else {
                        int RandomLeaf = Random.Range(0, list_Leaf.Count);
                        StartCoroutine(Delay_OfMove(list_Leaf[RandomLeaf].transform));
                    }

                }

                else {

                    int RandomLeaf = Random.Range(0, list_Leaf.Count);
                    StartCoroutine(Delay_OfMove(list_Leaf[RandomLeaf].transform));

                }
               
            }

        }
        else {
            DestoyedHandler();
        }
        


    }

    private IEnumerator Delay_OfMove(Transform target) {
        Vector2 direction = (target.position - transform.position).normalized;
        targetAngle = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        yield return new WaitForSeconds(1);
      
        this.flt_CurrentTime = 0;
        animator.SetTrigger("Jump");
        if(target != null)
        {
            movePostion = target.position;
            transform.parent = target;
            parentLeaf = target;
        }

    }

    private void DestoyedHandler() {

        int index = Random.Range(0, 100);
        if (index <= flt_ProbabiltyToJumpWater) {

            Debug.Log("destroy handleR");

            isAttacking = false;
            movePostion = transform.position + transform.right * flt_MaxDistance;
            transform.parent = null;
            parentLeaf = null;
            StartCoroutine(delay_destaroyed());
        }
       
    }

    private IEnumerator delay_destaroyed() {
        dieEffect.Play ( );
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.TryGetComponent<EnemyFrogg>(out EnemyFrogg enemy)) {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        else if (collision.TryGetComponent<PlayerFrogg>(out PlayerFrogg frogg)) {

            if (!frogg.IsbeeActiveted) {
                frogg.DieVfx(transform);
            }

            if(frogg.IsbeeActiveted)
            {
                AudioManager.instance.PlayDie_SFX ( );
                GameManager.instance.totalKill++;
                Destroy (gameObject,0.15f );
            }






            this.transform.position = parentLeaf.position;

           
        }
    }

   
}
