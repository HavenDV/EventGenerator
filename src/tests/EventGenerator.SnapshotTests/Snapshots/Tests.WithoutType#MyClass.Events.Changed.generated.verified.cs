//HintName: MyClass.Events.Changed.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        /// <summary>
        /// </summary>
        public event global::System.EventHandler? Changed;

        /// <summary>
        /// A helper method to subscribe the Changed event.
        /// </summary>
        public global::System.IDisposable SubscribeToChanged(global::System.EventHandler handler)
        {
            Changed += handler;

            return new global::EventGenerator.Disposable(() => Changed -= handler);
        }

        /// <summary>
        /// A helper method to raise the Changed event.
        /// </summary>
        protected virtual global::System.EventArgs OnChanged()
        {
            var args = new global::System.EventArgs();
            Changed?.Invoke(this, args);

            return args;
        }
    }
}