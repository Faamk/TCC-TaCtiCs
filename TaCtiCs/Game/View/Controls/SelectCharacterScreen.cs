using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaCtiCs.Game.AI.Simple;
using TaCtiCs.Game.View.Player;
using TaCtiCs.Game.Logic.Character;
namespace TaCtiCs.Game.View.Controls {
	public class SelectCharacterScreen: Screen {
		private
			const int FaceSize = 96;
		public Button Player1ThiefButton {
			get;
			set;
		}
		public Button Player1WarriorButton {
			get;
			set;
		}
		public Button Player1WizardButton {
			get;
			set;
		}
		public Button Player2ThiefButton {
			get;
			set;
		}
		public Button Player2WarriorButton {
			get;
			set;
		}
		public Button Player2WizardButton {
			get;
			set;
		}
		public Button NextButton {
			get;
			set;
		}
		public SelectCharacterScreen(TaCtiCsGame game): base(game) {}
		public override void Init()
		 {
			Player1ThiefButton = new Button((() => this.Game.ThiefFaceTexture), null, null, (() => this.Game.Font), new Point(50, 50), string.Empty, 96, 96);
			Player1WarriorButton = new Button((() => this.Game.WarriorFaceTexture), null, null, (() => this.Game.Font), new Point(50, 150), string.Empty, 96, 96);
			Player1WizardButton = new Button((() => this.Game.WizardFaceTexture), null, null, (() => this.Game.Font), new Point(50, 250), string.Empty, 96, 96);
			Player2ThiefButton = new Button((() => this.Game.ThiefFaceTexture), null, null, (() => this.Game.Font), new Point(600, 50), string.Empty, 96, 96);
			Player2WarriorButton = new Button((() => this.Game.WarriorFaceTexture), null, null, (() => this.Game.Font), new Point(600, 150), string.Empty, 96, 96);
			Player2WizardButton = new Button((() => this.Game.WizardFaceTexture), null, null, (() => this.Game.Font), new Point(600, 250), string.Empty, 96, 96);
			NextButton = new Button(() => this.Game.ButtonNormalTexture, () => this.Game.ButtonSelectedTexture, () => this.Game.ButtonDisabledTexture, () => this.Game.Font, new Point(650, 400), "Next ");
		}
			public override void Update(GameTime gameTime) {
			bool arePlayersSelected = this.Game.Player1.Character != null && this.Game.Player2.Character != null;
			this.Player1ThiefButton.Update(gameTime);
			this.Player1WarriorButton.Update(gameTime);
			this.Player1WizardButton.Update(gameTime);
			this.Player2ThiefButton.Update(gameTime);
			this.Player2WarriorButton.Update(gameTime);
			this.Player2WizardButton.Update(gameTime);
			this.NextButton.Update(gameTime, !arePlayersSelected);
			this.HandleNextClick(arePlayersSelected);
			this.HandleCharButtonClicked(this.Player1ThiefButton, this.Game.Player1, new Thief(), this.Game.ThiefTexture);
			this.HandleCharButtonClicked(this.Player2ThiefButton, this.Game.Player2, new Thief(), this.Game.ThiefTexture);
			this.HandleCharButtonClicked(this.Player1WizardButton, this.Game.Player1, new Wizard(), this.Game.WizardTexture);
			this.HandleCharButtonClicked(this.Player2WizardButton, this.Game.Player2, new Wizard(), this.Game.WizardTexture);
			this.HandleCharButtonClicked(this.Player1WarriorButton, this.Game.Player1, new Warrior(), this.Game.WarriorTexture);
			this.HandleCharButtonClicked(this.Player2WarriorButton, this.Game.Player2, new Warrior(), this.Game.WarriorTexture);
		}
		private void HandleNextClick(bool arePlayersSelected) {
			if (this.NextButton.WasClicked) {
				this.NextButton.WasClicked = false;
				if (arePlayersSelected) {
					this.Game.Screen = enGameScreen.Gameplay;
					this.Game.Battle.Player1 = this.Game.Player1;
					this.Game.Battle.Player2 = this.Game.Player2;
				}
			}
		}
		private void HandleCharButtonClicked(Button button, IUiPlayer player, ICharacter character, Texture2D texture) {
			if (button.WasClicked) {
				player.Character = character;
				player.CharTexture = texture;
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			this.Player1ThiefButton.Draw(spriteBatch);
			this.Player1WarriorButton.Draw(spriteBatch);
			this.Player1WizardButton.Draw(spriteBatch);
			this.Player2ThiefButton.Draw(spriteBatch);
			this.Player2WarriorButton.Draw(spriteBatch);
			this.Player2WizardButton.Draw(spriteBatch);
			this.NextButton.Draw(spriteBatch);
		}
	}
}