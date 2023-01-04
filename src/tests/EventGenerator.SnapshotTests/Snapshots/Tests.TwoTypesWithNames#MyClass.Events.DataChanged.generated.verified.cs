﻿//HintName: MyClass.Events.DataChanged.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        /// <summary>
        /// </summary>
        public event global::System.EventHandler<global::H.Generators.IntegrationTests.MyClass.DataChangedEventArgs>? DataChanged;

        /// <summary>
        /// A helper method to raise the DataChanged event.
        /// </summary>
        protected virtual global::H.Generators.IntegrationTests.MyClass.DataChangedEventArgs OnDataChanged(global::H.Generators.IntegrationTests.MyClass.DataChangedEventArgs args)
        {
            DataChanged?.Invoke(this, args);

            return args;
        }

        /// <summary>
        /// A helper method to raise the DataChanged event.
        /// </summary>
        protected virtual global::H.Generators.IntegrationTests.MyClass.DataChangedEventArgs OnDataChanged(
            string title,
            string description)
        {
            var args = new global::H.Generators.IntegrationTests.MyClass.DataChangedEventArgs(title, description);
            DataChanged?.Invoke(this, args);

            return args;
        }
    }
}