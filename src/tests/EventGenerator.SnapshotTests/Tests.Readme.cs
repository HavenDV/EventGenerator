namespace H.Generators.SnapshotTests;

[TestClass]
public partial class Tests
{
    [TestMethod]
    public Task WithoutType()
    {
        return CheckSourceAsync(@"
[Event(""Changed"")]
public partial class MyClass
{
}");
    }

    [TestMethod]
    public Task Readme()
    {
        return CheckSourceAsync(@"
public class MyEventArgs : System.EventArgs
{
}

[Event<MyEventArgs>(""Changed"")]
public partial class MyClass
{
}");
    }

    [TestMethod]
    public Task Generic()
    {
        return CheckSourceAsync(@"
public class MyEventArgs<T> : System.EventArgs
{
}

[Event<MyEventArgs<GenericType>>(""Changed"")]
public partial class MyClass<T>
{
}");
    }

    [TestMethod]
    public Task OneTypeNoArgs()
    {
        return CheckSourceAsync(@"
[Event<string>(""Changed"")]
public partial class MyClass
{
}");
    }

    [TestMethod]
    public Task TwoTypes()
    {
        return CheckSourceAsync(@"
[Event<string, string>(""Changed"")]
public partial class MyClass
{
}");
    }

    [TestMethod]
    public Task ThreeTypes()
    {
        return CheckSourceAsync(@"
[Event<string, string, System.Version>(""Changed"")]
public partial class MyClass
{
}");
    }

    [TestMethod]
    public Task TwoTypesWithNames()
    {
        return CheckSourceAsync(@"
[Event<string, string>(""DataChanged"", PropertyNames = new [] { ""Title"", ""Namespace"" })]
public partial class MyClass
{
}");
    }

    [TestMethod]
    public Task StaticEvent()
    {
        return CheckSourceAsync(@"
[Event(""StaticEvent"", IsStatic = true)]
public static partial class MyClass
{
}");
    }
    
    [TestMethod]
    public Task Interface()
    {
        return CheckSourceAsync(@"
[Event<int>(""Changed"")]
public partial interface IMyClass
{
}

[Event<int>(""Changed"")]
public partial class MyClass : IMyClass
{
}");
    }
}