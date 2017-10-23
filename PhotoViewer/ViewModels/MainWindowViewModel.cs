using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using PhotoViewer.Mvvm;
using System;
using System.Threading.Tasks;
using PhotoViewer.Helpers;

namespace PhotoViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            ImageCollection = new ObservableCollectionEx<ImageViewModel>();
            DropImagesCommand = new ActionCommand<DragEventArgs>(DropImages);
            NextImageCommand = new ActionCommand(NextImage);
            PrevImageCommand = new ActionCommand(PrevImage);
            ImageDoubleClickCommand = new ActionCommand(() => { SingleImageMode = true; });
            BackCommand = new ActionCommand(() => { SingleImageMode = false; });
            GaussianBlurCommand = new ActionCommand(() => { SelectedImage.HasGaussianBlurEffect = true; });
        }

        public ObservableCollectionEx<ImageViewModel> ImageCollection { get; }
        public ICommand DropImagesCommand { get; }
        public ICommand ImageDoubleClickCommand { get; }
        public ICommand GaussianBlurCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand PrevImageCommand { get; }
        public ICommand BackCommand { get; }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                SelectedImage = ImageCollection[_selectedIndex];
            }
        }

        private ImageViewModel _selectedImage;
        public ImageViewModel SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        private bool _singleImageMode;
        public bool SingleImageMode
        {
            get { return _singleImageMode; }
            set
            {
                _singleImageMode = value;
                OnPropertyChanged(nameof(SingleImageMode));
            }
        }


        private void PrevImage()
        {
            var prevIndex = SelectedIndex - 1;
            if (prevIndex >= 0)
            {
                SelectedIndex--;
            }
        }

        private void NextImage()
        {
            var nextIndex = SelectedIndex + 1;
            if (nextIndex < ImageCollection.Count)
            {
                SelectedIndex++;
            }
        }

        public void DropImages(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var pathes = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (pathes == null)
                    return;
                Task.Factory.StartNew(() =>
                {
                    var viewModels = new List<ImageViewModel>();
                    foreach (var path in pathes)
                    {
                        var attributes = File.GetAttributes(path);
                        bool isFolder = (attributes & FileAttributes.Directory) == FileAttributes.Directory;
                        if (isFolder)
                        {
                            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(ImageFormatHelper.IsImage);
                            var imageViewModels = files.Select(p => new ImageViewModel { Path = p });
                            viewModels.AddRange(imageViewModels);
                        }
                        else
                        {
                            if (ImageFormatHelper.IsImage(path))
                            {
                                viewModels.Add(new ImageViewModel { Path = path });
                            }
                        }
                        Application.Current.Dispatcher.Invoke((Action)(() => ImageCollection.AddRange(viewModels)));
                    }
                });
            }
        }
    }
}