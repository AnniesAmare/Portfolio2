using Datalayer;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//builder.Services.AddMvcCore();
builder.Services.AddControllers();
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();


/*
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseRouting();
*/

app.MapControllers();
app.Run();