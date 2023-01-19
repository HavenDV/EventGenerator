# EventGenerator
Generated events and OnEvent() methods. Can generate complex EventArgs from multi-type attributes.

## Install
```
Install-Package EventGenerator.Generator
```
```xml
<!-- compile asset is required for attributes. -->
<!-- ExcludeAssets="runtime" excludes EventGenerator.Attributes.dll from your output. -->
<PackageReference Include="EventGenerator.Generator" Version="0.3.3" PrivateAssets="all" ExcludeAssets="runtime" />
```

## Usage
```cs
[Event<string, string>(""DataChanged"", PropertyNames = new [] { ""Title"", ""Namespace"" })]
public partial class MyClass
{
}
```
will generate:
```csharp
//HintName: MyClass.Events.DataChanged.generated.cs
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
            string @namespace)
        {
            var args = new global::H.Generators.IntegrationTests.MyClass.DataChangedEventArgs(title, @namespace);
            DataChanged?.Invoke(this, args);

            return args;
        }
    }
}
```
```csharp
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

            /// <summary>
            /// 
            /// </summary>
            public override string ToString()
            {
                return $"(Title={Title}, Namespace={Namespace})";
            }
        }
    }
}
```

## Notes
To use generic attributes, you need to set up `LangVersion` in your .csproj:
```xml
<LangVersion>preview</LangVersion>
```
There are also non-Generic attributes here.

## Support
Priority place for bugs: https://github.com/HavenDV/EventGenerator/issues  
Priority place for ideas and general questions: https://github.com/HavenDV/EventGenerator/discussions  
I also have a Discord support channel:  
https://discord.gg/g8u2t9dKgE