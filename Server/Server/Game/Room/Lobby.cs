using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class Lobby : JobSerializer
    {
        public static Lobby Instance { get; } = new Lobby();
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();

        public void Init()
        {
            _sessions.Clear();
        }

        public void EnterLobby(ClientSession session)
        {
            if (session == null)
                return;

            _sessions.Add(session.SessionId, session);
            session.Send(new S_EnterLobby());
        }

        public void RefreshRoomList(int sessionId)
        {
            S_RefreshRoomList roomListPacket = new S_RefreshRoomList();

            List<GameRoom> roomList = RoomManager.Instance.GetRoomList();
            foreach (GameRoom room in roomList)
                roomListPacket.Rooms.Add(room.RoomInfo);

            ClientSession session = FindSession(sessionId);
            session.Send(roomListPacket);
        }

        public void TryEnterGame(int roomId, int sessionId)
        {
            GameRoom room = RoomManager.Instance.Find(roomId);
            if (room == null)
                return;

            ClientSession session = FindSession(sessionId);

            session.MyPlayer = ObjectManager.Instance.Add();
            {
                session.MyPlayer.Info.Name = $"Player_{session.MyPlayer.Info.ObjectId}";
                session.MyPlayer.Info.PosInfo.PosX = 0;
                session.MyPlayer.Info.PosInfo.PosY = 0;
                session.MyPlayer.Session = session;
            }

            room.Push(room.EnterGame, session.MyPlayer);
        }

        public ClientSession FindSession(int sessionId)
        {
            ClientSession session = null;
            if (_sessions.TryGetValue(sessionId, out session))
                return session;

            return null;
        }

        public void BroadCast(IMessage packet)
        {
            foreach (ClientSession session in _sessions.Values)
            {
                session.Send(packet);
            }
        }
    }
}
