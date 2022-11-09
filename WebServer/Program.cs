using DataLayer;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IDataService, DataService>();

//DATASERVICES
builder.Services.AddSingleton<IDataserviceSpecificPerson, DataserviceSpecificPerson>();
builder.Services.AddSingleton<IDataserviceSpecificTitle, DataserviceSpecificTitle>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.MapControllers();
app.Run();