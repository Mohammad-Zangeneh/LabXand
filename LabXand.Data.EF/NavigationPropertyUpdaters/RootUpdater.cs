using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public class RootUpdater<TRoot>(string key) : RootPropertyUpdaterBase<TRoot>()
    where TRoot : class
{
    public RootUpdater() : this(string.Empty) { }
    public override string Key => key;

    public override void Update(DbContext dbContext, TRoot current, TRoot original)
    {
        dbContext.SafeUpdate(current, original);
        foreach (var childUpdater in navigationPropertyUpdaters)
            childUpdater.Update(dbContext, current, original);
    }
}