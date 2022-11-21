namespace H.Generators.SnapshotTests;

[TestClass]
public partial class Tests : VerifyBase
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
public class MyArgs : System.EventArgs
{
}

[Event<MyArgs>(""Changed"")]
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
}