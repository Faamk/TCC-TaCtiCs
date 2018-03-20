using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Character {
	public class Wizard: ACharacter {
		public override enClass Name {
			get {
				return enClass.Wizard;
			}
		}
		public override int AttackRange {
			get {
				return 3;
			}
		}
		public override int SpecialAttackRange {
			get {
				return 2;
			}
		}
		public override int SpecialAttackCost {
			get {
				return 45;
			}
		}
		public Wizard(Tile tile = null) {
			this.CurrentEnergy = 100;
			this.CurrentHitPoints = 100;
			this.Attack = Constants.LowAttack;
			this.Defense = Constants.LowDefense;
			this.SpecialAttack = Constants.HighAttack;
			this.SpecialDefense = Constants.HighDefense;
			this.Speed = 2;
			this.Movement = 2;
			this.CurrentStatuses = new List < IStatus > ();
			this.CurrentTile = tile;
		}
		public override void ExecuteAttack(Tile tile) {
			base.ExecuteAttack(tile,isAttackPhysical: false, isDefensePhysical: false);
		}
		public override void ExecuteSpecial(Tile tile) {
			if (this.CanUseSpecialAttackOn(tile)) {
				this.CurrentEnergy-= this.SpecialAttackCost;
				ICharacter opponent = tile.GetCharacter();
				double difference = (this.SpecialAttack*1.18)-opponent.SpecialDefense;
				opponent.CurrentHitPoints-= this.GetDamage(difference);
				opponent.CurrentStatuses.Add(new Dizzy());
				int xDifference = Math.Abs(this.CurrentTile.X-opponent.CurrentTile.X);
				int yDifference = Math.Abs(this.CurrentTile.Y-opponent.CurrentTile.Y);
				if (xDifference > yDifference) {
					if (this.CurrentTile.X > opponent.CurrentTile.X) {
						while (opponent.CurrentTile.X > 0 && this.CheckDistanceAndTarget(opponent.CurrentTile, this.SpecialAttackRange)) {
							opponent.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X-1, opponent.CurrentTile.Y]);
						}
					} else {
						while (opponent.CurrentTile.X < Battlefield.GridSize-1 && this.CheckDistanceAndTarget(opponent.CurrentTile, this.SpecialAttackRange)) {
							opponent.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X + 1, opponent.CurrentTile.Y]);
						}
					}
				} else {
					if (this.CurrentTile.Y > opponent.CurrentTile.Y) {
						while (opponent.CurrentTile.Y > 0 && this.CheckDistanceAndTarget(opponent.CurrentTile, this.SpecialAttackRange)) {
							opponent.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X, opponent.CurrentTile.Y-1]);
						}
					} else {
						while (opponent.CurrentTile.Y < Battlefield.GridSize-1 && this.CheckDistanceAndTarget(opponent.CurrentTile, this.SpecialAttackRange)) {
							opponent.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X, opponent.CurrentTile.Y + 1]);
						}
					}
				}
			}
		}
	}
}