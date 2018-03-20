using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.Logic {
	public class Battlefield {
		public
			const int GridSize = 9;
		public Tile[, ] Tiles {
			get;
			set;
		}
		internal Battlefield() {
			this.Tiles = new Tile[GridSize, GridSize];
			for (int indexX = 0; indexX < GridSize; indexX++) {
				for (int indexY = 0; indexY < GridSize; indexY++) {
					this.Tiles[indexX, indexY] = new Tile(indexX, indexY, this);
				}
			}
		}
		public virtual int CalculateDistance(Tile tile1, Tile tile2) {
			int distanceX = Math.Abs(tile1.X− tile2.X);
			int distanceY = Math.Abs(tile1.Y− 149 tile2.Y);
			double distance = Math.Sqrt((distanceX∗ distanceX) + (distanceY∗ distanceY));
			return Convert.ToInt32(Math.Ceiling(distance));
		}
		public static Battlefield GenerateBattleField(ICharacter player1Char, ICharacter player2Char) {
			var battleField = new Battlefield();
			player1Char.MoveToTile(battleField.Tiles[4, 1]);
			player2Char.MoveToTile(battleField.Tiles[4, 7]);
			return battleField;
		}
	}
}