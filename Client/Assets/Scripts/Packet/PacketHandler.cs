using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using UnityEngine;
using UnityEngine.SceneManagement;

class PacketHandler
{
    private static ObjectInfo _spawnPlayer;

    public static void S_EnterLobbyHandler(PacketSession session, IMessage packet)
    {
        S_EnterLobby enterLobbyPacket = packet as S_EnterLobby;
        SceneManager.sceneLoaded += SpawnMyPlayer;
        SceneManager.LoadScene("Lobby");
    }

    public static void S_RefreshRoomListHandler(PacketSession session, IMessage packet)
    {
        S_RefreshRoomList roomListPacket = packet as S_RefreshRoomList;
        try
        {
            LobbyScene lobbyScene = GameObject.Find("LobbyScene").GetComponent<LobbyScene>();

            if (lobbyScene == null)
                return;

            lobbyScene.UpdateRoomList(roomListPacket);
        }
        catch
        {

        }
    }

    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;
        _spawnPlayer = enterGamePacket.Player;
        SceneManager.LoadScene("Game");
    }

    // Spawn
    static void SpawnMyPlayer(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (_spawnPlayer == null)
            return;

        if (scene.name == "Game")
        {
            Managers.Object.Add(_spawnPlayer, myPlayer: true);
        }
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

        PlayerController pc = go.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.OnDead();
        }
    }

    public static void S_ExplodeHandler(PacketSession session, IMessage packet)
    {
        S_Explode explodePacket = packet as S_Explode;

        Exploder exploder = Managers.Object.Exploder;
        if (exploder != null)
        {
            exploder.Explode(explodePacket.PatternId);
        }
    }
}
