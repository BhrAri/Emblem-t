using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Shadow : MonoBehaviour
{
    public GameManager gm;
    public Button wait;
    public Button move;
    public GameObject buttonBlock;
    public Button[] buttons;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        buttonBlock = FindObjectOfType<CuttonBlock>(true).gameObject;
        buttons = buttonBlock.GetComponentsInChildren<Button>(true);
        wait = buttons[0];
        move = buttons[2];
        StartCoroutine(FindNameEventually());

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (move.IsActive())
        {
            move.enabled = false;    
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (buttonBlock.activeInHierarchy) 
        {
            move.enabled = true;
        }
    }
    private IEnumerator FindNameEventually()
    {
        yield return new WaitForEndOfFrame();
        gameObject.name = GetComponentInParent<Transform>().GetComponentInParent<Unit>().gameObject.name + " Shadow";
    }

}
