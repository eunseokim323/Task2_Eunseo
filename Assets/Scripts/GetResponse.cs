using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

public class GetResponse : MonoBehaviour
{
    public GameObject input;
    public Button enterBtn;
    public TMP_InputField userText;
    public TextMeshPro teacherSay;
    
    private CameraManager cameraManager;
    // Class for accessing the API (external library)
    private OpenAIAPI api;
    // Preserve the dialogue from this period.
    private List<ChatMessage> messages;
    private GiveInfo giveInfo;
    
    // First assigned role prompt
    public string s = "Please ask me a random question out of the information I gave you. But don't give me the questions in the order I gave you. But you have to find the answer from the information given. And don't give me the answer. And if what I said is correct, tell me the answer under the same conditions, make me a new question, and if I'm wrong, tell me the answer. And make me a new question under the same conditions, and don't give me the answer.";
    private void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        giveInfo = FindObjectOfType<GiveInfo>();
        // Save the key to use the API using the constructor
        api = new OpenAIAPI(new APIAuthentication("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"));
        enterBtn.onClick.AddListener(GetChat);
    }
    
    // starting function 
    public void StartConversation()
    {
        // save role prompt
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, s)
        };
        userText.text = "Enter Text.";
        // The camera is pointed at the Teachr object.
        cameraManager.LookTeacher();
        // Send a repone and get a reply.
        SendResponse();
        //input.SetActive(true);
    }

    /// <summary>
    /// Send text in input field to API
    /// </summary>
    private void GetChat()
    {
        if (userText.text.Length < 1) return;

        //userText.text = "Enter Text.";
        
        // Create messages to send to List and API
        ChatMessage userMessage = new ChatMessage
        {
            // Generate messages for sending to the List and API.
            Role = ChatMessageRole.User,
            // Ask if the question in quotation marks is correct, and if this is the fourth question, stop asking.
            Content = '"' + userText.text + '"' + ", this is correct answer? if not correct answer tell me correct answer please, and give me new Question If Next Question is fourth you can stop make new question"
        };

        messages.Add(userMessage);
        input.SetActive(false);
        // Send Response 
        SendResponse();
    }

    private int cnt = 0;
    /// <summary>
    /// Asynchronous function for sending a response and receiving an answer.
    /// </summary>
    private async void SendResponse()
    {
        teacherSay.text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nWait for seconds..\n I Thinking Question";
        // If you responded to question number 4, then stop.
        if (cnt > 4)
        {
            input.SetActive(false);
            cameraManager.LookPlayer();
            return;
        }
        cnt++;
        // Send model, temperature, maximum token, and message to be sent through API -> Asynchronous processing until response comes
        ChatResult chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo, 
            Temperature = 0.8,
            MaxTokens = 100,
            Messages = messages
        });
        
        // Receiving a message and printing it to the scene
        ChatMessage responseMessage = new ChatMessage(chatResult.Choices[0].Message.Role, chatResult.Choices[0].Message.Content);
        messages.Add(responseMessage);
        StartCoroutine(TypeTextEffect(messages[^1].Content));
        

        Debug.Log(responseMessage.Content);
    }

    // Prints each character of the message string
    IEnumerator TypeTextEffect(string texts)
    {
        teacherSay.text = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < texts.Length; i++)
        {
            stringBuilder.Append(texts[i]);
            teacherSay.text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.03f);
        }
        input.SetActive(true);
        userText.text = "Enter Text.";
    }
}
