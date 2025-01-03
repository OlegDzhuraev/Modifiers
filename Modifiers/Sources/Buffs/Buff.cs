using UnityEngine;

namespace InsaneOne.Modifiers.Buffs
{
    [CreateAssetMenu(fileName = "NewBuff", menuName = "Modifiers/New buff")]
    public class Buff : ScriptableObject
    {
        [SerializeField] UnityModifier modifier;
        [Tooltip("Buff lifetime in seconds. Use 0 value to make endless buff.")]
        [SerializeField, Min(0f)] float lifeTime = 3f;
        [SerializeField, Min(1)] int maxStacks = 5;
        
        public UnityModifier Modifier => modifier;
        public float LifeTime => lifeTime;
        public int MaxStacks => maxStacks;
    }
}