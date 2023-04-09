using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField]
    private string abilityName;
    [SerializeField]
    private string abilityDescription;

    public Ability ability;

    public float activeTime;
    public float cooldownTime;

    public KeyCode key;

    //SETTING 'ACTIVATE' & 'BEGINCOOLDOWN' FUNCTIONS FOR TYPE 'Ability' SCRIPTS.
    public virtual void Activate(GameObject parent)
    {

    }

    public virtual void BeginCooldown(GameObject parent)
    {

    }
}
