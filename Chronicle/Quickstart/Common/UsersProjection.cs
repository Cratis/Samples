using Cratis.Chronicle.Projections;

namespace Quickstart.Common;

public class UsersProjection : IProjectionFor<User>
{
    public void Define(IProjectionBuilderFor<User> builder) => builder
        .From<UserOnboarded>(_ => _.AutoMap());
}
