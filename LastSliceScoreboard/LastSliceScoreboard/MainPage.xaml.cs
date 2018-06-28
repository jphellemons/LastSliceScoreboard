using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LastSliceScoreboard
{
	public partial class MainPage : ContentPage
	{
        private GameService _service;
        public ObservableCollection<Score> Scores { get; set; }

        public MainPage()
		{
            Scores = new ObservableCollection<Score>();
            InitializeComponent();

            BindingContext = this;
            this.BackgroundImage = "LastSliceScoreboard.GameOverScreen.png";
            imgTitle.Source = ImageSource.FromResource("leaderboard.png", typeof(MainPage).GetTypeInfo().Assembly);

            _service = new GameService();
            DisplayLeaderboardAsync();
        }

        private async void DisplayLeaderboardAsync()
        {
            if (Scores.Any())
                Scores = new ObservableCollection<Score>(); // to prevent double items when reloading

            if (!_service.HasUserLoggedIn())
                await _service.LoginAsync();
            try
            {
                var list = await _service.GetLeaderboardWithCurrentUserAsync();
                foreach (var s in list)
                    Scores.Add(s);

                var nr2 = list.Where(l => l.ChallengesCompleted > 1).ToList();

                lblTot.Text = $"{Scores.Count} have finished challenge 1. {nr2.Count} have finished challenge 2.";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Dismiss");
            }
        }

        private async void btnReload_Clicked(object sender, EventArgs e)
        {
            DisplayLeaderboardAsync();
        }
    }
}
