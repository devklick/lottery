using Lottery.DB.Context;

using Microsoft.EntityFrameworkCore;

namespace Lottery.Integration.Test;

[Collection("Database")]
public class ResultRepositoryTest(PostgreSqlFixture fixture)
{
    private readonly PostgreSqlFixture _fixture = fixture;
}