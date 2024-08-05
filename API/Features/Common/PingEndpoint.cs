using FastEndpoints;

namespace Presentation.Features.Common;

public class PingEndpoint : EndpointWithoutRequest<bool>
{
    public override void Configure()
    {
        Post("ping");
        AllowAnonymous();
        Options(o => o.WithTags("Common"));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        await SendAsync(true, 200, cancellationToken);
    }
}