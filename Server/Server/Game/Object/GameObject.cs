using Google.Protobuf.Protocol;
using System;

namespace Server.Game
{
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
