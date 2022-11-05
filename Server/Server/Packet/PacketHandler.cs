﻿using Google.Protobuf;
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

		lobby.Push(lobby.CreateRoom, addRoomPacket.RoomInfo);
	}

    public static void C_RefreshRoomListHandler(PacketSession session, IMessage packet)
	{
		// TODO : 룸 리스트 정보 돌려주기
		ClientSession clientSession = session as ClientSession;

		Lobby lobby = clientSession.Lobby;

		if (lobby == null)
			return;

		lobby.Push(lobby.RefreshRoomList, clientSession.SessionId);
	}

	public static void C_EnterGameHandler(PacketSession session, IMessage packet)
	{
		C_EnterGame enterGamePacket = packet as C_EnterGame;
		ClientSession clientSession = session as ClientSession;

		Lobby lobby = clientSession.Lobby;

		if (lobby == null)
			return;

		lobby.Push(lobby.TryEnterGame, enterGamePacket.RoomId, clientSession.SessionId);
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
}
