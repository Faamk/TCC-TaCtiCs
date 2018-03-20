using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.View.Player {
	public class LocalPlayer: IUiPlayer {
		public int Priority {
			get;
			set;
		}
		public ICharacter Character {
			get;
			set;
		}
		public enBattleResult BattleResult {
			get;
			set;
		}
		public IList < GameState > States {
			get;
			set;
		} = new List < GameState > ();
		public bool IsGettingAnAction {
			get;
			set;
		}
		internal bool HasAttacked {
			get;
			set;
		}
		internal bool HasMoved {
			get;
			set;
		}
		private int actionCount;
		private PlayerAction nextAction;
		internal PlayerAction NextAction {
			get {
				return this.nextAction;
			}
			set {
				if (value != null) {
					actionCount++;
					if (actionCount == 2 || value.Type == enActionType.Skip) {
						this.HasAttacked = false;
						this.HasMoved = false;
						actionCount = 0;
					}
					this.IsGettingAnAction = false;
					this.nextAction = value;
				}
			}
		}
		public PlayerAction GetAction() {
			this.IsGettingAnAction = true;
			while (IsGettingAnAction || this.NextAction == null) {
				Thread.Sleep(100);
			}
			PlayerAction nextAction = this.NextAction;
			this.NextAction = null;
			return nextAction;
		}
		internal bool CanExecuteAction(enActionType enActionType, Tile tile) {
			switch (enActionType) {
				case enActionType.Attack:
					return !this.HasAttacked && this.Character.CanAttack(tile);
				case enActionType.Special:
					return !this.HasAttacked && this.Character.CanUseSpecialAttackOn(tile);
				case enActionType.Move:
					return !this.HasMoved && this.Character.CanMoveTo(tile);
				case enActionType.Skip:
					return true;
				case enActionType.None:
					return true;
				default:
					return false;
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
	}
}