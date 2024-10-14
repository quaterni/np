using Dapper;
using Np.RelationsService.Application.Abstractions.Data;
using Np.RelationsService.Application.Abstractions.Messaging;
using Np.RelationsService.Domain.Abstractions;

namespace Np.RelationsService.Application.RootEntries.GetRootEntries;

internal class GetRootEntriesQueryHandler : IQueryHandler<GetRootEntriesQuery, GetRootEntriesResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetRootEntriesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<GetRootEntriesResponse>> Handle(GetRootEntriesQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var dbResponse = await connection.QueryAsync("SELECT id FROM root_entries");

        var rootEntryResponse = new GetRootEntriesResponse(dbResponse.Select(d => (Guid)d.id));

        return rootEntryResponse;
    }
}
