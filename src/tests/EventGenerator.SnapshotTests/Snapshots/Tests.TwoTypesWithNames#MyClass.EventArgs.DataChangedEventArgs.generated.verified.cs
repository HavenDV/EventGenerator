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
            public string Description { get; }

            /// <summary>
            /// 
            /// </summary>
            public DataChangedEventArgs(string title, string description)
            {
                Title = title;
                Description = description;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Deconstruct(out string title, out string description)
            {
                title = Title;
                description = Description;
            }
        }
    }
}