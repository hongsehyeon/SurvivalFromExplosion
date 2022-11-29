using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace Server.Game
{
    public class GameRoom : JobSerializer
    {
        public int RoomId { get; set; }
        public RoomInfo RoomInfo { get; set; }
        Dictionary<int, Player> _players = new Dictionary<int, Player>();
        public bool IsGameStarted { get; set; }

        public int Round { get; private set; } = 0;
        bool isExploded = false;

        public void Init()
        {
            
        }

        public void Update()
        {

            Flush();
        }

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            Player player = gameObject as Player;
            if (player == null)
                return;

            RoomInfo.PlayerCount++;
            
            _players.Add(player.Id, player);
            player.Room = this;

            // 본인한테 정보 전송
            {
                S_EnterGame enterPacket = new S_EnterGame();
                enterPacket.Player = player.Info;
                player.Session.Send(enterPacket);
                
                S_Spawn spawnPacket = new S_Spawn();
                foreach (Player p in _players.Values)
                {
                    if (player != p)
                        spawnPacket.Objects.Add(p.Info);
                }

                player.Session.Send(spawnPacket);
            }

            // 타인한테 정보 전송
            {
                S_Spawn spawnPacket = new S_Spawn();
                spawnPacket.Objects.Add(player.Info);
                foreach (Player p in _players.Values)
                {
                    if (p.Id != player.Id)
                        p.Session.Send(spawnPacket);
                }
            }
        }

        public void LeaveGame(int playerId)
        {
            Player player = null;
            if (_players.Remove(playerId, out player) == false)
                return;

            RoomInfo.PlayerCount--;

            player.Room = null;

            // 본인한테 정보 전송
            {
                S_LeaveGame leavePacket = new S_LeaveGame();
                player.Session.Send(leavePacket);
            }

            // 타인한테 정보 전송
            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectIds.Add(playerId);
                foreach (Player p in _players.Values)
                {
                    if (p.Id != playerId)
                        p.Session.Send(despawnPacket);
                }
            }
        }

        public void HandleMove(Player player, C_Move movePacket)
        {
            if (player == null)
                return;

            // TODO : 검증

            player.Info.PosInfo = movePacket.PosInfo;

            // 다른 플레이어한테도 알려준다
            S_Move resMovePacket = new S_Move();
            resMovePacket.ObjectId = player.Info.ObjectId;
            resMovePacket.PosInfo = movePacket.PosInfo;

            Broadcast(resMovePacket);
        }

        public void Explode()
        {
            S_Explode explodePacket = new S_Explode();
            List<int> patterIds = MakePattern();

            foreach (int patternId in patterIds)
                explodePacket.PatternId.Add(patternId);

            Broadcast(explodePacket);

            if (Round++ < 10)
                PushAfter(20000, Explode);
        }

        List<int> MakePattern()
        {
            Random rand = new Random();
            List<int> patterns = new List<int>();
            for (int i = 0; i < rand.Next(4, 7); i++)
                patterns.Add(rand.Next(0, 7));
            return patterns;
        }

        public void HandleChat(Player player, C_Chat chatPacket)
        {
            if (player == null)
                return;

            S_Chat resChatPacket = new S_Chat();
            resChatPacket.ObjectId = player.Info.ObjectId;
            resChatPacket.Text = chatPacket.Text;

            Broadcast(resChatPacket);
        }

        public Player FindPlayer(Func<GameObject, bool> condition)
        {
            foreach (Player player in _players.Values)
            {
                if (condition.Invoke(player))
                    return player;
            }

            return null;
        }

        public void Broadcast(IMessage packet)
        {
            foreach (Player p in _players.Values)
            {
                p.Session.Send(packet);
            }
        }
    }
}
