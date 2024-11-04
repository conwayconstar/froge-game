using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using  UnityEngine.EventSystems;


public class PlayerFrogg : MonoBehaviour {


    [Header("Componenet")]
    [SerializeField] private LineRenderer line_Jump;                   // Componenent Line rendror
    [SerializeField] private GameObject arrow;
    [SerializeField] private LineRenderer line_Eat;
    [SerializeField] private GameObject tounge;
    public SpriteRenderer sr;
    [SerializeField] private Animator animator;
    [field: SerializeField] public Transform parent { get; set; }   // Parent Leaf
    public Transform backLeaf;

    [Header("FroggMotion Handler")]
    
    [SerializeField] private LayerMask layer_Leaf;
    [SerializeField] private LayerMask layer_Player;
    [SerializeField] private LayerMask layerBee;
    [SerializeField] private float flt_maxDistanceJump;     // Max Distnce To Jump In Game
    [SerializeField] private float flt_MinDistanceToJump;    // Min Diatnce To Jump 
    [SerializeField] private float flt_Rotationspeed;     // RotationSpeed 
    [SerializeField] private float flt_MotionSpeed;    // Frogg Motion Speed
    [SerializeField] private bool isfroggJump;    // Status Is Frgg Jump or Not
    [SerializeField] private bool isFroggEat;      // Status Is Frgg eat or Not
    private float flt_EatRange = 4;   // Frogg Eating Range
    private float flt_ToungeRange = 4f;   // Frogg Eating Range
    [SerializeField] private ParticleSystem ps_waterWxpltion;

    [SerializeField] private PowerupIndicator indicator;

    private Vector3 WorldPostion;   // Camera WorldPostion
    private Vector3 direction;   //  Direction 
    
    private float flt_targetAngle;    // Target Rotation angle
    private float flt_CurrentAngle;      // Current Rotation Angle
    private float flt_CurrentDistance;   // Distance of Current postion to Input Postion
    private Vector3 movePostion;      // Target Move Postion



    private float flt_DistanceToBee;
    [field: SerializeField] public bool IsbeeActiveted;   // Status Of Bee Activeted Or  Not

    // Bee data
    private float flt_CurrentBeetime;   // Bee Activeted Power Current Time
    private float flt_BeemaxTime;   // maxTime To Bee Activeted
    private bool isGameStart;        // GameStatus

    [SerializeField] private Vector2 postion;
    [SerializeField] private float flt_Range;


    private void OnEnable() {
        GameManager.instance.isGameStart += ( bool Status ) => { isGameStart = Status; };
    }

  

    private void OnDisable() {
        GameManager.instance.isGameStart -= (bool Status) => { isGameStart = Status; };
    }

   


    private void Update() {

        if (!isGameStart) {
            return;
        }

        InputHandler();
        PlayerMotion();

        if (isFroggEat) {
            // Lerp Rotation As target Angle
            flt_CurrentAngle = Mathf.LerpAngle(flt_CurrentAngle, flt_targetAngle, flt_Rotationspeed * Time.deltaTime);

            transform.eulerAngles = new Vector3(0, 0, flt_CurrentAngle);
            line_Eat.SetPosition(0, transform.position);
            line_Eat.SetPosition(1, transform.position + transform.right * flt_DistanceToBee);
            tounge.transform.position = line_Eat.transform.position + transform.right * flt_DistanceToBee;
            animator.SetTrigger ( "Eat" );
            CheckBeeFindOrNot ();
        }

        BeeTimerHandler();


    }
    public void DeactivateLine()
    {
        line_Eat.gameObject.SetActive( false );
        line_Jump.gameObject.SetActive( false );
        arrow.SetActive( false );
        tounge.SetActive( false );
    }
 
  
    private void BeeTimerHandler() {
        if (!IsbeeActiveted) {
            return;
        }
        flt_CurrentBeetime += Time.deltaTime;
        if (flt_CurrentBeetime >= flt_BeemaxTime) {

            IsbeeActiveted = false;
            indicator.gameObject.SetActive ( false );
            //sr.color = Color.white;

        }
    }

