//HintName: MyClass.EventArgs.ChangedEventArgs.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        public class ChangedEventArgs : global::System.EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public string Value1 { get; }

            /// <summary>
            /// 
            /// </summary>
            public ChangedEventArgs(string value1)
            {
                Value1 = value1;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct(out string value1)
            {
                value1 = Value1;
            }
        }
    }
}