using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Timers;
using System.Windows.Input;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace DiaShowWpf
{
    public class WindowViewModel : ReactiveObject
    {
        private string _path;
        public string Path
        {
            get { return _path; }
            set { this.RaiseAndSetIfChanged(ref _path, value); }
        }

        private string _interval;
        public string Interval
        {
            get { return _interval; }
            set { this.RaiseAndSetIfChanged(ref _interval, value); }
        }

        private int _currentIndex = -1;
        private int _index = -1;
        private int Index
        {
            get { return _index; }
            set { this.RaiseAndSetIfChanged(ref _index, value); }
        }

        public ReactiveCommand<int, Images> GetImages { get; protected set; }
        private readonly ObservableAsPropertyHelper<Images> _images;
        public Images Images => _images.Value;

        public ReactiveCommand<string, Timer> ExecuteTimer { get; protected set; }
        private readonly ObservableAsPropertyHelper<Timer> _timer;
        public Timer Timer => _timer.Value;

        public ReactiveCommand ShufflePictures { get; protected set; }
        public ReactiveCommand<KeyEventArgs, bool> KeyDownCommand { get; protected set; }
        
        public ReactiveCommand<string, ListyList<string>> ExecuteSearch { get; protected set; }
        private readonly ObservableAsPropertyHelper<ListyList<string>> _searchResults;
        public ListyList<string> SearchResults => _searchResults.Value;

        public WindowViewModel()
        {
            ExecuteSearch = ReactiveCommand.Create<string, ListyList<string>>(GetResultsFromPath);
            this.WhenAnyValue(x => x.Path)
                .Select(x => x?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .InvokeCommand(ExecuteSearch);
            _searchResults = ExecuteSearch.ToProperty(this, x => x.SearchResults, new ListyList<string>());


            ExecuteTimer = ReactiveCommand.Create<string, Timer>(SetUpTimer);
            this.WhenAnyValue(x => x.Interval)
                .Select(x => x?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                // TODO: Validate is number?
                .InvokeCommand(ExecuteTimer);
            _timer = ExecuteTimer.ToProperty(this, x => x.Timer, new Timer());
            _timer.Value.Elapsed += Timer_Elapsed;


            ShufflePictures = ReactiveCommand.Create(() =>
            {
                _searchResults.Value.Shuffle();
                Index = Index + 1;
            });


            GetImages = ReactiveCommand.Create<int, Images>(LoadImages);
            this.WhenAnyValue(x => x.Index)
                .InvokeCommand(GetImages);
            GetImages.ThrownExceptions.Subscribe(ex => { /* TODO: ?*/ });
            _images = GetImages.ToProperty(this, x => x.Images, new Images());


            KeyDownCommand = ReactiveCommand.Create<KeyEventArgs, bool>(x =>
            {
                if(x.Key == Key.Right)
                    Index = Index + 1;
                if (x.Key == Key.Left)
                    Index = Index - 1;
                if (x.Key == Key.Space)
                    _timer.Value.Enabled = !_timer.Value.Enabled;
                return true;
            });
            KeyDownCommand.ThrownExceptions.Subscribe(ex => { throw ex; });


            Path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            Interval = "10";

            // TODO: chain shit together (pressing right or left to see something? no okay)
            // TODO: changing path should refresh "view"
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // TODO: this seems to be broken (the fast and buggious)
            // TODO: use that https://docs.reactiveui.net/en/user-guide/commands/invoking-commands.html ?
            Index = Index + 1;
        }

        private Timer SetUpTimer(string seconds)
        {
            var timer = new Timer();
            var number = TimeSpan.FromSeconds(10).TotalMilliseconds;
            if (double.TryParse(seconds.Replace(',', '.'), out number))
                number = TimeSpan.FromSeconds(number).TotalMilliseconds;
            timer.Interval = number;
            return Timer;
        }

        public ListyList<string> GetResultsFromPath(string path)
        {
            var files = new ListyList<string>();
            if (Directory.Exists(path))
            {
                files.AddRange(Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).ToList()
                    .Where(s => s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".jpg")
                                || s.ToLower().EndsWith(".jpeg") || s.ToLower().EndsWith(".bmp")).ToList());
            }
            Index = 0;
            return files;
        }

        private Images LoadImages(int index)
        {
            if (_searchResults.Value.Any())
            {
                var file = index > _currentIndex ? _searchResults.Value.Next() : _searchResults.Value.Previous();
                _currentIndex = index;
                return new Images(file,
                    _searchResults.Value.Next(-1),
                    _searchResults.Value.Next(-2),
                    _searchResults.Value.Next(-3),
                    _searchResults.Value.Next(-4),
                    _searchResults.Value.Next(-5),
                    _searchResults.Value.Next(1),
                    _searchResults.Value.Next(2),
                    _searchResults.Value.Next(3),
                    _searchResults.Value.Next(4),
                    _searchResults.Value.Next(5)
                );
            }
            return new Images(@"", "", "", "", "", "",
                "", "", "", "", "");
        }
    }
}
