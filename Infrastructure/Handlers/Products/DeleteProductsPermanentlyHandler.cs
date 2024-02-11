using Application.Mediatr.Commands.Products;
using Infrastructure.Data;
using MediatR;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Products;

public sealed class DeleteProductsPermanentlyHandler : IRequestHandler<DeleteProductsPermanentlyCommand, DeleteResult>
{
    private readonly ApiDataContext _apiDataContext;

    public DeleteProductsPermanentlyHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }
    
    public async Task<DeleteResult> Handle(DeleteProductsPermanentlyCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await _apiDataContext.DeletedProductsDocuments.DeleteManyAsync(
            d => d["deleteAt"] <= DateTime.UtcNow, cancellationToken);
        
        return deleteResult;
    }
}