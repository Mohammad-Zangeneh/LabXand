namespace LabXand.SharedKernel
{
    public static class SpecificationExtender
    {
        public static ISpecification<T, TIdentifier> And<T, TIdentifier>(this ISpecification<T, TIdentifier> current, ISpecification<T, TIdentifier> specification)
            where TIdentifier : struct
            where T : IEntity<TIdentifier>
            => new AndSpecification<T, TIdentifier>(current, specification);

        public static ISpecification<T, TIdentifier> Or<T, TIdentifier>(this ISpecification<T, TIdentifier> current, ISpecification<T, TIdentifier> specification)
            where T : IEntity<TIdentifier>
            where TIdentifier : struct
            => new OrSpecification<T, TIdentifier>(current, specification);
    }
}