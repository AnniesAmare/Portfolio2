using System.Runtime.InteropServices.ComTypes;
using System.Text;
using DataLayer;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebServer.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();


//DATASERVICES
builder.Services.AddSingleton<IDataserviceSpecificPerson, DataserviceSpecificPerson>();
builder.Services.AddSingleton<IDataserviceSpecificTitle, DataserviceSpecificTitle>();
builder.Services.AddSingleton<IDataserviceUsers, DataserviceUsers>();
builder.Services.AddSingleton<IDataserviceBookmarks, DataserviceBookmarks>();
builder.Services.AddSingleton<IDataserviceTitles, DataserviceTitles>();
builder.Services.AddSingleton<IDataservicePersons, DataservicePersons>();
builder.Services.AddSingleton<IDataserviceSearches, DataserviceSearches>();
builder.Services.AddSingleton<IDataserviceUserRatings, DataserviceUserRatings>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<Hashing>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Auth:secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.Run();