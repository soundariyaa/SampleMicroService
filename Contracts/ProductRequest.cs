using MediatR;

namespace SampleMicroService.Api.Contracts
{
    public sealed class ProductRequest : IRequest<ProductResponse>
    {
        public string? ProductId { get; set; }
    }
}
