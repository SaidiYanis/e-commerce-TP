using Ecommerce.BackOffice.Api.Http;
using Ecommerce.BackOffice.Orders.Application;

namespace Ecommerce.BackOffice.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var orders = app.MapGroup("/orders");
        orders.MapGet("/", async (OrderService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetAllAsync(cancellationToken);
            return Results.Ok(result);
        })
        .Produces<IReadOnlyCollection<OrderDto>>(StatusCodes.Status200OK);

        orders.MapGet("/{id:guid}", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetByIdAsync(id, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        orders.MapPost("/", async (CreateOrderCommand command, OrderService service, CancellationToken cancellationToken) =>
        {
            var result = await service.CreateAsync(command, cancellationToken);
            return result.ToHttpResult(dto => $"/orders/{dto.Id}");
        })
        .Produces<OrderDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        orders.MapPost("/{id:guid}/lines", async (Guid id, AddOrderLineCommand command, OrderService service, CancellationToken cancellationToken) =>
        {
            var result = await service.AddProductAsync(id, command, cancellationToken);
            return result.ToHttpResult();
        })
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        orders.MapPost("/{id:guid}/pay", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
        {
            var result = await service.PayAsync(id, cancellationToken);
            return result.ToHttpResult();
        })
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        orders.MapPost("/{id:guid}/cancel", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
        {
            var result = await service.CancelAsync(id, cancellationToken);
            return result.ToHttpResult();
        })
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        orders.MapPost("/{id:guid}/deliver", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
        {
            var result = await service.DeliverAsync(id, cancellationToken);
            return result.ToHttpResult();
        })
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
