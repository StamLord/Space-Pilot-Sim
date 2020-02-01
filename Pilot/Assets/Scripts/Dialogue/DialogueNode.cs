using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode
{
    private string _choice;
    private string _message;
    public string message { get{ return _message; }}
    private DialogueNode[] next = new DialogueNode[0];

    public DialogueNode(string _message)
    {
        this._message = _message;
    }

    public DialogueNode(string choice, string message)
    {
        this._choice = choice;
        this._message = message;
    }

    public DialogueNode(string choice, string message, DialogueNode[] next)
    {
        this.next = next;
        this._choice = choice;
        this._message = message;
    }

    public DialogueNode GetNext(int option)
    {
        option -= 1; // First option is 1
        if(option < next.Length)
            return next[option];
        else
            return null;
    }

    public string[] GetOptions()
    {
        string[] optionMessages = new string[next.Length];

        for(int i = 0; i < next.Length; i++)
        {
            optionMessages[i] = next[i]._choice;
        }

        return optionMessages;
    }
}
