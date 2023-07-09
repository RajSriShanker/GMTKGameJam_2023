using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshPro crowdText;

    string[] dialogueArray;
    string crowdHitFilePath;
    string crowdDodgeFilePath;
    
    // Start is called before the first frame update
    void Start()
    { 
        crowdHitFilePath = Application.dataPath + "/Scripts/crowdHit.txt";
        crowdDodgeFilePath = Application.dataPath + "/Scripts/crowdDodge.txt";
    }

    // Update is called once per frame
    void Update()
    {
        //Every time I press the space key, read one line from text file
        if (Input.GetKeyDown(KeyCode.K))
        {
            CrowdHitDialogue();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            CrowdDodgeDialogue();
        }
    }

    public void ReadFromFile(string filePath)
    {
        dialogueArray = File.ReadAllLines(filePath);
        foreach (string line in dialogueArray)
        {
            Debug.Log(line);
        }
    }

    public void CrowdHitDialogue()
    {
        dialogueArray = File.ReadAllLines(crowdHitFilePath);
        crowdText.text = dialogueArray[Random.Range(0, dialogueArray.Length)];
    }

    public void CrowdDodgeDialogue()
    {
        dialogueArray = File.ReadAllLines(crowdDodgeFilePath);
        crowdText.text = dialogueArray[Random.Range(0, dialogueArray.Length)];
    }

}
