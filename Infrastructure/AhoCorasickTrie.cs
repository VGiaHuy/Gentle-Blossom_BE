namespace GentleBlossom_BE.Infrastructure
{
    public class AhoCorasickTrie
    {
        private class Node
        {
            public Dictionary<char, Node> Children { get; } = new Dictionary<char, Node>();

            public Node FailureLink { get; set; }

            public List<object> Outputs { get; } = new List<object>();
        }

        private readonly Node _root = new Node();

        public void Add(string keyword, object value)
        {
            var current = _root;
            foreach (char c in keyword.ToLower())
            {
                if (!current.Children.ContainsKey(c))
                    current.Children[c] = new Node();
                current = current.Children[c];
            }
            current.Outputs.Add(value);
        }

        public void Build()
        {
            var queue = new Queue<Node>();
            _root.FailureLink = _root;

            foreach (var child in _root.Children.Values)
            {
                child.FailureLink = _root;
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var pair in current.Children)
                {
                    char c = pair.Key;
                    var child = pair.Value;
                    queue.Enqueue(child);

                    var failure = current.FailureLink;
                    while (!failure.Children.ContainsKey(c) && failure != _root)
                        failure = failure.FailureLink;

                    child.FailureLink = failure.Children.ContainsKey(c) ? failure.Children[c] : _root;
                    child.Outputs.AddRange(child.FailureLink.Outputs);
                }
            }
        }

        public IEnumerable<object> Search(string text)
        {
            var results = new List<object>();
            var current = _root;

            foreach (char c in text.ToLower())
            {
                while (!current.Children.ContainsKey(c) && current != _root)
                    current = current.FailureLink;

                current = current.Children.ContainsKey(c) ? current.Children[c] : _root;
                results.AddRange(current.Outputs);
            }

            return results;
        }
    }
}
