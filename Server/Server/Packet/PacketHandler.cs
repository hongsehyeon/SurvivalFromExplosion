using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game;
using ServerCore;

class PacketHandler
{
	public static void C_CreateRoomHandler(PacketSession session, IMessage packet)
	{
		C_CreateRoom addRoomPacket = packet as C_CreateRoom;
		ClientSession clientSession = session as ClientSession;

		Lobby lobby = clientSession.Lobby;

		if (lobby == null)
			return;

		lobby.CreateRoom(addRoomPacket.RoomInfo, addRoomPacket.Name, clientSession.SessionId);
	}

    public static void C_RefreshRoomListHandler(PacketSession session, IMessage packet)
	{
		// TODO : 룸 리스트 정보 돌려주기
		ClientSession clientSession = session as ClientSession;

		Lobby lobby = clientSession.Lobby;

		if (lobby == null)
			return;

		lobby.RefreshRoomList(clientSession.SessionId);
	}

	public static void C_EnterGameHandler(PacketSession session, IMessage packet)
	{
		C_EnterGame enterGamePacket = packet as C_EnterGame;
		ClientSession clientSession = session as ClientSession;
		
		Lobby lobby = clientSession.Lobby;

		if (lobby == null)
			return;

		lobby.TryEnterGame(enterGamePacket.RoomId, enterGamePacket.Name, clientSession.SessionId);
	}

    public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.Push(room.HandleMove, player, movePacket);
	}

	public static void C_DieHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

		player.OnDead();
    }

    public static void C_ChatHandler(PacketSession session, IMessage packet)
	{
		C_Chat chatPacket = packet as C_Chat;
		ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

		room.Push(room.HandleChat, player, chatPacket);
    }

	public static void C_StartGameHandler(PacketSession session, IMessage packet)
	{
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

		room.IsGameStarted = true;
    }

	public static void C_AddRank(PacketSession session, IMessage packet)
	{

	}

	public static void C_AddRankHandler(PacketSession session, IMessage packet)
	{

	}

	public static void C_RefreshRankListHandler(PacketSession session, IMessage packet)
	{

	}
}
