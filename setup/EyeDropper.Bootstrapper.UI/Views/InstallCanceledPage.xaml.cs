using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class InstallCanceledPage : Page
{
    public InstallCanceledPage(InstallCanceledViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}