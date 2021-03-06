﻿using System;
using System.Collections.Generic;

namespace WPNest {

	internal class ServiceContainer {

		private static readonly ServiceContainer _container;

		static ServiceContainer() {
			_container = new ServiceContainer();
		}

		public static void Clear() {
			_container.ClearServices();
		}

		public static T GetService<T>() where T : class {
			return _container.GetDependency<T>();
		}

		public static void RegisterService<T>(T value) where T : class {
			_container.RegisterDependency<T>(value);
		}

		public static void RegisterService<T, U>()
			where T : class
			where U : class {
			_container.RegisterDependency<T, U>();
		}

		private readonly Dictionary<Type, object> registeredDependencies = new Dictionary<Type, object>();
		private readonly Dictionary<Type, Type> registeredTypes = new Dictionary<Type, Type>();

		public void ClearServices() {
			registeredDependencies.Clear();
			registeredTypes.Clear();
		}

		public T GetDependency<T>() where T : class {
			if (registeredDependencies.ContainsKey(typeof(T)))
				return (T)registeredDependencies[typeof(T)];

			if (registeredTypes.ContainsKey(typeof(T))) {
				Type type = registeredTypes[typeof(T)];
				return (T)Activator.CreateInstance(type);
			}

			System.Diagnostics.Debug.WriteLine("Unknown dependency requested: {0}", typeof(T).Name);
			return null;
		}

		public void RegisterDependency<T>(T value) where T : class {
			if (registeredDependencies.ContainsKey(typeof(T))) {
				registeredDependencies[typeof(T)] = value;
			}
			else {
				registeredDependencies.Add(typeof(T), value);
			}
		}

		public void RegisterDependency<T, U>()
			where T : class
			where U : class {
			if (registeredTypes.ContainsKey(typeof(T))) {
				registeredTypes[typeof(T)] = typeof(U);
			}
			else {
				registeredTypes.Add(typeof(T), typeof(U));
			}
		}
	}
}
