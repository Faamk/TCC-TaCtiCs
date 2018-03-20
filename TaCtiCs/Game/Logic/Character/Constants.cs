using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Character {
	public static class Constants
		{
		public
			const int LowAttack = 55;
		public
			const int MedAttack = 65;
		public
			const int HighAttack = 75;
		public
			const int LowDefense = 55;
		public
			const int MedDefense = 60;
		public
			const int HighDefense = 65;
	}
	public enum enClass {
		Wizard = 1, Warrior = 2, Thief = 3
	}
	public interface ICharacter: IPlaceable {
		Tile CurrentTile {
			get;
			set;
		}
		List < IStatus > CurrentStatuses {
			get;
		}
		int AttackRange {
			get;
		}
		int SpecialAttackRange {
			get;
		}
		int SpecialAttackCost {
			get;
		}
		int Attack {
			get;
			set;
		}
		int SpecialAttack {
			get;
			set;
		}
		int Defense {
			get;
			set;
		}
		int SpecialDefense {
			get;
			set;
		}
		int CurrentHitPoints {
			get;
			set;
		}
		int CurrentEnergy {
			get;
			set;
		}
		int Speed {
			get;
			set;
		}
		int Movement {
			get;
			set;
		}
		enClass Name {
			get;
		}
		void ProcessEndOfTurn();
		bool CanAttack(Tile tile);
		void ExecuteAttack(Tile tile);
		bool CanUseSpecialAttackOn(Tile tile);
		void ExecuteSpecial(Tile tile);
		bool CanMoveTo(Tile tile);
		void MoveTo(Tile tile);
	}
}