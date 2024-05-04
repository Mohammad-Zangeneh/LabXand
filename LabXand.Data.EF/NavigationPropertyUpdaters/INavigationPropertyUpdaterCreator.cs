using LabXand.Core;
using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public class GetRequest
{
    // page
    // sort
}
public class GetRequest<TSearchModel> : GetRequest
{
    private List<(Expression<Func<TSearchModel, dynamic>> func, FilterOperations filterOperations, string propertyName)> filterSpecifications = [];
    public List<FilterSpecification<TSearchModel>> FilterSpecifications { get; set; }
    public GetRequest<TSearchModel> HasFilter(Expression<Func<TSearchModel, dynamic>> func, FilterOperations filterOperations, string propertyName)
    {
        filterSpecifications.Add(new(func, filterOperations, propertyName));
        return this;
    }
    public GetRequest<TSearchModel> CreateRequest(TSearchModel searchModel)
    {
        foreach (var item in filterSpecifications)
        {
            FilterSpecifications.Add(new FilterSpecification<TSearchModel>()
            {
                FilterOperation = item.filterOperations,
                PropertyName = item.propertyName,
                FilterValue = TypeHelper.GetPropertyValue(searchModel, ExpressionHelper.GetNameOfProperty(item.func))
            });
        }
        return this;
    }
}
public class TestSearchModel
{
    public string Name { get; set; }
    public int Value { get; set; }
}
public enum NavigationTypes
{
    OneToOne,
    ManyToOne,
    ManyToMany
}

internal class INavigationUpdaterCreator<TRoot>(NavigationTypes navigationType) where TRoot : class
{
    protected List<INavigationUpdaterCreator<TRoot>> navigationPropertyUpdaters = [];
    private readonly NavigationTypes navigationType = navigationType;

    INavigationUpdaterCreator<TRoot> AddInner(INavigationUpdaterCreator<TRoot> navigationUpdaterCreator)
    {
        var request = new GetRequest<TestSearchModel>().HasFilter(t => t.Name, FilterOperations.Like, "WriterMember.FirstName").HasFilter(t => t.Value, FilterOperations.GreaterThanOrEqual, "FinalScore");
        TestSearchModel searchModel = new()
        {
            Name = "Ali",
            Value = 12
        };
        request.CreateRequest(searchModel);

        navigationPropertyUpdaters.Add(navigationUpdaterCreator);
        return this;
    }
}