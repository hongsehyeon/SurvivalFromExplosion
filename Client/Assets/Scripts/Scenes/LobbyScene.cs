using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Protocol;
using Michsky.UI.ModernUIPack;

public class LobbyScene : MonoBehaviour
{
    public CustomInputField NewRoomNameField;
    public CustomDropdown MaxPlayerDropdown;

    public void OnClickMakeRoom()
    {
        RoomInfo roomInfo = new RoomInfo();
        roomInfo.RoomName = NewRoomNameField.inputText.text;
        roomInfo.MaxPlayer = MaxPlayerDropdown.selectedItemIndex + 4;

        C_CreateRoom createRoomPacket = new C_CreateRoom();
        createRoomPacket.RoomInfo = roomInfo;

        Managers.Network.Send(createRoomPacket);
    }

    public void OnClickRandomJoin()
    {

    }
}
