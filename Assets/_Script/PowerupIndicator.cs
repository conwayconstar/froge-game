using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerupIndicator : MonoBehaviour
{
    private float startingScaleValue = 0.4f;
    private float scaleValue = 0.5f;

    private void OnEnable ( )
    {
        DOTween.KillAll ( );
        transform.localScale = new Vector3 ( startingScaleValue, startingScaleValue, startingScaleValue );
        transform.DOScale ( new Vector3 ( scaleValue, scaleValue, scaleValue ), 0.8f ).SetLoops ( -1, LoopType.Yoyo );
    }
}
