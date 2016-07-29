using System.Threading.Tasks;

namespace gifbot.core.Tfs
{
	public interface ITfsProcess
	{
		Task WriteToChatroom(int roomid, string message);

		Task DeleteLastMessageFromChatroom(int roomId);
	}
}