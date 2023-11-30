using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected; //is this unit selected right now?
    public bool friend; //is the player allowed to move them?
    public bool moved; //has this unit moved this turn?
    public bool standby; //if true unable to act until next turn
    public bool advantage; //did their reach give them combat advantage
    public new string name; //name
    public int Hp; //health
    public int strength; //weapon strength
    public int magic; //magic strength
    public int speed; //how many times a unit can attack
    public int dexterity; //crit rate and dodge chance
    public int defense; //reduces weapon damage
    public int resistance; //reduces magic damage
    public int movement; //how far a unit can move
    public int attacks; //how many attacks are made in a combat
    public int reach; //how far unit can attack from
    public int attackType; //1 for magical, 2 for physical
    public GameObject childTurnOffer; //attack range and shadows are attached to 0
    public Transform[] children;
    bool changed = false;

    public NewBehaviourScript playerOnHigh;

    private void Start()
    {
        playerOnHigh = FindObjectOfType<NewBehaviourScript>();
        children = GetComponentsInChildren<Transform>();
        
    }
    private void Update()
    { 
        if (!changed && friend)
        {
            WaitBeforeCheck();
        }
        if (moved)
        {
            childTurnOffer.SetActive(false);
        }
        if (playerOnHigh.turnOffChildPlease)
        {
            childTurnOffer.SetActive(false);
        }
        if (gameObject.transform.position.y < -3)
        {
            standby = true;
            moved = true;
            Hp = 0;
        }
    }
    void WaitBeforeCheck()
    {
        
        if (children[2] != null)
        {
            children[2].GetComponent<Rigidbody>().mass = 0.001f;
            
        }
        childTurnOffer = children[1].gameObject;
        changed = true;
    }
}
