using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Protocol;
using Michsky.UI.ModernUIPack;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    public CustomInputField NewRoomNameField;
    public CustomDropdown MaxPlayerDropdown;
    public GameObject ScrollContent;
    public GameObject RoomItemPrefab;

    private List<GameObject> roomItems = new List<GameObject>();

    public void OnClickMakeRoom()
    {
        RoomInfo roomInfo = new RoomInfo();
        roomInfo.RoomName = NewRoomNameField.inputText.text;
        roomInfo.MaxPlayer = MaxPlayerDropdown.selectedItemIndex + 4;

        // TODO : 방 이름 검사

        C_CreateRoom createRoomPacket = new C_CreateRoom();
        createRoomPacket.RoomInfo = roomInfo;
        createRoomPacket.Name = PlayerPrefs.GetString("PlayerName");

        Managers.Network.Send(createRoomPacket);
    }

    public void OnClickRefreshRoomList()
    {
        C_RefreshRoomList roomListPacket = new C_RefreshRoomList();
        Managers.Network.Send(roomListPacket);
    }

    public void UpdateRoomList(S_RefreshRoomList roomListPacket)
    {
        foreach (GameObject roomItem in roomItems)
        {
            roomItems.Remove(roomItem);
            Destroy(roomItem);
        }

        foreach (RoomInfo roomInfo in roomListPacket.Rooms)
        {
            GameObject roomItem = Instantiate(RoomItemPrefab, ScrollContent.transform);
            RoomListItem room = roomItem.GetComponent<RoomListItem>();
            room.RoomInfo = roomInfo;
            room.UpdateUI();
            roomItems.Add(roomItem);
        }
    }

    public void OnClickRank()
    {
        SceneManager.LoadScene("Rank");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
