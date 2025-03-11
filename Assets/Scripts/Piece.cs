using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour{
    //currently selected piece
    private GameObject selectedObject;
    private bool isPlaced = false;
    //audio declarations
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip Bump, Click, Pop, RotateSound;
    //target position for placement
    public Transform target;
    private GameManager gameManager;

//initialize audio components and game manager
private void Start(){   
    //get the AudioSource component or add one if it doesn't exist
    source = GetComponent<AudioSource>();
    if (source == null) { 
        source = gameObject.AddComponent<AudioSource>(); 
    }
    gameManager = FindObjectOfType<GameManager>();
}

//called every frame
    private void Update(){
        //if left clicked and no object is currently selected, begin the selection process
        if (Input.GetMouseButtonDown(0)){
            if (selectedObject == null){
                source.PlayOneShot(Click);
                RaycastHit hit = CastRay();
                if (hit.collider != null){
                    if (!hit.collider.CompareTag("Drag")){
                        return;
                    }
                    //if an object is hit by the ray and it's tagged as Drag select it
                    selectedObject = hit.collider.gameObject;
                    Cursor.visible = false; //invisible cursor while dragging
                }
            }
            else {
                //if distance of piece is less than one play correct sound and snap to matching position
                if (target != null && Vector3.Distance(selectedObject.transform.position,  target.position) < 1f) {
                    source.PlayOneShot(Pop);
                    Piece pieceComponent = selectedObject.GetComponent<Piece>();
                    if (pieceComponent != null && !pieceComponent.isPlaced){
                        pieceComponent.isPlaced = true;
                        gameManager.PiecePlaced();
                        selectedObject.transform.position = target.position;
                    }
                }
                else {
                    source.PlayOneShot(Bump); //position is wrong so play bump
                }
                //update the position of the selected object based on the mouse position
                Vector3 position = new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);
                selectedObject = null;
                Cursor.visible = true;
            }
        }
        //if object is selected continue dragging and changing position
        if(selectedObject != null) {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);
            //if right click rotate 45 degrees and play sound
            if (Input.GetMouseButtonDown(1)) {
                source.PlayOneShot(RotateSound);
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 45f,
                    selectedObject.transform.rotation.eulerAngles.z));
            }
        }
    }

    //cast a ray from the camera to detect objects under the mouse
    private RaycastHit CastRay() {
        //create screen positions for clip planes based on the mouse position
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        //convert the screen space positions to world space
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        //return if any object hit
        return hit;
    }
}