using Application.Mediatr.Commands.Categories;
using Application.Mediatr.Queries.Categories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using SimpleGrpcProject.v1;

namespace GrpcService.Services.v1;

public sealed class GrpcCategoryService : CategoryServiceProto.CategoryServiceProtoBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<GrpcCategoryService> _logger;

    public GrpcCategoryService(IMediator mediator, IMapper mapper, ILogger<GrpcCategoryService> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }
    
    public override async Task<GetCategoriesResponse> GetCategories(GetCategoriesRequest request,
        ServerCallContext context)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), context.CancellationToken);

        var grpcCategories = categories.Select(coreCategory => _mapper.Map<Category>(coreCategory));

        return new GetCategoriesResponse
        {
            Categories = { grpcCategories }
        };
    }

    public override async Task GetCategoriesStream(GetCategoriesRequest request,
        IServerStreamWriter<GetCategoryResponse> responseStream, ServerCallContext context)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), context.CancellationToken);

        foreach (var coreCategory in categories)
        {
            var grpcCategoryResponse = new GetCategoryResponse
            {
                Category = _mapper.Map<Category>(coreCategory)
            };

            await responseStream.WriteAsync(grpcCategoryResponse, context.CancellationToken);
        }
    }

    public override async Task<GetCategoryResponse> GetCategory(GetCategoryRequest request, ServerCallContext context)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(request.CategoryId),
            context.CancellationToken);

        if (category is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Category was not found"));

        var grpcCategory = _mapper.Map<Category>(category);

        return new GetCategoryResponse
        {
            Category = grpcCategory
        };
    }


    public override async Task<GetCategoriesResponse> GetMultipleCategory(
        IAsyncStreamReader<GetCategoryRequest> requestStream, ServerCallContext context)
    {
        var categories = new GetCategoriesResponse();

        await foreach (var request in requestStream.ReadAllAsync())
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(request.CategoryId),
                context.CancellationToken);

            if (category is null) continue;

            var grpcCategory = _mapper.Map<Category>(category);

            categories.Categories.Add(grpcCategory);
        }

        return categories;
    }

    public override async Task GetCategoryBidirectional(IAsyncStreamReader<GetCategoryRequest> requestStream,
        IServerStreamWriter<GetCategoryResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(request.CategoryId),
                context.CancellationToken);

            if (category is null) continue;

            var grpcCategory = _mapper.Map<Category>(category);
            var categoryResponse = new GetCategoryResponse
            {
                Category = grpcCategory
            };

            await responseStream.WriteAsync(categoryResponse, context.CancellationToken);
        }
    }

    public override async Task<CreateCategoryResponse> CreateCategory(CreateCategoryRequest request,
        ServerCallContext context)
    {
        var category = await _mediator.Send(new CreateCategoryCommand(request.CategoryName), context.CancellationToken);

        var grpcCategory = _mapper.Map<Category>(category);

        return new CreateCategoryResponse
        {
            Category = grpcCategory
        };
    }

    public override async Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request,
        ServerCallContext context)
    {
        var category = await _mediator.Send(
            new UpdateCategoryCommand(request.CategoryId, request.Name),
            context.CancellationToken);

        if (category is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Category was not found"));

        var grpcCategory = _mapper.Map<Category>(category);

        return new UpdateCategoryResponse
        {
            Category = grpcCategory
        };
    }

    public override async Task<DeleteCategoryResponse> DeleteCategory(DeleteCategoryRequest request,
        ServerCallContext context)
    {
        var category = await _mediator.Send(new DeleteCategoryCommand(request.CategoryId),
            context.CancellationToken);

        if (category is null)
            throw new RpcException(new Status(StatusCode.NotFound, "The Category was not found"));

        var grpcCategory = _mapper.Map<Category>(category);

        return new DeleteCategoryResponse
        {
            Category = grpcCategory
        };
    }

    public override async Task<Empty> PrintCategory(IAsyncStreamReader<PrintCategoryRequest> requestStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            _logger.LogInformation("Print category {PrintCategory}", request.Category);
        }

        return new();
    }

    public override async Task GetPagedCategories(IAsyncStreamReader<GetPagedCategoriesRequest> requestStream,
        IServerStreamWriter<PagedCategoriesResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            var response = await _mediator.Send(
                new GetPagedCategoriesQuery(request.Page, request.PageSize, request.SortOrder),
                context.CancellationToken);

            var grpcCategories = response.Items.Select(coreCategory => _mapper.Map<Category>(coreCategory));

            var categoryResponse = new PagedCategoriesResponse
            {
                Page = response.Page,
                PageSize = response.PageSize,
                TotalCount = response.TotalCount,
                Items = { grpcCategories },
                IsNextPage = response.IsNextPage,
                IsPreviousPage = response.IsPreviousPage,
            };

            await responseStream.WriteAsync(categoryResponse, context.CancellationToken);
        }
    }

    public override async Task GetCursorPagedCategories(
        IAsyncStreamReader<GetCursorPagedCategoriesRequest> requestStream,
        IServerStreamWriter<CursorPagedCategoriesResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            var response = await _mediator.Send(new GetCursorPagedCategoriesQuery(request.Cursor,
                    request.PageSize,
                    request.SortOrder),
                context.CancellationToken);

            var grpcCategories = response.Items.Select(coreCategory => _mapper.Map<Category>(coreCategory));

            var categoryResponse = new CursorPagedCategoriesResponse
            {
                Cursor = response.Cursor ?? "",
                PageSize = response.PageSize,
                Items = { grpcCategories },
            };

            await responseStream.WriteAsync(categoryResponse, context.CancellationToken);
        }
    }
}