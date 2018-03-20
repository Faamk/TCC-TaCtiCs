using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.Logic {
	public class Tile {
		public Battlefield Field {
			get;
			set;
		}
		public IPlaceable Placeable {
			get;
			set;
		}
		public int X {
			get;
			set;
		}
		public int Y {
			get;
			set;
		}
		public Tile(int x, int y, Battlefield field = null)
		{
			this.X = x;
			this.Y = y;
			this.Field = field;
		}
		public ICharacter GetCharacter() {
			return this.Placeable as ICharacter;
		}
	}
}