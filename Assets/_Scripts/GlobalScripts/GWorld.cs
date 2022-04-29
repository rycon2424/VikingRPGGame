using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GWorld : MonoBehaviour
{
    [SerializeField] int playerBounty = 0;
    [Space]
    [SerializeField] int enemyAmountCombat = 0;
    [ReadOnly] public bool playerInCombat = false;
}
