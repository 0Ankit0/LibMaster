using Microsoft.Extensions.DependencyInjection;


namespace Authconfig.Configuration
{
    /// <summary>
    /// Configures a custom authentication policy using external configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the policy to.</param>
    /// <param name="policyName">The name of the policy to be added.</param>
    /// <param name="requiredRoles">An array of roles required for the policy.</param>
    /// <remarks>
    /// This method will configure the authentication policy using external configuration.
    /// To use this, simply add:
    /// <code>
    /// builder.Services.AddCustomAuthenticationPolicy("AdminOrManagerPolicy", new[] { "Admin", "Manager" });
    /// </code>
    /// </remarks>
    public static class AuthPolicy
    {
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
