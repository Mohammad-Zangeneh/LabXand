using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public class RootUpdater<TRoot> : RootPropertyUpdaterBase<TRoot>
    where TRoot : class
{
    private readonly string key;
    public RootUpdater(string key) => this.key = key;
    public RootUpdater(INavigationPropertyUpdaterCustomizer<TRoot> navigationUpdaterCustomizer) : base(navigationUpdaterCustomizer)
        => key = string.Empty;
    public override string Key => key;

    public override void Update(DbContext dbContext, TRoot current, TRoot original)
    {
        navigationPropertyUpdaterCustomizer?.OnBeforEditEntity(current, original);
        dbContext.SafeUpdate(current, original);
        foreach (var childUpdater in navigationPropertyUpdaters)
            childUpdater.Update(dbContext, current, original);
        navigationPropertyUpdaterCustomizer?.OnAfterEditEntity(current, original);
    }
}