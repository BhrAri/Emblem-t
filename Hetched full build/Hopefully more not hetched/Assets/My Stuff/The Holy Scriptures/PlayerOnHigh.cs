using System.Collections;
using UnityEngine;
using System;
using TMPro;
using Unity.VisualScripting;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject buttonBlock;
    public GameObject marker;
    public GameObject lastUnitSelected;
    public GameObject unitSelected;
    private GameObject shadow;
    public Camera GameCamera;
    [SerializeField] private Vector3 shadowStartPosition;
    public float cameraPanSpeed;
    private float holdingNumber;
    private Vector3 markerGoUp = new(0f, 5.5f, 0f);
    public Vector3 shadowPosition;
    public bool noReselect;
    [SerializeField] TextMeshProUGUI named;
    [SerializeField] TextMeshProUGUI stats1;
    [SerializeField] TextMeshProUGUI stats2;
    public bool turnOffChildPlease;
    private Rigidbody shadowRb;
    public GameObject gameManager;



    //testing variables
    [SerializeField] private int x;
    [SerializeField] private int z;
    [SerializeField] private Vector2 mousePosition;
    [SerializeField] private Vector2 startingMousePosition;
    [SerializeField] private float shadowMovementX;
    [SerializeField] private float shadowMovementZ;

    private void Start()
    {
        StartCoroutine(WaitToTurnOffChildren());
       
    }
    
    void Update()
    {
        mousePosition = Input.mousePosition;

        CameraControl();
       
        //unit tap detection
        if (Input.GetMouseButtonDown(0) && !noReselect)
        {
            
            var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                if (unitSelected != hit.collider.gameObject && !buttonBlock.activeInHierarchy)
                {
                    lastUnitSelected = unitSelected;
                    unitSelected = hit.collider.gameObject;
                    if (unitSelected.GetComponent<Unit>() && !unitSelected.GetComponent<Unit>().standby)
                    {
                        turnOffChildPlease = false;
                        unitSelected.GetComponent<Unit>().childTurnOffer.SetActive(true);
                    }
                }
                else if (!buttonBlock.activeInHierarchy)
                {
                    unitSelected.GetComponent<Unit>().childTurnOffer.SetActive(true);
                    lastUnitSelected = null;
                }
                
            }
            else 
            {
                unitSelected.GetComponent<Unit>().childTurnOffer.SetActive(false);
            }
            UnitToUi();
            MarkUnit();
        }
        //makes sure unit is a friend before moving it 
        if (Input.GetMouseButton(0) && unitSelected != null)
        {
            holdingNumber += 0.0001f;
            if (unitSelected.GetComponent<Unit>() && unitSelected.GetComponent<Unit>().friend && !unitSelected.GetComponent<Unit>().moved)
            {
                if (unitSelected.GetComponent<Unit>().friend && holdingNumber == 0.0001f)
                {
                    shadowStartPosition = unitSelected.transform.position;
                    startingMousePosition = Input.mousePosition;
                    shadow = GameObject.Find(unitSelected.name + " Shadow");
                    
                }
                MoveUnit();
            }
        } else
        {
            holdingNumber = 0;
        }
        if (lastUnitSelected != null &&  lastUnitSelected != unitSelected)
        {
            lastUnitSelected.GetComponent<Unit>().childTurnOffer.SetActive(false);
        }


    }

    private void UnitToUi()
    {
        if (unitSelected == null)
        {
            named.text = null;
            stats1.text = null;
            stats2.text = null;
            buttonBlock.SetActive(false);
        }
        else if (unitSelected.GetComponent<Unit>())
        {
            var bunit = unitSelected.GetComponent<Unit>();
            named.text = bunit.name;
            stats1.text = "Hp : " + bunit.Hp.ToString() + " Strength: " + bunit.strength.ToString() + " Magic: " + bunit.magic.ToString() + " Speed: " + bunit.speed.ToString();
            stats2.text = "Dexterity: " + bunit.dexterity.ToString() + " Defence: " + bunit.defense.ToString() + " Resistance: " + bunit.resistance.ToString();
        }
        
    }
    //moving units
    private void MoveUnit()
    {
       
        if (shadow != null)
        {
            shadowRb = shadow.GetComponent<Rigidbody>();
            if (Math.Abs(shadow.transform.position.x - shadowStartPosition.x) +
                Math.Abs(shadow.transform.position.z - shadowStartPosition.z) <=
                unitSelected.GetComponent<Unit>().movement)
            {
                float x = (startingMousePosition.x - mousePosition.x) / 50;
                if (x > 0 || x < 0) { startingMousePosition.x = Input.mousePosition.x; }

                float z = (startingMousePosition.y - mousePosition.y) / 50;
                if (z > 0 || z < 0) { startingMousePosition.y = Input.mousePosition.y; }

                shadow.transform.position = new Vector3 (shadow.transform.position.x + x, shadow.transform.position.y, shadow.transform.position.z + z);

               
                shadowPosition = shadow.transform.position;
                if (Math.Abs(shadow.transform.position.x - shadowStartPosition.x) +
                    Math.Abs(shadow.transform.position.z - shadowStartPosition.z) >
                    1)
                {
                    noReselect = true;
                } else { noReselect = false; }
                buttonBlock.SetActive(true);
            }
            if (Math.Abs(shadow.transform.position.x - shadowStartPosition.x) +
                   Math.Abs(shadow.transform.position.z - shadowStartPosition.z) >=
                   unitSelected.GetComponent<Unit>().movement)
            {
                
                float b =shadow.transform.position.x - unitSelected.transform.position.x;
                float gee = shadow.transform.position.z - unitSelected.transform.position.z;
                if (b != 0 && gee != 0)
                {
                    shadow.transform.position += new Vector3((b / Math.Abs(b)) * -0.2f, 0f, (gee / Math.Abs(gee)) * -0.2f);
                }
                else if (b == 0 && gee !=0) 
                {
                    shadow.transform.position += new Vector3(0f, 0f, (gee / Math.Abs(gee)) * -0.2f);
                }
                else if (b != 0 && gee == 0)
                {
                    shadow.transform.position += new Vector3((b / Math.Abs(b)) * -0.2f, 0f, 0f);
                }
                buttonBlock.SetActive(false);
                shadowPosition = shadow.transform.position;
            }
            shadowRb.velocity = Vector3.zero;
        }
        

    } 

    //control the camera
    private void CameraControl()
    {
        Vector2 move = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GameCamera.transform.position = GameCamera.transform.position + cameraPanSpeed * Time.deltaTime * new Vector3(-move.x, 0, -move.y);
    }

    //shines a light on selected unit
    private void MarkUnit()
    {
        if (unitSelected == null) { turnOffChildPlease = true; }
        else if (!unitSelected.GetComponent<Unit>())
             {
                 marker.SetActive(false);
                 marker.transform.SetParent(null);
             }
        else if (unitSelected != null && marker.transform.parent != unitSelected.transform)
        {
            marker.SetActive(true);
            marker.transform.SetParent(unitSelected.transform, false);
            marker.transform.localPosition = markerGoUp;
        }
    }

    public void Move()
    {
        float increasePlayerTransformX = (int)Math.Round(shadowPosition.x - unitSelected.transform.position.x);
        float increasePlayerTransformZ = (int)Math.Round(shadowPosition.z - unitSelected.transform.position.z);
        shadow.transform.localPosition = Vector3.zero;
        unitSelected.transform.position += new Vector3 (increasePlayerTransformX, 0, increasePlayerTransformZ);
        unitSelected.GetComponent<Unit>().moved = true;
        gameManager.GetComponent<GameManager>().StringAdderAndTextListUpdator(unitSelected.name + " moved");
    }
    public void Attack()
    {
        unitSelected.GetComponent<Unit>().moved = true;
        turnOffChildPlease = true;
        buttonBlock.SetActive(false);
        noReselect = true;
    }
    public void Wait()
    {
        if (!noReselect)
        {
            noReselect=true;
            StartCoroutine(WaitHarder());
        }
        unitSelected.GetComponent<Unit>().moved = true;
        turnOffChildPlease = true;
        unitSelected.GetComponent<Unit>().standby = true;
        buttonBlock.SetActive(false);
        noReselect = false;
        gameManager.GetComponent<GameManager>().StringAdderAndTextListUpdator(unitSelected.name + " waited");
    }
    private IEnumerator WaitHarder()
    {
        yield return null;
    }
    private IEnumerator WaitToTurnOffChildren()
    {
        yield return new WaitForNextFrameUnit();
        turnOffChildPlease = true;
    }
}
