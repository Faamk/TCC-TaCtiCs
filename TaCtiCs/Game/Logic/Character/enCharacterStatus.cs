using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Character {
	public enum enCharacterStatus {
			Slow = 1, Dazed = 2, Dizzy = 3
	}
	public interface IStatus {
		enCharacterStatus Status {
			get;
		}
		int Duration {
			get;
			set;
		}
	}
}