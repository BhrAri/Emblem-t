using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

public class GameManager : MonoBehaviour
{//p
    public Camera mainCamera;
    public GameObject playerOnHigh;
    public Unit playerAttacking;
    public Unit enemyAttacking;
    public List<Unit> nearbyEnemies;
    public float howNearbyIsEnemy;
    public List<Unit> friends;
    public List<Unit> enemies;
    public float distance = 100;
    public GameObject target;
    public int friendsStandingBy;
    public float distanceToTarget;
    public float distanceX;
    public float distanceZ;
    public RaycastHit hitinfo;
    public List<Button> enemySelectButtons;
    public List<TextMeshProUGUI> enemyButtonText;
    public GameObject buttonBlock;
    public bool enemyTurn;
    public int enemyIndex;
    public GameObject aboutToBeHit;
    public List<int> playerAttacks;
    public List<int> enemyAttacks;
    public List<int> death;
    private int damage;
    public Rigidbody[] shadows;
    public GameObject winScreen;
    public GameObject loseScreen;
    private int nameNumber;
    //testing variables
    [SerializeField] string enemyName;
    public GameObject everythingTurnOff;
    public GameObject enemyTurnScreen;
    public GameObject playerTurnScreen;
    public TextMeshProUGUI whatJustHappened;
    private List<string> whatJustHappenedTextList;
    public Slider whatJustHappenedSlider;
    public TextMeshProUGUI unitssYetToMove;
    public int whatIndex = 0;
    private int whereAmIInWhat = 0;

    void Start()
    {
        whatJustHappenedTextList = new List<string>(3);
        StringAdderAndTextListUpdator("The Game Begins");
        playerOnHigh = FindFirstObjectByType<NewBehaviourScript>().gameObject;
        Unit[] units = FindObjectsOfType<Unit>();
        foreach (var item in units)
        {
            nameNumber++;
            //makes sure there are no units with the same name
            
            foreach (var item2 in units)
            {
                
                if (item.gameObject.name == item2.gameObject.name && item.gameObject != item2.gameObject)
                {
                    item.gameObject.name = nameNumber + item.gameObject.name;

                }
            }
            //generate friend and enemy lists
            if (item.friend)
            {
                friends.Add(item);
            }
            else
            {
                enemies.Add(item);
            }

        }
        shadows = FindObjectsByType<Rigidbody>(0);
    }
    public void Update()
    {
        if (!enemyTurn) 
        {

        }
        if (enemies.Count == 0)
        {
            winScreen.SetActive(true);
            everythingTurnOff.SetActive(false);
        }
        if (friends.Count == 0)
        {
            loseScreen.SetActive(true);
            everythingTurnOff.SetActive(false);
        }
        if (friendsStandingBy == friends.Count)
        {
            friendsStandingBy = 0;
            EndTurn();
        }
        unitssYetToMove.text = "Number of friendly units who havent moved this turn: " + (friends.Count - friendsStandingBy);
    }

