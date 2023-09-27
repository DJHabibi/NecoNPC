using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Behaviour", menuName = "NPC Behaviour")]
public class ChatPrompt : ScriptableObject
{
   [SerializeField] public string NpcBehaviour;

    public string Content
    {
        get { return NpcBehaviour; }
    }
}
