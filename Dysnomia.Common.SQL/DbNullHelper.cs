﻿/*
	MIT License

	Copyright (c) 2020 Axel "Elanis" Soupé

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
*/
using System;

namespace Dysnomia.Common.SQL {
	public static class DbNullHelper {
		/// <summary>
		/// Return string or DbNull if the string is null or empty
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static object DbNullOrString(this string val) {
			if (string.IsNullOrEmpty(val)) {
				return DBNull.Value;
			}

			return val;
		}

		/// <summary>
		/// Return value or DbNull if the value is null
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static object DbNullOrValue(this object val) {
			return val ?? DBNull.Value;
		}
	}
}
