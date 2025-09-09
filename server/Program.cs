var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors( options => {
    options.AddPolicy( "AllowLocalFrontend",
        policy => {
            policy.WithOrigins( "http://localhost:3000" )
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        } );
} );

var app = builder.Build();

app.Use( async ( context, next ) => {
    try {
        await next.Invoke();
    } catch ( Exception ex ) {
        app.Logger.LogError( $"Caught unhandled exception: {ex}" );
        context.Response.StatusCode = 500;
    }
} );

app.UseRouting();
app.UseCors( "AllowLocalFrontend" );
app.MapControllers();

app.Run();
