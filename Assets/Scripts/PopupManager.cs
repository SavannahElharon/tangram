using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//handle buttons
public class PopupManager : MonoBehaviour {
    //declare objects for button that will be changed
    [SerializeField] private GameObject panel;
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI imageText;

    private Action onConfirm; //action to trigger when first play is pressed
    private Action onAgain; //action to trigger when play again is pressed


    public void Close(){
        //deactivates the panel, hiding the popup
        panel.SetActive(false);
    }
    public void Confirm(){
        onConfirm?.Invoke();
        Close();
    }
    public void Again(){
        //invoke actions when played again, close pop up and reset scene
        onAgain?.Invoke();
        Close();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowCompletionPopup(string message){
        //method for changing text on popup so user knows they won and can play again
        imageText.text = message;
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(Again); //add the "Again" method to the button's onClick listener
        panel.SetActive(true);
    }
}
