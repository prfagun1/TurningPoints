using api.Helpers;
using Newtonsoft.Json;
using repository;
using repository.Interface;

var builder = WebApplication.CreateBuilder(args);


string? connectionStringTurningPoints = null;
string? jwtKey = null;

int rateLimitQuantity;
int rateLimitTime;
int rateLimitQueue;


(connectionStringTurningPoints, jwtKey, rateLimitQuantity, rateLimitTime, rateLimitQueue) =
    api.Extensions.ServiceExtensions.GetParameters(builder, "ConnectionStrings:TurningPoints", "Jwt:key", "RateLimit:Quantity", "RateLimit:Time", "RateLimit:Queue");


api.Extensions.ServiceExtensions.ConfigureJWT(builder, jwtKey);
api.Extensions.ServiceExtensions.ConfigureSwagger(builder);
api.Extensions.ServiceExtensions.ConfigureContext(builder, connectionStringTurningPoints);
api.Extensions.ServiceExtensions.ConfigureControllerPolicy(builder, rateLimitQuantity, rateLimitTime, rateLimitQueue);

builder.Services.AddControllers();

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Newtonsoft.Json.Formatting.Indented,
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
};



builder.Services.AddLocalization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();


string[] supportedCultures = ["en-US", "pt-BR"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);


app.UseRequestLocalization(localizationOptions);


app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
);


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}



app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();



app.Logger.LogInformation("Iniciando aplicacao");

// Obter IHttpContextAccessor e adicionar MySqlLoggerProvider
var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
app.Services.GetRequiredService<ILoggerFactory>().AddProvider(new MySqlLoggerProvider(connectionStringTurningPoints, httpContextAccessor));


app.Run();
