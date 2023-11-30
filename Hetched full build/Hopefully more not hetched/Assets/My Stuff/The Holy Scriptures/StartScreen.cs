using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject[] gameObjects;
    public void DefualtGame()
    {
        gameObjects[0].SetActive(false);
        gameObjects[1].SetActive(true);
        gameObjects[2].SetActive(true);
        gameObject.SetActive(false);
    }
    public void CustomGame()
    {
        gameObjects[0].SetActive(true);
        gameObjects[1].SetActive(false);
        gameObjects[2].SetActive(false);
        gameObjects[3].SetActive(false);
        gameObject.SetActive(false);
    }
}
