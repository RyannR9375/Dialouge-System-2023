using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/Sova/Recon_Dart")]
public class Sova_Recon : Ability
{
    [SerializeField]
    private float arrowForce;

    public GameObject arrow;
    
    //ACTIVATE ABILITY FUNCTION
    public override void Activate(GameObject parent)
    {
        //GET COMPONENTS
        GameObject thisArrow = Instantiate(arrow, parent.transform.position, Quaternion.identity);
        Rigidbody2D arrowRB = thisArrow.GetComponent<Rigidbody2D>();

        //ADD FORCE TO THE ARROW SO IT "SHOOTS"
        arrowRB.AddForce(arrowRB.velocity * 2, ForceMode2D.Impulse);
    }

    //COOLDOWN FUNCTION
    public override void BeginCooldown(GameObject parent)
    {

    }
}
