using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialoguesText : MonoBehaviour {
    public Text display;
    public string[] sentences;
    private int _index, _i;
    public float typingSpeed;

    public SpawnDialogues _dt;
    public bool data;

    private void Start(){
        data = true;
    }

    private void Update(){
        if(_dt.initialDialogue && _i == 0){
            StartCoroutine(Type());
            _i++;
        }

        //Wait until text finishes its sentences THEN click random key to skip dialogue
        if(display.text == sentences[_index] && Input.GetKey(KeyCode.Space) && _dt.initialDialogue){
            NextSentence();
        }
    }

    private IEnumerator Type(){
        foreach(var letter in sentences[_index].ToCharArray()){
            display.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void NextSentence(){
        if(_index < sentences.Length - 1){
            _index++;
            display.text = "";
            StartCoroutine(Type());
        } else {
            display.text = "";
            _dt.dialogue.SetActive(false);
            data = false;
        }
    }
}
