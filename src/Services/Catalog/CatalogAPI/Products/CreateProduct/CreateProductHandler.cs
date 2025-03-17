
using CatalogAPI.Products.GetProductsByCategory;

namespace CatalogAPI.Products.CreateProduct
{

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);


    public class CreateProductCommandHandler(IDocumentSession session, ILogger<GetProductsByCategoryQuery> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"CreateProductCommandHandler.Handle called with Command: {command}");
            // create product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Category = command.Category,
            };

            // save to databse
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // return CreateProductResult
            return new CreateProductResult(product.Id);
        }
    }
}
