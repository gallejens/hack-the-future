namespace navigator
{
    public class PriorityQueue<T>
    {
        private SortedDictionary<int, Queue<T>> _queue = new SortedDictionary<int, Queue<T>>();

        public int Count
        {
            get { return _queue.Sum(q => q.Value.Count); }
        }

        public void Enqueue(T item, int priority)
        {
            if (!_queue.ContainsKey(priority))
                _queue[priority] = new Queue<T>();

            _queue[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            var first = _queue.First();
            var item = first.Value.Dequeue();

            if (first.Value.Count == 0)
                _queue.Remove(first.Key);

            return item;
        }
    }
}
