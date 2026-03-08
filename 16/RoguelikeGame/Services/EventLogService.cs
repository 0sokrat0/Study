namespace RoguelikeGame.Services
{
    public class EventLogService
    {
        private readonly List<string> _log = new();
        private const int MaxEntries = 100;

        public IReadOnlyList<string> Entries => _log;

        public void Add(string message)
        {
            _log.Add(message);
            if (_log.Count > MaxEntries)
                _log.RemoveAt(0);
        }

        public void Clear() => _log.Clear();

        public IEnumerable<string> GetLast(int count) =>
            _log.Count <= count ? _log : _log.Skip(_log.Count - count);
    }
}
