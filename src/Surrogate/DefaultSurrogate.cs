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
 * $Id: DefaultSurrogate.cs 14 2004-08-26 09:08:59Z Angelo $
 * */
#endregion

using System;
using CompactFormatter.Interfaces;
using CompactFormatter.Attributes;
using System.Reflection;

namespace CompactFormatter.Surrogate
{
	/// <summary>
	/// Summary description for FakeSurrogate.
	/// </summary>
	public class DefaultSurrogates
	{
		#region ISurrogate Members

		[Attributes.Surrogate(typeof(System.DBNull))]
		public static Object DBNullSurrogate(Type t)
		{
			return DBNull.Value;
		}


		[Attributes.Surrogate(typeof(System.Collections.Hashtable))]
		[Attributes.Surrogate(typeof(System.Collections.ArrayList))]
		public static Object DefaultSurrogate(Type t)
		{
			Type[] arrt = new Type[0];
			ConstructorInfo ci = t.GetConstructor(arrt);
			if (ci==null) Console.WriteLine("No parameterless constructor available for object {0}",t);
			return ci.Invoke(null);
		}

		#endregion
	}
}
