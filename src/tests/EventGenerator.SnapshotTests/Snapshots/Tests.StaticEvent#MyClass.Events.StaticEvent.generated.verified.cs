﻿//HintName: MyClass.Events.StaticEvent.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public static partial class MyClass
    {
        /// <summary>
        /// </summary>
        public static event global::System.EventHandler? StaticEvent;

        /// <summary>
        /// A helper method to raise the StaticEvent event.
        /// </summary>
        internal static global::System.EventArgs OnStaticEvent(object? sender)
        {
            var args = new global::System.EventArgs();
            StaticEvent?.Invoke(sender, args);

            return args;
        }
    }
}