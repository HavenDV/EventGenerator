namespace H.Generators.SnapshotTests;

[TestClass]
public partial class Tests : VerifyBase
{
    [TestMethod]
    public Task ReadmeExample()
    {
        return CheckSourceAsync(@"
[Event(""Changed"")]
public partial class MyClass
{
}");
    }
}