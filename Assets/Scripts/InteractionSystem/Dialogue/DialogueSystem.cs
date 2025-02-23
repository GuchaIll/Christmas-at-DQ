using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private int currentLine;
    [SerializeField] private string characterName;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI nameText;

    public void InitiateNewDialogue(string name, string[] lines )
    {
        characterName = name;
        dialogueLines = lines;
        currentLine = 0;
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        nameText.text = characterName + ":";
        dialogueText.text = dialogueLines[currentLine];
        dialoguePanel.SetActive(true);
    }

    public void EndDialogue()
    {
        nameText.text = "";
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }
}