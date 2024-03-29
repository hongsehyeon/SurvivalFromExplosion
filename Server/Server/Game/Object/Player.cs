﻿using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
	public class Player : GameObject
	{
		public ClientSession Session { get; set; }

		public int Score { get; set; }

		public override void OnDead()
		{
			base.OnDead();
		}
	}
}
