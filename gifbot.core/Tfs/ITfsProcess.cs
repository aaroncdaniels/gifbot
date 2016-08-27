using System.Threading.Tasks;

namespace gifbot.core.Tfs
{
	public interface ITfsProcess
	{
		Task<Entry> WriteToChatroomAsync(int roomid, string message);

		Task DeleteMessageFromChatroomAsync(Entry entry);
	}
}