    public void Wait()
    {
       friendsStandingBy++;
    }
    private void EndTurn()
    {
        enemyTurnScreen.SetActive(true);
        playerOnHigh.GetComponent<NewBehaviourScript>().unitSelected = null;
        playerOnHigh.GetComponent<NewBehaviourScript>().lastUnitSelected = null;

        enemyTurn = true;
        StringAdderAndTextListUpdator("The Enemy Turn Begins");
        foreach (var item in enemies)
        {
            item.gameObject.GetComponent<EnemyPushed>().enabled = false;
            distance = 100;
            foreach (var item2 in friends)
            {
                distanceX = Math.Abs(item2.gameObject.transform.position.x - item.gameObject.transform.position.x);
                distanceZ = Math.Abs(item2.gameObject.transform.position.z - item.gameObject.transform.position.z);
                {
                    if (distanceX + distanceZ < distance)
                    {
                        distance = distanceX + distanceZ;
                        target = item2.gameObject;
                    }
                }
                
            }
            StringAdderAndTextListUpdator("Enemy " + item.name + " set it's sights on " + target.name);
            for (var i = 0; i < item.movement; i++)
            {
                distanceX = target.transform.position.x - item.gameObject.transform.position.x;
                distanceZ = target.transform.position.z - item.gameObject.transform.position.z;
                distanceToTarget = (float)Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

                    if (distanceToTarget > item.reach && Math.Abs(distanceX) > Math.Abs(distanceZ))
                    {
                        if (!Physics.Raycast(item.transform.position, new Vector3(distanceX / Math.Abs(distanceX), 0f, 0f),out hitinfo, 1f))
                        {
                           
                            item.gameObject.transform.position += new Vector3(distanceX / Math.Abs(distanceX), 0f, 0f);
                            
                        } 
                    }
                    else if (distanceToTarget > item.reach)
                    {
                        if (!Physics.Raycast(item.transform.position, new Vector3(0f, 0f, distanceZ / Math.Abs(distanceZ)), out hitinfo, 1f))
                        {
                         
                            item.gameObject.transform.position += new Vector3(0f, 0f, distanceZ / Math.Abs(distanceZ));
                            
                        } 
                    }
                StartCoroutine(TurnWaiter());
            }
                  
            if (   Math.Abs(distanceX) <= item.reach + 0.1f && Math.Abs(distanceZ) == 0 ||
                   Math.Abs(distanceZ) <= item.reach + 0.1f && Math.Abs(distanceX) == 0)   
            {
                AdvantageCalculator(item, target.GetComponent<Unit>());
                item.standby = true; 
            } else if (distanceToTarget < item.reach + 0.1f)
            {
                AdvantageCalculator(item, target.GetComponent<Unit>());
                
                item.standby = true;
            }

            item.gameObject.GetComponent<EnemyPushed>().enabled = false;
        }

        DeathCheck();

        foreach (var item in friends)
        {
            item.standby = false;
            item.moved = false;
        }
        enemyTurn = false;
        foreach (var item in enemies)
        {
            item.standby = false;
        }
        if (!enemyTurnScreen.activeInHierarchy)
        {
            playerTurnScreen.SetActive(true);
        }

    }
    public void PlayerReach1Case(Unit playerAttacking)
    {
        //checks front back left and right for enemies
    
        if (Physics.Raycast(playerAttacking.transform.position, new(0, 0, 1), out hitinfo, playerAttacking.reach))
        {
            
                if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
                {
                    howNearbyIsEnemy = hitinfo.distance;
                    if (howNearbyIsEnemy <= playerAttacking.reach)
                    {
                        nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                    }
                }
        }
            if (Physics.Raycast(playerAttacking.transform.position, new(0, 0, -1), out hitinfo, playerAttacking.reach))
            {
                if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
                {
                    howNearbyIsEnemy = hitinfo.distance;
                    if (howNearbyIsEnemy <= playerAttacking.reach)
                    {
                        nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                    }
                }
            }
            if (Physics.Raycast(playerAttacking.transform.position, new(1, 0, 0), out hitinfo, playerAttacking.reach))
            {
                if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
                {
                    howNearbyIsEnemy = hitinfo.distance;
                    if (howNearbyIsEnemy <= playerAttacking.reach)
                    {
                        nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                    }
                }
            }
            if (Physics.Raycast(playerAttacking.transform.position, new(-1, 0, 0), out hitinfo, playerAttacking.reach))
            {
              
                if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
                {
                    howNearbyIsEnemy = hitinfo.distance;
                    if (howNearbyIsEnemy <= playerAttacking.reach)
                    {
                        nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                    }
                
                }
            }
               

    }
    public void PlayerReach2Case(Unit playerAttacking)
    {
        //checks diagonal directions for enemies

        if (Physics.Raycast(playerAttacking.transform.position, new(1, 0, 1), out hitinfo, playerAttacking.reach))
        {

            if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
            {
                howNearbyIsEnemy = hitinfo.distance;
                if (howNearbyIsEnemy <= 1)
                {
                    nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                }
            }
        }
        if (Physics.Raycast(playerAttacking.transform.position, new(1, 0, -1), out hitinfo, playerAttacking.reach))
        {
            if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
            {
                howNearbyIsEnemy = hitinfo.distance;
                if (howNearbyIsEnemy <= 1)
                {
                    nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                }
            }
        }
        if (Physics.Raycast(playerAttacking.transform.position, new(-1, 0, -1), out hitinfo, playerAttacking.reach))
        {
            if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
            {
                howNearbyIsEnemy = hitinfo.distance;
                if (howNearbyIsEnemy <= 1)
                {
                    nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                }
            }
        }
        if (Physics.Raycast(playerAttacking.transform.position, new(-1, 0, 1), out hitinfo, playerAttacking.reach))
        {
            if (hitinfo.collider.gameObject.GetComponent<Unit>() && hitinfo.collider.gameObject.GetComponent<Unit>().friend != playerAttacking.friend)
            {
                howNearbyIsEnemy = hitinfo.distance;
                if (howNearbyIsEnemy <= 1)
                {
                    nearbyEnemies.Add(hitinfo.collider.gameObject.GetComponent<Unit>());
                }

            }
        }

    }
    public void PlayerAttackButton()
    {
        buttonBlock.SetActive(false);
        playerAttacking = playerOnHigh.GetComponent<NewBehaviourScript>().unitSelected.GetComponent<Unit>();
        
        if (playerAttacking.reach == 1) { PlayerReach1Case(playerAttacking); }
        if (playerAttacking.reach == 2) { PlayerReach1Case(playerAttacking); PlayerReach2Case(playerAttacking); }
        
        PostReach();
        
    }
    private void PostReach()
    {
       
        int a = 0;
        foreach (Unit unit in nearbyEnemies)
        {
            if (!enemySelectButtons[a].IsActive())
            {
                enemySelectButtons[a].transform.position = mainCamera.WorldToScreenPoint(unit.transform.position);
                enemySelectButtons[a].GetComponent<EnemySelectButton>().enemyIndex = a;
                enemyButtonText[a].text = unit.gameObject.name;
                enemySelectButtons[a].gameObject.SetActive(true);

            }
            a++;
        }
        if (nearbyEnemies.Count == 0)
        {
          
            playerAttacking.standby = true;
            friendsStandingBy++;
            playerOnHigh.GetComponent<NewBehaviourScript>().noReselect = false;
        }
        playerAttacking.moved = true;
    }
    public void AdvantageCalculator(Unit enemy, Unit player)
    {
    
        distanceX = Math.Abs(enemy.transform.position.x - player.transform.position.x);
        distanceZ = Math.Abs(enemy.transform.position.z - player.transform.position.z);
        if (distanceX > enemy.reach + 0.1f || (distanceZ > enemy.reach + 0.1f)) { player.advantage = true; }
        else if (distanceX > player.reach + 0.1f || (distanceZ > player.reach + 0.1f)) { enemy.advantage = true; }
        else if (distanceX + distanceZ > enemy.reach + 0.1f) { player.advantage = true; }
        else if (distanceX + distanceZ > player.reach + 0.1f) { enemy.advantage = true; }


        EnemySelectExplosion(enemy,player);
        
        
    }
    public void EnemySelectButton()
    {
        enemyAttacking = nearbyEnemies[enemyIndex];
        if (enemyAttacking != null) 
        {
            AdvantageCalculator(enemyAttacking, playerAttacking);
        }
        playerOnHigh.GetComponent<NewBehaviourScript>().noReselect = false;
    }
    private void EnemySelectExplosion(Unit enemy, Unit player)
    {
        
        foreach (var item in enemySelectButtons)
        {
            item.gameObject.SetActive(false);
        }
        
        nearbyEnemies.Clear();
      
        AttackSpeeds(enemy, player, enemy.advantage, player.advantage);
    }
    public void AttackSpeeds(Unit enemy, Unit player, bool enemyReachAdvantage, bool playerReachAdvantage)
    {
      
        playerAttacking = null;
        enemyAttacking = null;
        if (enemy.speed != 0 && player.speed != 0)
        {
            if (enemy.speed / player.speed >= 6) { enemy.attacks = 3; player.attacks = 1; }
            else if (enemy.speed / player.speed >= 3) { enemy.attacks = 2; player.attacks = 1; }
            else if (enemy.speed / player.speed > 0.3333f) { enemy.attacks = 1; player.attacks = 1; }
            else if (enemy.speed / player.speed > 0.1112f) { player.attacks = 2; enemy.attacks = 1; }
            else if (enemy.speed / player.speed <= 0.1667f) { player.attacks = 3; enemy.attacks = 1; }
            if (enemyReachAdvantage) { player.attacks = 0; }
            if (playerReachAdvantage) { enemy.attacks = 0; }
        }
        else
        {
            enemy.attacks += enemy.speed;
            player.attacks += player.speed;
        }
        
        AttackCalculation(enemy, player);
    }       
       
