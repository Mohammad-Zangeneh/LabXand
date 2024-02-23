using LabXand.SharedKernel;
using System.Linq.Expressions;

namespace LabXand.Data
{
    public static class UpdateConfigarationExtender
    {
        public static INavigationPropertyUpdater<TRoot> CreateUpdater<TRoot>(this IUpdateConfiguration<TRoot> config)
            where TRoot : class
        {
            return config.CreateUpdater(config.PropertyUpdaterCustomizer);
        }

        #region Create Collection
        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, null, propertyUpdaterCustomizer);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, null, propertyUpdaterCustomizer, constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration,
            INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, innerConfiguration, propertyUpdaterCustomizer, new List<string>());
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration,
            List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateCollection<TRoot, T, I>(itemSelector, parentPropertyName, childtPropertyName, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateCollection<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, string parentPropertyName, string childtPropertyName, IUpdateConfiguration<T> innerConfiguration,
            INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            UpdateCollectionConfiguration<TRoot, T, I> config = new UpdateCollectionConfiguration<TRoot, T, I>(propertyUpdaterCustomizer, constantFields)
            {
                ItemSelector = itemSelector,
                ParentPropertyName = parentPropertyName,
                ChildtPropertyName = childtPropertyName,
            };
            if (innerConfiguration != null)
                ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }

        #endregion

        #region Create ManyToMany
        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
                    Expression<Func<TRoot, ICollection<T>>> itemSelector)
                    where TRoot : class
                    where T : class, IEntity<I>
                    where I : struct
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
                    Expression<Func<TRoot, ICollection<T>>> itemSelector, List<string> constantFields)
                    where TRoot : class
                    where T : class, IEntity<I>
                    where I : struct
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer, constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateManyToMany<TRoot, T, I>(itemSelector, innerConfiguration, propertyUpdaterCustomizer, new List<string>());
        }

        public static IUpdateConfiguration<TRoot> CreateManyToMany<TRoot, T, I>(
            Expression<Func<TRoot, ICollection<T>>> itemSelector, IUpdateConfiguration<T> innerConfiguration, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where I : struct
            where T : class, IEntity<I>
        {
            UpdateManyToManyCollection<TRoot, T, I> config = new UpdateManyToManyCollection<TRoot, T, I>(propertyUpdaterCustomizer, constantFields)
            {
                ItemSelector = itemSelector
            };
            if (innerConfiguration != null)
                ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }

        #endregion

        #region Create One
        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateOne<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateOne<TRoot, T, I>(itemSelector, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateOne<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where I : struct
            where T : class, IEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, null, propertyUpdaterCustomizer, constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration)
            where I : struct
            where TRoot : class
            where T : class, IEntity<I>
        {
            return CreateOne<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>());
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateOne<TRoot, T, I>(itemSelector, innerConfiguration, new EmptyPropertyUpdaterCustomizer<TRoot>(), constantFields);
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            return CreateOne<TRoot, T, I>(itemSelector, innerConfiguration, propertyUpdaterCustomizer, new List<string>());
        }

        public static IUpdateConfiguration<TRoot> CreateOne<TRoot, T, I>(
            Expression<Func<TRoot, T>> itemSelector, IUpdateConfiguration<T> innerConfiguration, INavigationPropertyUpdaterCustomizer<TRoot> propertyUpdaterCustomizer, List<string> constantFields)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            UpdateOneEntityConfiguration<TRoot, T, I> config = new UpdateOneEntityConfiguration<TRoot, T, I>(propertyUpdaterCustomizer, constantFields)
            {
                ItemSelector = itemSelector
            };
            if (innerConfiguration != null)
                ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }
        #endregion

        public static IUpdateConfiguration<TRoot> HasCollection<TRoot, T, I>(this IUpdateConfiguration<TRoot> config,
            IUpdateConfiguration<TRoot> innerConfiguration)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }
        public static IUpdateConfiguration<TRoot> HasOne<TRoot, T, I>(this IUpdateConfiguration<TRoot> config,
            IUpdateConfiguration<TRoot> innerConfiguration)
            where TRoot : class
            where T : class, IEntity<I>
            where I : struct
        {
            ((List<object>)config.InnerConfigurations).Add(innerConfiguration);
            return config;
        }

    }
}
