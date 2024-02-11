using Application.Mediatr.Commands.Products;
using MediatR;
using Quartz;

namespace Infrastructure.BackgroundJobs;

public sealed class DeleteProductsPermanentlyJob : IJob
{
    private readonly IMediator _mediator;

    public DeleteProductsPermanentlyJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await _mediator.Send(new DeleteProductsPermanentlyCommand(), context.CancellationToken);
    }
}