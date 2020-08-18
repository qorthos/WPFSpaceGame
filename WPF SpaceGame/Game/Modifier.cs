using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Converters;
using WPFSpaceGame.General;

namespace WPFSpaceGame.Game
{
    public delegate void ModifierValueChanged(Modifier modifier);

    public class Modifier : ObservableObject
    {
        public static bool PauseValueChangedEvent = false;

        string name;
        ModifierMathEnum modifierType;
        ForcingFunctionEnum forcingFunctionType;
        Func<double, double, double> forcingFunction;
        double forcingFunctionScale;
        ObservableCollection<Modifier> modifiers = new ObservableCollection<Modifier>();
        double baseValue;
        double value;        
        double floor;
        double ceiling;

        public event ModifierValueChanged ModifierValueChanged;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public ModifierMathEnum ModifierType
        {
            get
            {
                return modifierType;
            }
        }

        public ForcingFunctionEnum ForcingFunctionType
        {
            get
            {
                return forcingFunctionType;
            }
        }

        public double ForcingFunctionScale
        {
            get
            {
                return forcingFunctionScale;
            }
        }

        public ObservableCollection<Modifier> Modifiers
        {
            get
            {
                return modifiers;
            }
        }

        public double BaseValue
        {
            get
            {
                return baseValue;
            }

            set
            {
                baseValue = value;
                Notify();
                Recalc();
            }
        }

        public double Floor
        {
            get
            {
                return floor;
            }

            set
            {
                floor = value;
                Notify();
                Recalc();

            }
        }

        public double Ceiling
        {
            get
            {
                return ceiling;
            }

            set
            {
                ceiling = value;
                Notify();
                Recalc();
            }
        }

        public double Value
        {
            get
            {
                return value;
            }

            private set
            {
                this.value = value;
                Notify();
                if (PauseValueChangedEvent != true)
                    ModifierValueChanged?.Invoke(this);
            }
        }

        public Modifier(string name, double floor = 0.00, double ceiling = 1.0, ModifierMathEnum modifierType = ModifierMathEnum.Additive, ForcingFunctionEnum forcingFunctionType = ForcingFunctionEnum.Linear, double forcingFunctionScale = 1.0)
        {
            this.name = name;
            this.floor = floor;
            this.ceiling = ceiling;
            this.modifierType = modifierType;
            this.forcingFunctionType = forcingFunctionType;

            switch (forcingFunctionType)
            {
                case ForcingFunctionEnum.Linear:
                    forcingFunction = ForcingFunctions.Linear;
                    break;
                case ForcingFunctionEnum.None:
                    forcingFunction = ForcingFunctions.None;
                    break;

                case ForcingFunctionEnum.CircleIn:
                    forcingFunction = ForcingFunctions.CircleIn;
                    break;
                case ForcingFunctionEnum.CircleInOut:
                    forcingFunction = ForcingFunctions.CircleInOut;
                    break;
                case ForcingFunctionEnum.CircleOut:
                    forcingFunction = ForcingFunctions.CircleOut;
                    break;

                case ForcingFunctionEnum.CubicIn:
                    forcingFunction = ForcingFunctions.CubicIn;
                    break;
                case ForcingFunctionEnum.CubicInOut:
                    forcingFunction = ForcingFunctions.CubicInOut;
                    break;
                case ForcingFunctionEnum.CubicOut:
                    forcingFunction = ForcingFunctions.CubicOut;
                    break;

                case ForcingFunctionEnum.ExponentialIn:
                    forcingFunction = ForcingFunctions.ExponentialIn;
                    break;
                case ForcingFunctionEnum.ExponentialInOut:
                    forcingFunction = ForcingFunctions.ExponentialInOut;
                    break;
                case ForcingFunctionEnum.ExponentialOut:
                    forcingFunction = ForcingFunctions.ExponentialOut;
                    break;

                case ForcingFunctionEnum.QuadraticIn:
                    forcingFunction = ForcingFunctions.QuadraticIn;
                    break;
                case ForcingFunctionEnum.QuadraticInOut:
                    forcingFunction = ForcingFunctions.QuadraticInOut;
                    break;
                case ForcingFunctionEnum.QuadraticOut:
                    forcingFunction = ForcingFunctions.QuadraticOut;
                    break;

                case ForcingFunctionEnum.QuarticIn:
                    forcingFunction = ForcingFunctions.QuarticIn;
                    break;
                case ForcingFunctionEnum.QuarticInOut:
                    forcingFunction = ForcingFunctions.QuarticInOut;
                    break;
                case ForcingFunctionEnum.QuarticOut:
                    forcingFunction = ForcingFunctions.QuarticOut;
                    break;

                case ForcingFunctionEnum.QuinticIn:
                    forcingFunction = ForcingFunctions.QuinticIn;
                    break;
                case ForcingFunctionEnum.QuinticInOut:
                    forcingFunction = ForcingFunctions.QuinticInOut;
                    break;
                case ForcingFunctionEnum.QuinticOut:
                    forcingFunction = ForcingFunctions.QuinticOut;
                    break;

                case ForcingFunctionEnum.SineIn:
                    forcingFunction = ForcingFunctions.SineIn;
                    break;
                case ForcingFunctionEnum.SineInOut:
                    forcingFunction = ForcingFunctions.SineInOut;
                    break;
                case ForcingFunctionEnum.SineOut:
                    forcingFunction = ForcingFunctions.SineOut;
                    break;

            }
        }

