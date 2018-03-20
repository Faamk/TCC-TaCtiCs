using System;
using System.Collections.Generic;
using System.Linq;
namespace TaCtiCs.Game.View
{
	public static class Program
	{
		[STAThread]
		static void Main ( )
		{
			using ( var game = new TaCtiCsGame ( ))
			{
				game.IsMouseVisible = true ;
				game.Run();
			}
		}
	}
}