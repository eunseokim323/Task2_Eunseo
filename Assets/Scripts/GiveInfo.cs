using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GiveInfo : MonoBehaviour
{
    [SerializeField] private GetResponse _getResponse;
    [SerializeField] private TextMeshPro infoText; // pet msg
    [SerializeField] private GameObject chatObj;
    [SerializeField] private GameObject textObj;
    [SerializeField] private Transform contents;
    [SerializeField] private TMP_Text uiInfoText; // scroll view msg

    private Dictionary<char, List<string>> roomInfo = new Dictionary<char, List<string>>();
    private Queue<string> infoQueue = new Queue<string>();
    public GameObject input;

    private GetResponse getResponse;
    private int gotCoins = 0;
    private bool isShowAllInfos = false;
    
    private void Awake()
    {
        ReadFiles();
        getResponse = FindObjectOfType<GetResponse>();
    }

    private void ReadFiles()
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/RoomInformation");
        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name.Contains("meta"))
                continue;

            roomInfo[file.Name[4]] = new List<string>();

            using (StreamReader reader = file.OpenText())
            {
                string s = string.Empty;
                while ((s = reader.ReadLine()) != null)
                {
                    Debug.Log(s);
                    // Separate the 5th letter of the file [A, B, C, D]
                    roomInfo[file.Name[4]].Add(s);
                }
            }
        }
    }

    private readonly char[] testAlphas = {'A', 'B', 'C', 'D'};
    private void Update()
    {
        //Test Code
        if (Input.GetKeyDown(KeyCode.F3) && infoQueue.Any())
        {
             chatObj.SetActive(!chatObj.activeSelf);
        }
        
        if (!infoQueue.Any())
        {
            if (gotCoins >= 4)
            {
                chatObj.SetActive(false);
            }
        }

        if (infoText.gameObject.activeSelf)
        {
            infoText.transform.LookAt(Camera.main.transform.position);
        }

        if (Input.GetKeyDown(KeyCode.F) && !isShowAllInfos)
        {
            ShowText();
        }
    }

    public void EnqueueInfo(char alphabet)
    {
        gotCoins++;
        foreach (string s in roomInfo[alphabet])
        {
            infoQueue.Enqueue(s);
        }
        if (infoText.text == string.Empty)
            ShowText();
    }

    private void ShowText()
    {
        if (!infoQueue.Any())
        {
            infoText.text = string.Empty;
            if (gotCoins >= 4)
            {
                isShowAllInfos = true;
                getResponse.StartConversation();
            }
            return;
        }
        string info = infoQueue.Dequeue();
        _getResponse.s += "\n'" + info + "'\n";
        if (info.Contains("Room"))
        {
            ShowText();
            return;
        }
        infoText.text = info;
        GameObject obj = Instantiate(textObj);
        obj.transform.parent = contents;
        obj.GetComponentInChildren<TMP_Text>().text = info;
        obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        // uiInfoText.text = info;

        Debug.Log(info);
        
        // infoText.gameObject.SetActive(true);
        // infoText.text = roomInfo[alphabet][Random.Range(0, roomInfo[alphabet].Count)];
        // infoList.Add(infoText.text);

        // If you eat the coin again before the speech bubble disappears
        // CancelInvoke(nameof(HideText));
        // Invoke(nameof(HideText), 15f);

    }
}
