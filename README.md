# EventGenerator
Generated events and OnEvent() methods. 

## Install
```
Install-Package EventGenerator.Generator
```

## Usage
```cs
using EventGenerator;

#nullable enable

namespace H.Generators.IntegrationTests;

public class MyArgs : EventArgs
{
}

[Event<MyArgs>("Changed")]
public partial class MyClass
{
}
```
will generate:
```cs
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
        protected global::H.Generators.IntegrationTests.MyArgs OnChanged(global::H.Generators.IntegrationTests.MyArgs args)
        {
            Changed?.Invoke(this, args);

            return args;
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