using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Dialogue System/Dialouge Table")]
public class DialogueDatabase : ScriptableObject
{
    [FormerlySerializedAs("data")] //ALLOWS YOU TO CHANGE EVERYTHING THAT WAS FORMERLY SAVED AS 'data' TO 'list'
    public List<DialogueLineData> list;

    public AudioClip clickSound;
}
