using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class UninstallProgressPage : Page
{
    public UninstallProgressPage(UninstallProgressViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}