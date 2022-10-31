using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game;
using ServerCore;

class PacketHandler
{
	public static void C_AddRoomHandler(PacketSession session, IMessage packet)
	{
		C_AddRoom addRoomPacket = packet as C_AddRoom;

		// TODO : 룸 생성 코드
	}

    public static void C_RefreshRoomHandler(PacketSession session, IMessage packet)
	{
		C_RefreshRoom refreshRoomPacket = packet as C_RefreshRoom;

		// TODO : 룸 리스트 정보 돌려주기
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
