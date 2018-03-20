using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TaCtiCs.Game.Logic.Character {
	public class Slow: IStatus {
		public enCharacterStatus Status {
			get {
				return enCharacterStatus.Slow;
			}
		}
		public int Duration {
			get;
			set;
		}
		public Slow() {
			this.Duration = 2;
		}
	}
}