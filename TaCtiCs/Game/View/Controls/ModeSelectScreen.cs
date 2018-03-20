using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TaCtiCs.Game.AI.Simple;
using TaCtiCs.Game.View.Player;
using TaCtiCs.Game.AI.PlayerAi;
namespace TaCtiCs.Game.View.Controls {
	public class ModeSelectScreen: Screen {
		public Button VersusLocalPlayerButton {
			get;
			set;
		}
		public Button VersusAiButton {
			get;
			set;
		}
		public Button AiVersusAiButton {
			get;
			set;
		}
		public Button VersusPlayerAiButton {
			get;
			set;
		}
		public ModeSelectScreen(TaCtiCsGame game): base(game) {}
		public override void Init() {
			this.VersusLocalPlayerButton = new Button(() => this.Game.ButtonNormalTexture, () => this.Game.ButtonSelectedTexture, () => this.Game.ButtonDisabledTexture, () => this.Game.Font, new Point(50, 50), "Vs␣Player", 200);
			this.VersusAiButton = new Button(() => this.Game.ButtonNormalTexture, () => this.Game.ButtonSelectedTexture, () => this.Game.ButtonDisabledTexture, () => this.Game.Font, new Point(50, 150), "Vs␣ AI", 200);
			this.AiVersusAiButton = new Button(() => this.Game.ButtonNormalTexture, () => this.Game.ButtonSelectedTexture, () => this.Game.ButtonDisabledTexture, () =>this.Game.Font, new Point(50, 250), "AI␣Vs␣AI", 200);
			this.VersusPlayerAiButton = new Button(() => this.Game.ButtonNormalTexture, () => this.Game.ButtonSelectedTexture, () => this.Game.ButtonDisabledTexture, () => this.Game.Font, new Point(50, 350), "Vs␣Player␣AI", 200);
		}
		public override void Update(GameTime gameTime) {
			this.VersusLocalPlayerButton.Update(gameTime);
			this.VersusAiButton.Update(gameTime);
			this.AiVersusAiButton.Update(gameTime);
			this.VersusPlayerAiButton.Update(gameTime);
			if (this.VersusLocalPlayerButton.WasClicked) {
				this.Game.Player1 = new LocalPlayer();
				this.Game.Player2 = new LocalPlayer();
			}
			if (this.VersusAiButton.WasClicked) {
				this.Game.Player1 = new LocalPlayer();
				this.Game.Player2 = new AIPlayerWrapper(new SimpleAiPlayer());
			}
			if (this.AiVersusAiButton.WasClicked) {
				this.Game.Player1 = new	AIPlayerWrapper(new SimpleAiPlayer());
				this.Game.Player2 = new AIPlayerWrapper(new SimpleAiPlayer());
			}
			if (this.VersusPlayerAiButton.WasClicked) {
				this.Game.Player1 = new AIPlayerWrapper(new SimpleAiPlayer());
				this.Game.Player2 = new AIPlayerWrapper(new RBFuzzyAi(" p1Mage"));
			}
			if (this.VersusAiButton.WasClicked || this.VersusLocalPlayerButton.WasClicked || this.AiVersusAiButton.WasClicked || this.VersusPlayerAiButton.WasClicked) {
				this.Game.Screen = enGameScreen.SelectCharacter;
			}
		}
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			this.AiVersusAiButton.Draw(spriteBatch);
			this.VersusLocalPlayerButton.Draw(spriteBatch);
			this.VersusAiButton.Draw(spriteBatch);
			this.VersusPlayerAiButton.Draw(spriteBatch);
		}
	}
}