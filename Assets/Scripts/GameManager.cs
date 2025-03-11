using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //audio declarations
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip Win;


    //game related variables
    private int correctPiecesCount = 0; //count to keep track of correctly placed pieces
    private int totalPieces = 7; //total pieces in tangram
    private bool gameCompleted = false;
    [SerializeField] private PopupManager popupManager; 


    void Start() {
        //find and assign the PopupManager component in the scene
        popupManager = FindObjectOfType<PopupManager>();
    }

    public void PiecePlaced() {
        //increment pieces placed count and check for game completion every time to avoid incrementing over 7
        correctPiecesCount++;
        CheckGameCompletion();
    }

    private void CheckGameCompletion() {
        //as long as correct pieces is equal to 7 set game completed to true and call completed.
        if (correctPiecesCount >= totalPieces && !gameCompleted)
        {
            gameCompleted = true;
            Completed();
        }
    }

    public void Completed() {
        //play fun winning sound and display new message on pop up so user can play again!
        source.PlayOneShot(Win);
        popupManager.ShowCompletionPopup("You did it! Play again?");
    }

}
