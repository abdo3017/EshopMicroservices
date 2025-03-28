﻿namespace CatalogAPI.Products.GetProductsByCategory
{

    public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

    public class GetProductsByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category,int PageNumber, int PageSize, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByCategoryQuery(category, PageNumber, PageSize));
                var response = result.Adapt<GetProductsByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductsByCategory")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products By Category")
            .WithDescription("Get Products By Category");
        }
    }
}
