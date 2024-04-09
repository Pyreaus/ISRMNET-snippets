#region with use of prerequisite MediatR coupled with CQRS, i.e  
public interface ICommandHandler<TC> : IRequestHandler<TC, Result> where TC : ICommand {}
public interface ICommandHandler<TC, TResponse> : IRequestHandler<TC, Result<TR>> where TC : ICommand<TR> {}
#endregion
internal static sealed class ResultExtensions 
{
    internal static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> res, Func<TIn, Task<Result<TOut>>> func)
    {
        return res.IsFailure ? res.Failure<TOut>(res.Errors) : await func(res.Value);
    }
    #region Implementation
    // [..]
    public async Task<IActionResult> UpdateMember([PFID] string id,
        [FromBody] UpdateMemberRequest req, CancellationToken ctk)
    {
        return await Result
        .Create(
            new UpdateUserCommand(
                id, 
                req.FirstName,
                req.LastName))
        .Bind(command => Sender.Send(command, ctk))
        .Match(() => NoContent(), result => HandleFailure())
    }
    // [..]
    return await Result.Create(new UpdateUserCommand(id, req.FirstName, req.LastName)
        .Bind(cmd => Sender.Send(cmd, ctk)).Match(() => NoContent(), result => HandleFailure());
    #endregion
}
