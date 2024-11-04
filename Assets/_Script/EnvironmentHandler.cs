using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{
    [SerializeField] private Transform environment_Stuff;

    private float aspectRatio, gameObjectHeight, gameObjectWidth;

    private float scaleX = 0, scaleY = 0;
    void Start()
    {
        ArrangeEnvironment();
    }

    private void ArrangeEnvironment()
    {
        aspectRatio = (float)Screen.width / Screen.height;

        gameObjectHeight = Camera.main.orthographicSize * 2;
        gameObjectWidth = gameObjectHeight * aspectRatio;

        scaleX = gameObjectWidth / environment_Stuff.GetComponent<SpriteRenderer>().bounds.size.x;
        scaleY = gameObjectHeight / environment_Stuff.GetComponent<SpriteRenderer>().bounds.size.y;

        transform.localScale = new Vector3(scaleX , scaleY, 1);
    }
}
       

   

