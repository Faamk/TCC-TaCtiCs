using Microsoft.Xna.Framework.Graphics;
using TaCtiCs.Game.Logic;
namespace TaCtiCs.Game.View.Player {
	public interface IUiPlayer: IPlayer {
		Texture2D CharTexture {
			get;
			set;
		}
		Texture2D CharFaceTexture {
			get;
			set;
		}
	}
}