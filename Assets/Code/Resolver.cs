using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Code
{
    interface IResolveable {}

    class AutoResolve : Attribute {}

    class Resolver : MonoBehaviour
    {
        [SerializeField] private ManualResolverContent[] ManualContent;

        private static readonly Dictionary<Type, object> SortedContent = new Dictionary<Type, object>();
        private static bool _isContentSorted = false;

        protected void Awake()
        {
            var resolveables = GetComponentsInChildren<IResolveable>();

            foreach (var reseolvable in resolveables)
                RegisterItem(reseolvable.GetType(), reseolvable);
            foreach (var manualContent in ManualContent)
	

            _isContentSorted = true;
        }

        private void RegisterItem(Type registerType, object item)
        {
            if (SortedContent.ContainsKey(registerType))
                return;

			Debug.Log(registerType);

            SortedContent.Add(registerType, item);
        }

        public static void AutoResolve(object subject)
        {
            var type = subject.GetType();

            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                foreach (var attribute in field.GetCustomAttributes(true))
                {
                    var autoResolve = attribute as AutoResolve;
                    if (autoResolve != null)
                        field.SetValue(subject, Resolve(field.FieldType));
                }
            }
        }
        
        public static void Resolve<T>(out T subject) where T : class, IResolveable
        {
            if (!_isContentSorted)
                Debug.Log("WARNING! resolver has not yet been sorted!");

            var targetType = typeof(T);

            if (SortedContent.ContainsKey(targetType))
                subject = SortedContent[targetType] as T;
            else
                subject = null;
        }

        private static object Resolve(Type targetType)
        {
            if (!_isContentSorted)
                Debug.Log("WARNING! resolver has not yet been sorted!");
            

            if (SortedContent.ContainsKey(targetType))
                return SortedContent[targetType];

            Debug.Log(string.Format("WARNING! resolver has no {0}!", targetType));
            return null;
        }

        public static T Resolve<T>() where T : class, IResolveable
        {
            if (!_isContentSorted)
                Debug.Log("WARNING! resolver has not yet been sorted!");

            var targetType = typeof(T);

            if (SortedContent.ContainsKey(targetType))
                return SortedContent[targetType] as T;

            Debug.Log(string.Format("WARNING! resolver has no {0}!", targetType));
            return null;
        }
    }
}
