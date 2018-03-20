using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TaCtiCs.Game.Logic.Character {
	public static class ICharacterExtension {
		public static bool CheckDistanceAndTarget(this ICharacter character, Tile tile, int range) {
			if (CheckDistance(character, tile, range)) {
				if (tile.Placeable as ICharacter != null) return true;
			}
			return false;
		}
		public static bool CheckDistanceAndTileEmpty(this ICharacter character, Tile tile, int range) {
			if (CheckDistance(character, tile, range)) {
				if (tile.Placeable == null) return true;
			}
			return false;
		}
			public static void MoveToTile(this ICharacter character, Tile tile) {
			if (character.CurrentTile != null) {
				character.CurrentTile.Placeable = null;
			}
			character.CurrentTile = tile;
			tile.Placeable = character;
		}
		public static int GetDamage(this ICharacter character, double difference) {
			difference = difference < 0 ? 1 : difference;
			return (int) Math.Ceiling(difference*1.25);
		}
		public static bool CheckDistance(this ICharacter character, Tile tile, int range) {
			int distance = character.CurrentTile.Field.CalculateDistance(character.CurrentTile, tile);
			return distance <= range;
		}
		public static int CalculateDistance(this ICharacter character, ICharacter enemy) {
			return character.CurrentTile.Field.CalculateDistance(character.CurrentTile, enemy.CurrentTile);
		}
	}
}