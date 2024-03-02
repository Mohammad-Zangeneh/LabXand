namespace LabXand.Data
{
    public interface INavigationPropertyUpdaterCustomizer<TRoot>
        where TRoot : class
    {
        Action<TRoot, object> OnBeforAddEntity(TRoot rootEntity, object entity);
        Action<TRoot, object> OnBeforEditEntity(TRoot rootEntity, object entity);
        Action<TRoot, object> OnBeforRemoveEntity(TRoot rootEntity, object entity);
        Action<TRoot, object> OnAfterAddEntity(TRoot rootEntity, object entity);
        Action<TRoot, object> OnAfterEditEntity(TRoot rootEntity, object entity);
        Action<TRoot, object> OnAfterRemoveEntity(TRoot rootEntity, object entity);
    }
}
