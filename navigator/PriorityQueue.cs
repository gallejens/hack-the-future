namespace navigator
{
    public class PriorityQueue<T>
    {
        private SortedDictionary<int, Queue<T>> queue = new SortedDictionary<int, Queue<T>>();

        public int Count
        {
            get { return queue.Sum(q => q.Value.Count); }
        }

        public void Enqueue(T item, int priority)
        {
            if (!queue.ContainsKey(priority))
            {
                queue[priority] = new Queue<T>();
            }

            queue[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            var highestPriorityQueue = queue.First();
            var item = highestPriorityQueue.Value.Dequeue();

            if (highestPriorityQueue.Value.Count == 0)
            {
                queue.Remove(highestPriorityQueue.Key);
            }

            return item;
        }
    }
}
