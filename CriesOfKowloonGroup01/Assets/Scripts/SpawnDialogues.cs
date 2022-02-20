using UnityEngine;
using UnityEngine.UI;

public class SpawnDialogues : MonoBehaviour {
    public GameObject dialogue;
    public Text dialogueText;
    public bool initialDialogue;
    
    private DialoguesText _ds;

    private void Start(){
        initialDialogue = false;
        dialogue.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D dialogueInfo){
        if(!_ds.data){
            dialogue.SetActive(false);
        }

        if (!_ds.data || !dialogueInfo.CompareTag("Player")) return;
        dialogue.SetActive(true);
        initialDialogue = true;
    }

    private void OnTriggerExit2D(Collider2D dialogueInfo){
        dialogue.SetActive(false);
        initialDialogue = false;
    }
}
