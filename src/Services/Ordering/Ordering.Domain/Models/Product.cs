﻿using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Models;

public class Product: Entity<ProductId>
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }= default!;
    public static Product Create(ProductId productId, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        var product = new Product
        {
            Id = productId,
            Name = name,
            Price = price
        };
        return product;

    }

}