    private void CheckBeeFindOrNot() {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, flt_EatRange, layerBee);
        if (hit.collider != null) {

            if (hit.collider.TryGetComponent<BeeMotion>(out BeeMotion bee)) {

                IsbeeActiveted = true;
                flt_CurrentBeetime = 0;
                flt_BeemaxTime = bee.GetBeePowerupTime;
                indicator.gameObject.SetActive( true );
                //sr.color = Color.blue;
                bee.DestroyedBee();
                AudioManager.instance.PlayEatFly_SFX();
            }
        }
         
    }
    

    /// <summary>
    /// Mouse Button Input handler
    /// down Set Line
    /// Continue find Target As Per Motion
    /// Up Move That Postion And Check Game Over Or Not
    /// </summary>
    private void InputHandler() {

        if (Input.GetMouseButtonDown(0) && !isfroggJump) {


            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero,layer_Player);

            // Check if the collider has a specific tag or perform other checks
            if (hit.collider != null) {
              
                if(IsPointerOverUI ( ) )
                {
                    // if ui Game Object Return
                    return;
                }
                
                
                Player_jumpRange player = hit.collider.GetComponentInChildren<Player_jumpRange>();
               
                if (player) {
                    isfroggJump = true;
                    AudioManager.instance.PlayLeafClick_SFX();
                    JumpMouseDownHandler();
                }
                else {
                    EatHandler();
                }

            }
            else {
                // eat Handler

                if( IsPointerOverUI() )
                {
                    return;
                }
              
                EatHandler();

            }

        }
       
        if (Input.GetMouseButton(0) && isfroggJump) {

          
            GetMouseButtonJump();

        }
        else if (Input.GetMouseButtonUp(0) && isfroggJump) {

            AudioManager.instance.PlayJumpStart_SFX();
            GetMousButtonJUmpUP();
        }
        else if (parent != null) {

            // When Frogg Dinnot get Input As That Time Move as Per parent
            movePostion = parent.transform.position;
        }
    }
    public bool IsPointerOverUI ( )
    {
        // Check for touch input
        if( Input.touchCount > 0 )
        {
            Touch touch = Input.GetTouch ( 0 );
            if( EventSystem.current.IsPointerOverGameObject ( touch.fingerId ) )
                return true;
        }

        // Check for mouse input
        if( EventSystem.current.IsPointerOverGameObject ( ) )
            return true;

        return false;
    }

    //private void EatHandler ( )
    //{
    //    isFroggEat = true;

    //    Find Direction
    //    WorldPostion = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
    //    WorldPostion = new Vector3 ( WorldPostion.x, WorldPostion.y, 0 );
    //    direction = (WorldPostion - transform.position).normalized;


    //    Debug.DrawRay ( transform.position, direction * 5, Color.red, 0.5f );
    //    RaycastHit2D hit = Physics2D.Raycast ( transform.position, direction, 5, layerBee );
    //    if( hit.collider != null )
    //    {
    //        if( hit.collider.TryGetComponent<BeeMotion> ( out BeeMotion bee ) )
    //        {
    //            Debug.Log ( "BeeTouch" );
    //            flt_DistanceToBee = Mathf.Abs ( Vector2.Distance ( transform.position, bee.transform.position ) );
    //            line_Eat.gameObject.SetActive ( true );
    //            tounge.SetActive ( true );
    //            line_Eat.SetPosition ( 0, transform.position );
    //            line_Eat.SetPosition ( 1, transform.position + direction * flt_DistanceToBee );
    //            tounge.transform.position = line_Eat.transform.position + direction * flt_DistanceToBee;
    //           
    //            StartCoroutine ( cour_Line ( ) );
    //        }
    //        else
    //        {
    //            line_Eat.SetPosition ( 0, transform.position );
    //            line_Eat.SetPosition ( 1, Vector3.zero );
    //            StartCoroutine ( LineEatAnim ( ) );

    //            line_Eat.gameObject.SetActive ( true );
    //            tounge.SetActive ( true );
    //            line_Eat.SetPosition ( 0, transform.position );
    //            line_Eat.SetPosition ( 1, transform.position + transform.right * flt_ToungeRange );
    //            animator.SetTrigger ( "Eat" );
    //            tounge.transform.position = line_Eat.transform.position + transform.right * flt_ToungeRange;
    //            flt_DistanceToBee = flt_ToungeRange;
    //            StartCoroutine ( cour_Line ( ) );
    //        }

    //    }
    //    else
    //    {
    //        line_Eat.SetPosition ( 0, transform.position );
    //        line_Eat.SetPosition ( 1, Vector3.zero );
    //        StartCoroutine ( LineEatAnim ( ) );

    //        line_Eat.gameObject.SetActive ( true );
    //        tounge.SetActive ( true );
    //        line_Eat.SetPosition ( 0, transform.position );
    //        line_Eat.SetPosition ( 1, transform.position + transform.right * flt_ToungeRange );
    //        tounge.transform.position = line_Eat.transform.position + transform.right * flt_ToungeRange;
    //        flt_DistanceToBee = flt_ToungeRange;
    //        StartCoroutine ( cour_Line ( ) );
    //    }

    //    Find target angle As Per Direction
    //    flt_targetAngle = Mathf.Atan2 ( direction.y, direction.x ) * Mathf.Rad2Deg;
    //}

    //private IEnumerator LineEatAnim ( )
    //{
    //    float timer = 0f;
    //    float duration = 0.10f;

    //    line_Eat.gameObject.SetActive ( true );
    //    tounge.SetActive ( true );

    //    while( timer < duration )
    //    {
    //        timer += Time.deltaTime;

    //        line_Eat.SetPosition ( 0, transform.position );
    //        line_Eat.SetPosition ( 1, transform.position + transform.right * Mathf.Lerp ( 0, flt_ToungeRange, timer / duration ) );
    //        tounge.transform.position = Vector3.Lerp ( transform.position, line_Eat.transform.position + transform.right * flt_ToungeRange, timer / duration );

    //        flt_DistanceToBee = flt_ToungeRange;
    //        StartCoroutine ( cour_Line ( ) );

    //        yield return null;
    //    }
    //}

    private void EatHandler ( )
    {
        StopAllCoroutines();

        isFroggEat = true;

        // Find Direction 
        WorldPostion = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
        WorldPostion = new Vector3 ( WorldPostion.x, WorldPostion.y, 0 );
        direction = (WorldPostion - transform.position).normalized;

        Debug.DrawRay ( transform.position, direction * 5, Color.red, 0.5f );
        RaycastHit2D hit = Physics2D.Raycast ( transform.position, direction, 5, layerBee );

        if( hit.collider != null && hit.collider.TryGetComponent<BeeMotion> ( out BeeMotion bee ) )
        {
            float distanceToBee = Mathf.Abs ( Vector2.Distance ( transform.position, bee.transform.position ) );
            StartCoroutine ( LineEatAnim ( transform.position, transform.position + direction * distanceToBee ) );
        }
        else
        {
            StartCoroutine ( LineEatAnim ( transform.position, transform.position + direction * flt_ToungeRange ) );
        }
         
        // Find target angle As Per Direction
        flt_targetAngle = Mathf.Atan2 ( direction.y, direction.x ) * Mathf.Rad2Deg;
    }

    private IEnumerator LineEatAnim ( Vector3 startPos, Vector3 endPos )
    {
        //float timer = 0f;
        //float duration = 0.2f;

        //line_Eat.gameObject.SetActive ( true );
        //tounge.SetActive ( true );

        //while( timer < duration )
        //{
        //    timer += Time.deltaTime;

        //    line_Eat.SetPosition ( 0, startPos );
        //    line_Eat.SetPosition ( 1, Vector3.Lerp ( startPos, endPos, timer / duration ) );
        //    tounge.transform.position = Vector3.Lerp ( startPos, endPos, timer / duration );

        //    yield return null;
        //}


        float timer = 0f;
        float duration = 0.12f;

        line_Eat.gameObject.SetActive ( true );
        tounge.SetActive ( true );

      
        while( timer < duration )
        {
            timer += Time.deltaTime;

            line_Eat.SetPosition ( 0, startPos );
            line_Eat.SetPosition ( 1, Vector3.Lerp ( startPos, endPos, timer / duration ) );
            tounge.transform.position = Vector3.Lerp ( startPos, endPos, timer / duration );

            yield return null;
        }

       
        line_Eat.SetPosition ( 0, startPos );
        line_Eat.SetPosition ( 1, endPos );
        tounge.transform.position = endPos;

       
        timer = 0f;
        while( timer < duration )
        {
            timer += Time.deltaTime;

            line_Eat.SetPosition ( 1, Vector3.Lerp ( endPos, transform.position, timer / duration ) );
            tounge.transform.position = Vector3.Lerp ( endPos, transform.position, timer / duration );

            yield return null;
        }

      
        line_Eat.gameObject.SetActive ( false );
        tounge.SetActive ( false );
        isFroggEat = false;
        line_Eat.SetPosition ( 1, Vector3.zero );
        tounge.transform.position = Vector3.zero;

    }

    private IEnumerator cour_Line() {
        yield return new WaitForSeconds ( 0.25f );

        //Vector3 startPos = line_Eat.GetPosition ( 1 );
        //Vector3 endPos = Vector3.zero;

        //float duration = 1f;
        //float timer = 0;

        //while( timer < duration )
        //{
        //    timer += Time.deltaTime;

        //    line_Eat.SetPosition ( 1, Vector3.Lerp ( startPos, endPos, timer / duration ) );

        //    yield return null;
        //}

        //line_Eat.SetPosition ( 1, endPos );

        line_Eat.SetPosition ( 0, Vector3.zero );
        line_Eat.SetPosition ( 1, Vector3.zero );
        line_Eat.gameObject.SetActive ( false );
        tounge.SetActive ( false );
        isFroggEat = false;
    }
      

    private void GetMousButtonJUmpUP() {
        // find ray cast If Any Leaf Or Not 
        if (flt_CurrentDistance <= flt_MinDistanceToJump) {
            line_Jump.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
            return;
        }

        backLeaf = parent;

        RaycastHit2D[] allCollider = Physics2D.RaycastAll(transform.position, direction, flt_CurrentDistance, layer_Leaf);


        // If Leaf Find so set Target
        if (allCollider != null) {
            for (int i = 0; i < allCollider.Length; i++) {

                if (allCollider[i].transform == parent) {


                    transform.parent = null;
                    parent = null;
                    if (allCollider[i].transform.TryGetComponent<LeafMotion>(out LeafMotion My_leaf)) {
                        My_leaf.TouchHandler();
                    }

                    continue;
                }

                if (allCollider[i].collider.TryGetComponent<LeafMotion>(out LeafMotion leaf)) {

                    flt_CurrentDistance = MathF.Abs(Vector3.Distance(transform.position, allCollider[i].collider.transform.position));
                    transform.parent = allCollider[i].transform;
                    parent = allCollider[i].transform;


                }
            }

            // If Nothng Get Leaf So GameOver

            if (parent == null) {
                //Debug.Log("ui_GameOver");
                StartCoroutine(DelayOfGameGameOver());
            }
            else {
                animator.SetTrigger("Jump");
                GameManager.instance.IncresedScore(1);
            }
        }
        else {

            // If Nothng Get Leaf So GameOver
            Debug.Log("Game Over");
            StartCoroutine(DelayOfGameGameOver());
            transform.parent = null;
            parent = null;
        }

        // Set Postion 
        movePostion = transform.position + direction * flt_CurrentDistance;
        line_Jump.gameObject.SetActive(false);
        arrow.gameObject.SetActive(false);
       
    }

    private void GetMouseButtonJump() {

        // Find Direction 
        WorldPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        WorldPostion = new Vector3(WorldPostion.x, WorldPostion.y, 0);
        direction = (WorldPostion - transform.position).normalized;


        // Find target angle As Per Direction
        flt_targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Lerp Rotation As target Angle
        flt_CurrentAngle = Mathf.LerpAngle(flt_CurrentAngle, flt_targetAngle, flt_Rotationspeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, flt_CurrentAngle);

        flt_CurrentDistance = MathF.Abs(Vector3.Distance(transform.position, WorldPostion));
        // If Distance Gretar Than Range So Set Distance  As Per range
        if (flt_CurrentDistance >= flt_maxDistanceJump) {
            flt_CurrentDistance = flt_maxDistanceJump;
        }

        // Set Line Rebdror Postion
        line_Jump.SetPosition(0, transform.position);
        line_Jump.SetPosition(1, transform.position + direction * MathF.Abs(flt_CurrentDistance - 0.5f));
        arrow.transform.position = line_Jump.transform.position + direction * MathF.Abs ( flt_CurrentDistance - 0.05f );
    }

    private void JumpMouseDownHandler() {
        // Line Activation
        line_Jump.gameObject.SetActive(true);
        arrow.gameObject.SetActive(true);
       // RaycastHit2D[] all_Collider = Physics2D.RaycastAll(transform.position, )
    }

    private IEnumerator DelayOfGameGameOver() {

        arrow.gameObject.SetActive ( false );
        line_Eat.gameObject.SetActive(false);
        line_Jump.gameObject.SetActive ( false );
        tounge.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        indicator.gameObject.SetActive ( false );


        yield return new WaitForSeconds(0.75f);

        //GameManager.instance.isPlayerAlive = false;
        GameManager.instance.GameOver();
    }

    /// <summary>
    /// Player Movement 
    /// </summary>
    private void PlayerMotion() {


        if (!line_Jump.gameObject.activeSelf) {

            transform.position = Vector3.Lerp(transform.position, movePostion, flt_MotionSpeed * Time.deltaTime);
            float flt_Disatnce = Vector2.Distance(transform.position, movePostion);
            if (flt_Disatnce <= 0.5f) {

                if (isfroggJump) {
                    AudioManager.instance.PlayJumpEnd_SFX();
                    if (parent != null )
                    {
                        parent.GetComponent<LeafMotion> ( ).Play_LeafParticle ( );

                    }
                    else {
                        GameManager.instance.isGameStart?.Invoke(false);
                        sr.enabled = false;
                        ps_waterWxpltion.Play();
                    }
                   

                }
               
                isfroggJump = false;
            }

        }
        
    }

    public void DieVfx(Transform enemy) {

       
        GameManager.instance.isGameStart?.Invoke(false);
       
        transform.position = parent.position;
        StartCoroutine (enemyDestoyrd( enemy ) );


    }
    private IEnumerator enemyDestoyrd( Transform enemy ) {
       
        line_Jump.gameObject.SetActive(false);  
        line_Eat.gameObject.SetActive(false);
        indicator.gameObject.SetActive ( false );
        arrow.gameObject.SetActive ( false );

        yield return new WaitForSeconds(0.15f);
       
       
        sr.enabled = false;
        ps_waterWxpltion.Play();

        yield return new WaitForSeconds(0.75f);
        //GameManager.instance.isPlayerAlive = false;
        GameManager.instance.GameOver();
       
    }
    public void RewiveFrog()
    {
        isfroggJump = false;
        //foreach (EnemyFrogg enemyFrog in LeafMotion) { }
        //if( backLeaf == null )
        //{
            Collider2D[] all_Collider = Physics2D.OverlapCircleAll (postion, flt_Range );

            for( int i = 0; i < all_Collider.Length; i++ )
            {
                if( all_Collider[i].TryGetComponent<LeafMotion> ( out LeafMotion leaf ) )
                {
                    //EnemyFrogg enemyfrogg =  leaf.GetComponentInChildren<EnemyFrogg> ();
                    //if( enemyfrogg != null )  Destroy ( enemyfrogg ); 

                    transform.position = leaf.transform.position;
                    backLeaf = leaf.transform;
                    break;
                }
            }
        //}

        parent = backLeaf;
        transform.parent = parent;

        transform.position = parent.transform.position; 
        sr.enabled = true;
    }
  
    private void OnDrawGizmos ( )
    {
        Gizmos.DrawWireSphere( postion, flt_Range );
        Gizmos.color = Color.yellow;
    }

}
        
