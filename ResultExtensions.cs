#region with use of prerequisite MediatR coupled with CQRS, i.e
public interface IQuery<TR> : IRequest<Result<TR>> {}
public interface IQueryHandler<TQ, TR> : IRequestHandler<TQ, Result<TR>> where TQ : IQuery<TR> {}
public interface ICommand<TR> : IRequest<Result<TR>> {}
public interface ICommandHandler<TC, TR> : IRequestHandler<TC, Result<TR>> where TC : ICommand<TR> {}
#endregion
internal static sealed class ResultExtensions 
{
    internal static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> res, Func<TIn, Task<Result<TOut>>> func)
    {
        return res.IsFailure ? res.Failure<TOut>(res.Errors) : await func(res.Value);
    }
}
// <Implementation>
public async Task<IActionResult> UpdateMember(
    [SFID] string id, [FromBody] UpdateMemberRequest req, CancellationToken ctk)
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

                           
                           /** non-generic version:
public sealed record CreateCommand(args) : IRequest;
public sealed record GetUserByIdQuery(args) : IRequest;
internal sealed class CreateCommandHandler : IRequestHandler<CreateCommand> [...]
internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery> [...] **/
