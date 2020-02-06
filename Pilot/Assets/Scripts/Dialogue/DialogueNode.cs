using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode
{   
    /// Text displayed in the choice list
    private string _choice;
    /// Text displayed when node is chosen
    private string _message;
    public string message { get{ return _message; }}
    /// Connected nodes that are displayed as choices
    private DialogueNode[] next = new DialogueNode[0];
    /// Required stat and level
    private Stat _requirement;
    public Stat requirement { get{ return _requirement; }}

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

    public DialogueNode(string choice, string message, DialogueNode[] next, Stat requirement)
    {
        this.next = next;
        this._choice = choice;
        this._message = message;
        this._requirement = requirement;
    }

    public DialogueNode GetNext(int option)
    {
        option -= 1; // First option is 1
        if(option < next.Length)
            return next[option];
        else
            return null;
    }

    public string[] GetOptions(Character character)
    {
        List<string> optionMessages = new List<string>(); 
        
        for(int i = 0; i < next.Length; i++)
        {
            if(next[i].requirement != null)
            {
                Stat stat = next[i].requirement;
                int level = character.GetStat(stat.name);
                if(level >= stat.level)
                    optionMessages.Add("[" + stat.name.ToString() + ": " + stat.level.ToString() + "] " + next[i]._choice);
            }
            else
                optionMessages.Add(next[i]._choice);
        }

        return optionMessages.ToArray();
    }
}
