using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    [SerializeField] private Slider slider_Loading;
    //[SerializeField] private TextMeshProUGUI txt_Percentage;


    private void Start ( )
    {
        StartCoroutine ( LoadSceneWithLoadingScreen ( ) );
    }

    private IEnumerator LoadSceneWithLoadingScreen ( )
    {
        slider_Loading.value = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync ( 1 );
        operation.allowSceneActivation = false;
        float timer = 0f;
     
        while( !operation.isDone )
        {
            timer += Time.deltaTime;
            slider_Loading.value = timer / 2;

            int percentage = Mathf.RoundToInt ( slider_Loading.value * 100 );
            //txt_Percentage.text = percentage.ToString ( ) + "%";

            if( timer >= 2f )
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

  

    //private IEnumerator delayLoadScene() {
    //    yield return new WaitForSeconds(0);
    //    SceneManager.LoadScene(1);
    //}
}
