using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TaCtiCs.Game.Logic;
using TaCtiCs.Game.Logic.Action;
namespace TaCtiCs.Game.AI.PlayerAi.Matlab {
	internal class RBFuzzyWrapper: IDisposable {
		MLApp.MLApp matlab = new MLApp.MLApp();
		internal void TrainAi(string filePath) {
			string rootPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),@"PlayerAi\Matlab\RBFuzzy");
			matlab.Execute($"cd␣ ’{rootPath}’");
			matlab.Execute($"dadosArquivo␣=␣ ’C:\\{ filePath }.csv ’");
			matlab.Execute("run");
		}
			internal enAiActionType[] GetActions(GameState state) {
			Random random = new Random();
			matlab.Execute($"aiResult␣=␣evalfis ({ state .ToMatlabArray()},FIS)");
			object aiResult = matlab.GetVariable(" aiResult", "base");
			double denormalized = Convert.ToDouble(aiResult)*Enum.GetValues(typeof(enAiActionType)).OfType < enAiActionType > ().Select(x => (int) x).Max();
			int round = (int) Math.Round(denormalized, MidpointRounding.AwayFromZero);
			int roundUp = (int) Math.Ceiling(denormalized);
			int roundDown = (int) Math.Floor(denormalized);
			return new [] {
				(enAiActionType)(round == roundUp ? roundUp : roundDown), (enAiActionType)(round == roundUp ? roundDown : roundUp)
			};
		}
		public void Dispose() {
			matlab.Quit();
		}
	}
}