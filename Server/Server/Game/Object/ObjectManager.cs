using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{
    public class ObjectManager
    {
        public static ObjectManager Instance { get; } = new ObjectManager();

        object _lock = new object();
        Dictionary<int, Player> _players = new Dictionary<int, Player>();

        int _counter = 0;

        public Player Add()
        {
            Player player = new Player();

            lock (_lock)
            {
                player.Id = GenerateId();
                _players.Add(player.Id, player);
            }

            return player;
        }

        int GenerateId()
        {
            lock (_lock)
            {
                return _counter++;
            }
        }

        public bool Remove(int playerId)
        {
            lock (_lock)
            {
                return _players.Remove(playerId);
            }

            return false;
        }

        public Player Find(int playerId)
        {
            lock (_lock)
            {
                Player player = null;
                if (_players.TryGetValue(playerId, out player))
                    return player;
            }

            return null;
        }
    }
}
