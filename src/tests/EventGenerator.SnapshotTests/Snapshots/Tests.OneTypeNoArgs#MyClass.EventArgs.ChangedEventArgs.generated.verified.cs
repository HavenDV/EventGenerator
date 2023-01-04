//HintName: MyClass.EventArgs.ChangedEventArgs.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        /// <summary>
        /// 
        /// </summary>
        public class ChangedEventArgs : global::System.EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public string Value { get; }

            /// <summary>
            /// 
            /// </summary>
            public ChangedEventArgs(string value)
            {
                Value = value;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct(out string value)
            {
                value = Value;
            }
        }
    }
}