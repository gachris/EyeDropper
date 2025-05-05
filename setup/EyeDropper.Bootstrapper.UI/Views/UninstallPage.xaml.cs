using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class UninstallPage : Page
{
    public UninstallPage(UninstallViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}