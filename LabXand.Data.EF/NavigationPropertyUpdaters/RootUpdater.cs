using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public class RootUpdater<TRoot>() : RootPropertyUpdaterBase<TRoot>()
    where TRoot : class
{
    public override string Key => string.Empty;

    public override void Update(DbContext dbContext, TRoot current, TRoot original)
    {
        dbContext.SafeUpdate(current, original);
        foreach (var childUpdater in navigationPropertyUpdaters)
            childUpdater.Update(dbContext, current, original);
    }
}