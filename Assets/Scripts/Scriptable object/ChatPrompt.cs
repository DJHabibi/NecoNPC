using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Behaviour", menuName = "NPC Behaviour")]
public class ChatPrompt : ScriptableObject
{
    [TextArea(15, 20)]
    [SerializeField] public string NpcBehaviour;

    public string Content
    {
        get { return NpcBehaviour; }
    }
}
