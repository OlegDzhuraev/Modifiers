using UnityEngine;

namespace InsaneOne.Modifiers
{
    [CreateAssetMenu(fileName = "NewBuff", menuName = "Modifiers/New buff")]
    public class Buff : ScriptableObject
    {
        [SerializeField] Modifier modifier;
        [SerializeField] float lifeTime = 3f;
        [SerializeField] int maxStacks = 5;
        
        public Modifier Modifier => modifier;
        public float LifeTime => lifeTime;
        public int MaxStacks => maxStacks;
    }
}