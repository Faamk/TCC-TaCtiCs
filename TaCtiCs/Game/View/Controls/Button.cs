using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace TaCtiCs.Game.View.Controls {
	public class Button {
		public enum enButtonState {
			Normal,
			Selected,
			Disabled
		}
		private Func < Texture2D > NormalTextureFunc {
			get;
			set;
		}
		private Func < Texture2D > SelectedTextureFunc {
			get;
			set;
		}
		private Func < Texture2D > DisabledTextureFunc {
			get;
			set;
		}
		private Func < SpriteFont > FontFunc {
			get;
			set;
		}
		public Texture2D NormalTexture {
			get {
				return this.NormalTextureFunc != null ? this.NormalTextureFunc.Invoke() : null;
			}
		}
		public Texture2D SelectedTexture {
			get {
				return this.SelectedTextureFunc != null ? this.SelectedTextureFunc.Invoke() : null;
			}
		}
		public Texture2D DisabledTexture {
			get {
				return this.DisabledTextureFunc != null ? this.DisabledTextureFunc.Invoke() : null;
			}
		}
		public SpriteFont Font {
			get {
				return this.FontFunc != null ? this.FontFunc.Invoke() : null;
			}
		}
		public Point Position {
			get;
			set;
		}
		public Rectangle Rectangle {
			get {
				return new Rectangle(this.Position.X, this.Position.Y, this.Width, this.Height);
			}
		}
		public string Text {
			get;
			set;
		}
		public bool WasClicked {
			get;
			set;
		}
		public enButtonState State {
			get;
			set;
		}
		public int Width {
			get;
			set;
		}
		public int Height {
			get;
			set;
		}
		public Button(Func < Texture2D > normalTexture, Func < Texture2D > selectedTexture, Func < Texture2D > disabledTexture, Func < SpriteFont > font, Point position, string text, int width = 100, int Height = 50) {
			this.State = enButtonState.Normal;
			this.NormalTextureFunc = normalTexture;
			this.DisabledTextureFunc = disabledTexture;
			this.SelectedTextureFunc = selectedTexture;
			this.FontFunc = font;
			this.Position = position;
			this.Text = text;
			this.Height = Height;
			this.Width = width;
		}
		public Button(Texture2D normalTexture, Texture2D selectedTexture, Texture2D disabledTexture, SpriteFont font, Point position, string text, int width = 100, int Height = 50): this((() => normalTexture), (() => selectedTexture), (() => disabledTexture), (() => font), position, text, width, Height) {}
		public void Update(GameTime gameTime, bool disable = false) {
			if (disable) {
				this.State = enButtonState.Disabled;
			} else if (this.State == enButtonState.Disabled) {
				this.State = enButtonState.Normal;
			}
			if (this.State != enButtonState.Disabled)
			 	{
				MouseState mouse = Mouse.GetState();
				Rectangle rectangle = this.Rectangle;
				if (this.State == enButtonState.Normal) {
					if (this.isMouseOver() && mouse.LeftButton == ButtonState.Pressed) {
						this.State = enButtonState.Selected;
					}
				} else {
					if (this.isMouseOver() && mouse.LeftButton == ButtonState.Released) {
						this.State = enButtonState.Normal;
						this.WasClicked = true;
					}
				}
				if (!this.isMouseOver()) {
					this.State = enButtonState.Normal;
				}
			}
		}
		private bool isMouseOver() {
			MouseState mouse = Mouse.GetState();
			Rectangle rectangle = this.Rectangle;
			return mouse.X >= rectangle.X && mouse.X <= rectangle.X + (rectangle.Width) && mouse.Y >= rectangle.Y && mouse.Y <= rectangle.Y + (rectangle.Height);
		}
		public void Draw(SpriteBatch spriteBatch) {
			switch (this.State) {
				case enButtonState.Normal:
					spriteBatch.Draw(this.NormalTexture, this.Rectangle, Color.White);
					break;
				case enButtonState.Selected:
					spriteBatch.Draw(this.SelectedTexture ?? this.NormalTexture, this.Rectangle, Color.Gray);
					break;
				case enButtonState.Disabled:
					spriteBatch.Draw(this.DisabledTexture ?? this.NormalTexture, this.Rectangle, Color.LightGray);
					break;
				default:
					break;
			}
			if (!string.IsNullOrWhiteSpace(this.Text)) {
				spriteBatch.DrawString(this.Font, this.Text, new Vector2(this.Position.X, this.Position.Y), Color.White);
			}
		}
	}
}