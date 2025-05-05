using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class InstallProgressPage : Page
{
    public InstallProgressPage(InstallProgressViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}