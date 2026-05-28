using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Image> Images { get; } = new();
}