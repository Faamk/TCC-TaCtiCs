using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Character {
	public class Warrior: ACharacter {
		public override enClass Name {
			get {
				return enClass.Warrior;
			}
		}
		public override int AttackRange {
			get {
				return 1;
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
		public Warrior(Tile tile = null) {
			this.CurrentEnergy = 100;
			this.CurrentHitPoints = 100;
			this.Attack = Constants.HighAttack;
			this.Defense = Constants.HighDefense;
			this.SpecialAttack = Constants.LowAttack;
			this.SpecialDefense = Constants.MedDefense;
			this.Speed = 1;
			this.Movement = 2;
			this.CurrentStatuses = new List < IStatus > ();
			this.CurrentTile = tile;
		}
		public override void ExecuteAttack(Tile tile) {
			base.ExecuteAttack(tile, isAttackPhysical: true, isDefensePhysical: true);
		}
		public override void ExecuteSpecial(Tile tile) {
			if (this.CanUseSpecialAttackOn(tile)) {
				this.CurrentEnergy-= this.SpecialAttackCost;
				ICharacter opponent = tile.GetCharacter();
				double difference = (this.Attack*1.155)-opponent.Defense;
				opponent.CurrentHitPoints-= this.GetDamage(difference);
				opponent.CurrentStatuses.Add(new Slow());
				int xDifference = Math.Abs(this.CurrentTile.X-opponent.CurrentTile.X);
				int yDifference = Math.Abs(this.CurrentTile.Y-opponent.CurrentTile.Y);
				if (xDifference > yDifference)
				{
					if (this.CurrentTile.X > opponent.CurrentTile.X) {
						this.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X + 1, opponent.CurrentTile.Y]);
					} else {
						this.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X-1, opponent.CurrentTile.Y]);
					}
				} else {
					if (this.CurrentTile.Y > opponent.CurrentTile.Y) {
						this.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X, opponent.CurrentTile.Y + 1]);
					} else {
						this.MoveToTile(this.CurrentTile.Field.Tiles[opponent.CurrentTile.X, opponent.CurrentTile.Y-1]);
					}
				}
			}
		}
	}
}