using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.Logic {
	public interface IPlayer {
		bool IsGettingAnAction {
			get;
			set;
		}
		int Priority {
			get;
			set;
		}
		ICharacter Character {
			get;
			set;
		}
		enBattleResult BattleResult {
			get;
			set;
		}
		IList < GameState > States {
			get;
			set;
		}
		PlayerAction GetAction();
	}
}