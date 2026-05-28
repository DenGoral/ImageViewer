using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;

namespace ImageViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ImageItemViewModel> Images { get; } = new();

    public MainWindowViewModel()
    {
        Images.Add(new ImageItemViewModel("/Users/DenysGoral/Programming/ImageViewer/ImageViewer/Assets/Images/1.png", "1920x1080", DateTime.Now));
        Images.Add(new ImageItemViewModel("/Users/DenysGoral/Programming/ImageViewer/ImageViewer/Assets/Images/2.png", "1920x1080", DateTime.Now));
        Images.Add(new ImageItemViewModel("/Users/DenysGoral/Programming/ImageViewer/ImageViewer/Assets/Images/3.png", "1920x1080", DateTime.Now));
        
    }
}