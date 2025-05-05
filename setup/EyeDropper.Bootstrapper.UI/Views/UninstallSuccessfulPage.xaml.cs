using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class UninstallSuccessfulPage : Page
{
    public UninstallSuccessfulPage(UninstallSuccessfulViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}