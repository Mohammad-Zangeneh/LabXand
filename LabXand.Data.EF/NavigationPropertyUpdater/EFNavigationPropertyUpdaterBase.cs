using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    internal abstract class EFNavigationPropertyUpdaterBase<TRoot, TConfig>(DbContext dbContext, TConfig updateConfig) : NavigationPropertyUpdaterBase<TRoot, TConfig>(updateConfig)
        where TRoot : class
        where TConfig : IUpdateConfiguration<TRoot>
    {
        protected readonly DbContext dbContext = dbContext;
    }
}
