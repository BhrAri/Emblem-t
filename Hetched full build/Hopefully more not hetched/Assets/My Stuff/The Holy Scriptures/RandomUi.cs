using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomUi : MonoBehaviour
{
    public GameObject enemyTurn;
    public GameObject playerTurn;
    public Slider scrollText;
    public TextMeshProUGUI whatJustHappened;
    public GameObject help;
    public TextMeshProUGUI explain;
    public void EnemyTurn()
    {
        enemyTurn.SetActive(false);
    }
    public void PlayerTurn()
    {
        playerTurn.SetActive(false);
    }
    public void SliderSlides()
    {
        whatJustHappened.pageToDisplay = (int)scrollText.value;
    }
    public void HelpActivate()
    {
        if (!help.activeInHierarchy)
        {
            help.SetActive(true);
            explain.text = "Close Explination";
        }
        else { help.SetActive(false); explain.text = "Explain?"; }
    }
}
