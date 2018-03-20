using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.AI.Simple {
	
	public class SimpleAiPlayer: IPlayer {
		public int Priority {
			get;
			set;
		}
		public ICharacter Character {
			get;
			set;
		}
		public ICharacter Opponent {
			get;
			set;
		}
		public enBattleResult BattleResult {
			get;
			set;
		}
		public bool IsGettingAnAction {
			get;
			set;
		}
		public IList < GameState > States {
			get;
			set;
		} = new List < GameState > ();
		internal bool HasAttacked {
			get;
			set;
		}
		internal bool HasMoved {
			get;
			set;
		}
		internal int Actions {
			get;
			set;
		}
		public virtual PlayerAction GetAction() {
			this.IsGettingAnAction = true;
			enActionType type = enActionType.Skip;
			Tile target = null;
			if (!this.HasAttacked) {
				type = this.TryAttacking();
				if (type != enActionType.Skip) {
					target = this.Opponent.CurrentTile;
					this.HasAttacked = true;
				}
			}
			if (type == enActionType.Skip && !this.HasMoved) {
				target = this.TryMoving();
				if (target != null) {
					type = enActionType.Move;
					this.HasMoved = true;
				}
			}
			Thread.Sleep(1000);
			return ReturnAction(type, target);
		}
		private enActionType TryAttacking() {
			enActionType type = enActionType.Skip;
			if (this.Character.CanUseSpecialAttackOn(Opponent.CurrentTile)) {
				type = enActionType.Special;
			} else if (this.Character.CanAttack(Opponent.CurrentTile)) {
				type = enActionType.Attack;
			}
			return type;
		}
		private Tile TryMoving() {
			Tile target = null;
			var field = this.Character.CurrentTile.Field;
			int longestRange = Math.Max(this.Character.AttackRange, (this.Character.CurrentEnergy >= this.Character.SpecialAttackCost ? this.Character.SpecialAttackRange : 0));
			int distance = field.CalculateDistance(this.Character.CurrentTile, this.Opponent.CurrentTile);
			if (distance != longestRange) {
				int movement = 0;
				movement = this.Character.Movement;
				if (distance <= longestRange + this.Character.Movement) {
					movement = Math.Min(Math.Abs(distance-longestRange), movement);
				}
				target = FindTile(movement, longestRange, field);
			}
			return target;
		}
		protected PlayerAction ReturnAction(enActionType type, Tile target = null) {
			this.Actions++;
			if (Actions == 2) {
				this.IsGettingAnAction = false;
				this.HasAttacked = false;
				this.HasMoved = false;
				this.Actions = 0;
			}
			return new PlayerAction {
				Target =target, Type = type
			};
		}
		private Tile FindTile(int movement, int range, Battlefield field) {
			var moveableTiles = field.Tiles.OfType < Tile > ().Where(x => field.CalculateDistance(this.Character.CurrentTile, x) == movement);
			var targeteableTiles = moveableTiles.Where(x => field.CalculateDistance(x, this.Opponent.CurrentTile) == range);
			if (targeteableTiles.Any()) {
				return targeteableTiles.Where(x => x.Placeable != this.Opponent).OrderByDescending(x => field.CalculateDistance(x, this.Opponent.CurrentTile)).First();
			} else // If we can ’t , we return te closest to the oponent { return moveableTiles .Where(x=> x. Placeable != this .Opponent) . OrderBy(x => field . CalculateDistance(x, this . Opponent. CurrentTile)) . First () ; } }
		}
	}
	}