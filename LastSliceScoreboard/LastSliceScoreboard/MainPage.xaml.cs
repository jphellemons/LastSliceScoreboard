using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LastSliceScoreboard
{
	public partial class MainPage : ContentPage
	{
        private GameService _service;

		public MainPage()
		{
			InitializeComponent();

            this.BackgroundImage = "GameOverScreen.png";
            imgTitle.Source = ImageSource.FromResource("leaderboard.png");

            _service = new GameService();

            DisplayLeaderboardAsync();
        }

        private async void DisplayLeaderboardAsync()
        {
            if (!_service.HasUserLoggedIn())
                await _service.Login();
            try
            {
                var leaderBoard = await _service.GetLeaderboardWithCurrentUserAsync();
                lv.BindingContext = leaderBoard;
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
