using Discount.Grpc;
using Dsiscount.Grpc;
using Dsiscount.Grpc.Data;
using Dsiscount.Grpc.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Dsiscount.Grpc.Services
{
    public class DiscountService(ILogger<DiscountService> logger, DiscountContext discountContext)
        : DiscountProtoService.DiscountProtoServiceBase
    {

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            return coupon.Adapt<CouponModel>();
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            discountContext.Coupons.Add(coupon);
            var added = await discountContext.SaveChangesAsync();
            if (added == 1)
                logger.LogInformation("Discount is successfully added for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            else
                logger.LogError("Discount not added for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            return request.Coupon;

        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            discountContext.Coupons.Update(coupon);
            var updated = await discountContext.SaveChangesAsync();
            if (updated == 1)
                logger.LogInformation("Discount is successfully updated for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            else
                logger.LogError("Discount not updated for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            return request.Coupon;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

            discountContext.Coupons.Remove(coupon);
            var deleted = await discountContext.SaveChangesAsync();
            if (deleted == 1)
                logger.LogInformation("Discount is deleted for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
            else
                logger.LogError("Discount not deleted for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            return new DeleteDiscountResponse { Success = deleted == 1 };
        }
    }
}
