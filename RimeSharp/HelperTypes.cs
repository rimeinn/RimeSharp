namespace RimeSharp
{
    public record class CandidateItem
    {
        public string Text { get; }
        public string? Comment { get; }

        public CandidateItem(string text, string? comment)
        {
            Text = text;
            Comment = comment;
        }
    };

    public record class SchemaListItem
    {
        public string SchemaId { get; }
        public string Name { get; }

        public SchemaListItem(string schemaId, string name)
        {
            SchemaId = schemaId;
            Name = name;
        }
    };

    public class Disposable<T> : IDisposable
    {
        protected T value;
        private readonly ActionRef<T> _disposeAction;
        private bool _disposed = false;

        public delegate bool ActionRef<TRef>(ref TRef arg);

        public Disposable(ref T value, ActionRef<T> disposeAction)
        {
            this.value = value;
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposeAction(ref value);
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
