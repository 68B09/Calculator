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
2016/10/09	ZZO(68B09)	First Release.
*/

using System;

namespace Calculators
{
	#region CalculatorValueBase
	/// <summary>
	/// 数値基本クラス
	/// </summary>
	public class CalculatorValueBase : CalculatorItemBase
	{
	}
	#endregion

	#region CalculatorValue
	/// <summary>
	/// 数値クラス
	/// </summary>
	public class CalculatorValue : CalculatorValueBase
	{
		#region フィールド/プロパティー
		/// <summary>
		/// 数値
		/// </summary>
		private double val = double.NaN;

		/// <summary>
		/// 数値取得/設定
		/// </summary>
		public virtual double Value
		{
			get {
				return this.val;
			}

			set {
				this.val = value;
			}
		}
		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorValue()
		{
		}

		/// <summary>
		/// コンストラクタ(初期値指定)
		/// </summary>
		/// <param name="pValue">初期値</param>
		public CalculatorValue(double pValue)
			: this()
		{
			this.val = pValue;
		}

		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="pSrc">コピー元</param>
		public CalculatorValue(CalculatorValue pSrc)
		{
			this.val = pSrc.val;
		}
		#endregion

		#region 公開メソッド
		/// <summary>
		/// 文字列化
		/// </summary>
		/// <returns>string</returns>
		public override string ToString()
		{
			return base.ToString() + " Value:" + this.val;
		}
		#endregion
	}
	#endregion

	#region π値
	/// <summary>
	/// π値クラス
	/// </summary>
	public class CalculatorValuePi : CalculatorValue
	{
		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorValuePi()
			: base(Math.PI)
		{
		}
		#endregion
	}
	#endregion

	#region e値クラス
	/// <summary>
	/// e値クラス
	/// </summary>
	public class CalculatorValueE : CalculatorValue
	{
		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorValueE()
			: base(Math.E)
		{
		}
		#endregion
	}
	#endregion
}
