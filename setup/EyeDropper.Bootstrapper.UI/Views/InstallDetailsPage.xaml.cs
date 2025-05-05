using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class InstallDetailsPage : Page
{
    public InstallDetailsPage(InstallDetailsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}