//HintName: MyClass{T}.Events.Changed.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass<T>
    {
        /// <summary>
        /// </summary>
        public event global::System.EventHandler<global::H.Generators.IntegrationTests.MyEventArgs<T>>? Changed;

        /// <summary>
        /// A helper method to subscribe the Changed event.
        /// </summary>
        public global::System.IDisposable SubscribeToChanged(global::System.EventHandler<global::H.Generators.IntegrationTests.MyEventArgs<T>> handler)
        {
            Changed += handler;

            return new global::EventGenerator.Disposable(() => Changed -= handler);
        }

        /// <summary>
        /// A helper method to raise the Changed event.
        /// </summary>
        protected virtual global::H.Generators.IntegrationTests.MyEventArgs<T> OnChanged(global::H.Generators.IntegrationTests.MyEventArgs<T> args)
        {
            Changed?.Invoke(this, args);

            return args;
        }
    }
}