using System.Threading.Tasks;

namespace gifbot.core
{
	public interface IAuditor
	{
		void RecordActivity(Entry entry);

		Task StrikeFromTheRecordAsync(int lastNumberOfEntries);
	}
}