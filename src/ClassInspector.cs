#define CACHE_ENABLED
#region LGPL License
/* 
 * CompactFormatter: A generic formatter for the .NET Compact Framework
 * Copyright (C) 2004  Angelo Scotto (scotto_a@hotmail.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 * $Id: ClassInspector.cs 1 2004-08-13 18:29:52Z Angelo $
 * */
#endregion
using System;
using System.Reflection;
using System.Collections;

namespace CompactFormatter
{
	/// <summary>
	/// Class Inspector is the class responsible to extract fields from 
	/// a given type.
	/// The define symbol CACHE_ENABLED is used to activate field cache table.
	/// For each type serialized a copy of its field list is cached, therefore
	/// eliminating delay caused by reflection.
	/// </summary>
	public class ClassInspector
	{
#if CACHE_ENABLED
		private static Hashtable ReflectionCache = new Hashtable();
#endif

		public static void Clear()
		{
#if CACHE_ENABLED
			ReflectionCache.Clear();
#endif
		}

		public static FieldInfo[] InspectClass(Type type)
		{
#if CACHE_ENABLED
			if (ReflectionCache.Contains(type))
				return (FieldInfo[])ReflectionCache[type];
#endif

			FieldInfo[] array = type.GetFields(BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.Instance|BindingFlags.DeclaredOnly);
			ArrayList list = new ArrayList(array);

			//FieldInfo[] list = new FieldInfo[array.Length];
			//int index = 0;

			for(int i = 0; i < array.Length; i++)
			{
				//I'm going to consider only fields not marked with the 
				//NotSerialized attribute
				// !array[i].FieldType.IsPointer this was the old CFormatter, is still necessary?
				if ( array[i].GetCustomAttributes(typeof
					(Attributes.NotSerializedAttribute),false).Length!=0)
					list.Remove(array[i]);
			}

			return (FieldInfo[])list.ToArray(typeof(FieldInfo));
		}
	}
}
