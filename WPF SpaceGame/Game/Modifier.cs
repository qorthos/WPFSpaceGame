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

        public Modifier(string name, double floor = 0.05, double ceiling = 3.0)
        {
            this.name = name;
            this.floor = floor;
            this.ceiling = ceiling;
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

            Value = DoubleHelper.Clamp((baseValue + adders) * multis, floor, ceiling);
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
}
