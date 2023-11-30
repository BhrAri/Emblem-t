using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatGovernanceIndex : MonoBehaviour
{
    public int a;
    public Spawnig spawnig;
    private void Start()
    {
        spawnig = FindFirstObjectByType<Spawnig>();
    }
    public void GovernStatIndex()
    {
        spawnig.statGovernanceIndex = a;
    }
}
