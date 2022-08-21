using UnityEngine;
using UnityEngine.Events;

public class Booster : MonoBehaviour //& Booster Class
{

    //? Maybe I should separate all this enums and classes.

    [System.Serializable]
    public enum BoosterParameter
    {
        NumberMatches,
        Points,
        TimeRemaining,
        NumberResources
    }

    [System.Serializable]
    public enum ResourceType
    {
        Coin,
        Food,
        Iron,
        Stone,
        Water,
        Wood
    }

    [System.Serializable]
    public class Condition : ScriptableObject
    {
        public BoosterParameter BoosterParameter;
        public enum ComparisonType
        {
            LessThan,
            Equals,
            BiggerThan
        }

        public ComparisonType Comparison;
        public int Quantity;
        public ResourceType Resource;
    }

    public class BoosterInfo
    {
        public string Name;
        public Condition[] Conditions;
        public GameObject VisualPrefab;
        public UnityEvent OnBoosterPerformed;
    }

    public BoosterInfo[] Boosters;
}
