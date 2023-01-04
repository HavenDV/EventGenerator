//HintName: MyClass.EventArgs.DataChangedEventArgs.generated.cs
#nullable enable

namespace H.Generators.IntegrationTests
{
    public partial class MyClass
    {
        /// <summary>
        /// 
        /// </summary>
        public class DataChangedEventArgs : global::System.EventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            public string Title { get; }

            /// <summary>
            /// 
            /// </summary>
            public string Namespace { get; }

            /// <summary>
            /// 
            /// </summary>
            public DataChangedEventArgs(string title, string @namespace)
            {
                Title = title;
                Namespace = @namespace;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct(out string title, out string @namespace)
            {
                title = Title;
                @namespace = Namespace;
            }
        }
    }
}