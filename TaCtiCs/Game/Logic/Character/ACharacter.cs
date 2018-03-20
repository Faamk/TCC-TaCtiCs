using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TaCtiCs.Game.Logic.Character {
 public abstract class ACharacter: ICharacter {
  private int attack;
  private int specialAttack;
  private int defense;
  private int movement;
  public abstract enClass Name {
   get;
  }
  public abstract int AttackRange {
   get;
  }
  public abstract int SpecialAttackCost {
   get;
  }
  public abstract int SpecialAttackRange {
   get;
  }
  public int CurrentEnergy {
   get;
   set;
  }
  public int CurrentHitPoints {
   get;
   set;
  }
  public List < IStatus > CurrentStatuses {
   get;
   protected set;
  }
  public Tile CurrentTile {
   get;
   set;
  }
  public int Attack {
   get {
    return this.attack-(this.CurrentStatuses.Any(x => x.Status == enCharacterStatus.Dizzy) ? 10 : 0);
   }
   set {
    this.attack = value;
   }
  }
  public int Defense {
   get {
    return this.defense-(this.CurrentStatuses.Any(x => x.Status == enCharacterStatus.Dazed) ? 9 : 0);
   }
   set {
    this.defense = value;
   }
  }
  public int Movement {
   get {
    return this.movement-(this.CurrentStatuses.Any(x => x.Status == enCharacterStatus.Slow) ? 1 : 0);
   }
   set {
    this.movement = value;
   }
  }
  public int SpecialAttack {get  {  
  		return this.specialAttack-(this.CurrentStatuses.Any(x => x.Status == enCharacterStatus.Dizzy) ? 10 : 0);
   }
   set {
    this.specialAttack = value;
   }
  }
  public int SpecialDefense {
   get;
   set;
  }
  public int Speed {
   get;
   set;
  }
  public abstract void ExecuteAttack(Tile tile);
  public abstract void ExecuteSpecial(Tile tile);
  public bool CanAttack(Tile tile) {
   return this.CheckDistanceAndTarget(tile, this.AttackRange);
  }
  public bool CanUseSpecialAttackOn(Tile tile) {
   return this.CheckDistanceAndTarget(tile, this.SpecialAttackRange) && this.CurrentEnergy >= this.SpecialAttackCost;
  }
  public bool CanMoveTo(Tile tile) {
   return this.CheckDistanceAndTileEmpty(tile, this.Movement);
  }
  public void MoveTo(Tile tile) {
   if (this.CanMoveTo(tile)) {
    this.MoveToTile(tile);
   }
  }

  public void ExecuteAttack(Tile tile, bool isAttackPhysical, bool isDefensePhysical) {
   if (this.CanAttack(tile)) {
    ICharacter opponent = tile.GetCharacter();
    int attack = isAttackPhysical ? this.Attack : this.SpecialAttack;
    int defense = isDefensePhysical ? opponent.Defense : opponent.SpecialDefense;
    int difference = attack-defense;
    opponent.CurrentHitPoints-= this.GetDamage(difference);
   }
  }
  public void ProcessEndOfTurn() {
   foreach(IStatus status in this.CurrentStatuses) {
    status.Duration--;
   }
   this.CurrentStatuses.RemoveAll(x => x.Duration == 0);
   this.CurrentEnergy = Math.Min(this.CurrentEnergy + 15, 100);
   if (this.CurrentStatuses.Any(x => x.Status == enCharacterStatus.Dazed)) {
    this.CurrentEnergy-= 5;
   }
  }
 }
}