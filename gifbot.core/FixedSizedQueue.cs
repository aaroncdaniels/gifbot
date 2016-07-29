using System.Collections.Concurrent;

namespace gifbot.core
{
	/// <summary>
	/// Represents a fixed sized queue.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="System.Collections.Concurrent.ConcurrentQueue{T}" />
	/// <remarks>
	/// http://stackoverflow.com/a/10299662/37064
	/// </remarks>
	public class FixedSizedQueue<T> : ConcurrentQueue<T>
	{
		private readonly object _syncObject = new object();

		public int Size { get; }

		public FixedSizedQueue(int size)
		{
			Size = size;
		}

		public new void Enqueue(T obj)
		{
			base.Enqueue(obj);
			lock (_syncObject)
			{
				while (Count > Size)
				{
					T outObj;
					TryDequeue(out outObj);
				}
			}
		}
	}
}