using GerenciamentoTarefas.Data;
using GerenciamentoTarefas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext (Banco de dados)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro do ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;  // Ou true, se desejar confirmação de conta
    options.User.RequireUniqueEmail = true;  // Exigir emails únicos para usuários
})
.AddEntityFrameworkStores<ApplicationDbContext>()  // Conectando o Identity com o ApplicationDbContext
.AddDefaultTokenProviders();  // Provedores de tokens padrão (para reset de senha, etc.)

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de requisição HTTP
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

app.UseAuthentication();  // Habilita a autenticação
app.UseAuthorization();  // Habilita a autorização

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
