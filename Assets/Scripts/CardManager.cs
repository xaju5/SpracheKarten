

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private CSVController cSVController;
    private List<Verb> verbList;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI word;
    [SerializeField] private TextMeshProUGUI topicToGuess;
    [SerializeField] private float flipSpeed;
    [SerializeField] private float slideSpeed;

    private int cardIndex;
    private bool isFacingFront;
    private bool enableFlip;
    private bool flipDirection;
    private bool changedFlipDirection;
    private int flipCounter;
    private Quaternion targetRotation;
    private bool enableSlide;
    private GameObject oldCanvas;
    private Renderer oldCanvasRenderer;
    private Vector3 targetDirection;

    void Awake() {
        cSVController = new CSVController();
        verbList = cSVController.ReadCSV();
    }

    void Start()
    {
        isFacingFront = true;
        cardIndex = 1;
        title.text = verbList[0].infiniv;
        word.text = verbList[cardIndex].infiniv;
        topicToGuess.text = verbList[0].translation + "?";
        ResetFlipBool();
        enableSlide = false;
    }


    void Update() {                    
        if(enableFlip) FlipCardAnimation();
        if(enableSlide) SlideCardAnimation();
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Required to call OnEndDrag
        //TODO: Move a little bit the card in the direction of the drag
    }
    public void OnEndDrag(PointerEventData eventData){
        if(enableFlip || enableSlide) return;
        targetDirection = (eventData.position - eventData.pressPosition).normalized;
        SetNextCard();
    }

    public void OnPointerClick(PointerEventData eventData){
        if(enableFlip || enableSlide) return;
        FlipCard();
    }

    public void ReciveEvent(){
        //Target for the events
    }


/// FLIP CARD
    private void FlipCard(){
        enableFlip = true;
    }
    private void FlipCardAnimation(){
        if(transform.rotation == new Quaternion(0,0,0,1)) changedFlipDirection = true;
        if(transform.rotation == new Quaternion(-0.707106829f,0,0,0.707106829f)) changedFlipDirection = true;

        if(changedFlipDirection){
            flipDirection = !flipDirection;
            flipCounter++;
            targetRotation = flipDirection ? new Quaternion(-0.707106829f,0,0,0.707106829f) : new Quaternion(0,0,0,1);
            changedFlipDirection = false;
            
            if(flipCounter == 2 ){
                FlipCardText();
            } 

            if(flipCounter == 3 ){
                ResetFlipBool();
                return;
            } 
        }
        if(flipCounter == 1 || flipCounter == 2)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, flipSpeed * Time.deltaTime);
    }
    private void ResetFlipBool(){
        flipCounter = 0;
        changedFlipDirection = false;
        flipDirection = false;
        enableFlip = false;
    }
    private void FlipCardText(){

        if(isFacingFront){
            //TODO: No pasar verbos sino caras estandarizadas
            isFacingFront = false;
            title.text = verbList[0].translation + ":";
            word.text = verbList[cardIndex].translation;
            topicToGuess.text = verbList[0].infiniv + "?";
        }
        else{
            isFacingFront = true;
            title.text = verbList[0].infiniv + ":";
            word.text = verbList[cardIndex].infiniv;
            topicToGuess.text = verbList[0].translation + "?";
        }

    }

/// NEXT CARD
    private void SetNextCard(){
        oldCanvas = Instantiate(gameObject);
        oldCanvasRenderer = oldCanvas.GetComponent<Renderer>();
        Destroy(oldCanvas.GetComponent<CardManager>());
        enableSlide = true;

        GetNextIndex();
        isFacingFront = true;
        title.text = verbList[0].infiniv;
        word.text = verbList[cardIndex].infiniv;
        topicToGuess.text = verbList[0].translation + "?";
    }
    private void GetNextIndex(){
        cardIndex++;
        if (cardIndex == 0) cardIndex = 1;
        if (cardIndex >= verbList.Count) cardIndex = 1;
    }
    private void SlideCardAnimation(){

        oldCanvas.transform.position +=  targetDirection * slideSpeed * Time.deltaTime; //Vector3.MoveTowards(oldcanvas.transform.position, targetPosition , slideSpeed * Time.deltaTime);

        if(oldCanvas.transform.position.magnitude > 0.25 && !oldCanvasRenderer.isVisible){
            Destroy(oldCanvas);
            enableSlide = false;
        }
    }
}
