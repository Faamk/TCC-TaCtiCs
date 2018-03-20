using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TaCtiCs.Game.View.Controls {
 public abstract class Screen {
  public bool IsOn {
   get;
   set;
  }
  protected TaCtiCsGame Game {
   get;
   set;
  }
  public Screen(TaCtiCsGame game) {
   this.Game = game;
  }
  public abstract void Init();
  public abstract void Update(GameTime gameTime);
  public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
 }
}