using Microsoft.EntityFrameworkCore;

namespace LabXand.Infrastructure.Data.EF.UpdateConfiguration
{
    public interface INavigationPropertyUpdaterCustomizer<TRoot>
        where TRoot : class
    {
        Action<DbContext,TRoot, object>  OnBeforAddEntity(DbContext dbContext, TRoot rootEntity, object entity);
        Action<DbContext,TRoot, object>  OnBeforEditEntity(DbContext dbContext, TRoot rootEntity, object entity);
        Action<DbContext,TRoot, object>  OnBeforRemoveEntity(DbContext dbContext, TRoot rootEntity, object entity);
        Action<DbContext,TRoot, object>  OnAfterAddEntity(DbContext dbContext, TRoot rootEntity, object entity);
        Action<DbContext,TRoot, object>  OnAfterEditEntity(DbContext dbContext, TRoot rootEntity, object entity);
        Action<DbContext,TRoot, object>  OnAfterRemoveEntity(DbContext dbContext, TRoot rootEntity, object entity);
    }
}
