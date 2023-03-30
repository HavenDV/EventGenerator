using EventGenerator;

namespace H.Generators.IntegrationTests;

[TestClass]
public class DependencyPropertyGeneratorTests
{   
    [TestMethod]
    public void SubscribeToChangedWorksCorrectly()
    {
        var myClass = new MyClass();

        myClass.IsSubscribedToChanged(Handler).Should().BeFalse();
        
        using (var _ = myClass.SubscribeToChanged(Handler))
        {
            myClass.IsSubscribedToChanged(Handler).Should().BeTrue();
        }
        
        myClass.IsSubscribedToChanged(Handler).Should().BeFalse();
    }

    private static void Handler(object? sender, EventArgs e)
    {
    }
}

[Event("Changed")]
public partial class MyClass
{
    public bool IsSubscribedToChanged(EventHandler @delegate)
    {
        return
            Changed != null &&
            Changed.GetInvocationList().Contains(@delegate);
    }
}