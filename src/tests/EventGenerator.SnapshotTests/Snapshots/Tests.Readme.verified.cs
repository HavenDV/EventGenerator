//HintName: MyClass.Events.Changed.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        /// <summary>
        /// </summary>
        public event global::System.EventHandler<global::H.Generators.IntegrationTests.MyArgs>? Changed;

        /// <summary>
        /// A helper method to raise the Changed event.
        /// </summary>
        protected virtual global::H.Generators.IntegrationTests.MyArgs OnChanged(global::H.Generators.IntegrationTests.MyArgs args)
        {
            Changed?.Invoke(this, args);

            return args;
        }
    }
}