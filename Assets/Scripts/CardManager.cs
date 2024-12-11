
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private CSVController cSVController;
    private List<Verb> verbList;
    [SerializeField] private TextMeshProUGUI word1;
    [SerializeField] private TextMeshProUGUI word2;
    [SerializeField] private TextMeshProUGUI word3;

    private int index;

    void  Awake() {
        cSVController = new CSVController();
        verbList = cSVController.ReadCSV();
    }

    void Start()
    {
        index = 0;
        word1.text = verbList[index].infiniv;
        word2.text = verbList[index].perfect;
        word3.text = verbList[index].translation;
    }


    void Update() {
        if(Input.GetKeyDown(KeyCode.Return))
            SetNextVerb();
    }

    private void SetNextVerb(){
        index++;
        if (index >= verbList.Count) index = 0;

        word1.text = verbList[index].infiniv;
        word2.text = verbList[index].perfect;
        word3.text = verbList[index].translation;
    }
}
