#region     with use of MediatR coupled with CQRS, i.e  
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand {}
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
{
}
#endregion

internal static sealed class ResultExtensions 
{
    internal static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> res, Func<TIn, Task<Result<TOut>>> func)
    {
        if (res.IsFailure) return res.Failure<TOut>(res.Errors);
        else return await func(res.Value);
    }

    public async Task<IActionResult> UpdateMember(
        Guid id, [FromBody] UpdateMemberRequest req, CancellationToken ctk
    )

    return await Result
    .Create(
        new UpdateUserCommand(
            id, 
            req.FirstName,
            req.LastName))
    .Bind(command => Sender.Send(command, ctk))
    .Match(() => NoContent(), result => HandleFailure())
}
