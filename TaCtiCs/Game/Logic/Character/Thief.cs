using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Character {
	public class Thief: ACharacter {
		public override enClass Name {
			get {
				return enClass.Thief;
			}
		}
		public override int AttackRange {
			get {
				return 1;
			}
		}
		public override int SpecialAttackRange {
			get {
				return 3;
			}
		}
		public override int SpecialAttackCost {
			get {
				return 35;
			}
		}
		public Thief(Tile tile = null) {
			this.CurrentEnergy = 100;
			this.CurrentHitPoints = 100;
			this.Attack = Constants.MedAttack;
			this.Defense = Constants.MedDefense;
			this.SpecialAttack = Constants.MedAttack;
			this.SpecialDefense = Constants.MedDefense;
			this.Speed = 3;
			this.Movement = 3;
			this.CurrentStatuses = new List  <IStatus> ();
			this.CurrentTile = tile;
		}
		public override void ExecuteAttack(Tile tile) {
			base.ExecuteAttack(tile, isAttackPhysical: true, isDefensePhysical: true);
		}
		public override void ExecuteSpecial(Tile tile) {
			if (this.CanUseSpecialAttackOn(tile)) {
				this.CurrentEnergy-= this.SpecialAttackCost;
				ICharacter opponent = tile.GetCharacter();
				double difference = (this.Attack*1.1)-opponent.Defense;
				opponent.CurrentHitPoints-= this.GetDamage(difference);
				opponent.CurrentStatuses.Add(new Dazed());
				opponent.CurrentEnergy-= 5;
			}
		}
	}
}