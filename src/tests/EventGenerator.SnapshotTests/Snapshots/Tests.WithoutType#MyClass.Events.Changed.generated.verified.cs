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