using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class DowngradeDetectedPage : Page
{
    public DowngradeDetectedPage(DowngradeDetectedViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}