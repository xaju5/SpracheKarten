
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private CSVController cSVController;
    private List<Verb> verbList;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI word;
    [SerializeField] private TextMeshProUGUI TopicToGuess;
    [SerializeField] private float flipSpeed;

    private int index;
    private bool enableFlip;
    private bool isFacingFront;

    void  Awake() {
        cSVController = new CSVController();
        verbList = cSVController.ReadCSV();
    }

    void Start()
    {
        isFacingFront = true;
        index = 1;
        title.text = verbList[0].infiniv;
        word.text = verbList[index].infiniv;
        TopicToGuess.text = verbList[0].translation + "?";
    }


    void Update() {
        if(Input.GetKeyDown(KeyCode.Return))
            GetNextIndex();
        if(Input.GetKeyDown(KeyCode.Space))
            FlipCard();
        FlipCardAnimation();
    }

    private void GetNextIndex(){
        index++;
        if (index == 0) index = 1;
        if (index >= verbList.Count) index = 1;

        isFacingFront = true;
        title.text = verbList[0].infiniv;
        word.text = verbList[index].infiniv;
        TopicToGuess.text = verbList[0].translation + "?";
    }

    private void FlipCard(){

        if(isFacingFront){
            //TODO: No pasar verbos sino caras estandarizadas
            isFacingFront = false;
            title.text = verbList[0].translation;
            word.text = verbList[index].translation;
            TopicToGuess.text = verbList[0].infiniv + "?";
        }
        else{
            isFacingFront = true;
            title.text = verbList[0].infiniv;
            word.text = verbList[index].infiniv;
            TopicToGuess.text = verbList[0].translation + "?";
        }

        //FlipCardAnimation();
    }


    private void FlipCardAnimation(){
        if(transform.rotation == new Quaternion(0,0,0,1)) enableFlip = true;
        if(transform.rotation == new Quaternion(-0.707106829f,0,0,0.707106829f)) enableFlip = false;

        Quaternion targetRotation = enableFlip ? new Quaternion(-0.707106829f,0,0,0.707106829f) : new Quaternion(0,0,0,1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, flipSpeed * Time.deltaTime);
    }
}
