using Application.Mediatr.Commands.Categories;
using MediatR;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class DeleteCategoriesPermanentlyJob : IJob
{
    private readonly IMediator _mediator;

    public DeleteCategoriesPermanentlyJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await _mediator.Send(new DeleteCategoriesPermanentlyCommand(), context.CancellationToken);
    }
}