using Cratis.Chronicle.Projections;
using Events.Authors;

namespace Read.Authors;

public class AuthorProjection : IProjectionFor<Author>
{
    public void Define(IProjectionBuilderFor<Author> builder) => builder
        .AutoMap()
        .From<AuthorRegistered>();
}
