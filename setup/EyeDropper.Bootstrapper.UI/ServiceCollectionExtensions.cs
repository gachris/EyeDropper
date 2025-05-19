using EyeDropper.Bootstrapper.UI.Constants;
using DevToolbox.Core.Contracts;
using EyeDropper.Bootstrapper.UI.ViewModels;
using EyeDropper.Bootstrapper.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using DevToolbox.Wpf.Services;

namespace EyeDropper.Bootstrapper.UI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUI(this IServiceCollection services)
    {
        services.AddPageService(options =>
        {
            options.Configure(PageKeys.InstallPage, typeof(InstallPage));
            options.Configure(PageKeys.InstallDetailsPage, typeof(InstallDetailsPage));
            options.Configure(PageKeys.InstallProgressPage, typeof(InstallProgressPage));
            options.Configure(PageKeys.InstallSuccessfulPage, typeof(InstallSuccessfulPage));
            options.Configure(PageKeys.UninstallPage, typeof(UninstallPage));
            options.Configure(PageKeys.UninstallProgressPage, typeof(UninstallProgressPage));
            options.Configure(PageKeys.UninstallSuccessfulPage, typeof(UninstallSuccessfulPage));
            options.Configure(PageKeys.InstallCanceledPage, typeof(InstallCanceledPage));
            options.Configure(PageKeys.UninstallCanceledPage, typeof(UninstallCanceledPage));
            options.Configure(PageKeys.DowngradeDetectedPage, typeof(DowngradeDetectedPage));
            options.Configure(PageKeys.ErrorPage, typeof(ErrorPage));
            options.Configure(PageKeys.ElevatedErrorPage, typeof(ElevateErrorPage));
        });

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<InstallViewModel>();
        services.AddSingleton<InstallDetailsViewModel>();
        services.AddSingleton<InstallProgressViewModel>();
        services.AddSingleton<InstallSuccessfulViewModel>();
        services.AddSingleton<InstallCanceledViewModel>();
        services.AddSingleton<UninstallViewModel>();
        services.AddSingleton<UninstallProgressViewModel>();
        services.AddSingleton<UninstallSuccessfulViewModel>();
        services.AddSingleton<UninstallCanceledViewModel>();
        services.AddSingleton<DowngradeDetectedViewModel>();
        services.AddSingleton<ErrorViewModel>();
        services.AddSingleton<ElevateErrorViewModel>();

        services.AddSingleton<InstallPage>();
        services.AddSingleton<InstallDetailsPage>();
        services.AddSingleton<InstallProgressPage>();
        services.AddSingleton<InstallSuccessfulPage>();
        services.AddSingleton<InstallCanceledPage>();
        services.AddSingleton<UninstallPage>();
        services.AddSingleton<UninstallProgressPage>();
        services.AddSingleton<UninstallSuccessfulPage>();
        services.AddSingleton<UninstallCanceledPage>();
        services.AddSingleton<DowngradeDetectedPage>();
        services.AddSingleton<ErrorPage>();
        services.AddSingleton<ElevateErrorPage>();

        return services;
    }
}