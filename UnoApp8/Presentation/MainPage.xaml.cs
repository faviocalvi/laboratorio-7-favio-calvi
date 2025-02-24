namespace UnoApp8.Presentation;

public sealed partial class MainPage : Page
{
    public ViewModel ViewModel { get; }

    public MainPage()
    {
        this.InitializeComponent();
        ViewModel = new ViewModel();
        this.DataContext = ViewModel;
    }
}

