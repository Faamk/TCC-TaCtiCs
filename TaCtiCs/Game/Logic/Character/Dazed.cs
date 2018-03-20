using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TaCtiCs.Game.Logic.Character {
 public class Dazed: IStatus {
  public enCharacterStatus Status {
   get {
    return enCharacterStatus.Dazed;
   }
  }
  public int Duration {
   get;
   set;
  }
  public Dazed() {
   this.Duration = 2;
  }
 }
}