using Locamart.Nava.Domain.Entities.Location;
using Locamart.Nava.Domain.Entities.Location.Abstracts;
using Locamart.Nava.Domain.Entities.Location.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Repositories;

public class ProvinceRepository(LocamartNavaDbContext dbContext) : IProvinceRepository
{
    public async Task Add(ProvinceEntity province, CancellationToken ct)
    {
        await dbContext.Provinces.AddAsync(province, ct);
    }

    public async Task<ProvinceEntity?> GetById(ProvinceId provinceId, CancellationToken ct)
    {
        return await dbContext.Provinces.SingleOrDefaultAsync(x => x.Id == provinceId, cancellationToken: ct);
    }

    public Task Update(ProvinceEntity province, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}

