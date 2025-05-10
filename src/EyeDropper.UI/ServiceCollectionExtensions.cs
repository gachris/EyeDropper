using DevToolbox.Core.Contracts;
using DevToolbox.Wpf.Services;
using EyeDropper.UI.Contracts;
using EyeDropper.UI.ViewModels;
using EyeDropper.UI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EyeDropper.UI;

/// <summary>
/// Provides extension methods for registering UI services and view models into the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers UI-related services, dialog services, and view models with the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add UI services to.</param>
    /// <returns>The original <see cref="IServiceCollection"/>, for chaining.</returns>
    public static IServiceCollection AddUI(this IServiceCollection services)
    {
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<IDialogService, DialogService>();

        services.AddSingleton<AdvanceColorPickerDialogViewModel>();
        services.AddSingleton<EyeDropperViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<TaskbarViewModel>();

        services.AddSingleton<IEyeDropperService, EyeDropperHost>();

        return services;
    }
}
