using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Action {
	public enum enAiActionType {
		None = enActionType.None, Skip = enActionType.Skip, Attack = enActionType.Attack, Special = enActionType.Special, KeepDistance = 2, Approach = 5, Retreat = 6
	}
	public enum enActionType {
		None = 0, Skip = 1, Attack = 3, Special = 4, Move = 5,
	}
	public enum enBattleResult {
		Win,
		Lose,
		Draw,
		None
	}
}