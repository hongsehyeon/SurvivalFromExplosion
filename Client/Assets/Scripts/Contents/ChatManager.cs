using Google.Protobuf.Protocol;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public Transform ChatScrollContent;
    public ScrollRect ChatScrollViewRect;
    public GameObject ChatPrefab;
    public CustomInputField ChatInputField;
    public TMP_Text PlaceHolderText;

    public void SendChat()
    {
        C_Chat chatPacket = new C_Chat();
        chatPacket.Text = ChatInputField.inputText.text;
        ChatInputField.inputText.text = "";
        PlaceHolderText.text = "채팅을 입력하세요.";
        Managers.Network.Send(chatPacket);
    }

    public void CreateChatLog(string name, string text)
    {
        GameObject go = Instantiate(ChatPrefab, ChatScrollContent);
        if (go.TryGetComponent(out TMP_Text chatText))
            chatText.text = $"{name} : {text}";
        ChatScrollViewRect.verticalNormalizedPosition = 0.0f;
    }
}
