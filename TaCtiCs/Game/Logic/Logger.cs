using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
namespace TaCtiCs.Game.Logic {
	public class Logger: IDisposable {
		private StringBuilder log;
		private string filename;
		private bool liveWrite;
		public Logger(string filename = null, bool liveWrite = false) {
			this.filename = filename ? ? DateTime.Now.ToString("yyyy−MM −dd␣HHmmss") + DateTime.Now.Millisecond + ". txt";;
			this.liveWrite = liveWrite;
			this.log = new StringBuilder();
		}
		private string FullFilePath {
			get {
				return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), filename);
			}
		}
		public void AppendLineFormat(string text, params object[] parameters) {
			if (liveWrite) {
				using(StreamWriter writer = new StreamWriter(FullFilePath, true)) {
					writer.WriteLine(string.Format(text, parameters));
				}
			} else
				{
				this.log.AppendFormat(text, parameters);
				this.log.AppendLine();
			}
		}
		public void InsertHeader(string headerText) {
			if (!File.Exists(FullFilePath)) {
				AppendLine(headerText);
			}
		}
		public void AppendLine(string text = "") {
			if (liveWrite) {
				using(StreamWriter writer = new StreamWriter(FullFilePath, true)) {
					writer.WriteLine(text);
				}
			} else {
				this.log.AppendLine(text);
			}
		}
		public void Dispose() {
			if (!this.liveWrite) {
				using(StreamWriter writer = new StreamWriter(FullFilePath)) {
					writer.WriteLine(log.ToString());
				}
			}
		}
	}
}