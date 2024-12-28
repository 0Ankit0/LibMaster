using Microsoft.Extensions.DependencyInjection;


namespace Authconfig.Configuration
{
    public static class AuthPolicy
    {
        // This method will configure the authentication policy using external configuration
        //to use this simply add:
        //builder.Services.AddCustomAuthenticationPolicy("AdminOrManagerPolicy", new[] { "Admin", "Manager" });

        public static void AddCustomAuthPolicy(this IServiceCollection services, string policyName, string[] requiredRoles)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    foreach (var role in requiredRoles)
                    {
                        policy.RequireRole(role); // Require all roles passed from the main project
                    }
                });
            });
        }
    }
}
