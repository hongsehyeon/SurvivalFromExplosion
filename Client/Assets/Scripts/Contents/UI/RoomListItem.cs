using UnityEngine;
using Google.Protobuf.Protocol;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    public RoomInfo RoomInfo { get; set; }
    public TMP_Text RoomNameText;
    public TMP_Text PlayerCountText;

    public void UpdateUI()
    {
        RoomNameText.text = RoomInfo.RoomName;
        PlayerCountText.text = $"{RoomInfo.PlayerCount}/{RoomInfo.MaxPlayer}";
    }

    public void OnClickRoom()
    {
        C_EnterGame enterGamePacket = new C_EnterGame();
        enterGamePacket.RoomId = RoomInfo.RoomId + 1;
        Debug.Log($"{RoomInfo.RoomId}, {RoomInfo.RoomName}, {RoomInfo.MaxPlayer}");
        Managers.Network.Send(enterGamePacket);
    }
}
