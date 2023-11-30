using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemySelectButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    [SerializeField] GameManager gameManager;
    public int enemyIndex;
    public PointerEventData isBe;

    public void OnMouseEnter()
    {
        gameManager.enemyIndex = enemyIndex;
    }


}
