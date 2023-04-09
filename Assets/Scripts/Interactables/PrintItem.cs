using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintItem : Interactable
{
    public string text;

    public override void OnInteract()
    {
        Debug.Log(text);
    }
}
