using Google.Protobuf;
using Google.Protobuf.Protocol;
using System.Linq;

namespace Server.Game
{
    public class Lobby
    {
        public static Lobby Instance { get; } = new Lobby();
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        Dictionary<string, int> _rankDic = new Dictionary<string, int>();

        public void Init()
        {
            _sessions.Clear();
        }

        public void EnterLobby(int sessionId)
        {
            ClientSession session = SessionManager.Instance.Find(sessionId);
            
            if (session == null)
                return;
            
            _sessions.Add(session.SessionId, session);

            S_EnterLobby enterLobbyPacket = new S_EnterLobby();
            session.Send(enterLobbyPacket);

            S_RefreshRoomList roomListPacket = MakeRoomListPacket();
            session.Send(roomListPacket);
        }

        public void CreateRoom(RoomInfo roomInfo, string playerName, int sessionId)
        {
            GameRoom newRoom = RoomManager.Instance.Add(roomInfo);
            Program.TickRoom(newRoom);
            newRoom.Push(newRoom.Init);

            S_RefreshRoomList roomListPacket = MakeRoomListPacket();
            BroadCast(roomListPacket);

            TryEnterGame(newRoom.RoomId, playerName, sessionId);
        }

        public void RefreshRoomList(int sessionId)
        {
            S_RefreshRoomList roomListPacket = MakeRoomListPacket();
            ClientSession session = FindSession(sessionId);

            if (session == null)
                return;

            session.Send(roomListPacket);
        }

        public void TryEnterGame(int roomId, string playerName, int sessionId)
        {
            GameRoom room = RoomManager.Instance.Find(roomId);
            
            if (room == null)
                return;

            if (room.RoomInfo.PlayerCount >= room.RoomInfo.MaxPlayer)
                return;

            ClientSession session = FindSession(sessionId);

            session.MyPlayer = ObjectManager.Instance.Add();
            {
                session.MyPlayer.Info.Name = playerName;
                session.MyPlayer.Info.PosInfo.PosX = 0;
                session.MyPlayer.Info.PosInfo.PosY = 0;
                session.MyPlayer.Session = session;
            }

            room.Push(room.EnterGame, session.MyPlayer);

            _sessions.Remove(sessionId);
        }

        public void AddRank(string name, int score)
        {
            _rankDic.Add(name, score);
            _rankDic = SortDictionary(_rankDic);
        }

        Dictionary<string, int> SortDictionary(Dictionary<string, int> dict)
        {
            var sortVar = from item in dict
                          orderby item.Value descending
                          select item;

            return sortVar.ToDictionary(x => x.Key, x => x.Value);
        }

        public ClientSession FindSession(int sessionId)
        {
            ClientSession session = null;
            _sessions.TryGetValue(sessionId, out session);
            return session;
        }

        public void BroadCast(IMessage packet)
        {
            foreach (ClientSession session in _sessions.Values)
            {
                session.Send(packet);
            }
        }

        private S_RefreshRoomList MakeRoomListPacket()
        {
            S_RefreshRoomList roomListPacket = new S_RefreshRoomList();

            List<GameRoom> roomList = RoomManager.Instance.GetRoomList();
            foreach (GameRoom room in roomList)
                roomListPacket.Rooms.Add(room.RoomInfo);

            return roomListPacket;
        }
    }
}
