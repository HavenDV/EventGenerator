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
            public string Value1 { get; }

            /// <summary>
            /// 
            /// </summary>
            public string Value2 { get; }

            /// <summary>
            /// 
            /// </summary>
            public global::System.Version Value3 { get; }

            /// <summary>
            /// 
            /// </summary>
            public ChangedEventArgs(string value1, string value2, global::System.Version value3)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = value3;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct(out string value1, out string value2, out global::System.Version value3)
            {
                value1 = Value1;
                value2 = Value2;
                value3 = Value3;
            }
        }
    }
}