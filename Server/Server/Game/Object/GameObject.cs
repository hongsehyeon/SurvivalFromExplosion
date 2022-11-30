using Google.Protobuf.Protocol;
using System;

namespace Server.Game
{
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y) { this.x = x; this.y = y; }

        public static Vector2Int up { get { return new Vector2Int(0, 1); } }
        public static Vector2Int down { get { return new Vector2Int(0, -1); } }
        public static Vector2Int left { get { return new Vector2Int(-1, 0); } }
        public static Vector2Int right { get { return new Vector2Int(1, 0); } }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        public float magnitude { get { return (float)Math.Sqrt(sqrMagnitude); } }
        public int sqrMagnitude { get { return (x * x + y * y); } }
        public int cellDistFromZero { get { return Math.Abs(x) + Math.Abs(y); } }
    }

    public class GameObject
	{
		public bool IsDead { get; set; }

		public int Id
		{
			get { return Info.ObjectId; }
			set { Info.ObjectId = value; }
		}

		public GameRoom Room { get; set; }

		public ObjectInfo Info { get; set; } = new ObjectInfo();
		public PositionInfo PosInfo { get; private set; } = new PositionInfo();

		public GameObject()
		{
			Info.PosInfo = PosInfo;
		}

		public virtual void Update()
		{
			
		}

		public virtual void OnDead()
		{
			if (Room == null)
				return;

			S_Die diePacket = new S_Die();
			diePacket.ObjectId = Id;
			Room.Broadcast(diePacket);

			PosInfo.PosX = 0;
			PosInfo.PosY = 0;

			IsDead = true;
		}
	}
}
