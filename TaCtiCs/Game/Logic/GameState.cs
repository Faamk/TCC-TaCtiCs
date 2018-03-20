using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.Logic {
	public class GameState {
		private
			const double ActionCountMax = 30d;
		private
			const double DistanceMax = 16d;
		
		public static void WriteGameStateFile(IEnumerable < GameState > states, string filename) {}
		public GameState(IPlayer player, ICharacter enemy) {
			this.PlayerClass = player.Character.Name;
			this.PlayerHP = player.Character.CurrentHitPoints;
			this.PlayerEnergy = player.Character.
			CurrentEnergy;
			IStatus playerStatus = player.Character.CurrentStatuses.LastOrDefault();
			if (playerStatus != null) {
				this.PlayerStatus = playerStatus.Status;
				this.PlayerStatusDuration = playerStatus.Duration;
			}
			this.EnemyClass = enemy.Name;
			this.EnemyHP = enemy.CurrentHitPoints;
			this.EnemyEnergy = enemy.CurrentEnergy;
			IStatus enemyStatus = enemy.CurrentStatuses.LastOrDefault();
			if (enemyStatus != null) {
				this.EnemyStatus = enemyStatus.Status;
				this.EnemyStatusDuration = enemyStatus.Duration;
			}
			this.PreviousDistance = player.Character.CalculateDistance(enemy);
			this.ActionCount = player.States.Count;
		}
		public enClass PlayerClass {
			get;
			set;
		}
		public int PlayerHP {
			get;
			set;
		}
		public int PlayerEnergy {
			get;
			set;
		}
		public enCharacterStatus ? PlayerStatus {
			get;
			set;
		}
		public int PlayerStatusDuration {
			get;
			set;
		}
		public enClass EnemyClass {
			get;
			set;
		}
		public int EnemyHP {
			get;
			set;
		}
		public int EnemyEnergy {
			get;
			set;
		}
		public enCharacterStatus ? EnemyStatus {
			get;
			set;
		}
		public int EnemyStatusDuration {
			get;
			set;
		}
			public int ActionCount {
			get;
			set;
		}
		public int PreviousDistance {
			get;
			set;
		}
		public int CurrentDistance {
			get;
			set;
		}
		public enActionType ActionPerformed {
			get;
			set;
		}
		public string ToMatlabArray() {
			StringBuilder toString = new StringBuilder();
			toString.AppendFormat("[{0} ,", (int) this.PlayerClass);
			toString.AppendFormat("{0},", ((double) this.PlayerHP / 100d));
			toString.AppendFormat("{0},", ((double) this.PlayerEnergy / 100d));
			toString.AppendFormat("{0},", this.PlayerStatus.HasValue ? (int ? ) this.PlayerStatus.Value : 0);
			toString.AppendFormat("{0},", this.PlayerStatusDuration);
			toString.AppendFormat("{0},", (int) this.EnemyClass);
			toString.AppendFormat("{0},", ((double) this.EnemyHP / 100d));
			toString.AppendFormat("{0},", ((double) this.EnemyEnergy / 100d));
			toString.AppendFormat("{0},", this.EnemyStatus.HasValue ? (int ? ) this.EnemyStatus.Value : 0);
			toString.AppendFormat("{0},", this.EnemyStatusDuration);
			toString.AppendFormat("{0},", ((double) this.ActionCount / ActionCountMax));
			toString.AppendFormat("{0}]", ((double) this.PreviousDistance / DistanceMax));
			return toString.ToString();
		}
		public string ToMatlabArray2() {
			StringBuilder toString = new StringBuilder();
			toString.AppendFormat("[{0} ,", (int) this.PlayerClass);
			toString.AppendFormat("{0},", ((double) this.PlayerHP / 100d));
			toString.AppendFormat("{0},", ((double) this.PlayerEnergy / 100d));
			toString.AppendFormat("{0},", (int) this.EnemyClass);
			toString.AppendFormat("{0},", ((double) this.EnemyHP / 100d));
			toString.AppendFormat("{0},", ((double) this.EnemyEnergy / 100d));
			toString.AppendFormat("{0},", ((double) this.ActionCount / ActionCountMax));
			toString.AppendFormat("{0}]", ((double) this.PreviousDistance / DistanceMax));
			return toString.ToString();
		}
		public string ToMatlabArray3() {
			StringBuilder toString = new StringBuilder();
			toString.AppendFormat("[{0} ,", (int) this.PlayerClass);
			toString.AppendFormat("{0},", ((double) this.PlayerHP / 100d));
			toString.AppendFormat("{0},", (int) this.EnemyClass);
			toString.AppendFormat("{0},", ((double) this.EnemyHP / 100d));
			toString.AppendFormat("{0}]", ((double) this.PreviousDistance / DistanceMax));
			return toString.ToString();
		}
		public override string ToString() {
			StringBuilder toString = new StringBuilder();
			toString.AppendFormat("{0},", (int) this.PlayerClass);
			toString.AppendFormat("{0},", ((double) this.PlayerHP / 100d));
			toString.AppendFormat("{0},", ((double) this.PlayerEnergy / 100d));
			toString.AppendFormat("{0},", this.PlayerStatus.HasValue ? (int ? ) this.PlayerStatus.Value : 0);
			toString.AppendFormat("{0},", this.PlayerStatusDuration);
			toString.AppendFormat("{0},", (int) this.EnemyClass);
			toString.AppendFormat("{0},", ((double) this.EnemyHP / 100d));
			toString.AppendFormat("{0},", ((double) this.EnemyEnergy / 100d));
			toString.AppendFormat("{0},", this.EnemyStatus.HasValue ? (int ? ) this.EnemyStatus.Value : 0);
			toString.AppendFormat("{0},", this.EnemyStatusDuration);
			toString.AppendFormat("{0},", ((double) this.ActionCount / ActionCountMax));
			toString.AppendFormat("{0},", ((double) this.PreviousDistance / DistanceMax));
			if (this.ActionPerformed == enActionType.Move) {
				toString.AppendFormat("{0}", (int)(this.PreviousDistance > this.CurrentDistance ? enAiActionType.Approach : this.PreviousDistance < this.CurrentDistance ? enAiActionType.Retreat : enAiActionType.KeepDistance));
			} else {
				toString.AppendFormat("{0}", (int) this.ActionPerformed);
			}
			return toString.ToString();
		}
	}
}