using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Entities;
using NKRY_API.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("NKRY-APIContext") ??
throw new InvalidOperationException("Connections string: NKRY-APIContext was not found")));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

// set up Jwt auth services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

    });


// Add services to the container.
builder.Services.AddControllers(setupAction =>
{
    setupAction.ReturnHttpNotAcceptable = true; // disallow unsupported contentTypes
}).AddXmlDataContractSerializerFormatters();
// Add this service to the container to get run on runtime
builder.Services.ConfigureUnitOfWork();

builder.Services.ConfigureAutoMapper();

builder.Services.AddSwaggerGen(
    c =>
    {
        // configurations to add the authorize lock symbol on Swagger UI
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "NKRY API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. <BR/>  
                      Enter 'Bearer' [space] and then your token in the text input below.
                      <BR/> Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
        //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //c.IncludeXmlComments(xmlPath);
    }
    );

// Add Cors services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    //options.AddPolicy("AllowSpecificOrigin", builder =>
    //{
    //    builder
    //        //.WithOrigins("http://localhost:3000") // Allow requests from your frontend application
    //        .AllowAnyOrigin()
    //        .AllowAnyHeader()
    //        .AllowAnyMethod();
    //});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("A Fault occurred in the server. Please Try again later");
        });
    });
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSwagger();

// This middleware serves the Swagger documentation UI
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NKRY API V1");
    c.RoutePrefix = string.Empty; // swagger UI now is accessible in the root path of the app
});

app.UseCors();

// set up our api to use attribute based routing
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
}
);

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers().RequireAuthorization();
//    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
//});

app.Run();
