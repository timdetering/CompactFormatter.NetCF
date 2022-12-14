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
 * $Id: GhostDataSetOverrider.cs 1 2004-08-13 18:29:52Z Angelo $
 * */
#endregion

using System;
using System.Data;
using System.Collections;

namespace CompactFormatter.Surrogate
{
	/// <summary>
	/// Summary description for DataTableOverrider.
	/// </summary>
	[Attributes.Overrider(typeof(System.Data.DataTable))]
	public class GhostDataTableOverrider : Interfaces.IOverrider
	{
		public GhostDataTableOverrider()
		{}

		#region ICFormatter Members

		public void Serialize(CompactFormatter parent, System.IO.Stream serializationStream, object graph)
		{
			ArrayList colNames = new ArrayList();
			ArrayList colTypes = new ArrayList();
			ArrayList dataRows = new ArrayList();

			DataTable dt = (DataTable)graph;
			foreach(DataColumn col in dt.Columns) 
			{
				colNames.Add(col.ColumnName); 
				colTypes.Add(col.DataType.FullName);   
			}

			foreach(DataRow row in dt.Rows)
				dataRows.Add(row.ItemArray);

			// Now i've to serialize three ArrayList using the CompactFormatter main routines
			parent.Serialize(serializationStream,colNames);
			parent.Serialize(serializationStream,colTypes);
			parent.Serialize(serializationStream,dataRows);
		}

		public object Deserialize(CompactFormatter parent, System.IO.Stream serializationStream)
		{

			ArrayList colNames = (ArrayList)parent.Deserialize(serializationStream);
			ArrayList colTypes = (ArrayList)parent.Deserialize(serializationStream);
			ArrayList dataRows = (ArrayList)parent.Deserialize(serializationStream);

			DataTable dt = new DataTable();

			// Add columns
			for(int i=0; i<colNames.Count; i++)
			{
				DataColumn col = new DataColumn(colNames[i].ToString(), 
					Type.GetType(colTypes[i].ToString()));     
				dt.Columns.Add(col);
			}

			// Add rows
			for(int i=0; i<dataRows.Count; i++)
			{
				DataRow row = dt.NewRow();
				row.ItemArray = (Object[])dataRows[i];
				dt.Rows.Add(row);
			}

			dt.AcceptChanges();
			return dt;
		}

		#endregion
	}
}
