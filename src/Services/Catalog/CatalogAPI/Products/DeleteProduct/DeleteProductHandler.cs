
using CatalogAPI.Products.UpdateProduct;

namespace CatalogAPI.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
        }
    }

    public class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommand> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {

        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation($"DeleteProductCommandHandler.Handle called with Command: {command}");

            var product = await session.LoadAsync<Product>(command.Id,cancellationToken).ConfigureAwait(false);
            if (product is null)
            {
                throw new ProductNotFoundException(command.Id);
            }
            session.Delete(product);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
