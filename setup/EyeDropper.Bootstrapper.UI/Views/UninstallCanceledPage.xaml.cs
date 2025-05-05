using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class UninstallCanceledPage : Page
{
    public UninstallCanceledPage(UninstallCanceledViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}