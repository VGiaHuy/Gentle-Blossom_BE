using System.Collections.Concurrent;

namespace GentleBlossom_BE.Infrastructure
{
    public class InMemoryQueue<T>
    {
        // ConcurrentQueue để đảm bảo thread-safe khi thêm/xóa các mục
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        // SemaphoreSlim để báo hiệu khi có mục mới trong hàng đợi
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        // Thêm một mục vào hàng đợi
        public async Task EnqueueAsync(T item)
        {
            _queue.Enqueue(item); // Thêm mục vào queue
            _signal.Release();   // Báo hiệu rằng có mục mới
            await Task.CompletedTask; // Đảm bảo async
        }

        // Lấy một mục từ hàng đợi, chờ nếu queue rỗng
        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken); // Chờ tín hiệu khi queue có mục
            _queue.TryDequeue(out var item);           // Lấy mục từ queue
            return item;                               // Trả về mục
        }
    }
}
