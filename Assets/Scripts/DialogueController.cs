using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshPro crowdText;

    string[] dialogueArray;
    string crowdFilePath;
    
    // Start is called before the first frame update
    void Start()
    {
        crowdFilePath = Application.dataPath + "/Scripts/crowd.txt";
    }

    // Update is called once per frame
    void Update()
    {
        //Every time I press the space key, read one line from text file
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueArray = File.ReadAllLines(crowdFilePath);
            crowdText.text = dialogueArray[Random.Range(0, dialogueArray.Length)];
        }
    }

    public void ReadFromFile()
    {
        dialogueArray = File.ReadAllLines(crowdFilePath);
        foreach (string line in dialogueArray)
        {
            Debug.Log(line);
        }
    }
}
