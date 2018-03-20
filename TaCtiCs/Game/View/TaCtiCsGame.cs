using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using TaCtiCs.Game.AI.Simple;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.View.Player;
using TaCtiCs.Game.Logic.Character;
using System.Threading;
using TaCtiCs.Game.Logic.Action;
using TaCtiCs.Game.View.Controls;
namespace TaCtiCs.Game.View {
	public class TaCtiCsGame: Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		public enGameScreen Screen {
			get;
			set;
		}
		Dictionary < enGameScreen, Screen > gameScreens;
		public TaCtiCsGame(): base() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			gameScreens = new Dictionary <enGameScreen,Screen>();
		}
		public Battle Battle {
			get;
			set;
		}
		public IUiPlayer Player1 {
			get;
			set;
		}
		public IUiPlayer Player2 {
			get;
			set;
		}
		public Texture2D GrassTexture {
			get;
			set;
		}
		public Texture2D ThiefTexture {
			get;
			set;
		}
		public Texture2D WarriorTexture {
			get;
			set;
		}
		public Texture2D WizardTexture {
			get;
			set;
		}
		public Texture2D ThiefFaceTexture {
			get;
			set;
		}
		public Texture2D WarriorFaceTexture {
			get;
			set;
		}
		public Texture2D WizardFaceTexture {
			get;
			set;
		}
		public Texture2D ButtonNormalTexture {
			get;
			set;
		}
		public Texture2D ButtonSelectedTexture {
			get;
			set;
		}
		public Texture2D ButtonDisabledTexture {
			get;
			set;
		}
		public SpriteFont Font {
			get;
			set;
		}
		protected override void Initialize() {
			this.gameScreens.Clear();
			Screen = enGameScreen.ModeSelect;
			this.Player1 = new LocalPlayer();
			this.Player2 = new LocalPlayer();
			this.Battle = new Battle(this.Player1, this.Player2);
			this.gameScreens.Add(enGameScreen.ModeSelect, new ModeSelectScreen(this));
			this.gameScreens.Add(enGameScreen.SelectCharacter, new SelectCharacterScreen(this));
			this.gameScreens.Add(enGameScreen.Gameplay, new GameScreen(this));
			foreach(KeyValuePair < enGameScreen, Screen > gameScreen in gameScreens) {
				gameScreen.Value.Init();
			}
			base.Initialize();
		}
		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			this.GrassTexture = this.Content.Load < Texture2D > ("GrassTile");
			this.ThiefTexture = this.Content.Load < Texture2D > ("Thief");
			this.WarriorTexture = this.Content.Load < Texture2D > ("Warrior");
			this.WizardTexture = this.Content.Load <
				Texture2D > ("Wizard");
			this.ThiefFaceTexture = this.Content.Load < Texture2D > ("ThiefFace");
			this.WarriorFaceTexture = this.Content.Load < Texture2D > ("WarriorFace");
			this.WizardFaceTexture = this.Content.Load < Texture2D > ("WizardFace");
			this.ButtonNormalTexture = this.Content.Load < Texture2D > ("ButtonNormal");
			this.ButtonSelectedTexture = this.Content.Load < Texture2D > (" ButtonSelected");
			this.ButtonDisabledTexture = this.Content.Load < Texture2D > (" ButtonDisabled");
			this.Font = this.Content.Load < SpriteFont > ("SpriteFont1");
		}
		protected override void UnloadContent() {
			((GameScreen) this.gameScreens[enGameScreen.Gameplay]).AbortBattleThread();
		}
		protected override void Update(GameTime gameTime) {
			if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
			this.gameScreens[this.Screen].Update(gameTime);
			if (this.Screen == enGameScreen.105 Gameplay && this.Battle.IsOver) {
				if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
					this.Initialize();
				}
			}
			base.Update(gameTime);
		}
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();
			if (this.Screen == enGameScreen.Gameplay && this.Battle.IsOver) {
				if (this.Player1.BattleResult == enBattleResult.Win) {
					spriteBatch.DrawString(this.Font, "Player␣1␣Wins", new Vector2(250, 250), Color.Black);
				}
				if (this.Player2.BattleResult == enBattleResult.Win) {
					spriteBatch.DrawString(this.Font, "Player␣2␣Wins", new Vector2(250, 250), Color.Black);
				}
				spriteBatch.DrawString(this.Font, "Go␣ Again ? ␣[Press␣ Enter]" , new Vector2(250, 300) , Color .Black) ;
			} else {
				this.gameScreens[this.Screen].Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}