
namespace CatalogAPI.Products.GetProductsByCategory
{

    public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<Product> Products);


    public class GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQuery> logger) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {

        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation($"GetProductsByCategoryQueryHandler.Handle called with Query: {query}");

            var products = await session.Query<Product>().Where( p=>p.Category.Contains(query.Category)).ToListAsync(cancellationToken).ConfigureAwait(false);
            
            return new GetProductsByCategoryResult(products);
        }
    }
}
