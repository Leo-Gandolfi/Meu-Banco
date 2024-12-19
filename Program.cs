var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("CadastroAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5089/api/");
});

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseAuthorization(); 

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();  
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
