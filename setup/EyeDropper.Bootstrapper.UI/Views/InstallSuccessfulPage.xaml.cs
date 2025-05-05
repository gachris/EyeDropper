using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class InstallSuccessfulPage : Page
{
    public InstallSuccessfulPage(InstallSuccessfulViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}