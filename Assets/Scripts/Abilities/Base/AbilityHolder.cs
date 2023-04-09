using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public List<Ability> Abilities;

    private void Awake()
    {
        Abilities = new List<Ability>();
    }
}
