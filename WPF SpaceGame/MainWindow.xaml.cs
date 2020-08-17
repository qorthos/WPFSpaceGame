using System.Windows;
using MahApps.Metro.Controls;
using WPFSpaceGame.General;
using WPFSpaceGame.Views;
using WPFSpaceGame.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace WPFSpaceGame
{
    public partial class MainWindow : MetroWindow
    {
        GameService gameService;
        public MainWindow()
        {
            var services = ServiceProvider.Instance;
            gameService = new GameService();
            services.AddService<GameService>(gameService);

            var vmManager = new VMManager();
            services.AddService<VMManager>(vmManager);

            DataContext = vmManager;
            vmManager.TransitionToVM<TitleVM>();

            _ = new WPFSpaceGame.Game.Systems.GSStellarBodies(); 
            _ = new WPFSpaceGame.Game.Politics.GSPolitics();
            _ = new WPFSpaceGame.Game.Systems.GSVessels();
            _ = new WPFSpaceGame.Game.Systems.GSPlanets();
            _ = new WPFSpaceGame.Game.GSNewGame();

            gameService.Initialize();

            InitializeComponent();
        }


        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/qorthos/WPFSpaceGame",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void AdvanceDay(object sender, RoutedEventArgs e)
        {
            gameService.TickDay();
        }
    }
}
