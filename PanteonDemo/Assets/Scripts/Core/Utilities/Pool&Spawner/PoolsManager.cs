using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities.Pool_Spawner.Interfaces;
using Core.Utilities.Pool_Spawner.Pools;
using Core.Utilities.Singleton;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = System.Object;
using Type = System.Type;

namespace Core.Utilities.Pool_Spawner
{
    [Serializable]
    public class SimplePoolObjects
    {
        public int maxNumberOfObjects;
        public int initSpawnCount;
        public GameObject gameObject;
        
        public SimplePoolObjects(GameObject gameObject)
        {
            this.gameObject = gameObject;
            maxNumberOfObjects = 999;
        }
    }

    [Serializable]
    public class TypedPoolObjects
    {
        [HideInInspector] public string typeOfUnityObject;
        public UnityEngine.Object unityObject;
        public List<SimplePoolObjects> typedObjects;

        public TypedPoolObjects(SimplePoolObjects typedObject)
        {
            typedObjects = new List<SimplePoolObjects> { typedObject };
        }
    }

    public struct TypedPoolsDictionary<TLogic, T> where T: MonoBehaviour, IPoolMemberWithType<TLogic>
    {
        private Dictionary<TLogic, MonoBehaviorPool<T>> _dictionary;
        public Dictionary<TLogic, MonoBehaviorPool<T>> GetAllTypedPools() => _dictionary;

        //Don't change the name, It will give you error
        public void AddToDictionary(TLogic tLogic, MonoBehaviorPool<T> pool)
        {
            _dictionary ??= new Dictionary<TLogic, MonoBehaviorPool<T>>();
            _dictionary.Add(tLogic, pool);
        }
    }
    
    public class PoolsManager : SingletonMonoBehaviour<PoolsManager>
    {
        [SerializeField] private List<SimplePoolObjects> simplePoolObjects;
        [Space] 
        [SerializeField] private List<TypedPoolObjects> typedPoolObjects;

        private Dictionary<Type, Object> _monoSimplePoolDictionary; //Object is MonoBehaviorPoolSimple
        private Dictionary<Type, Object> _monoTypedPoolDictionary; //Object is TypedPoolsDictionary

        protected override void InternalAwake()
        {
            MonoPoolsInitiate();
            MonoPoolTypeInvolvedInitiate();
            simplePoolObjects = null;
            typedPoolObjects = null;
        }
        
        public MonoBehaviorPool<T> GetMyPoolSimple<T>() where T : MonoBehaviour, IPoolMemberSimple
        {
            var pool = (MonoBehaviorPool<T>) _monoSimplePoolDictionary[typeof(T)];
            if(pool == null)
                Debug.LogError("Pool Not Exist");
            
            return pool;
        }
        
        public MonoBehaviorPool<T> GetMyPoolTyped<T, TLogic>(TLogic tLogic) where T : MonoBehaviour, IPoolMemberWithType<TLogic>
        {
            var typedPoolsDictionary = GetMyPoolsOfTyped<T, TLogic>();
            return typedPoolsDictionary[tLogic];
        }
        
        public Dictionary<TLogic, MonoBehaviorPool<T>> GetMyPoolsOfTyped<T, TLogic>() where T : MonoBehaviour, IPoolMemberWithType<TLogic>
        {
            var typedPoolsDictionary = (TypedPoolsDictionary<TLogic, T>)_monoTypedPoolDictionary[typeof(TLogic)];
            return typedPoolsDictionary.GetAllTypedPools();
        }

#if UNITY_EDITOR
        [Button("Fetch All Pool Objects")]
        private void FetchAllPoolObjects()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
            var typedInterFaceName = "IPoolMemberWithType`1";
            
            foreach (string guid in guids)
            {
                string myObjectPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(myObjectPath);
                
                var simplePoolMemberComponent = GetMonoWithInterface(go, "IPoolMemberSimple");
                var typedPoolMemberComponent = GetMonoWithInterface(go, typedInterFaceName);

                if (simplePoolMemberComponent != null && simplePoolObjects.FirstOrDefault(sp => sp.gameObject == go) == null)
                    simplePoolObjects.Add(new SimplePoolObjects(go));
                if (typedPoolMemberComponent != null)
                {
                    var typedPoolMemberInterfaceArgument = typedPoolMemberComponent.GetType().GetInterface(typedInterFaceName).GetGenericArguments()[0];
                    var getTypedPoolObject = typedPoolObjects.FirstOrDefault(tp =>
                    {
                        var classOfObject = ((MonoScript)tp.unityObject).GetClass();
                        tp.typeOfUnityObject = classOfObject.AssemblyQualifiedName;
                        return classOfObject.GetInterface(typedInterFaceName).GetGenericArguments()[0] ==
                               typedPoolMemberInterfaceArgument;
                    });
                    
                    if (getTypedPoolObject == null)
                        Debug.LogError($"{typedPoolMemberComponent} hasn't add yet", typedPoolMemberComponent);
                    else
                    {
                        var typedObject = getTypedPoolObject.typedObjects.FirstOrDefault(t => t.gameObject == go);
                        if (typedObject == null)
                            getTypedPoolObject.typedObjects.Add(new SimplePoolObjects(go));
                    }
                }
            }
        }
#endif
        
