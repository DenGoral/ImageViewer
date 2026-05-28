using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ImageViewer.ViewModels;

public partial class ImageItemViewModel : ViewModelBase
{
    [ObservableProperty] private string _fileName;
    [ObservableProperty] private string _resolution;
    [ObservableProperty] private DateTime _dateModified;
    [ObservableProperty] private Bitmap? _imageSource; 
}