using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessageData
{
    public string messageType;
    public bool isSent = false;
    public bool canBeSent = false;
    public string senderName;
    public string fullMessage;
}

[System.Serializable]
public class MessageList
{
    public List<MessageData> allMessages;
}