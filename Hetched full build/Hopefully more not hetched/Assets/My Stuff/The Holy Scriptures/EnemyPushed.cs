using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyPushed : MonoBehaviour
{
    public Vector3 lastMovementDirection;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            //compare where they are on the enemy list maybe to make sure only one of them moves backwards
            transform.position -= lastMovementDirection;
        }
    }

}
