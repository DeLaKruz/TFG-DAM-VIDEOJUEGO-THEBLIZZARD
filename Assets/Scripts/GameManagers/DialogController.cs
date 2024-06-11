using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public static DialogController instance;
    private Animator anim;
    private Queue<string> dialogqueue = new Queue<string>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    Texts text;
    public bool finalboss;
    [SerializeField] Text screenText;


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ActivateCartel(Texts textObject)
    {
        if (anim == null || textObject == null)
        {
            return;
        }

        anim.SetBool("Cartel", true);
        text = textObject;
        ActivateText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isTyping)
        {
            NextText();
        }
    }

    public void ActivateText()
    {
        dialogqueue.Clear();
        foreach (string savetext in text.textsArray)
        {
            dialogqueue.Enqueue(savetext);
        }

        if (dialogqueue.Count > 0)
        {
            NextText();
        }
    }

    public void NextText()
    {
        if (dialogqueue.Count == 0)
        {
            if (finalboss)
            {
                FinalBoss.instance.endBattleText = true;
                FinalBoss.instance.canStartBattle = true;
            }
            CloseCartel();
            return;
        }

        if (isTyping)
        {
            return;
        }

        string actualText = dialogqueue.Dequeue();

        if (typingCoroutine != null)
        {

            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(ShowCharacters(actualText));
    }

    public void CloseCartel()
    {
        anim.SetBool("Cartel", false);
    }

    IEnumerator ShowCharacters(string textToShow)
    {
        isTyping = true;
        Debug.Log("Started typing: " + textToShow);
        screenText.text = "";
        foreach (char character in textToShow.ToCharArray())
        {
            screenText.text += character;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;
    }
}