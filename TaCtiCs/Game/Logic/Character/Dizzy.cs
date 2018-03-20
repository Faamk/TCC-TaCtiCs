using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TaCtiCs.Game.Logic.Character {
	public class Dizzy: IStatus
	{
		public enCharacterStatus Status {
			get {
				return enCharacterStatus.Dizzy;
			}
		}
		public int Duration {
			get;
			set;
		}
		public Dizzy() {
			this.Duration = 2;
		}
	}
}