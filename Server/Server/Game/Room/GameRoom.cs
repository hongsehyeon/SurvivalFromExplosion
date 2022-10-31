using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Server.Game
{
    public class GameRoom : JobSerializer
    {
        public int RoomId { get; set; }
        Dictionary<int, Player> _players = new Dictionary<int, Player>();
        bool _isExplode = false;

        public void Init()
        {
            
        }

        public void Update()
        {
            if (!_isExplode)
            {
                _isExplode = true;
                PushAfter(3000, Explode, 1);
            }

            Flush();
        }

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            Player player = gameObject as Player;
            if (player == null)
                return;

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

            // 다른 플레이어한테도 알려준다
            S_Move resMovePacket = new S_Move();
            resMovePacket.ObjectId = player.Info.ObjectId;
            resMovePacket.PosInfo = movePacket.PosInfo;

            Broadcast(resMovePacket);
        }

        public void Explode(int patternId)
        {
            S_Explode explodePacket = new S_Explode();
            explodePacket.PatternId = patternId;

            Broadcast(explodePacket);
            _isExplode = false;
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
