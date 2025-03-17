
using CatalogAPI.Products.GetProductsByCategory;

namespace CatalogAPI.Products.UpdateProduct
{

    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);


    public class UpdateProductCommandHandler(IDocumentSession session, ILogger<GetProductsByCategoryQuery> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"UpdateProductCommandHandler.Handle called with Command: {command}");
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken).ConfigureAwait(false);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }
            // update  product entity from command object
            product.Name = command.Name;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;
            product.Category = command.Category;

            // save to databse
            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);
            // return UpdateProductResult
            return new UpdateProductResult(true);
        }
    }
}
