using System.Collections.Generic;
using System.Threading.Tasks;
using gifbot.core.gifs;
using gifbot.core.Tfs;

namespace gifbot.core
{
	public class Auditor : IAuditor
	{
		private static readonly object Lock = new object();
		private readonly LinkedList<Entry> _ledger;

		private readonly ITfsProcess _tfsProcess;
		private readonly IConfiguration _configuration;

		public Auditor(ITfsProcess tfsProcess, IConfiguration configuration)
		{
			_tfsProcess = tfsProcess;
			_configuration = configuration;

			_ledger = new LinkedList<Entry>();
		}

		public void RecordActivity(Entry entry)
		{
			lock (Lock)
			{
				if (_ledger.Count == _configuration.LedgerSize)
					_ledger.RemoveLast();

				_ledger.AddFirst(entry);
			}
		}

		public Task StrikeFromTheRecordAsync(int lastNumberOfEntries)
		{
			var deleteTasks = new List<Task>();

			lock (Lock)
			{
				for (var i = 0; i < lastNumberOfEntries; i++)
				{
					if (_ledger.Count < 1)
						break;

					var entry = _ledger.First.Value;
					deleteTasks.Add(_tfsProcess.DeleteMessageFromChatroomAsync(entry));
					_ledger.RemoveFirst();
				}
			}

			return Task.WhenAll(deleteTasks);
		}
	}
}