        private void Recalc()
        {
            double adders = 0.0;
            double multis = 1.0;

            foreach (Modifier modifier in modifiers)
            {
                if (modifier.ModifierType == ModifierMathEnum.Additive)
                    adders += modifier.Value;
                else
                    multis *= modifier.Value;
            }

            var tempValue = DoubleHelper.Clamp((baseValue + adders) * multis, floor, ceiling);
            Value = forcingFunction(tempValue, forcingFunctionScale);


        }

        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
            modifier.ModifierValueChanged += Modifier_ModifierValueChanged;
        }

        public void RemoveModifier(Modifier modifier)
        {
            modifiers.Remove(modifier);
            modifier.ModifierValueChanged -= Modifier_ModifierValueChanged;
        }

        private void Modifier_ModifierValueChanged(Modifier modifier)
        {
            Recalc();
        }
    }

    public enum ModifierMathEnum
    {
        Additive,
        Multiplier,
    }

    public enum ForcingFunctionEnum
    {
        None,
        Linear,

        CubicIn,
        CubicOut,
        CubicInOut,

        QuadraticIn,
        QuadraticOut,
        QuadraticInOut,

        QuarticIn,
        QuarticOut,
        QuarticInOut,

        QuinticIn,
        QuinticOut,
        QuinticInOut,

        SineIn,
        SineOut,
        SineInOut,

        ExponentialIn,
        ExponentialOut,
        ExponentialInOut,

        CircleIn,
        CircleOut,
        CircleInOut,
    }

    public static class ForcingFunctions
    {
        public static double None(double value, double scale) => 0;
        public static double Linear(double value, double scale) => scale * value;

        public static double CubicIn(double value, double scale) => scale * Power.In(value, 3);
        public static double CubicOut(double value, double scale) => scale * Power.Out(value, 3);
        public static double CubicInOut(double value, double scale) => scale * Power.InOut(value, 3);

        public static double QuadraticIn(double value, double scale) => scale * Power.In(value, 2);
        public static double QuadraticOut(double value, double scale) => scale * Power.Out(value, 2);
        public static double QuadraticInOut(double value, double scale) => scale * Power.InOut(value, 2);

        public static double QuarticIn(double value, double scale) => scale * Power.In(value, 4);
        public static double QuarticOut(double value, double scale) => scale * Power.Out(value, 4);
        public static double QuarticInOut(double value, double scale) => scale * Power.InOut(value, 4);

        public static double QuinticIn(double value, double scale) => scale * Power.In(value, 5);
        public static double QuinticOut(double value, double scale) => scale * Power.Out(value, 5);
        public static double QuinticInOut(double value, double scale) => scale * Power.InOut(value, 5);

        public static double SineIn(double value, double scale) => scale * Math.Sin(value * DoubleHelper.PiOver2 - DoubleHelper.PiOver2) + 1;
        public static double SineOut(double value, double scale) => scale * Math.Sin(value * DoubleHelper.PiOver2);
        public static double SineInOut(double value, double scale) => scale * (Math.Sin(value * DoubleHelper.Pi - DoubleHelper.PiOver2) + 1) / 2;

        public static double ExponentialIn(double value, double scale) => scale*Math.Pow(2, 10 * (value - 1));
        public static double ExponentialOut(double value, double scale) => Out(value, scale, ExponentialIn);
        public static double ExponentialInOut(double value, double scale) => InOut(value, scale, ExponentialIn);

        public static double CircleIn(double value, double scale) => scale * -(Math.Sqrt(1 - value * value) - 1);
        public static double CircleOut(double value, double scale) => scale*Math.Sqrt(1 - (value - 1) * (value - 1));
        public static double CircleInOut(double value, double scale) => scale*(value <= .5 ? (Math.Sqrt(1 - value * value * 4) - 1) / -2 : (Math.Sqrt(1 - (value * 2 - 2) * (value * 2 - 2)) + 1) / 2);




        private static double Out(double value, double scale, Func<double, double, double> function)
        {
            return 1 - function(1 - value, scale);
        }

        private static double InOut(double value, double scale, Func<double, double, double> function)
        {
            if (value < 0.5f)
                return 0.5f * function(value * 2, scale);

            return 1f - 0.5f * function(2 - value * 2, scale);
        }

        private static class Power
        {
            public static double In(double value, int power)
            {
                return (double)Math.Pow(value, power);
            }

            public static double Out(double value, int power)
            {
                var sign = power % 2 == 0 ? -1 : 1;
                return (double)(sign * (Math.Pow(value - 1, power) + sign));
            }

            public static double InOut(double s, int power)
            {
                s *= 2;

                if (s < 1)
                    return In(s, power) / 2;

                var sign = power % 2 == 0 ? -1 : 1;
                return (double)(sign / 2.0 * (Math.Pow(s - 2, power) + sign * 2));
            }
        }
    }
}
