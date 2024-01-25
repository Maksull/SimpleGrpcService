using Application.Mediatr.Commands.Products;
using Application.Mediatr.Queries.Products;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using SimpleGrpcProject;

namespace GrpcService.Services;

public sealed class GrpcProductService : ProductServiceProto.ProductServiceProtoBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<GrpcProductService> _logger;

    public GrpcProductService(IMediator mediator, IMapper mapper, ILogger<GrpcProductService> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
    {
        var products = await _mediator.Send(new GetProductsQuery());

        var grpcProducts = products.Select(coreProduct => _mapper.Map<Product>(coreProduct));

        return new GetProductsResponse
        {
            Products = { grpcProducts }
        };
    }

    public override async Task GetProductsStream(GetProductsRequest request,
        IServerStreamWriter<GetProductResponse> responseStream, ServerCallContext context)
    {
        var products = await _mediator.Send(new GetProductsQuery(), context.CancellationToken);

        foreach (var coreProduct in products)
        {
            var grpcProductsResponse = new GetProductResponse
            {
                Product = _mapper.Map<Product>(coreProduct)
            };

            await responseStream.WriteAsync(grpcProductsResponse, context.CancellationToken);
        }
    }

    public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId));

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product was not found"));

        var grpcProduct = _mapper.Map<Product>(product);

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

            var grpcProduct = _mapper.Map<Product>(product);

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

            var grpcProduct = _mapper.Map<Product>(product);
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
        var product = await _mediator.Send(
            new CreateProductCommand(request.Name, request.Description, request.CategoryId),
            context.CancellationToken);

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product's category was not found"));

        var grpcProduct = _mapper.Map<Product>(product);

        return new CreateProductResponse()
        {
            Product = grpcProduct
        };
    }

    public override async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request,
        ServerCallContext context)
    {
        var product = await _mediator.Send(new UpdateProductCommand(request.Product.ProductId, request.Product.Name,
                request.Product.Description, request.Product.CategoryId),
            context.CancellationToken);

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product's category was not found"));

        var grpcProduct = _mapper.Map<Product>(product);

        return new UpdateProductResponse
        {
            Product = grpcProduct
        };
    }

    public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request,
        ServerCallContext context)
    {
        var product = await _mediator.Send(new DeleteProductCommand(request.ProductId),
            context.CancellationToken);

        if (product is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Product's category was not found"));

        var grpcProduct = _mapper.Map<Product>(product);

        return new DeleteProductResponse()
        {
            Product = grpcProduct
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
            var response = await _mediator.Send(new GetPagedProductsQuery(
                    request.Page, request.PageSize, request.SortOrder),
                context.CancellationToken);

            var grpcProducts = response.Items.Select(coreProduct => _mapper.Map<Product>(coreProduct));

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
                    request.SortOrder),
                context.CancellationToken);

            var grpcProducts = response.Items.Select(coreProduct => _mapper.Map<Product>(coreProduct));

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