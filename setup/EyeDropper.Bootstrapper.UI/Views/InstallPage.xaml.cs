using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class InstallPage : Page
{
    public InstallPage(InstallViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}