using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartDialogueLine : Interactable
{
    public DialogueDatabase dialogueTree; //DIALOGUE DATABASE

    private DialogueManager dialogueManager; //DIALOGUE MANAGER GAME OBJECT
    private DialogueLineData lineToStart; //DIALOGUE LINE TO START

    public void Awake()
    {
        //AUTOMATICALLY FIND THE DIALOGUE MANAGER, SINCE IT'S A SINGLETON, SHOULDN'T BE A PROBLEM, JUST BAD FOR MEMORY?
        dialogueManager = FindObjectOfType<DialogueManager>();
        lineToStart = dialogueTree.list[0]; //START THE FIRST LINE IN THE DIALOGUE TREE
    }
    public override void OnInteract()
    {
        dialogueManager.StartDialogue(lineToStart); //START THE DIALOGUE FROM THE DIALOGUE TREE ON THE OBJECT
    }
}
