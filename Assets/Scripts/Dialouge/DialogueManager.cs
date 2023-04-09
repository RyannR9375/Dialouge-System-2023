using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    public DialogueDatabase database;

    private float dialogueWaitTime = 0.0f;
    private const float kDefaultWaitTime = 2.0f;
    private DialogueLineData currentDialogue = null;
    private float currentTime = 0.0f;

    public KeyCode debugKey = KeyCode.F;
    public KeyCode skipDialogue = KeyCode.Space;

    //DICTIONARY FOR DIALOUGE
    Dictionary<string, DialogueLineData> dialogueDatabase = new Dictionary<string, DialogueLineData>();

    public void Start()
    {
        LoadDataBase();
    }
    public void LoadDataBase()
    {
        //ITERATE THROUGH DIALOUGE DATABASE AND ADD IT TO THE DICTIONARY
        foreach(DialogueLineData line in database.list) {
            dialogueDatabase.Add(line.name, line);
        }
    }

    //OVERLOAD FUNCTION THAT CALLS ORIGINAL FUNCTION BY "OBJECTS NAME INSIDE THE UNITY EDITOR" INSTEAD OF BY DATA TYPE
    public void StartDialogue(string dialogueName)
    {
        DialogueLineData data = null;

        if (dialogueDatabase.TryGetValue(dialogueName, out data))
        {
            if(data != currentDialogue)
                StartDialogue(data);
        }
    }

    //STARTS DIALOUGE
    public void StartDialogue(DialogueLineData lineToStart)
    {
        //SHOW TEXT ON SCREEN
        ShowDialougeText message = new ShowDialougeText();
        message.text = lineToStart.text;
        message.id   = lineToStart.character;

        EvtSystem.EventDispatcher.Raise<ShowDialougeText>(message);

        //PLAY DIALOUGE AUDIO
        if (lineToStart.dialogueAudio != null)
        {
            //ACTUAL DIALOGUE VOICE AUDIO
            PlayAudio audioMessage  = new PlayAudio();
            audioMessage.clipToPlay = lineToStart.dialogueAudio;
            audioMessage.isPriority = true;

            //RAISE THE EVENT SO THE AUDIO PLAYS
            EvtSystem.EventDispatcher.Raise<PlayAudio>(audioMessage);

            dialogueWaitTime = lineToStart.dialogueAudio.length;
        }
        else
        {
            dialogueWaitTime = kDefaultWaitTime;
        }
        //RESET VALUES OF DIALOGUE AND TIME
        currentDialogue = lineToStart;
        currentTime = 0.0f;

        //FREEZES CURSOR
        CursorMovement cursor = new CursorMovement();
        cursor.canMove = true;
        EvtSystem.EventDispatcher.Raise<CursorMovement>(cursor);

        //UNFREEZES PLAYER MOVEMENT
        FreezePlayerMovement freezePlayer = new FreezePlayerMovement();
        freezePlayer.canMove = false;
        EvtSystem.EventDispatcher.Raise<FreezePlayerMovement>(freezePlayer);
    }

    private void PlayResponseLine(int currentResponseIndex)
    {
        //RAISE THE EVENT TO HIDE THE CURRENT UI, AND THEN START THE NEW ONE
        EvtSystem.EventDispatcher.Raise<DisableUI>(new DisableUI());

        //IF THERE ARE MORE DIALOGUE RESPONSES IN THE TREE, CONTINUE DISPLAYING RESPONSES
        if (currentDialogue.responses.Length > currentResponseIndex)
        {
            DialogueLineData line = currentDialogue.responses[currentResponseIndex];
            StartDialogue(line); //THIS TIME, START THE DIALOGUE AT THE CURRENT INDEX INSTEAD OF RESTARTING IT

            //DIALOGUE CLICK AUDIO
            PlayAudio dialogueClickAudio = new PlayAudio();
            dialogueClickAudio.clipToPlay = database.clickSound;
            dialogueClickAudio.isPriority = false;

            EvtSystem.EventDispatcher.Raise<PlayAudio>(dialogueClickAudio); //PLAY THE AUDIO START SOUND AFTER THE DIALOGUE AUDIO SO THAT IT DOESN'T GET CUT OFF
        }
    }
    
    private void CreateResponseMessage()
    {
        //HOW MANY RESPONSES YOU HAVE
        int numResponses = currentDialogue.responses.Length;
        if (numResponses > 1)
        {
            ShowResponses responseMessage = new ShowResponses();

            responseMessage.responses = new ResponseData[numResponses];

            int index = 0;
            foreach(DialogueLineData response in currentDialogue.responses)// FOR EACH DIALOGUE LINE, MAKE SURE YOU ADJUST THE CORRECT VALUES
            {
                responseMessage.responses[index].text = response.text;
                responseMessage.responses[index].karmaScore = response.karmaScore;

                int currentIndex = index;
                responseMessage.responses[index].buttonAction = () => { this.PlayResponseLine(currentIndex); };
                index++;
            }

            //RAISE THE EVENT SO THAT THE UI MANAGER CAN DISPLAY THESE MESSAGES
            EvtSystem.EventDispatcher.Raise<ShowResponses>(responseMessage);
        }
        else if (numResponses == 1)
        {
            PlayResponseLine(0);
        }
        else //IF THE 'numResponses' IS 0, HIDE UI
        {
            //RAISE THE EVENT TO HIDE THE CURRENT UI, AND THEN START THE NEW ONE
            EvtSystem.EventDispatcher.Raise<DisableUI>(new DisableUI());
            currentDialogue = null;

            //UNFREEZES CURSOR
            CursorMovement cursor = new CursorMovement();
            cursor.canMove = false;
            EvtSystem.EventDispatcher.Raise<CursorMovement>(cursor);

            //UNFREEZES PLAYER MOVEMENT
            FreezePlayerMovement freezePlayer = new FreezePlayerMovement();
            freezePlayer.canMove = true;
            EvtSystem.EventDispatcher.Raise<FreezePlayerMovement>(freezePlayer);
        }
    }

    public void Update()
    {
        //CHECK IF CURRENT DIALOGUE IS PLAYING
        if (currentDialogue != null && dialogueWaitTime > 0.0f)
        {
            //ASSIGN SKIP DIALOGUE BUTTON TO SPACE
            bool shouldSkip = Input.GetKeyUp(skipDialogue);

            //IF IT IS, ADD TIME TO TIMER
            currentTime += Time.deltaTime;

            //IF TIMER REACHES DESIGNATED WAIT TIME, PLAY AUDIO
            if(currentTime >= dialogueWaitTime)
            {
                dialogueWaitTime = 0.0f;
                CreateResponseMessage();
            }
        }

        //STARTS DIALOGUE, USED FOR DEBUGGING AS OF RIGHT NOW
        if (Input.GetKeyUp(debugKey))
        {
            StartDialogue("DialogueOne"); // WILL BE REPLACED WITH ITEM LATER ON IN THE GAME
        }
    }
}