using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : Interactable
{
    private MeshRenderer baseMesh = null;
    private MaterialPropertyBlock block = null;
    private MaterialPropertyBlock offBlock = null;
    private bool isRed = false;

    public void Start()
    {
        baseMesh = GetComponent<MeshRenderer>();

        block = new MaterialPropertyBlock();
        block.SetColor("_Color", new Color(1.0f, 0.0f, 0.0f));

        offBlock = new MaterialPropertyBlock();
        offBlock.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f));
    }

    public override void OnInteract()
    {
        baseMesh.SetPropertyBlock(isRed ? offBlock : block);

        isRed = !isRed;
    }
}
