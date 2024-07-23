
// StatScript v0.35 designed by Sefa Kuscu

/***Future Plans
 *  There is going to be other types of stats like experience, attributes, defances, resistances in StatSystem
 */

/* Version History
 * v0,35
 * - Stat System now have it's own namespace
 * - Added method Descriptions
 * - Regions added
 * - Max Value Modifiers now implimented and being calculated
 * - Text values calculated internaly
 * - Filler Values calculated internaly
 */

namespace StatSystem
{
    using System;

    /// <summary>
    /// this class defines the Stat class
    /// </summary>
    public class Stat
    {
        #region Values

        // stat values
        private float maxValue;
        private float maxValueCalculated;
        private float initialValue;
        private float currentValue;


        // regen values
        private float baseRegenRate = 1f;
        private float regenMultiplier = 1f;
        private float regenPerSec = 0f;

        // modifier values
        private float modValueFlatAdd;
        private float modValuePercAdd;
        private float modValuePercMult;

        //Calculate Condition
        private bool isDirty = true;

        #endregion

        #region Public Values

        public float MaxValue
        {
            get
            {
                if (isDirty)
                {
                    maxValueCalculated = CalculateMaxValue();
                }
                return maxValueCalculated;
            }
            set { maxValue = value; }
        }

        /// <summary>
        /// if initialValue hasn't set (if it's 0) returns max value, if it has been set return initialValue
        /// </summary>
        public float InitialValue
        {
            //here i can return current value on lvl load in the future
            get
            {
                if (initialValue == 0)
                {
                    return MaxValue;
                }
                else
                {
                    return initialValue;
                }
            }
            set
            {
                initialValue = value;
            }
        }

        /// <summary>
        /// sets currentValue betweem 0 and maxValue
        /// gets currentValue
        /// </summary>
        public float CurrentValue
        {
            get
            {
                if (currentValue > MaxValue)
                {
                    currentValue = MaxValue;
                }
                return currentValue;
            }
            set
            {
                if (value > MaxValue)
                {
                    currentValue = MaxValue;
                }
                else if (value < 0f)
                {
                    currentValue = 0f;
                }
                else
                {
                    currentValue = value;
                }
            }
        }

        /// <summary>
        /// calculates and returns regeneration amount
        /// </summary>
        public float RegenerationAmount
        {
            get
            {
                float regenerationAmount = ((MaxValue / 100 * baseRegenRate) + regenPerSec) / 100 * regenMultiplier;
                return regenerationAmount;
            }
        }

        public float ModValueFlatAdd
        {
            get { return modValueFlatAdd; }
            set
            {
                isDirty = true;
                modValueFlatAdd = value;
            }
        }
        public float ModValuePercAdd
        {
            get { return modValuePercAdd; }
            set
            {
                isDirty = true;
                modValuePercAdd = value;
            }
        }
        public float ModValuePercMult
        {
            get { return modValuePercMult; }
            set
            {
                isDirty = true;
                modValuePercMult = value;
            }
        }


        #endregion

        #region UI Values

        /// <summary>
        /// calculates the UI fill amount of the stat, 
        /// returns a result between 0 and 1
        /// </summary>
        public float FillAmount
        {
            get
            {
                float _fillAmount = CurrentValue / MaxValue;
                return _fillAmount;
            }
        }

        /// <summary>
        /// Text value of a stat to be used from UIManager
        /// </summary>
        public string TextValue
        {
            get
            {
                return (((int)CurrentValue).ToString() + "/" + ((int)MaxValue).ToString());
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// This method initializes a stat
        /// </summary>
        /// <param name="_maxValue"></param>
        /// <param name="_initialValue"></param>
        public void InitializeStat(float _maxValue, float _initialValue)
        {
            MaxValue = _maxValue;
            InitialValue = _initialValue;
            CurrentValue = InitialValue;
        }

        /// <summary>
        /// Regenerate the stat until it reaches the maximum value while conditions are met.
        /// </summary>
        public void Regenerate()
        {
            if (CurrentValue < MaxValue)
            {
                CurrentValue += RegenerationAmount;
            }
        }

        /// <summary>
        /// Lose spesified amount from currentValue
        /// </summary>
        /// <param name="_value"></param>
        public void LoseValue(float _value)
        {
            CurrentValue -= _value;
        }

        /// <summary>
        /// Gain spesified amount from currentValue
        /// </summary>
        /// <param name="_value"></param>
        public void GainValue(float _value)
        {
            CurrentValue += _value;
        }

        /// <summary>
        /// Calculates max value with ordered modifiers 1.Flat 2.Percent Additive 3.Percent Multiplicitive
        /// </summary>
        /// <returns></returns>
        private float CalculateMaxValue()
        {
            float finalValue = maxValue;
            // ordered
            // 1
            if (modValueFlatAdd != 0)
            {
                finalValue += modValueFlatAdd;
            }
            // 2
            if (modValuePercAdd != 0)
            {
                finalValue *= 1 + (modValuePercAdd / 100f);
            }
            // 3
            if (modValuePercMult != 0)
            {
                finalValue *= 1 + (modValuePercMult / 100f);
            }
            isDirty = false;
            return (float)Math.Round(finalValue, 4);
        }

        #endregion

    }
}