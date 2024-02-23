using Microsoft.EntityFrameworkCore;

namespace LabXand.Data
{
    public abstract class NavigationPropertyUpdaterCustomizerBase<TRoot> : INavigationPropertyUpdaterCustomizer<TRoot>
        where TRoot : class
    {
        public abstract Action<TRoot, object> OnBeforAddEntity(TRoot rootEntity, object entity);
        public abstract Action<TRoot, object> OnBeforEditEntity(TRoot rootEntity, object entity);
        public abstract Action<TRoot, object> OnBeforRemoveEntity(TRoot rootEntity, object entity);
        public abstract Action<TRoot, object> OnAfterAddEntity(TRoot rootEntity, object entity);
        public abstract Action<TRoot, object> OnAfterEditEntity(TRoot rootEntity, object entity);
        public abstract Action< TRoot, object> OnAfterRemoveEntity(TRoot rootEntity, object entity);
    }
}
