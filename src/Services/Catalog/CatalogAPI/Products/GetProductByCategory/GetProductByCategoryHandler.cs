
namespace CatalogAPI.Products.GetProductsByCategory
{

    public record GetProductsByCategoryQuery(string Category, int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<Product> Products);


    public class GetProductsByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {

        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>().Where(p => p.Category.Contains(query.Category)).ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken).ConfigureAwait(false);

            return new GetProductsByCategoryResult(products);
        }
    }
}
