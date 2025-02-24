using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Collections.Generic;
using UnoApp8;

public class ViewModel
{
    public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();
    public ICommand ExportCommand { get; }
    public ICommand ImportCommand { get; }

    public ViewModel()
    {
        ExportCommand = new RelayCommand(async () => await ExportData());
        ImportCommand = new RelayCommand(async () => await ImportData());
    }

    private async System.Threading.Tasks.Task ExportData()
    {
        var picker = new FileSavePicker();
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        picker.FileTypeChoices.Add("Text File", new List<string>() { ".txt" });
        picker.SuggestedFileName = "data";

        var file = await picker.PickSaveFileAsync();
        if (file != null)
        {
            await FileIO.WriteLinesAsync(file, Items);
            var dialog = new ContentDialog()
            {
                Title = "Éxito",
                Content = "Datos exportados exitosamente.",
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }

    private async System.Threading.Tasks.Task ImportData()
    {
        var picker = new FileOpenPicker();
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        picker.FileTypeFilter.Add(".txt");


        var file = await picker.PickSingleFileAsync();
        if (file != null)
        {
            var lines = await FileIO.ReadLinesAsync(file);
            Items.Clear();
            foreach (var line in lines)
            {
                Items.Add(line);
            }
            var dialog = new ContentDialog()
            {
                Title = "Éxito",
                Content = "Datos importados exitosamente.",
                CloseButtonText = "OK",
                XamlRoot = Microsoft.UI.Xaml.Window.Current.Content.XamlRoot 
            };


            await dialog.ShowAsync();
        }
    }
}

public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
    public void Execute(object parameter) => _execute();
    public event EventHandler CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
