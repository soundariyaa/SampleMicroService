using MediatR;

namespace SampleMicroService.Api.Contracts;

public class CreateProductRequest : Product, IRequest<CreateResponse>;
