
namespace CatalogAPI.Products.CreateProduct
{

    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Products);


    public class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQuery> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation($"GetProductByIdQueryHandler.Handle called with Query: {query}");

            var product = await session.LoadAsync<Product>(query.Id,cancellationToken).ConfigureAwait(false);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }
            return new GetProductByIdResult(product);
        }
    }
}
