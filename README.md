# EventGenerator
Generated events and OnEvent() methods. 

## Install
```
Install-Package EventGenerator.Generator // Generator
Install-Package EventGenerator.Core // Attributes
```

## Usage
```cs
using EventGenerator;

#nullable enable

namespace H.Generators.IntegrationTests;

[Event<MyArgs>("Changed")]
public partial class MyClass
{
}
```
will generate:
```cs

```

### XML documentation
If for some reason you need to save xml documentation for your properties, 
there is an option to specify xml text for both DependencyProperty and getter/setter 
via XmlDocumentation/PropertyXmlDocumentation attribute properties.

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