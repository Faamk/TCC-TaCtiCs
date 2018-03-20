using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TaCtiCs.Game.AI.Simple;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.View.Player;
namespace TaCtiCs.Game.View.Controls {
	public class GameScreen: Screen {
		Thread BattleThread {
			get;
			set;
		}
		Battlefield Field {
			get;
			set;
		}
		Button MoveButton {
			get;
			set;
		}
			Button AttackButton {
			get;
			set;
		}
		Button SpecialButton {
			get;
			set;
		}
		Button SkipButton {
			get;
			set;
		}
		int PlayerNumber {
			get;
			set;
		}
		List < Tile > HighlitedTiles {
			get;
			set;
		}
		enActionType CurrentAction {
			get;
			set;
		}
		IUiPlayer CurrentPlayer {
			get {
				return this.Game.Player1.IsGettingAnAction ? this.Game.Player1 : this.Game.Player2.IsGettingAnAction ? this.Game.Player2 : null;
			}
		}
		public GameScreen(TaCtiCsGame game): base(game) {}
		public override void Init() {
			this.Field = null;
			this.HighlitedTiles = new List <Tile> ();
			BattleThread = new Thread(this.Game.Battle.RunGame);
			this.MoveButton = new Button((() => this.Game.ButtonNormalTexture), (() => this.Game.ButtonSelectedTexture), (() => this.Game.ButtonDisabledTexture), (() => this.Game.Font), new Point(550, 50), " Move");
			this.AttackButton = new Button((() => this.Game.ButtonNormalTexture), (() => this.Game.ButtonSelectedTexture), (() => this.Game.ButtonDisabledTexture), (() => this.Game.Font), new Point(550, 125), " Attack");
				this.SpecialButton = new Button((() => this.Game.ButtonNormalTexture), (() => this.Game.ButtonSelectedTexture), (() => this.Game.ButtonDisabledTexture), (() => this.Game.Font), new Point(550, 200), " Special");
			this.SkipButton = new Button((() => this.Game.ButtonNormalTexture), (() => this.Game.ButtonSelectedTexture), (() => this.Game.ButtonDisabledTexture), (() => this.Game.Font), new Point(550, 275), " Skip");
		}
		private void SetAiOpponents() {
			var aiWrapper = this.Game.Player2 as AIPlayerWrapper;
			if (aiWrapper != null) {
				((SimpleAiPlayer) aiWrapper.AiPlayer).Opponent = this.Game.Player1.Character;
				((SimpleAiPlayer) aiWrapper.AiPlayer).Character = this.Game.Player2.Character;
			}
			aiWrapper = this.Game.Player1 as AIPlayerWrapper;
			if (aiWrapper != null) {
				((SimpleAiPlayer) aiWrapper.AiPlayer).Opponent = this.Game.Player2.Character;
				((SimpleAiPlayer) aiWrapper.AiPlayer).Character = this.Game.Player1.Character;
			}
		}
		public override void Update(GameTime gameTime) {
			this.Field = this.Field ? ? Battlefield.GenerateBattleField(this.Game.Player1.Character, this.Game.Player2.Character);
			SetAiOpponents();
			if (BattleThread.ThreadState == ThreadState.Unstarted) {
				BattleThread.Start();
				Thread.Sleep(100);
			}
			if (!this.Game.Battle.IsOver) {
				this.SetPlayerNumber();
				if (this.CurrentPlayer != null) {
					IUiPlayer player = this.CurrentPlayer;
					LocalPlayer localPlayer = player as LocalPlayer;
					if (localPlayer != null) {
						this.UpdateButtons(gameTime, localPlayer);
						this.HandleButtonsClicks(localPlayer);
						if (this.CurrentAction != enActionType.None && this.HighlitedTiles.Any()) {
							Tile tile = this.GetTileClicked();
								if (tile != null && this.HighlitedTiles.Contains(tile)) {
								this.ExecutePlayerAction(localPlayer, tile);
							}
						}
					}
				}
			}
		}
		private void ExecutePlayerAction(LocalPlayer player, Tile tile) {
			if (player.CanExecuteAction(this.CurrentAction, tile)) {
				switch (this.CurrentAction) {
					case enActionType.Attack:
					case enActionType.Special:
						if (tile.GetCharacter() != null) {
							player.HasAttacked = true;
							player.NextAction = new PlayerAction {
								Target = tile, Type = CurrentAction
							};
						}
						break;
					case enActionType.Move:
						if (tile.GetCharacter() == null) {
							127
								player.HasMoved = true;
							player.NextAction = new PlayerAction {
								Target = tile, Type = CurrentAction
							};
						}
						break;
					default:
						break;
				}
			}
			this.CurrentAction = enActionType.None;
			this.HighlitedTiles.Clear();
		}
		private void HandleButtonsClicks(LocalPlayer player) {
			HandleButtonClicked(this.MoveButton, enActionType.Move);
			HandleButtonClicked(this.AttackButton, enActionType.Attack);
			HandleButtonClicked(this.SpecialButton, enActionType.Special);
			if (SkipButton.WasClicked) {
				player.NextAction = new PlayerAction {
					Type = enActionType.Skip
				};
				this.CurrentAction = enActionType.None;
				this.HighlitedTiles.Clear();
				SkipButton.WasClicked = false;
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
				Color specialHighlight = Color.Transparent;
			if (this.CurrentAction != enActionType.None) {
				switch (this.CurrentAction) {
					case enActionType.Move:
						specialHighlight = Color.DodgerBlue;
						break;
					case enActionType.Attack:
						specialHighlight = Color.Green;
						break;
					case enActionType.Special:
						specialHighlight = Color.Red;
						break;
					default:
						specialHighlight = Color.White;
						break;
				}
			}
			if (this.Field != null) {
				foreach(Tile tile in this.Field.Tiles) {
					Color color = this.HighlitedTiles.Contains(tile) ? specialHighlight : Color.White;
					spriteBatch.Draw(this.Game.GrassTexture, new Rectangle(15 + (tile.X∗ 50), 15 + (tile.Y∗ 50), 50, 50), color);
						if (tile.Placeable != null) {
						Texture2D texture = null;
						if (tile.Placeable == this.Game.Player1.Character) texture = this.Game.Player1.CharTexture;
						if (tile.Placeable == this.Game.Player2.Character) texture = this.Game.Player2.CharTexture;
						spriteBatch.Draw(texture ? ? this.Game.ThiefTexture, new Rectangle(24 + (tile.X∗ 50), 24 + (tile.Y∗ 50), 32, 32), Color.White);
					}
					spriteBatch.DrawString(this.Game.Font, "Player␣" + this.PlayerNumber + " ’s␣Turn", new Vector2(550, 10), Color.Black);
					spriteBatch.DrawString(this.Game.Font, string.Format(" Player␣1:␣Hp␣{0}␣Energy␣{1} ", this.Game.Player1.Character.CurrentHitPoints, this.Game.Player1.Character.CurrentEnergy), new Vector2(470, 400), Color.Black);
					spriteBatch.DrawString(this.Game.Font, string.Format(" Player␣2:␣Hp␣{0}␣Energy␣{1} ", this.Game.Player2.Character.CurrentHitPoints, this.Game.Player2.Character.CurrentEnergy),new Vector2(470, 450), Color.Black);
				}
			}
			this.MoveButton.Draw(spriteBatch);
			this.AttackButton.Draw(spriteBatch);
			this.SpecialButton.Draw(spriteBatch);
			this.SkipButton.Draw(spriteBatch);
		}
		internal void AbortBattleThread() {
			this.BattleThread.Abort();
		}
		private void SetPlayerNumber() {
			if (this.Game.Player1.IsGettingAnAction) {
				this.PlayerNumber = 1;
			}
			if (this.Game.Player2.IsGettingAnAction) {
				this.PlayerNumber = 2;
			}
		}
		private void UpdateButtons(GameTime gameTime, LocalPlayer player) {
			this.MoveButton.Update(gameTime, player.HasMoved);
			this.AttackButton.Update(gameTime, player.HasAttacked);
			this.SpecialButton.Update(gameTime, player.HasAttacked || player.Character.CurrentEnergy < player.Character.SpecialAttackCost);
			this.SkipButton.Update(gameTime);
		}
		private Tile GetTileClicked() {
			Tile tile = null;
			MouseState mouse = Mouse.GetState();
			if (mouse.LeftButton == ButtonState.Pressed) {
				if ((mouse.X < 15 + (9*50) && mouse.X >= 15) && (mouse.Y < 15 + (9*50) && mouse.Y >= 15)) {
					int x = mouse.X-15;
					x = x / 50;
					int y = mouse.Y-15;
					y = y / 50;
					tile = this.Field.Tiles[x, y];
				}
			}
			return tile;
		}
		private void HandleButtonClicked(Button button, enActionType buttonAction) {
			if (button.WasClicked) {
				button.WasClicked = false;
				if (this.CurrentAction == buttonAction) {
					this.CurrentAction = enActionType.None;
					this.HighlitedTiles.Clear();
				} else
					{
					this.CurrentAction = buttonAction;
					this.HighlitedTiles = this.Field.HighlightTiles(this.CurrentPlayer.Character, this.CurrentAction);
				}
			}
		}
	}
}