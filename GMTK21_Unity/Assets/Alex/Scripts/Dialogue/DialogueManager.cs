using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textTMP;
    public Animator animator;
    public Animator characterPortaits;


    private AudioSource bgM;
    private Queue<string> sentences;
    private bool isOpen;
    private GameObject player;
    private bool playerTalking;

    void Start()
    {
        sentences = new Queue<string>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        bgM = GameObject.FindGameObjectWithTag("BackgroundAudio").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                DisplayNextSentence();
            }
        }

        if (playerTalking)
        {
            characterPortaits.SetFloat("PlayerTalkWeight", Mathf.MoveTowards(characterPortaits.GetFloat("PlayerTalkWeight"), -1, Time.deltaTime * 5));
        }
        else
        {
            characterPortaits.SetFloat("PlayerTalkWeight", Mathf.MoveTowards(characterPortaits.GetFloat("PlayerTalkWeight"), 1, Time.deltaTime * 5));
        }
            
    }

    public void StartDialogue (Dialogue dialogue)
    {
        isOpen = true;

        bgM.volume = 0.1f;

        player.GetComponentInChildren<Animator>().SetFloat("WalkSpeed", -1);
        player.GetComponent<IsometricCharacterController>().enabled = false;

        characterPortaits.SetFloat("PlayerTalkWeight", 0);

        animator.SetBool("DialogueOpen", true);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        if (dialogue.playerStartsTalking)
        {
            playerTalking = false;
        }
        else
        {
            playerTalking = true;
        }
        

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (playerTalking)
        {
            playerTalking = false;
        }
        else
        {
            playerTalking = true;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        textTMP.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textTMP.text += letter;
            yield return null;
            yield return null;
        }
    }

    void EndDialogue()
    {
        bgM.volume = 0.25f;

        characterPortaits.SetFloat("PlayerTalkWeight", 0);

        animator.SetBool("DialogueOpen", false);

        player.GetComponent<IsometricCharacterController>().enabled = true;

        isOpen = false;
    }
}
