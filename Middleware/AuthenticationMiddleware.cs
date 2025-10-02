using Microsoft.AspNetCore.Components.Authorization;
using PNC.Services;

namespace PNC.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Ne pas rediriger si c'est la page de login ou logout
        var path = context.Request.Path.Value?.ToLower();
        if (path == "/login" || path == "/logout")
        {
            await _next(context);
            return;
        }

        // Vérifier si l'utilisateur est authentifié via notre CustomAuthenticationStateProvider
        var authStateProvider = context.RequestServices.GetRequiredService<CustomAuthenticationStateProvider>();
        var authState = await authStateProvider.GetAuthenticationStateAsync();

        if (authState.User.Identity?.IsAuthenticated == true)
        {
            // L'utilisateur est authentifié, continuer
            await _next(context);
        }
        else
        {
            // L'utilisateur n'est pas authentifié, rediriger vers la page de login
            context.Response.Redirect("/login");
            return;
        }
    }
}
