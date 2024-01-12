using Application.Mediatr.Commands.Products;
using Application.Mediatr.Queries.Products;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using SimpleGrpcProject;

namespace GrpcService.Services;

public sealed class GrpcProductService : ProductServiceProto.ProductServiceProtoBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GrpcProductService> _logger;

    public GrpcProductService(IMediator mediator, ILogger<GrpcProductService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
    {
        var products = await _mediator.Send(new GetProductsQuery());

        var grpcProducts = products.Select(coreProduct => new Product
        {
            ProductId = coreProduct.ProductId.ToString(),
            Name = coreProduct.Name,
            Description = coreProduct.Description,
            CategoryId = coreProduct.CategoryId.ToString()
        });

        return new GetProductsResponse
        {
            Products = { grpcProducts }
        };
    }

    public override async Task GetProductsStream(GetProductsRequest request,
        IServerStreamWriter<GetProductResponse> responseStream, ServerCallContext context)
    {
        var products = await _mediator.Send(new GetProductsQuery(), context.CancellationToken);

        try
        {
            foreach (var coreProduct in products)
            {
                var grpcProductsResponse = new GetProductResponse
                {
                    Product = new()
                    {
                        ProductId = coreProduct.ProductId,
                        Name = coreProduct.Name,
                        Description = coreProduct.Description,
                        CategoryId = coreProduct.CategoryId
                    }
                };


                await responseStream.WriteAsync(grpcProductsResponse, context.CancellationToken);
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation("Operation was cancelled {CancelledOperationException}", ex.ToString());
        }
    }

    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId));

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product was not found"));

        var grpcProduct = new Product
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId
        };

        return new GetProductResponse
        {
            Product = grpcProduct
        };
    }

    public override async Task<GetProductsResponse> GetMultipleProduct(
        IAsyncStreamReader<GetProductRequest> requestStream, ServerCallContext context)
    {
        var products = new GetProductsResponse();

        await foreach (var request in requestStream.ReadAllAsync())
        {
            var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId),
                context.CancellationToken);

            if (product is null) continue;

            var grpcProduct = new Product
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId
            };

            products.Products.Add(grpcProduct);
        }

        return products;
    }

    public override async Task GetProductBidirectional(IAsyncStreamReader<GetProductRequest> requestStream,
        IServerStreamWriter<GetProductResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId),
                context.CancellationToken);

            if (product is null) continue;

            var grpcProduct = new Product
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId
            };
            var productResponse = new GetProductResponse
            {
                Product = grpcProduct
            };

            await responseStream.WriteAsync(productResponse, context.CancellationToken);
        }
    }

    public override async Task<CreateProductResponse> CreateProduct(CreateProductRequest request,
        ServerCallContext context)
    {
        Domain.Entities.Product newProduct = new()
        {
            ProductId = Ulid.Empty.ToString(),
            Name = request.Product.Name,
            Description = request.Product.Description,
            CategoryId = request.Product.CategoryId,
        };

        var product = await _mediator.Send(new CreateProductCommand(newProduct), context.CancellationToken);

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product's category was not found"));

        return new CreateProductResponse()
        {
            Product = new()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
            }
        };
    }

    public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request,
        ServerCallContext context)
    {
        Domain.Entities.Product updateProduct = new()
        {
            ProductId = request.Product.ProductId,
            Name = request.Product.Name,
            Description = request.Product.Description,
            CategoryId = request.Product.CategoryId,
        };

        var product = await _mediator.Send(new UpdateProductCommand(updateProduct), context.CancellationToken);

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product's category was not found"));

        return new UpdateProductResponse
        {
            Product = new()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
            }
        };
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request,
        ServerCallContext context)
    {
        var product = await _mediator.Send(new DeleteProductCommand(request.ProductId),
            context.CancellationToken);

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product's category was not found"));

        return new DeleteProductResponse()
        {
            Product = new()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
            }
        };
    }

    public override async Task<Empty> PrintProduct(IAsyncStreamReader<PrintProductRequest> requestStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            _logger.LogInformation("Print product {PrintProduct}", request.Product);
        }

        return new();
    }

    public override async Task GetPagedProducts(IAsyncStreamReader<GetPagedProductsRequest> requestStream,
        IServerStreamWriter<PagedProductsResponse> responseStream, ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            var response = await _mediator.Send(new GetPagedProductsQuery(request.SearchTerm, request.SortColumn,
                    request.SortOrder, request.Page, request.PageSize),
                context.CancellationToken);

            var grpcProducts = response.Items.Select(coreProduct => new Product
            {
                ProductId = coreProduct.ProductId.ToString(),
                Name = coreProduct.Name,
                Description = coreProduct.Description,
                CategoryId = coreProduct.CategoryId.ToString()
            });

            var categoryResponse = new PagedProductsResponse()
            {
                Page = response.Page,
                PageSize = response.PageSize,
                TotalCount = response.TotalCount,
                Items = { grpcProducts },
                IsNextPage = response.IsNextPage,
                IsPreviousPage = response.IsPreviousPage,
            };

            await responseStream.WriteAsync(categoryResponse, context.CancellationToken);
        }
    }

    public override async Task GetCursorPagedProducts(IAsyncStreamReader<GetCursorPagedProductsRequest> requestStream,
        IServerStreamWriter<CursorPagedProductsResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            var response = await _mediator.Send(new GetCursorPagedProductsQuery(request.Cursor,
                    request.PageSize,
                    request.SearchTerm, request.SortColumn, request.SortOrder),
                context.CancellationToken);

            var grpcProducts = response.Items.Select(coreProduct => new Product
            {
                ProductId = coreProduct.ProductId.ToString(),
                Name = coreProduct.Name,
                Description = coreProduct.Description,
                CategoryId = coreProduct.CategoryId.ToString()
            });

            var productResponse = new CursorPagedProductsResponse
            {
                Cursor = response.Cursor ?? "",
                PageSize = response.PageSize,
                Items = { grpcProducts },
            };

            await responseStream.WriteAsync(productResponse, context.CancellationToken);
        }
    }
}