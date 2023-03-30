//HintName: EventGenerator.Disposable.g.cs

namespace EventGenerator
{
    internal class Disposable : global::System.IDisposable
    {
        private readonly global::System.Action action;

        public Disposable(global::System.Action action)
        {
            this.action = action;
        }

        public void Dispose()
        {
            action();
        }
    }
}