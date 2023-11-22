﻿//HintName: MyClass.Events.Changed.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        /// <summary>
        /// </summary>
        public event global::System.EventHandler<global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs>? Changed;

        /// <summary>
        /// A helper method to subscribe the Changed event.
        /// </summary>
        public global::System.IDisposable SubscribeToChanged(global::System.EventHandler<global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs> handler)
        {
            Changed += handler;

            return new global::EventGenerator.Disposable(() => Changed -= handler);
        }

        /// <summary>
        /// A helper method to raise the Changed event.
        /// </summary>
        protected virtual global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs OnChanged(global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs args)
        {
            Changed?.Invoke(this, args);

            return args;
        }

        /// <summary>
        /// A helper method to raise the Changed event.
        /// </summary>
        protected virtual global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs OnChanged(
            int value)
        {
            var args = new global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs(value);
            Changed?.Invoke(this, args);

            return args;
        }
    }
}