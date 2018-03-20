using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TaCtiCs.Game.AI.PlayerAi.Matlab;
using TaCtiCs.Game.AI.Simple;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.AI.PlayerAi {
	public class RBFuzzyAi: SimpleAiPlayer {
		private string aiFilePath;
		private Logger Log;
		RBFuzzyWrapper rbfuzzyWrapper;
		public RBFuzzyAi(string aiFilePath) {
			this.aiFilePath = aiFilePath;
			this.Log = new Logger($"AiLog−{ aiFilePath }.csv", true);
			States = new List < GameState > ();
			rbfuzzyWrapper = new RBFuzzyWrapper();
			rbfuzzyWrapper.TrainAi(this.aiFilePath);
			this.Log.InsertHeader("AiActionIndex , IsRBFuzzy , Character ,Action , TurnAction ,GameActionCount");
		}
		public override PlayerAction GetAction() {
			this.IsGettingAnAction = true;
			enAiActionType[] actions = this.rbfuzzyWrapper.GetActions(new GameState(this, this.Opponent));
			Thread.Sleep(800);
			foreach(var action in actions) {
				PlayerAction playerAction = null;
				switch (action)
					{
				case enAiActionType.Skip:
					playerAction = new PlayerAction {
						Type = enActionType.Skip
					};
					break;
				case enAiActionType.Attack:
				case enAiActionType.Special:
					playerAction = TryAttacking((enActionType) action);
					break;
				case enAiActionType.KeepDistance:
				case enAiActionType.Approach:
				case enAiActionType.Retreat:
					playerAction = TryMoving(action);
					break;
				default:
					break;
				}
				if (playerAction != null && playerAction.Type != enActionType.None) {
					this.Log.AppendLine($"{actions.ToList().IndexOf(action) +1},1,{this.Character .Name },{action},{ this.Actions},{this.States.Count},{string.Join(",",actions)}");
					return playerAction;
				}
			}
			var baseAction = base.GetAction();
			this.Log.AppendLine($"0,0,{ this . Character .Name},{baseAction .Type},{ this . Actions},{ this . States .Count},{ string . Join(", " ,␣actions)}");
			return baseAction;
		}
			private PlayerAction TryAttacking(enActionType action) {
			if (!this.HasAttacked) {
				if ((action == enActionType.Attack && this.Character.CanAttack(this.Opponent.CurrentTile)) || (action == enActionType.Special && this.Character.CanUseSpecialAttackOn(this.Opponent.CurrentTile))) {
					this.HasAttacked = true;
					return this.ReturnAction(action, this.Opponent.CurrentTile);
				}
			}
			return new PlayerAction {
				Type = enActionType.None
			};
		}
		private PlayerAction TryMoving(enAiActionType action) {
			int currentDistance = this.Character.CalculateDistance(this.Opponent);
			var field = this.Character.CurrentTile.Field;
			var moveableTiles = field.Tiles.OfType < Tile > ().Where(x => field.CalculateDistance(this.Character.CurrentTile, x) <= this.Character.Movement);
			if (!this.HasMoved) {
				if (action == enAiActionType.KeepDistance) {
					return this.KeepDistance(moveableTiles, field, currentDistance);
				}
				if (action == enAiActionType.Retreat) {
					return this.Retreat(moveableTiles, field, currentDistance);
				}
				if (action == enAiActionType.Approach) {
					return this.Approach(moveableTiles, field, currentDistance);
				}
			}
			return new PlayerAction {
				Type = enActionType.None
			};
		}
		private PlayerAction KeepDistance(IEnumerable < Tile > moveableTiles, Battlefield field, int currentDistance) {
			var targets = moveableTiles.Where(x => field.CalculateDistance(this.Character.CurrentTile, x) == currentDistance);
			if (targets.Any()) {
				this.HasMoved = true;
				return this.ReturnAction(enActionType.Move, targets.First());
			} else
			 {
				return this.ReturnAction(enActionType.Skip);
			}
		}
		private PlayerAction Retreat(IEnumerable < Tile > moveableTiles, Battlefield field, int currentDistance) {
			var targets = moveableTiles.Where(x => field.CalculateDistance(this.Character.CurrentTile, x) > currentDistance).OrderByDescending(x => field.CalculateDistance(this.Character.CurrentTile, x));
			if (targets.Any()) {
				this.HasMoved = true;
				return this.ReturnAction(enActionType.Move, targets.First());
			}
			return new PlayerAction {
				Type = enActionType.None
			};
		}
		private PlayerAction Approach(IEnumerable < Tile > moveableTiles, Battlefield field, int currentDistance) {
			var targets = moveableTiles.Where(x => field.CalculateDistance(this.Character.CurrentTile, x) < currentDistance && field.CalculateDistance(this.Character.CurrentTile, x) > 0).OrderByDescending(x => field.CalculateDistance(this.Character.CurrentTile, x));
			if (targets.Any()) {
				this.HasMoved = true;
				return this.ReturnAction(enActionType.Move, targets.First());
			}
			return new PlayerAction {
				Type = enActionType.None
			};
		}
	}
}