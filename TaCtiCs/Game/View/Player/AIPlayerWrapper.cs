using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.View.Player {
	public class AIPlayerWrapper: IUiPlayer {
		internal IPlayer AiPlayer {
			get;
			set;
		}
		public bool IsGettingAnAction {
			get {
				return AiPlayer.IsGettingAnAction;
			}
			set {
				AiPlayer.IsGettingAnAction = value;
			}
		}
		public IList < GameState > States {
			get {
				return AiPlayer.States;
			}
			set {
				AiPlayer.States = value;
			}
		}
		public int Priority {
			get {
				return AiPlayer.Priority;
			}
			set {
				AiPlayer.Priority = value;
			}
		}
		public ICharacter Character {
			get {
				return AiPlayer.Character;
			}
			set {
				AiPlayer.Character = value;
			}
		}
		public PlayerAction GetAction() {
			return AiPlayer.GetAction();
		}
		public enBattleResult BattleResult {
			get {
				return AiPlayer.BattleResult;
			}
			set {
				AiPlayer.BattleResult = value;
			}
		}
		public Texture2D CharTexture {
			get;
			set;
		}
		public Texture2D CharFaceTexture {
			get;
			set;
		}
		public AIPlayerWrapper(IPlayer player) {
			this.AiPlayer = player;
		}
	}
}