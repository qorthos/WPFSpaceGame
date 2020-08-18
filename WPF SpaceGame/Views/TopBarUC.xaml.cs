using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFSpaceGame.Game;
using WPFSpaceGame.General;
using WPFSpaceGame.Views.Colonies;
using WPFSpaceGame.Views.Map;

namespace WPFSpaceGame.Views
{
    /// <summary>
    /// Interaction logic for TopBar.xaml
    /// </summary>
    public partial class TopBarUC : UserControl
    {
        ServiceProvider services;
        GameService gameService;
        VMManager vmManager;
        

        public GameData GameData
        {
            get
            {
                return gameService.GameData;
            }
        }

        public TopBarUC()
        {
            services = ServiceProvider.Instance;
            gameService = services.GetService<GameService>();
            vmManager = services.GetService<VMManager>();

            InitializeComponent();
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            vmManager.TransitionToVM<MapVM>();
        }

        private void ShipsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ColoniesButton_Click(object sender, RoutedEventArgs e)
        {
            vmManager.TransitionToVM<ColonyVM>();
        }

        private void BodiesButton_Click(object sender, RoutedEventArgs e)
        {
            vmManager.TransitionToVM<ColonyVM>();
        }
    }
}
