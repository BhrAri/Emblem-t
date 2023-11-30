using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawnig : MonoBehaviour
{//pppp
    public GameObject[] friendPrefabse;
    public GameObject[] enemyPrefabse;
    [SerializeField] private List<GameObject> units;
    private GameObject newUnitEntering;
    public TMP_InputField[] inputs;
    [SerializeField] private Button[] buttons;
    public TextMeshProUGUI statPointsToWorkWith;
    public TextMeshProUGUI numberOfFriends;
    private int intNumberOfFriends;
    public TextMeshProUGUI numberOfFoes;
    public int intNumberOfFoes;
    [SerializeField] private Toggle friend;
    readonly int unitStatTotal = 28;
    public int statPointsLeft = 28;
    [SerializeField] private int[] statsTotalArray;
    private int[] baseStatsArray;
    public int statTotalArraySum;
    public int statGovernanceIndex;
    public GameObject everythingTurnOff;
    public GameObject everythingStillTurnOff;
    public List<Vector3> friendPositions;
    public List<Vector3> enemyPositions;
    private void Start()
    {

        newUnitEntering = GameObject.Find("Character Creator");
        inputs = newUnitEntering.GetComponentsInChildren<TMP_InputField>(true);
        buttons = newUnitEntering.GetComponentsInChildren<Button>(true);
        friend = newUnitEntering.GetComponentInChildren<Toggle>(true);

        //listeners (:
        int a = 1;
        foreach (TMP_InputField field in inputs)
        {
            field.GetComponent<StatGovernanceIndex>().a = a;
            a++;
        }
        buttons[0].onClick.AddListener(delegate { RandomStatSpread(); });
        buttons[1].onClick.AddListener(delegate { RandomName(); });
        buttons[2].onClick.AddListener(delegate { Spawn(); });
        statsTotalArray = new int[11];
        baseStatsArray = new int[11];
    }
    
    public void StatPointUpdator()
    {
        int a = statGovernanceIndex;
        if (a == 0) {}
        else
        if (a <= 6)
        {
            if (int.TryParse(inputs[a - 1].text, out int storte))
            {
               
                if (storte - 1 <= statPointsLeft && storte != 0)
                {
                    StatPointLowerer(a - 1, storte - 1, storte);
                }
                else { inputs[a-1].text = ""; StatPointLowerer(a-1, 0, 0); }
            }
            else
            { inputs[a - 1].text = ""; StatPointLowerer(a-1, 0, 0); }

        }
        else if (a == 7)
        {
            
            if (int.TryParse(inputs[a - 1].text, out int storte))
            {
                if (storte != 0)
                {
                    
                    int borte = storte;
                    while (storte % 5 != 0)
                    {
          
                        storte++;
                    }
                   
                    storte /= 5;
                   
                    if (storte <= statPointsLeft)
                    { StatPointLowerer(a - 1, storte, borte); }
                    else { inputs[a - 1].text = ""; StatPointLowerer(a - 1, 0, 0); }
                }
                else { inputs[a - 1].text = ""; StatPointLowerer(a - 1, 0, 0); }
            }
            else
            { inputs[a - 1].text = ""; StatPointLowerer(a - 1, 0, 0); }

        }
        else if (a == 8)
        {
            if (int.TryParse(inputs[a - 1].text, out int storte))
            {
               
                if (storte <= 3 && storte > 0)
                {
                    StatPointLowerer(a - 1, 1, storte);
                }
                else
                {
                    int borte = storte;
                    storte = storte * (storte - 3) - 2;
                    if (storte <= statPointsLeft && storte != 0)
                    { StatPointLowerer(a - 1, storte, borte); }
                    else { inputs[a - 1].text = ""; StatPointLowerer(a - 1, 0, 0); }
                }

            }
            else
            { inputs[a - 1].text = ""; StatPointLowerer(a - 1, 0, 0); }


        }
        else if (a == 9)
        {
            if (int.TryParse(inputs[a - 1].text, out int storte))
            {
                
                if (storte == 1) { StatPointLowerer(a - 1, 0, 0); }
                else if (storte == 2 && statPointsLeft > 2)
                {
                    StatPointLowerer(a - 1, 3, storte);
                } else
                {
                    inputs[a - 1].text = ""; StatPointLowerer(a - 1, 0, 0); 
                }
            }
            else
            {
                inputs[a - 1].text = ""; StatPointLowerer(a - 1, 0, 0);
            }

        }
        else if (a == 10)
        {
            if (int.TryParse(inputs[a - 1].text, out int storte))
            {
               
                if (storte != 1 && storte != 2)
                {
                    inputs[a - 1].text = "";
                    StatPointLowerer(a, 0, storte);
                }
            }
            else
            {
                inputs[a - 1].text = "";
                StatPointLowerer(a, 0, storte);
            }
        }
        else { Debug.Log("Something went Wrong"); }
    }
    public void RandomStatSpread()
    {
        int a = 0;
        foreach (var item in inputs)
        {
            if (a == 0 || a == 2)
            {
                item.text = "4";
                StatPointLowerer(a, 4, 4);
            }
            else if (a == 6)
            {
                item.text = "20";
                StatPointLowerer(a, 4, 20);
            }
            else if (a < 8)
            { 
                item.text = "3";
                StatPointLowerer(a, 2, 3);
            }
            else
            {
                item.text = "2";
                StatPointLowerer(a, 2, 2);
            }
             a++;
            
        }
    }
    public void RandomName()
    {
        inputs[10].text = "Bolognald";
    }
    private void StatPointLowerer(int placeInArray, int amountToBeLoweredBy, int baseStat)
    {
        statPointsLeft = unitStatTotal;
        statTotalArraySum = 0;
        
        statsTotalArray[placeInArray] = amountToBeLoweredBy;
        baseStatsArray[placeInArray] = baseStat;
        for (int i = 0; i < statsTotalArray.Length; i++)
        {
            statTotalArraySum += statsTotalArray[i];
        }
        statPointsLeft = unitStatTotal;
        statPointsLeft -= statTotalArraySum;
        statPointsToWorkWith.text = "Stat Points To Work With: " + statPointsLeft;
       
    }
    public void Spawn()
    {
        statPointsLeft = unitStatTotal;
        statPointsToWorkWith.text = "Stat Points To Work With: " + statPointsLeft;
       
        if (inputs[9].text == "1" && friend.isOn == true)
        {
            if (baseStatsArray[0] == 0) { friendPrefabse[0].GetComponent<Unit>().strength = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().strength = baseStatsArray[0]; }
            if (baseStatsArray[1] == 0) { friendPrefabse[0].GetComponent<Unit>().defense = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().defense = baseStatsArray[1]; }
            if (baseStatsArray[2] == 0) { friendPrefabse[0].GetComponent<Unit>().magic = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().magic = baseStatsArray[2]; }
            if (baseStatsArray[3] == 0) { friendPrefabse[0].GetComponent<Unit>().resistance = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().resistance = baseStatsArray[3]; }
            if (baseStatsArray[4] == 0) { friendPrefabse[0].GetComponent<Unit>().speed = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().speed = baseStatsArray[4]; }
            if (baseStatsArray[5] == 0) { friendPrefabse[0].GetComponent<Unit>().dexterity = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().dexterity = baseStatsArray[5]; }
            if (baseStatsArray[6] == 0) { friendPrefabse[0].GetComponent<Unit>().Hp = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().Hp = baseStatsArray[6]; }
            if (baseStatsArray[7] == 0) { friendPrefabse[0].GetComponent<Unit>().movement = 3; }
            else { friendPrefabse[0].GetComponent<Unit>().movement = baseStatsArray[7]; }
            if (baseStatsArray[8] == 0) { friendPrefabse[0].GetComponent<Unit>().reach = 1; }
            else { friendPrefabse[0].GetComponent<Unit>().reach = baseStatsArray[8]; }
            if (inputs[10].text == null) { }
            else { friendPrefabse[0].GetComponent<Unit>().name = inputs[10].text; }
            
            GameObject newUnit = Instantiate(friendPrefabse[0], SetNewUnitPositions(friend.isOn), friendPrefabse[0].transform.rotation);
            units.Add(newUnit);
            newUnit.SetActive(false);
            SetAllTheThingsTo0WhenSpawnIsPressed();
            intNumberOfFriends++;
            numberOfFriends.text = "Number Of Friends: " + intNumberOfFriends;
            
        }
        else if (inputs[9].text == "2" && friend.isOn == true)
        {

            if (baseStatsArray[0] == 0) { friendPrefabse[1].GetComponent<Unit>().strength = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().strength = baseStatsArray[0]; }
            if (baseStatsArray[1] == 0) { friendPrefabse[1].GetComponent<Unit>().defense = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().defense = baseStatsArray[1]; }
            if (baseStatsArray[2] == 0) { friendPrefabse[1].GetComponent<Unit>().magic = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().magic = baseStatsArray[2]; }
            if (baseStatsArray[3] == 0) { friendPrefabse[1].GetComponent<Unit>().resistance = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().resistance = baseStatsArray[3]; }
            if (baseStatsArray[4] == 0) { friendPrefabse[1].GetComponent<Unit>().speed = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().speed = baseStatsArray[4]; }
            if (baseStatsArray[5] == 0) { friendPrefabse[1].GetComponent<Unit>().dexterity = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().dexterity = baseStatsArray[5]; }
            if (baseStatsArray[6] == 0) { friendPrefabse[1].GetComponent<Unit>().Hp = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().Hp = baseStatsArray[6]; }
            if (baseStatsArray[7] == 0) { friendPrefabse[1].GetComponent<Unit>().movement = 3; }
            else { friendPrefabse[1].GetComponent<Unit>().movement = baseStatsArray[7]; }
            if (baseStatsArray[8] == 0) { friendPrefabse[1].GetComponent<Unit>().reach = 1; }
            else { friendPrefabse[1].GetComponent<Unit>().reach = baseStatsArray[8]; }
            if (inputs[10].text == null) { }
            else { friendPrefabse[1].GetComponent<Unit>().name = inputs[10].text; }

            GameObject newUnit = Instantiate(friendPrefabse[1], SetNewUnitPositions(friend.isOn), friendPrefabse[1].transform.rotation);
            units.Add(newUnit);
            newUnit.SetActive(false);
            SetAllTheThingsTo0WhenSpawnIsPressed();
            intNumberOfFriends++;
            numberOfFriends.text = "Number Of Friends: " + intNumberOfFriends;
            
        }
        else if (inputs[10].text != "" && friend.isOn != true)
        {
            if (baseStatsArray[0] == 0) { enemyPrefabse[0].GetComponent<Unit>().strength = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().strength = baseStatsArray[0]; }
            if (baseStatsArray[1] == 0) { enemyPrefabse[0].GetComponent<Unit>().defense = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().defense = baseStatsArray[1]; }
            if (baseStatsArray[2] == 0) { enemyPrefabse[0].GetComponent<Unit>().magic = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().magic = baseStatsArray[2]; }
            if (baseStatsArray[3] == 0) { enemyPrefabse[0].GetComponent<Unit>().resistance = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().resistance = baseStatsArray[3]; }
            if (baseStatsArray[4] == 0) { enemyPrefabse[0].GetComponent<Unit>().speed = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().speed = baseStatsArray[4]; }
            if (baseStatsArray[5] == 0) { enemyPrefabse[0].GetComponent<Unit>().dexterity = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().dexterity = baseStatsArray[5]; }
            if (baseStatsArray[6] == 0) { enemyPrefabse[0].GetComponent<Unit>().Hp = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().Hp = baseStatsArray[6]; }
            if (inputs[7].text == null) { enemyPrefabse[0].GetComponent<Unit>().movement = 3; }
            else { enemyPrefabse[0].GetComponent<Unit>().movement = baseStatsArray[7]; }
            if (baseStatsArray[8] == 0) { enemyPrefabse[0].GetComponent<Unit>().reach = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().reach = baseStatsArray[8]; }
            if (baseStatsArray[9] == 0) { enemyPrefabse[0].GetComponent<Unit>().attackType = 1; }
            else { enemyPrefabse[0].GetComponent<Unit>().attackType = baseStatsArray[9]; }
            if (inputs[10].text == null) { }
            else { enemyPrefabse[0].GetComponent<Unit>().name = inputs[10].text; }
            
            GameObject newUnit = Instantiate(enemyPrefabse[0], SetNewUnitPositions(friend.isOn), enemyPrefabse[0].transform.rotation);
            units.Add(newUnit);
            newUnit.SetActive(false);
            SetAllTheThingsTo0WhenSpawnIsPressed();
            intNumberOfFoes++;
            numberOfFoes.text = "Number Of Foes: " + intNumberOfFoes;
        }
        for (int i = 0; i < statsTotalArray.Length; i++)
        {
            statsTotalArray[i] = 0;
        }
    }
    public void SetAllTheThingsTo0WhenSpawnIsPressed()
    {
        foreach (var input in inputs)
        {
            input.text = "";
        }

    }
    public Vector3 SetNewUnitPositions(bool friend)
    {
        if (friend)
        {
            Vector3 spawn;
            bool changed = false;
            //position on this side of the field
            // x + or -  13-13 all around
            // z must be +
            retry:
            spawn = SubFriendSpawnCalculation();
            foreach (var position in friendPositions)
            {
                if (position == spawn)
                {
                    changed = true;
                }

            }
            if (changed == true)
            {
                changed = false;
                goto retry;
            }
            return spawn;
        }
        else
        {
            Vector3 spawn;
            
            bool changed = false;
            //position on that side of the field
            retry:
            spawn = SubEnemySpawnCalculation();
            foreach (var position in enemyPositions)
            {
                if (position == spawn)
                {
                    changed = true;
                }  
            }
            if (changed == true)
            {
                changed = false;
                goto retry;
            }
            return spawn;
        }
    }
    private Vector3 SubFriendSpawnCalculation()
    {
            return new Vector3(Random.Range(-7, 7) + .5f, 0.25f, Random.Range(0, 7) + .5f);
    }
    private Vector3 SubEnemySpawnCalculation()
    {
            return new Vector3(Random.Range(-7, 7) + 0.5f, 1f, Random.Range(-7, 0) + .5f);
    }
    public void Finished()
    {
        everythingTurnOff.SetActive(true);
        everythingStillTurnOff.SetActive(true);
        foreach (GameObject unit in units)
        {
            unit.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
