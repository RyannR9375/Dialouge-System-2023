using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

class StringReveal
{
    string textToReveal = null;

    float currentTime;
    float secondsPerChar;
    int currentStringIndex = 0;

    public void StartReveal(string text, float duration)
    {
        //MATH FOR TYPEWRITER EFFECT
        secondsPerChar = duration / text.Length;
        textToReveal = text;

        //RESET VALUES
        currentStringIndex = 0;
        currentTime = 0.0f;
    }

    public bool isDone()
    {
        //RETURNS TRUE OR FALSE
        return (textToReveal == null || currentStringIndex == (textToReveal.Length - 1));
    }

    public void ForceFinish()
    {
        //SET THE CURRENT STRING INDEX TO THE LENGTH OF WHAT TO REVEAL.
        //BASICALLY SAYING FINISH THE DIALOGUE.
        currentStringIndex = (textToReveal.Length - 1);

        currentTime = 0.0f; //RESET DIALOGUE'S CURRENT TIME SINCE THE TEXT IS FINISHED.
    }


    public string GetCurrentRevealedText()
    {
        //INCREMENT TIME
        currentTime += Time.deltaTime;

        //IF THE TYPEWRITER IS SUPPOSED TO BE RUNNING, KEEP RUNNING
        if (currentTime >= secondsPerChar && currentStringIndex < (textToReveal.Length - 1))
        {
            currentStringIndex++;
            currentTime = 0.0f;
        }

        // 'AsSpan' better in this case because: 'AsSpan' returns the pointer to the memory block of the size asked for '(0, currentStringIndex)
        // Then cast to String using 'ToString();'
        return textToReveal.AsSpan(0, currentStringIndex).ToString();
    }
}

public class UISystem : Singleton<UISystem>
{
    public TextMeshProUGUI dialogueText; //USE A REGISTRATION SYSTEM BY TYPE OF DATA, OR NAME( IN FUTURE ) INSTEAD OF PUBLIC REFERENCE
    public GameObject buttonContainer;
    public GameObject buttonPrefab;
    public GameObject UIRoot;
    public Color positiveKarmaColor;
    public Color negativeKarmaColor;
    public Color neutralKarmaColor;

    private Queue<GameObject> buttonPool;
    private List<GameObject> activeButtons;

    private StringReveal typewriter = new StringReveal();

    private void Start()
    {
        //CREATE NEW INSTANCE OF THIS LIST
        activeButtons = new List<GameObject>();

        //SUBSCRIBING TO EVENT 'ShowDialogueText', INVOKE 'ShowUI'
        EvtSystem.EventDispatcher.AddListener<ShowDialougeText>(ShowUI);

        //SUBSCRIBE TO EVENT 'ShowResponses', INVOKE 'ShowResponsesButtons'
        EvtSystem.EventDispatcher.AddListener<ShowResponses>(ShowResponseButtons);

        //SUBSCRIBE TO EVENT 'DisableUI', INVOKE 'HideUI'
        EvtSystem.EventDispatcher.AddListener<DisableUI>(HideUI);

        buttonPool = new Queue<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            var button = GameObject.Instantiate(buttonPrefab, buttonContainer.transform);
            button.SetActive(false);
            buttonPool.Enqueue(button);
        }


        //SET COLORS TO POSITIVE, NEGATIVE, AND NEUTRAL KARMA RESPONSES
        positiveKarmaColor = Color.green;
        negativeKarmaColor = Color.red;
        neutralKarmaColor  = Color.grey;
    }

    private void OnDisable()
    {
        //IF THE UI MANAGER IS DISABLED, REMOVE THE LISTENER
        EvtSystem.EventDispatcher.RemoveListener<ShowDialougeText>(ShowUI);
    }

    private void ShowUI(ShowDialougeText eventData)
    {
        //SET ENTIRE UI TO ACTIVE
        UIRoot.SetActive(true);

        //DISPLAYS TEXT
        //dialogueText.text = eventData.text;

        //DISPLAYS TEXT USING TYPE WRITER EFFECT
        typewriter.StartReveal(eventData.text, eventData.duration);
    }

    private void HideUI(DisableUI data)
    {
        //REFERENCE EVERY BUTTON RESPONSE ACTIVE
        foreach(GameObject button in activeButtons)
        {
            //SET THEM TO FALSE
            button.SetActive(false);
            buttonPool.Enqueue(button);
        }

        //CLEAR THE LIST SO THAT OLD RESPONSES DON'T SHOW UP IN NEW DIALOGUE
        activeButtons.Clear();

        //SET THE UI TO INVISIBLE
        UIRoot.SetActive(false);
    }

    private void ShowResponseButtons(ShowResponses eventData)
    {
        //FORCE THE DIALOGUE TO DISPLAY FULLY
        if (!typewriter.isDone())
        {
            typewriter.ForceFinish();
            dialogueText.text = typewriter.GetCurrentRevealedText();
        }

        foreach (ResponseData response in eventData.responses)
        {
            GameObject button = null;

            //INSTANTIATING BUTTON RESPONSES
            if(!buttonPool.TryDequeue(out button))
            {
                button = GameObject.Instantiate(buttonPrefab, buttonContainer.transform);
            }
            button.SetActive(true);

            //NULL CHECK FOR LABEL, IF NOT NULL, WRITE THE RESPONSE INTO THE BUTTON
            TMPro.TextMeshProUGUI label = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (label != null)
            {
                label.text = response.text;
            }

            //CHANGE BUTTON COLORS BASED ON KARMA RESPONSE
            Button uiButton = button.GetComponent<Button>();
            ColorBlock buttonColor = uiButton.colors;
            if (response.karmaScore > 0) //GOOD KARMA RESPONSE
                buttonColor.normalColor = positiveKarmaColor;
            else if (response.karmaScore < 0) //BAD KARMA RESPONSE
                buttonColor.normalColor = negativeKarmaColor;
            else
                buttonColor.normalColor = neutralKarmaColor; //NEUTRAL KARMA RESPONSE 

            uiButton.colors = buttonColor;

            //REMOVE ALL LISTENERS BEFORE ADDING NEW ONES
            uiButton.onClick.RemoveAllListeners();
            uiButton.onClick.AddListener(response.buttonAction);

            //ADD THE BUTTONS TO THE LIST
            activeButtons.Add(button);
        }
    }

    private void Update()
    {
        //CHECK IF THE TYPEWRITER IS DONE,
        if (UIRoot.activeSelf && !typewriter.isDone())
            //IF NOT DONE, THEN KEEP DISPLAYING MORE TEXT
            dialogueText.text = typewriter.GetCurrentRevealedText();
    }

}
