using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

public class OpenAIController : MonoBehaviour
{
    [SerializeField] private Button sendBtn;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text textField;
    
    private OpenAIAPI api;
    private List<ChatMessage> messages;
    private void Start()
    {
        api = new OpenAIAPI(new APIAuthentication("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"));
        StartConversation();
        sendBtn.onClick.AddListener(() => GetResponse());
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage>
        {
            new ChatMessage(ChatMessageRole.System, "you can only answer with [yes, no].")
        };
        inputField.text = string.Empty;
        textField.text = "입력하세요.";
    }
    
    private async void GetResponse()
    {
        if (inputField.text.Length < 1) return;

        // off the button
        sendBtn.enabled = false;
        
        // add the message
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        // 100letter restructuib
        // if (userMessage.Content.Length > 100)
        // {
        //     userMessage.Content = userMessage.Content.Substring(0, 100);
        // }
        
        messages.Add(userMessage);
        textField.text = $"You: {userMessage.Content}";
        inputField.text = string.Empty;

        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 50,
            Messages = messages
        });
        
        // receive the msg
        ChatMessage responseMessage = new ChatMessage(chatResult.Choices[0].Message.Role, chatResult.Choices[0].Message.Content);
        Debug.Log($"{responseMessage.Role}: {responseMessage.Content}");
        
        messages.Add(responseMessage);
        
        textField.text = $"You: {userMessage.Content}\n\nGuard: {responseMessage.Content}";
        
        sendBtn.enabled = true;
    }
}
