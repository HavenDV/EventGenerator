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
            public int Value { get; }

            /// <summary>
            /// 
            /// </summary>
            public ChangedEventArgs(int value)
            {
                Value = value;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct(out int value)
            {
                value = Value;
            }

            /// <summary>
            /// 
            /// </summary>
            public override string ToString()
            {
                return $"(Value={Value})";
            }
        }
    }
}