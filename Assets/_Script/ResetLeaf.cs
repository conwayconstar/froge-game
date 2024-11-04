using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLeaf : MonoBehaviour {


    private void OnTriggerEnter2D ( Collider2D collision )
    {
        if( collision.gameObject.TryGetComponent<LeafMotion> ( out LeafMotion leaf ) )
        {

            PlayerFrogg frogg = leaf.GetComponentInChildren<PlayerFrogg> ( );
            if( frogg != null )
            {
                Debug.Log ( "Game Over" );
                frogg.DeactivateLine ();
                frogg.transform.parent = null;
                frogg.sr.enabled = false;
                
                GameManager.instance.isGameStart?.Invoke ( false );
                //GameManager.instance.isPlayerAlive = false;
                GameManager.instance.GameOver ( );
            }
            Destroy ( leaf.gameObject );
        }

        if( collision.gameObject.CompareTag ( "LotusLeaf" ) )
        {
            Destroy ( collision.gameObject );
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if (collision.gameObject.TryGetComponent<LeafMotion>(out LeafMotion leaf)) {

    //        PlayerFrogg frogg = leaf.GetComponentInChildren<PlayerFrogg>();
    //        if (frogg != null) {
    //            Debug.Log("Game Over");
    //            GameManager.instance.GameOver();
    //        }
    //        Destroy(leaf.gameObject);
    //    }
    //}



}
