HEADERnamespace a;

    public class ConsistentHashRing
    {
        private readonly SortedDictionary<uint, string> _ring = new();
        private readonly int _virtualNodesPerServer;
        private readonly HashAlgorithm _hashAlgorithm;

        public ConsistentHashRing(int virtualNodesPerServer = 150)
        {
            _virtualNodesPerServer = virtualNodesPerServer;
            _hashAlgorithm = SHA1.Create();
        }

        public void AddServer(string server)
        {
            for (int i = 0; i < _virtualNodesPerServer; i++)
            {
                var virtualNodeKey = $"{server}:{i}";
                var hash = ComputeHash(virtualNodeKey);
                _ring[hash] = server;
            }
        }

        public void RemoveServer(string server)
        {
            for (int i = 0; i < _virtualNodesPerServer; i++)
            {
                var virtualNodeKey = $"{server}:{i}";
                var hash = ComputeHash(virtualNodeKey);
                _ring.Remove(hash);
            }
        }

        public string GetServer(string key)
        {
            if (_ring.Count == 0)
                throw new InvalidOperationException("No servers available in the ring");

            var hash = ComputeHash(key);
            
            // Find the first server with hash >= key hash
            var server = _ring.FirstOrDefault(kvp => kvp.Key >= hash);
            
            // If no server found, wrap around to the first server
            if (server.Key == 0)
                server = _ring.First();

            return server.Value;
        }

        public IEnumerable<string> GetServers()
        {
            return _ring.Values.Distinct();
        }

        public int GetServerCount()
        {
            return _ring.Values.Distinct().Count();
        }

        private uint ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = _hashAlgorithm.ComputeHash(bytes);
            
            // Take first 4 bytes and convert to uint
            return BitConverter.ToUInt32(hashBytes, 0);
        }

        public void Dispose()
        {
            _hashAlgorithm?.Dispose();
        }
    }