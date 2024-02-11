using Application.Mediatr.Commands.Categories;
using Infrastructure.Data;
using MediatR;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Categories;

public sealed class DeleteCategoriesPermanentlyHandler : IRequestHandler<DeleteCategoriesPermanentlyCommand, DeleteResult>
{
    private readonly ApiDataContext _apiDataContext;

    public DeleteCategoriesPermanentlyHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }
    
    public async Task<DeleteResult> Handle(DeleteCategoriesPermanentlyCommand request, CancellationToken cancellationToken)
    {
        var deleteResult = await _apiDataContext.DeletedCategoriesDocuments.DeleteManyAsync(
            d => d["deleteAt"] <= DateTime.UtcNow, cancellationToken);
        
        return deleteResult;
    }
}