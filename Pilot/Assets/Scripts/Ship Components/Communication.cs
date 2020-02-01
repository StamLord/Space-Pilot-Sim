using UnityEngine;

public class Communication : Component
{
    public PilotInterface pi;
    public Targeting targeting;

    [SerializeField] private bool isOpen;
    [SerializeField] private DialogueNode _dialogue;
    public DialogueNode dialogue { get{ return _dialogue; }}

    void Awake()
    {
        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnCommunication += Activate;
        }

        if(targeting == null)
        {
            targeting = GetComponent<Targeting>();
        }
    }

    void Activate()
    {
        if(functional == false)
            return;

        isOpen = !isOpen;
        if(isOpen) 
        {
            if(targeting)
            {
                // Check for communication component on target
                Communication c = targeting.target.GetComponent<Communication>();
                if(c)
                {
                    // Check for a dialogue node
                    DialogueNode d = c.dialogue;
                    if(d != null)
                        DialogueReader.instance.StartDialogue(d);
                }
            }
        }
        else if(isOpen) 
            DialogueReader.instance.EndDialogue();
    }

}