using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable, IDamagable
{
    Character character;
    DialogueNode dialogue;
    // AI object
    // Schedule object

    void Awake()
    {
        DialogueNode node0 = new DialogueNode("");
        DialogueNode node1 = new DialogueNode("I want to know about the Reavers", "Whatever they are, they are not human anymore. Not if the stories are true.", new DialogueNode[]{node0});
        DialogueNode node2 = new DialogueNode("I want to know about the war", "Don't know much. People around act as if it doesn't exist.", new DialogueNode[]{node0});
        DialogueNode node3 = new DialogueNode("Farewell", "Goodbye");
        node0 = new DialogueNode("I want to ask some more questions", "How can I help?", new DialogueNode[] {node1, node2, node3});
        dialogue = node0;

    }
    public void Interact()
    {
        DialogueReader.instance.StartDialogue(dialogue);
    }

    public void Damage(int amount)
    {
        character.Damage(amount);
    }
}
