using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PNC.Components;
using PNC.Data;
using PNC.Middleware;
using PNC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // Configuration pour améliorer la stabilité des connexions
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(5);
        options.MaxBufferedUnacknowledgedRenderBatches = 10;
        // Activer les erreurs détaillées pour le débogage
        options.DetailedErrors = true;
    });

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Keys\DataProtection"))
    .SetApplicationName("PNC");


builder.WebHost.UseIISIntegration();

// Configuration du circuit Blazor Server pour les gros messages
builder.Services.Configure<CircuitOptions>(options =>
{
    // Augmenter la taille maximale des messages JavaScript (10MB)
    options.MaxBufferedUnacknowledgedRenderBatches = 10;
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(5);
});

// Ajouter les contrôleurs API
builder.Services.AddControllers();

// Ajouter HttpContextAccessor pour l'accès au HttpContext dans Blazor
builder.Services.AddHttpContextAccessor();

// Configuration pour la stabilité des connexions SignalR/Blazor
builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.EnableDetailedErrors = true;
    // Augmenter la taille maximale des messages pour les photos (10MB)
    options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
});

// Configuration de l'authentification personnalisée (en mémoire)

// Enregistrement des services métier
builder.Services.AddScoped<IPolicierValidationService, PolicierValidationService>();
builder.Services.AddScoped<IPolicierCollectionService, PolicierCollectionService>();
builder.Services.AddScoped<IPolicierService, PolicierService>();
builder.Services.AddScoped<IPolicierStatisticsService, PolicierStatisticsService>();
builder.Services.AddScoped<ICommissariatService, CommissariatService>();
builder.Services.AddScoped<IDateService, DateService>();
builder.Services.AddScoped<GeographieRdcService>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IEmpreinteService, EmpreinteService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<INutpService, NutpService>();

// Services de gestion des utilisateurs et rôles
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Services d'authentification
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddDbContextFactory<BdPolicePncContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.CommandTimeout(1200) // Timeout en secondes (ici 20 minutes)
    ));



var app = builder.Build();

// Réinitialisation automatique du mot de passe au démarrage
using (var scope = app.Services.CreateScope())
{
    try
    {
        var utilisateurService = scope.ServiceProvider.GetRequiredService<IUtilisateurService>();
        await ResetPasswordOnStartup(utilisateurService);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de la réinitialisation du mot de passe: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Configuration de l'authentification personnalisée
// app.UseMiddleware<AuthenticationMiddleware>(); // Temporairement désactivé

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Mapper les contrôleurs API
app.MapControllers();

// Méthode pour réinitialiser le mot de passe au démarrage
static async Task ResetPasswordOnStartup(IUtilisateurService utilisateurService)
{
    try
    {
        const string email = "joelmuhindok@gmail.com";
        const string newPassword = "GOPgUnbP3CzS";
        
        Console.WriteLine($"🔄 Recherche de l'utilisateur avec l'email: {email}");
        
        // Rechercher l'utilisateur par email
        var utilisateur = await utilisateurService.GetUtilisateurByEmailAsync(email);
        
        if (utilisateur != null)
        {
            Console.WriteLine($"✅ Utilisateur trouvé: {utilisateur.Nom} {utilisateur.Prenom} ({utilisateur.NomUtilisateur})");
            
            // Mettre à jour le mot de passe
            var success = await utilisateurService.ResetMotDePasseAsync(utilisateur.Id, newPassword);
            
            if (success)
            {
                Console.WriteLine($"🔑 Mot de passe réinitialisé avec succès pour {email}");
                Console.WriteLine($"📧 Email: {email}");
                Console.WriteLine($"🔐 Nouveau mot de passe: {newPassword}");
                Console.WriteLine(new string('=', 50));
            }
            else
            {
                Console.WriteLine($"❌ Échec de la réinitialisation du mot de passe pour {email}");
            }
        }
        else
        {
            Console.WriteLine($"❌ Aucun utilisateur trouvé avec l'email: {email}");
            Console.WriteLine("💡 Vérifiez que l'utilisateur existe dans la base de données");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erreur lors de la réinitialisation du mot de passe: {ex.Message}");
    }
}

app.Run();