var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Name = "Ecommerce BackOffice API",
    Status = "Bootstrapped"
}));

app.Run();
