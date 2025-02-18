using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = dbContext
           .Coupons
           .FirstOrDefaultAsync(x => x.ProductName == request.ProductName, cancellationToken: context.CancellationToken).Result;
        
        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

        logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return Task.FromResult(couponModel);

    }
    public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<CouponModel, Coupon>().Ignore(dest => dest.Id);  
        var coupon = request.Coupon.Adapt<Coupon>(config);
        
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        dbContext.Coupons.AddAsync(coupon, cancellationToken: context.CancellationToken);
        dbContext.SaveChangesAsync(cancellationToken: context.CancellationToken);
        logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return Task.FromResult(couponModel);
    }

    public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
        dbContext.Coupons.Update(coupon);
        dbContext.SaveChangesAsync(cancellationToken: context.CancellationToken);
        logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return Task.FromResult(couponModel);
    }
    public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = dbContext
          .Coupons
          .FirstOrDefaultAsync(x => x.ProductName == request.ProductName).Result;

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

        dbContext.Coupons.Remove(coupon);
        dbContext.SaveChangesAsync(cancellationToken: context.CancellationToken);

        logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", request.ProductName);

        return Task.FromResult(new DeleteDiscountResponse { Success = true });
    }
    public override Task<CouponModelList> GetAllDiscounts(Empty request, ServerCallContext context)
    {
        var coupons = dbContext.Coupons.ToListAsync(cancellationToken: context.CancellationToken).Result;
        var response = new CouponModelList
        {
            Coupons = { coupons.Adapt<List<CouponModel>>() }  // 🔥 Map list automatically
        };
        return Task.FromResult(response);
    }
}
