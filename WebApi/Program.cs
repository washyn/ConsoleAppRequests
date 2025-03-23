using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services
    .AddOptions<JwtBearer>()
    .BindConfiguration(nameof(JwtBearer));


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            // The signing key must match!
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearer:SecurityKey"])),

            // Validate the JWT Issuer (iss) claim
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtBearer:Issuer"],

            // Validate the JWT Audience (aud) claim
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtBearer:Audience"],

            // Validate the token expiry
            ValidateLifetime = true,

            ClockSkew = TimeSpan.FromMinutes(0), // para que pasados 0 min expire el token...
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Name = "Authorization",
        Description = "",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
    });

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "App API", Version = "v1" });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.DocInclusionPredicate((docName, description) => true);
    options.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
