﻿
namespace SampleMicroService.Core.Models;

public sealed class ProductSpecifications
{
    public string Id { get; set; } = string.Empty;    
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageFile { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
