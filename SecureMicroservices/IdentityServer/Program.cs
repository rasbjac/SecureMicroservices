using IdentityServer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddDeveloperSigningCredential();

builder.Services.AddRazorPages();

var app = builder.Build();

// --- Configure middleware pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// This is important: UseIdentityServer includes authentication middleware needed by it
app.UseIdentityServer();

app.UseAuthorization();

// Map endpoints / UI / controllers
app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();
