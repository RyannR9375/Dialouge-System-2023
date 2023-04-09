using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EvtSystem;

public class ShowDialougeText : EvtSystem.Event
{
    public string text;
    public CharacterID id;
    public float duration;
}

public class PlayAudio : EvtSystem.Event
{
    public AudioClip clipToPlay;
    public bool isPriority;
}

public struct ResponseData
{
    public string text;
    public int karmaScore;

    public UnityAction buttonAction;
}

public class ShowResponses : EvtSystem.Event
{
    public ResponseData[] responses; 
}

public class DisableUI : EvtSystem.Event
{

}

public class PlayerInteract : EvtSystem.Event
{
    public Vector3 interactPosition;
    public Vector3 interactDirection;
    public float interactDistance;
}

public class MyEvent : EvtSystem.Event
{
    public int i;
}

public class CursorMovement : EvtSystem.Event
{
    public bool canMove;
    public float lookSpeed;
    public CursorLockMode lockMode;
}

public class FreezePlayerMovement : EvtSystem.Event
{
    public bool canMove;
    public float moveSpeed;
}