using Cratis.Chronicle.EventSequences;

namespace Quickstart.Common;

public class DemoData(IEventLog eventLog)
{
    public async Task Initialize()
    {
        await eventLog.Append(Guid.NewGuid(), new UserOnboarded("Jane Doe", "jane@interwebs.net"));
        await eventLog.Append(Guid.NewGuid(), new UserOnboarded("John Doe", "john@interwebs.net"));

        await eventLog.Append(Guid.NewGuid(), new BookAddedToInventory("Metaprogramming in C#: Automate your .NET development and simplify overcomplicated code", "Einar Ingebrigtsen", "978-1837635429"));
        await eventLog.Append(Guid.NewGuid(), new BookAddedToInventory("Understanding Eventsourcing: Planning and Implementing scalable Systems with Eventmodeling and Eventsourcing", "Martin Dilger", "979-8300933043"));
    }
}