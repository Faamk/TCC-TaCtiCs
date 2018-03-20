using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Action {
	public class PlayerAction {
		public enActionType Type {
			get;
			set;
		}
		public Tile Target {
			get;
			set;
		}
	}
}