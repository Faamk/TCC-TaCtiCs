using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.View {
	public static class Extensions {
		public static List < Tile > HighlightTiles(this Battlefield field, ICharacter character, enActionType highlightType) {
			List < Tile > highLightedTiles = new List < Tile > ();
			int range = 0;
			switch (highlightType) {
				case enActionType.Move:
					range = character.Movement;
					107
						break;
				case enActionType.Attack:
					range = character.AttackRange;
					break;
				case enActionType.Special:
					range = character.SpecialAttackRange;
					break;
				default:
					break;
			}
			range++;
			FindTiles(character.CurrentTile, range, highLightedTiles, highlightType, true);
			return highLightedTiles;
		}
		private static void FindTiles(Tile position, int movementLeft, List < Tile > highlightedTiles, enActionType highlightType, bool firstTime = false) {
			if (movementLeft > 0) {
				if (!firstTime) {
					if (position.Placeable != null && position.Placeable as ICharacter != null && highlightType != enActionType.Move) {
						if (!highlightedTiles.Any(tile => tile.X == position.X && tile.Y == position.Y)) {
							highlightedTiles.Add(position);
						}
					} else if (position.Placeable == null) {
						if (!highlightedTiles.Any(tile => tile.X == position.X && tile.Y == position.Y)) {
							highlightedTiles.Add(position);
						}
					}
				}
				if (position.X < 8) FindTiles(position.Field.Tiles[position.X + 1, position.Y], movementLeft− 1, highlightedTiles, highlightType);
				if (position.X > 0) FindTiles(position.Field.Tiles[position.X− 1, position.Y], movementLeft− 1, highlightedTiles, highlightType);
				if (position.Y < 8) FindTiles(position.Field.Tiles[position.X, position.Y + 1], movementLeft− 1, highlightedTiles, highlightType);
				if (position.Y > 0) FindTiles(position.Field.Tiles[position.X, position.Y− 1], movementLeft− 1, highlightedTiles, highlightType);
			}
		}
	}
}