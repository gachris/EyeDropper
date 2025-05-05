using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class ErrorPage : Page
{
    public ErrorPage(ErrorViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}