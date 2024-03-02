using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    public static class DbContextExtensions
    {
        public static void SafeUpdate<TEntity>(this DbContext context, TEntity currentValue, TEntity originalValue, List<string> constantFields)
            where TEntity : class
        {
            var entry = context.Entry(originalValue);
            entry.CurrentValues.SetValues(currentValue);

            if (constantFields != null)
            {
                foreach (string field in constantFields)
                {
                    entry.Property(field).IsModified = false;
                }
            }
        }
    }
}
