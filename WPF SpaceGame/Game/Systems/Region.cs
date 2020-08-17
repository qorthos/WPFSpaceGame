using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSpaceGame.Game.Systems
{
    public class Region
    {
        public ObservableCollection<Settlement> Settlements = new ObservableCollection<Settlement>();

        RegionTypeEnum regionType;

        double area;
        double elevation;
        double latitude;
        double longitude;
        double meanTemperature;
        double highTemperature;
        double lowTemperature;



    }

    public enum RegionTypeEnum
    {
        Surface,
        Subterranean,
        Cloud,
    }


    public enum RegionAttributes
    {
        Coastal,
        Plains,
        Mountains,
        Hills,
        Wasteland,
        Ocean,
    }
}
