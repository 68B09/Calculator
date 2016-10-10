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
2016/10/10	ZZO(68B09)	演算時にスタックに積まれた値要素を破壊しないように変更
						v1 = v1 op v2 → v3 = v1 op v2
*/

using System;
using System.Collections.Generic;

/*
priority	operator/function
	 0		'(',Sqrt,Floor,Ceiling,Round,Sign,Abs,…
	10		+,-
	20		*,/,%
	30		Max,Min,Pow,…
*/

namespace Calculators
{
	#region 演算子基本クラス
	/// <summary>
	/// 演算子基本クラス
	/// </summary>
	public class CalculatorOperatorBase : CalculatorItemBase
	{
		#region 固定値
		/// <summary>
		/// 演算子タイプ定義
		/// </summary>
		public enum EnumOperatorType
		{
			Operator = 0,
			Open = 1,
			Close = 2,
		}

		/// <summary>
		/// 加算系演算優先順位
		/// </summary>
		public const int PriorityAdd = 10;

		/// <summary>
		/// 乗算系演算優先順位
		/// </summary>
		public const int PriorityMul = 20;

		/// <summary>
		/// 関数系演算子優先順位
		/// </summary>
		public const int PriorityFunc = 30;
		#endregion

		#region フィールド/プロパティー
		/// <summary>
		/// 演算子タイプ
		/// </summary>
		protected EnumOperatorType operatorType = EnumOperatorType.Operator;

		/// <summary>
		/// 演算子タイプ取得/設定
		/// </summary>
		public EnumOperatorType OperatorType
		{
			get {
				return this.operatorType;
			}

			set {
				this.operatorType = value;
			}
		}
		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorBase()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="pPriority">演算優先順位</param>
		public CalculatorOperatorBase(int pPriority) : base(pPriority)
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="pPriority">演算優先順位</param>
		/// <param name="pType">演算子タイプ</param>
		public CalculatorOperatorBase(int pPriority, EnumOperatorType pType) : base(pPriority)
		{
			this.operatorType = pType;
		}
		#endregion

		#region 公開メソッド
		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public virtual void Calculation(ValueStack pStackValue)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region 内部メソッド
		/// <summary>
		/// １値取得
		/// </summary>
		/// <param name="pValueStack">ValueStack</param>
		/// <param name="pValue1">値１</param>
		/// <returns>値１のコピー(pValueStackの先頭にpush済み)</returns>
		protected virtual CalculatorValue Get1Value(ValueStack pValueStack, out CalculatorValue pValue1)
		{
			pValue1 = pValueStack.Pop();

			// pValue1破壊防止のため、身代わりのコピーを作ってスタックに積む
			CalculatorValue newValue = new CalculatorValue(pValue1);
			pValueStack.Push(newValue);
			return newValue;
		}

		/// <summary>
		/// ２値取得
		/// </summary>
		/// <param name="pValueStack">ValueStack</param>
		/// <param name="pValue1">値１</param>
		/// <param name="pValue2">値２</param>
		/// <returns>値１のコピー(pValueStackの先頭にpush済み)</returns>
		protected virtual CalculatorValue Get2Value(ValueStack pValueStack, out CalculatorValue pValue1, out CalculatorValue pValue2)
		{
			pValue2 = pValueStack.Pop();
			pValue1 = pValueStack.Pop();

			// pValue1破壊防止のため、身代わりのコピーを作ってスタックに積む
			CalculatorValue newValue = new CalculatorValue(pValue1);
			pValueStack.Push(newValue);
			return newValue;
		}
		#endregion
	}
	#endregion

