
namespace CatalogAPI.Products.CreateProduct
{

    public record GetProductsQuery() : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);


    public class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQuery> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {

        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation($"GetProductsQueryHandler.Handle called with Query: {query}");

            var products = await session.Query<Product>().ToListAsync(cancellationToken).ConfigureAwait(false);
            
            return new GetProductsResult(products);
        }
    }
}
