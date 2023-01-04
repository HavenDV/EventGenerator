//HintName: MyClass.Events.Changed.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        /// <summary>
        /// </summary>
        public event global::System.EventHandler<global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs>? Changed;

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
            string value1,
            string value2,
            global::System.Version value3)
        {
            var args = new global::H.Generators.IntegrationTests.MyClass.ChangedEventArgs(value1, value2, value3);
            Changed?.Invoke(this, args);

            return args;
        }
    }
}