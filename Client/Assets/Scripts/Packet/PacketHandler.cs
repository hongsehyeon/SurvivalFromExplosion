using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class PacketHandler
{
    public static void S_EnterLobbyHandler(PacketSession session, IMessage packet)
    {
        S_EnterLobby enterLobbyPacket = packet as S_EnterLobby;
        SceneManager.LoadScene("Lobby");
    }

    public static void S_RefreshRoomListHandler(PacketSession session, IMessage packet)
    {
        S_RefreshRoomList roomListPacket = packet as S_RefreshRoomList;

        try
        {
            if (GameObject.Find("LobbyScene").TryGetComponent(out LobbyScene lobbyScene))
                lobbyScene.UpdateRoomList(roomListPacket);
        }
        catch
        {

        }
    }

    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;
        Managers.Object.Add(enterGamePacket.Player, true);
        SceneManager.LoadScene("Game");
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGameHandler = packet as S_LeaveGame;
        Managers.Object.Clear();
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        S_Spawn spawnPacket = packet as S_Spawn;
        foreach (ObjectInfo obj in spawnPacket.Objects)
            Managers.Object.Add(obj, myPlayer: false);
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        S_Despawn despawnPacket = packet as S_Despawn;
        foreach (int id in despawnPacket.ObjectIds)
        {
            Managers.Object.Remove(id);
        }
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move movePacket = packet as S_Move;

        GameObject go = Managers.Object.FindById(movePacket.ObjectId);
        if (go == null)
            return;

        if (Managers.Object.MyPlayer.Id == movePacket.ObjectId)
            return;

        PlayerController pc = go.GetComponent<PlayerController>();
        if (pc == null)
            return;

        pc.PosInfo = movePacket.PosInfo;
    }

    public static void S_DieHandler(PacketSession session, IMessage packet)
    {
        S_Die diePacket = packet as S_Die;

        GameObject go = Managers.Object.FindById(diePacket.ObjectId);
        if (go == null)
            return;

        if (go.TryGetComponent(out PlayerController pc))
            pc.OnDead();
    }

    public static void S_ExplodeHandler(PacketSession session, IMessage packet)
    {
        S_Explode explodePacket = packet as S_Explode;

        Exploder exploder = Managers.Object.Exploder;
        if (exploder != null)
            exploder.Explode(explodePacket.PatternId);
    }

    public static void S_ChatHandler(PacketSession session, IMessage packet)
    {
        S_Chat chatPacket = packet as S_Chat;

        GameObject go = Managers.Object.FindById(chatPacket.ObjectId);
        if (go == null)
            return;

        try
        {
            if (go.TryGetComponent(out PlayerController pc))
                if (GameObject.Find("ChatManager").TryGetComponent(out ChatManager chatManager))
                    chatManager.CreateChatLog(pc.Name, chatPacket.Text);
        }
        catch
        {

        }
    }

    public static void S_StartGameHandler(PacketSession session, IMessage packet)
    {
        try
        {
            if (GameObject.Find("GameScene").TryGetComponent(out GameScene gameScene))
                gameScene.GameStart();
        }
        catch
        {

        }
    }

    public static void S_EndGameHandler(PacketSession session, IMessage packet)
    {

    }

    public static void S_RefreshRankListHandler(PacketSession session, IMessage packet)
    {

    }
}
