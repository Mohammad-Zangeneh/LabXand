namespace LabXand.Data
{
    public class EmptyPropertyUpdaterCustomizer<TRoot> : NavigationPropertyUpdaterCustomizerBase<TRoot>
        where TRoot : class
    {
        public override Action<TRoot, object> OnAfterAddEntity(TRoot rootEntity, object entity) => (root, t) => { };
        public override Action<TRoot, object> OnAfterEditEntity(TRoot rootEntity, object entity) => (root, t) => { };
        public override Action<TRoot, object> OnAfterRemoveEntity(TRoot rootEntity, object entity) => (root, t) => { };
        public override Action<TRoot, object> OnBeforAddEntity(TRoot rootEntity, object entity) => (root, t) => { };
        public override Action<TRoot, object> OnBeforEditEntity(TRoot rootEntity, object entity) => (root, t) => { };
        public override Action<TRoot, object> OnBeforRemoveEntity(TRoot rootEntity, object entity) => (root, t) => { };
    }
}