    private void AttackCalculation(Unit enemy, Unit player)
    {
        enemyAttacks.Clear();
        playerAttacks.Clear();
   
        if (enemy.attackType == 1)
        {
            for (int i = 0; i < enemy.attacks; i++)
            {
                if (UnityEngine.Random.Range(0, 100 + player.dexterity)! <= 100 && player.defense < enemy.strength)
                {
                    damage = enemy.strength - player.defense;
                    enemyAttacks.Add(damage);
                }
            }
        }
        if (player.attackType == 1)
        {
            for (int i = 0; i < player.attacks; i++)
            {
                if (UnityEngine.Random.Range(0, 100 + player.dexterity)! <= 100 && enemy.defense < player.strength)
                {
                    damage = player.strength - enemy.defense;
                   
                    playerAttacks.Add(damage);
                }
            }
        }

        if (enemy.attackType == 2)
        {
            for (int i = 0; i < enemy.attacks; i++)
            {
                if (UnityEngine.Random.Range(0, 100 + player.dexterity)! <= 100 && player.resistance < enemy.magic)
                {
                    damage = enemy.magic - player.resistance;
                   
                    enemyAttacks.Add(damage);
                }
            }
        }

        if (player.attackType == 2)
        {
            for (int i = 0; i < player.attacks; i++)
            {
                if (UnityEngine.Random.Range(0, 100 + player.dexterity)! <= 100 && enemy.resistance < player.magic)
                {
                    damage = player.magic - enemy.resistance;
                   
                    playerAttacks.Add(damage);
                }
            }
        }
        BattleTime(enemy, player);
    }
    void BattleTime(Unit enemy, Unit player)
    {
        player.attacks = 0;
        enemy.attacks = 0;
        if (!enemyTurn) 
        {
            int a = 0;
            foreach (var item in playerAttacks)
            {
                if (playerAttacks.Count - a > 0 && enemy.Hp > 0)
                {
                    enemy.Hp -= playerAttacks[a];
                    StringAdderAndTextListUpdator("Companion " + player.name + " attacks Enemy " + enemy.name + " for " + playerAttacks[a] + " damage!");
                }
                if (enemyAttacks.Count - a > 0 && player.Hp > 0)
                {
                    player.Hp -= enemyAttacks[a];
                    StringAdderAndTextListUpdator("Enemy " + enemy.name + " attacks Companion " + player.name + " for " + enemyAttacks[a] + " damage!");
                }
                a++;
            }
            AttackConclusion(player);
        } 
        else
        {
            int a = 0;
            foreach (var item in enemyAttacks)
            {
                
                if (enemyAttacks.Count - a > 0 && player.Hp > 0)
                {
                    player.Hp -= enemyAttacks[a];
                    StringAdderAndTextListUpdator("Enemy " + enemy.name + " attacks Companion " + player.name + " for " + enemyAttacks[a] + " damage!");

                }

                if (playerAttacks.Count - a > 0 && enemy.Hp > 0)
                {
                    enemy.Hp -= playerAttacks[a];
                    StringAdderAndTextListUpdator("Companion " + player.name + " attacks Enemy " + enemy.name + " for " + playerAttacks[a] + " damage!");
                }
                a++;
                
            }
            AttackConclusion(enemy);
        }

    }
    private void AttackConclusion(Unit player)
    {
        playerAttacks.Clear();
        enemyAttacks.Clear();
        if (player.friend)
        {
            player.standby = true;
            friendsStandingBy++;
            DeathCheck();
        }
        
    }
    private void DeathCheck()
    {
        playerOnHigh.GetComponent<NewBehaviourScript>().unitSelected = null;
        playerOnHigh.GetComponent<NewBehaviourScript>().lastUnitSelected = null;
        StartCoroutine(TurnWaiter());
        int b = 0;
        foreach (var item in friends)
        {
            if (item.Hp <= 0)
            {
                StringAdderAndTextListUpdator(item.name + " dies");
                Destroy(item.gameObject);
                death.Add(1);
            }
            else { death.Add(0); }

        }
        foreach (var item in death)
        {
            if (item == 0) { b++; continue; }
            if (item == 1) { friends.RemoveAt(b); continue; }
        }
        death.Clear();
        b = 0;
        StartCoroutine(TurnWaiter());
        foreach (var item in enemies)
        {
            if (item.Hp <= 0)
            {
                StringAdderAndTextListUpdator(item.name + " dies");
                Destroy(item.gameObject);
                death.Add(1);
            }
            else { death.Add(0); }
        }
        foreach (var item in death)
        {
            if (item == 0) { b++; continue; }
            if (item == 1) { enemies.RemoveAt(b); continue; }
        }
        death.Clear();
    }
    private IEnumerator TurnWaiter()
    {
        yield return null;
      
    }
    public void StringAdderAndTextListUpdator(string newText)
    {
        whatJustHappenedTextList.Insert(whatIndex, newText);
        whatJustHappened.text += whereAmIInWhat + whatJustHappenedTextList[whatIndex] + "\n";
        whereAmIInWhat++;
        if (whatIndex < 2)
        {
            whatIndex++; 
        }
        else 
        {
            whatIndex = 0; 
            whatJustHappenedSlider.maxValue = ((whatJustHappened.textInfo.lineCount / 2) + 2);
            whatJustHappenedSlider.value++;
        }

    }
}   