        private void MonoPoolsInitiate()
        {
            Type openType = typeof(MonoBehaviorPool<>);
            _monoSimplePoolDictionary = new Dictionary<Type, Object>(simplePoolObjects.Count);

            foreach (var simplePoolObject in simplePoolObjects)
            {
                var component = GetMonoWithInterface(simplePoolObject.gameObject, "IPoolMemberSimple");
                if (component == null)
                {
                    Debug.LogError("There is no IPoolMember inherited from this element");
                    return;
                }
                
                var componentType = component.GetType();
                Type closedType = openType.MakeGenericType(componentType);
                var objectParent = new GameObject($"{component.name} Pool") {transform = {parent = transform}};
                var monoPool = Activator.CreateInstance(closedType,
                    component,
                    objectParent.transform,
                    simplePoolObject.maxNumberOfObjects,
                    simplePoolObject.initSpawnCount);
                
                _monoSimplePoolDictionary[componentType] = monoPool;
            }
        }

        private void MonoPoolTypeInvolvedInitiate()
        {
            Type poolOpenType = typeof(MonoBehaviorPool<>);
            _monoTypedPoolDictionary = new Dictionary<Type, Object>(typedPoolObjects.Count);

            var interFaceName = "IPoolMemberWithType`1";
            foreach (var typedPoolObject in typedPoolObjects)
            {
                Type objectType = Type.GetType(typedPoolObject.typeOfUnityObject);
                Type poolClosedType = poolOpenType.MakeGenericType(objectType);
                var categoryParent = new GameObject($"{typedPoolObject.unityObject.name} : Pools") {transform = {parent = transform}};
                
                foreach (var typedObject in typedPoolObject.typedObjects)
                {
                    var component = GetMonoWithInterface(typedObject.gameObject, interFaceName);
                    if (component == null)
                    {
                        Debug.LogError("There is no IPoolMember inherited from this element", component);
                        return;
                    }
                    
                    var componentType = component.GetType();
                    Type interfaceType = componentType.GetInterface(interFaceName);
                    Type interfaceArgument = interfaceType.GenericTypeArguments[0];
                    Type typedPoolDictionaryCloseType = typeof(TypedPoolsDictionary<,>).MakeGenericType(interfaceArgument, objectType);
                    
                    Object typedPoolDictionary;
                    if (!_monoTypedPoolDictionary.TryGetValue(interfaceArgument, out typedPoolDictionary))
                    {
                        typedPoolDictionary = Activator.CreateInstance(typedPoolDictionaryCloseType);
                        _monoTypedPoolDictionary.Add(interfaceArgument, typedPoolDictionary);
                    }
                    
                    var getTypeForPoolMethod = interfaceType.GetMethod("GetTypeForPool");
                    var addToDictionaryMethod = typedPoolDictionaryCloseType.GetMethod("AddToDictionary");
                    var valueOfGenericTypeOfInterface = getTypeForPoolMethod.Invoke(component, null);
                    var poolParent = new GameObject($"{componentType.Name} : {valueOfGenericTypeOfInterface}") {transform = {parent = categoryParent.transform}};
                    var monoPool = Activator.CreateInstance(poolClosedType,
                        component,
                        poolParent.transform,
                        typedObject.maxNumberOfObjects,
                        typedObject.initSpawnCount);
                    
                    addToDictionaryMethod.Invoke(typedPoolDictionary, new[] { valueOfGenericTypeOfInterface, monoPool });
                }
            }
        }

        private MonoBehaviour GetMonoWithInterface(GameObject gameObjectOfPoolMember, string interfaceName)
        {
            var components = gameObjectOfPoolMember.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component == null)
                {
                    Debug.Log($"{gameObjectOfPoolMember.name}", gameObjectOfPoolMember);
                    throw new NullReferenceException("There is prefab has contains missing script");
                }
                    
                var componentType = component.GetType();
                var interfaceType = componentType.GetInterface(interfaceName);
                if (interfaceType != null)
                    return component;
            }
            return null;
        }
    }
}