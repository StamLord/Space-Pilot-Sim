using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueReader : MonoBehaviour
{
    public Character player;

    public Text messageBody;
    public Text[] choices;

    DialogueNode activeNode;
    bool awaitingInput;
    int options;

    #region Singleton

    public static DialogueReader instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Debug.LogWarning("More than 1 instance of DialogueReader exists!");
            Destroy(this.gameObject);
        }
    }
    #endregion

    void Start()
    {
        DialogueNode o1 = new DialogueNode("Request Landing", "Rejected");
        DialogueNode o21 = new DialogueNode("Information 1", "Here", null, new Stat(StatName.CHARISMA, 1));
        DialogueNode o22 = new DialogueNode("Information 2", "Here");
        DialogueNode o23 = new DialogueNode("Information 3", "Here");
        DialogueNode o2 = new DialogueNode("Ask for information", "Which Information would you want?", new DialogueNode[] {o21, o22, o23});
        DialogueNode o3 = new DialogueNode("End Conversation", "Farewell");
        DialogueNode n = new DialogueNode("", "Communications channel open.", new DialogueNode[] {o1, o2, o3});

        StartDialogue(n);
    }

    public void StartDialogue(DialogueNode initialNode)
    {
        activeNode = initialNode;
        DrawNode();
    }

    private void DrawNode()
    {
        // Clear previous node
        ClearMessage();
        ClearOptions();

        awaitingInput = false;
        
        DrawMessage(activeNode.message);
        DrawOptions(activeNode.GetOptions(player));
        
        awaitingInput = true;
    }

    private void DrawMessage(string message)
    {
        if(messageBody) 
        {
            if(activeNode == null)
                messageBody.text = "";
            else
                messageBody.text = activeNode.message;
        }

        Debug.Log("MSG: " + message);
    }

    private void DrawOptions(string[] options) // Clear all choices
    {
        if(options == null)
        {
            for(int i = 0; i < choices.Length; i++)
            {
                choices[i].text = "";
                choices[i].gameObject.SetActive(false);
            }

            this.options = 0;
        }
        else if(options.Length == 0) // Add End Dialogue choice
        {
            choices[0].gameObject.SetActive(true);
            choices[0].text = "1. End Dialogue";
            Debug.Log("1. End Dialogue");

            for(int i = 1; i < choices.Length; i++)
            {
                choices[i].text = "";
                choices[i].gameObject.SetActive(false);
            }

            this.options = 1;
        }
        else // Display Choices
        {
            int i = 0;
            for(; i < options.Length; i++)
            {
                choices[i].gameObject.SetActive(true);
                choices[i].text = (i + 1) + ". " + options[i];
                Debug.Log((i + 1) + ". " + options[i]);
            }

            for(; i < choices.Length; i++)
                choices[i].gameObject.SetActive(false);

            this.options = options.Length;
        }
    }

    void ClearMessage()
    {
        DrawMessage("");
    }

    void ClearOptions()
    {
        DrawOptions(null);
    }

    void Update()
    {
        if(awaitingInput)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1) && options > 0)
                AdvanceNode(1);
            else if(Input.GetKeyDown(KeyCode.Alpha2) && options > 1)
                AdvanceNode(2);
            else if(Input.GetKeyDown(KeyCode.Alpha3) && options > 2)
                AdvanceNode(3);
            else if(Input.GetKeyDown(KeyCode.Alpha4) && options > 3)
                AdvanceNode(4);
        }
    }

    void AdvanceNode(int option)
    {
        DialogueNode next = activeNode.GetNext(option);
        if(next != null)
        {
            activeNode = next;
            DrawNode();
        }
        else
            EndDialogue();
    }

    public void EndDialogue()
    {
        activeNode = null;
        awaitingInput = false;
        ClearMessage();
        ClearOptions();
        Debug.Log("Dialogue Ended");
    }
}
