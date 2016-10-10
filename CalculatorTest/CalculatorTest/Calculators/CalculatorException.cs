/*
The MIT License (MIT)

Copyright (c) 2016 ZZO

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

[Update History]
2016/10/10	ZZO(68B09)	First Release.
*/

using System;
using System.Runtime.Serialization;
using System.Security;

namespace Calculators
{
	/// <summary>
	/// InvalidArithmeticExpressionException
	/// </summary>
	public class InvalidArithmeticExpressionException : Exception
	{
		#region コンストラクタ
		public InvalidArithmeticExpressionException() : base()
		{
		}

		public InvalidArithmeticExpressionException(string message) : base(message)
		{
		}

		public InvalidArithmeticExpressionException(string message, Exception innerException) : base(message, innerException)
		{
		}

		[SecuritySafeCritical]
		protected InvalidArithmeticExpressionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
		#endregion
	}
}
