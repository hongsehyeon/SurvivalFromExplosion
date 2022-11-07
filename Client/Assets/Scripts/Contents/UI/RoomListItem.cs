using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("πÊ ¿‘¿Â");
        C_EnterGame enterGamePacket = new C_EnterGame();
        enterGamePacket.RoomId = RoomInfo.RoomId;

        Managers.Network.Send(enterGamePacket);
    }
}
