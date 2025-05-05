using System.Windows.Controls;
using EyeDropper.Bootstrapper.UI.ViewModels;

namespace EyeDropper.Bootstrapper.UI.Views;

public partial class ElevateErrorPage : Page
{
    public ElevateErrorPage(ElevateErrorViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}