using Megafon.UI.Forms;

using Microsoft.Extensions.DependencyInjection;

namespace Megafon.UI;

public static class Register
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddTransient<MainForm>();
        
        return services;
    }
}