	#region CalculatorOperatorOpen
	public class CalculatorOperatorOpen : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorOpen() : base(0, EnumOperatorType.Open)
		{
			// NOP
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
		}
	}
	#endregion

	#region CalculatorOperatorClose
	public class CalculatorOperatorClose : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorClose() : base(int.MaxValue, EnumOperatorType.Close)
		{
		}
	}
	#endregion

	#region CalculatorOperatorAdd
	public class CalculatorOperatorAdd : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorAdd() : base(PriorityAdd)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = v1.Value + v2.Value;
		}
	}
	#endregion

	#region CalculatorOperatorSub
	public class CalculatorOperatorSub : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorSub() : base(PriorityAdd)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = v1.Value - v2.Value;
		}
	}
	#endregion

	#region CalculatorOperatorMul
	public class CalculatorOperatorMul : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorMul() : base(PriorityMul)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = v1.Value * v2.Value;
		}
	}
	#endregion

	#region CalculatorOperatorDiv
	public class CalculatorOperatorDiv : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorDiv() : base(PriorityMul)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = v1.Value / v2.Value;
		}
	}
	#endregion

	#region CalculatorOperatorMod
	public class CalculatorOperatorMod : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorMod() : base(PriorityMul)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = v1.Value % v2.Value;
		}
	}
	#endregion

	#region CalculatorOperatorMax
	public class CalculatorOperatorMax : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorMax() : base(PriorityFunc)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = Math.Max(v1.Value, v2.Value);
		}
	}
	#endregion

	#region CalculatorOperatorMin
	public class CalculatorOperatorMin : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorMin() : base(PriorityFunc)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = Math.Min(v1.Value, v2.Value);
		}
	}
	#endregion

	#region CalculatorOperatorPow
	public class CalculatorOperatorPow : CalculatorOperatorBase
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorPow() : base(PriorityFunc)
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1, v2;
			CalculatorValue newValue = this.Get2Value(pStackValue, out v1, out v2);

			newValue.Value = Math.Pow(v1.Value, v2.Value);
		}
	}
	#endregion

	#region CalculatorOperatorSqrt
	public class CalculatorOperatorSqrt : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorSqrt()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Sqrt(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorFloor
	public class CalculatorOperatorFloor : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorFloor()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Floor(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorCeiling
	public class CalculatorOperatorCeiling : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorCeiling()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Ceiling(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorRound
	public class CalculatorOperatorRound : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorRound()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Round(v1.Value, MidpointRounding.AwayFromZero);
		}
	}
	#endregion

	#region CalculatorOperatorSign
	public class CalculatorOperatorSign : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorSign()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Sign(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorAbs
	public class CalculatorOperatorAbs : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorAbs()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Abs(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorSin
	public class CalculatorOperatorSin : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorSin()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Sin(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorCos
	public class CalculatorOperatorCos : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorCos()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Cos(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorTan
	public class CalculatorOperatorTan : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorTan()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Tan(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorAsin
	public class CalculatorOperatorAsin : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorAsin()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Asin(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorAcos
	public class CalculatorOperatorAcos : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorAcos()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Acos(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorAtan
	public class CalculatorOperatorAtan : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorAtan()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Atan(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorHsin
	public class CalculatorOperatorHsin : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorHsin()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Sinh(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorHcos
	public class CalculatorOperatorHcos : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorHcos()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Cosh(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorHtan
	public class CalculatorOperatorHtan : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorHtan()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Tanh(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorLog
	public class CalculatorOperatorLog : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorLog()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Log(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorLog10
	public class CalculatorOperatorLog10 : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorLog10()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Log10(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorExp
	public class CalculatorOperatorExp : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorExp()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Exp(v1.Value);
		}
	}
	#endregion

	#region CalculatorOperatorDegToRad
	public class CalculatorOperatorDegToRad : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorDegToRad()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = v1.Value * Math.PI / 180.0;
		}
	}
	#endregion

	#region CalculatorOperatorRadToDeg
	public class CalculatorOperatorRadToDeg : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorRadToDeg()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = v1.Value * 180.0 / Math.PI;
		}
	}
	#endregion

	#region CalculatorOperatorTrunc
	public class CalculatorOperatorTrunc : CalculatorOperatorOpen
	{
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorOperatorTrunc()
		{
		}

		/// <summary>
		/// 演算
		/// </summary>
		/// <param name="pStackValue">値スタック</param>
		public override void Calculation(ValueStack pStackValue)
		{
			CalculatorValue v1;
			CalculatorValue newValue = this.Get1Value(pStackValue, out v1);

			newValue.Value = Math.Truncate(v1.Value);
		}
	}
	#endregion
}
