using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.Logic {
	public class Battle {
		public Logger Log {
			get;
			set;
		}
		public IPlayer Player1 {
			get;
			set;
		}
		public IPlayer Player2 {
			get;
			set;
		}
		public bool IsOver {
			get {
				return this.Player1.Character.CurrentHitPoints <= 0 || this.Player2.Character.CurrentHitPoints <= 0;
			}
		}
		public Battle(IPlayer player1, IPlayer player2) {
			this.Player1 = player1;
			this.Player2 = player2;
			this.Player1.BattleResult = enBattleResult.None;
			this.Player2.BattleResult = enBattleResult.None;
		}
		public void RunGame() {
				using(this.Log = new Logger()) {
				this.Log.AppendLineFormat("Battle :␣ {0}␣vs␣{1}␣START!", this.Player1.Character.Name, this.Player2.Character.Name);
				while (!this.IsOver) {
					IPlayer currentPlayer = this.FindNextPlayer();
					this.Log.AppendLineFormat("{0}s ␣turn .", currentPlayer.Character.Name);
					IPlayer enemyPlayer = this.Player1 == currentPlayer ? this.Player2 : this.Player1;
					GameState state = new GameState(currentPlayer, enemyPlayer.Character);
					enActionType firstAction = enActionType.None;
					while (firstAction == enActionType.None) {
						PlayerAction nextAction = currentPlayer.GetAction();
						firstAction = ProcessAction(currentPlayer, nextAction);
					}
					state.ActionPerformed = firstAction;
					state.CurrentDistance = currentPlayer.Character.CalculateDistance(enemyPlayer.Character);
					currentPlayer.States.Add(state);
						this.Log.AppendLineFormat("{0}␣ first ␣action :␣{1}", currentPlayer.Character.Name, firstAction);
					if (this.IsOver) break;
					if (firstAction != enActionType.Skip) {
						state = new GameState(currentPlayer, enemyPlayer.Character);
						enActionType secondAction = enActionType.None;
						while (secondAction == enActionType.None) {
							PlayerAction nextAction = currentPlayer.GetAction();
							secondAction = ProcessAction(currentPlayer, nextAction, firstAction);
						}
						state.ActionPerformed = secondAction;
						state.CurrentDistance = currentPlayer.Character.CalculateDistance(enemyPlayer.Character);
						currentPlayer.States.Add(state);
						this.Log.AppendLineFormat(" {0}␣ first ␣action :␣{1}", currentPlayer.Character.Name, secondAction);
						if (this.IsOver) break;
					}
					currentPlayer.Character.
					ProcessEndOfTurn();
					this.Log.AppendLineFormat("{0}␣ Ended␣the␣Turn!", currentPlayer.Character.Name);
					this.LogCharacterStatus(this.Player1);
					this.LogCharacterStatus(this.Player2);
					this.Log.AppendLine(" −−−−−−−−−−−−");
				}
				this.Log.AppendLine("Game␣Ended");
				if (this.Player1.Character.CurrentHitPoints <= 0 && this.Player2.Character.CurrentHitPoints <= 0) {
					this.Player1.BattleResult = enBattleResult.Draw;
					this.Player2.BattleResult = enBattleResult.Draw;
					this.Log.AppendLine("Result :␣ Draw!");
				} else if (this.Player1.Character.CurrentHitPoints <= 0) {
					this.Player1.BattleResult = enBattleResult.Lose;
					this.Player2.BattleResult = enBattleResult.Win;
					this.Log.AppendLineFormat(" Result :␣{0}␣Won!", this.Player2.Character.Name);
				} else {
					this.Player1.BattleResult = enBattleResult.Win;
					this.Player2.BattleResult =
					enBattleResult.Lose;
					this.Log.AppendLineFormat(" Result :␣{0}␣Won!", this.Player1.Character.Name);
				}
			}
			GameState.WriteGameStateFile(Player1.States, "p1");
			GameState.WriteGameStateFile(Player2.States, "p2");
		}
		private void LogCharacterStatus(IPlayer player) {
			this.Log.AppendLineFormat("{0}:␣HP:␣{1} ␣MP:␣{2}␣Status :␣{3}", player.Character.Name, player.Character.CurrentHitPoints, player.Character.CurrentEnergy, String.Join(" ,", player.Character.CurrentStatuses.Select(x => string.Format(" {0}({1})", x.Status, x.Duration))));
			this.Log.AppendLineFormat("{0}␣on␣ coordinates :␣{1}−{2}", player.Character.Name, player.Character.CurrentTile.X, player.Character.CurrentTile.Y);
		}
		public IPlayer FindNextPlayer() {
			if (this.Player1.Priority == 0 && this.Player2.Priority == 0) {
				if (this.Player1.Character.Speed >
				    this.Player2.Character.Speed) {
					this.Player1.Priority = 1;
				} else if (this.Player1.Character.Speed < this.Player2.Character.Speed) {
					this.Player2.Priority = 1;
				} else {
					if (FlipCoin() == 1) {
						this.Player1.Priority = 1;
					} else {
						this.Player2.Priority = 1;
					}
				}
			}
			while (this.Player1.Priority < 3 && this.Player2.Priority < 3) {
				this.Player1.Priority++;
				this.Player2.Priority++;
			}
			if (this.Player1.Priority == this.Player2.Priority) {
				if (FlipCoin() == 1) {
					return this.Player1;
				} else {
					return this.Player2;
				}
			} else if (this.Player1.Priority == 3)
			{
				return this.Player1;
			} else {
				return this.Player2;
			}
		}
		public virtual int FlipCoin() {
			Random randomGenerator = new Random();
			return randomGenerator.Next(10, 29) / 10;
		}
		public enActionType ProcessAction(IPlayer player, PlayerAction action, enActionType previousAction = enActionType.None) {
			switch (action.Type) {
				case enActionType.Attack:
					if (player.Character.CanAttack(action.Target) && (previousAction != enActionType.Attack && previousAction != enActionType.Special)) {
						player.Character.ExecuteAttack(action.Target);
					} else return enActionType.None;
					break;
				case enActionType.Special:
					if (player.Character.CanUseSpecialAttackOn(action.Target) && (
						previousAction != enActionType.Attack && previousAction != enActionType.Special)) {
						player.Character.ExecuteSpecial(action.Target);
					} else return enActionType.None;
					break;
				case enActionType.Move:
					if (player.Character.CanMoveTo(action.Target) && previousAction != enActionType.Move) {
						player.Character.MoveTo(action.Target);
					} else return enActionType.None;
					break;
				case enActionType.Skip:
					if (previousAction == enActionType.Move || previousAction == enActionType.None) {
						player.Priority = 2;
					}
					break;
				default:
					return enActionType.None;
			}
			if (action.Type == enActionType.Attack || action.Type == enActionType.Special) {
				player.Priority = 0;
			}
			return action.Type;
		}
	}
}