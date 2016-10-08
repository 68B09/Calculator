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
using System.Globalization;

namespace Calculators
{
	/// <summary>
	/// 基本要素クラス
	/// </summary>
	public class CalculatorItemBase
	{
		#region フィールド/プロパティー
		/// <summary>
		/// 演算優先順位
		/// </summary>
		/// <remarks>
		/// 値が大きいほど優先順位が高い
		/// </remarks>
		private int priority = 0;

		/// <summary>
		/// 演算優先順位取得/設定
		/// </summary>
		public int Priority
		{
			get {
				return this.priority;
			}

			set {
				this.priority = value;
			}
		}
		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public CalculatorItemBase()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="pPriority">演算優先順位</param>
		public CalculatorItemBase(int pPriority)
		{
			this.priority = pPriority;
		}
		#endregion

		#region 公開メソッド
		/// <summary>
		/// 優先順位比較
		/// </summary>
		/// <param name="pTarget">比較対象</param>
		/// <returns>負=this＜pTarget、0=this==pTarget、正=this＞pTarget</returns>
		public int ComparePriority(CalculatorItemBase pTarget)
		{
			if (this.Priority < pTarget.Priority) {
				return -1;
			} else if (this.Priority > pTarget.Priority) {
				return 1;
			} else {
				return 0;
			}
		}

		/// <summary>
		/// 要素推測・生成
		/// </summary>
		/// <param name="pObject">推測元オブジェクト</param>
		/// <returns>object</returns>
		/// <seealso cref="Calculators.OperatorKeyWord"/>
		/// <remarks>
		/// pObjectから推測した要素を作成しこれを返します。
		/// 推測可能なオブジェクト(文字列)は OperatorKeyWord を参照のこと。
		/// OperatorKeyWord に無く、かつIConvertibleを継承するオブジェクトは数値と見なされます。
		/// (ex)
		/// Make("(");
		/// </remarks>
		static public object Make(object pObject)
		{
			object obj = CalculatorItemBase.Make(InstanceTable, pObject);
			if (obj != null) {
				return obj;
			}

			if (pObject is IConvertible) {
				return new CalculatorValue(((IConvertible)pObject).ToDouble(CultureInfo.CurrentCulture.NumberFormat));
			}

			return null;
		}

		/// <summary>
		/// 要素生成
		/// </summary>
		/// <param name="pTable">キーワードテーブル</param>
		/// <param name="pKey">要素キーワード</param>
		/// <returns>object</returns>
		static public object Make(string[,] pTable, object pKey)
		{
			string strwk = pKey.ToString().Trim().ToUpper();

			for (int i = 0; i < pTable.GetLength(0); i++) {
				if (pTable[i, 0].CompareTo(strwk) == 0) {
					Type t = Type.GetType(pTable[i, 1]);
					object obj = t.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);
					return obj;
				}
			}

			return null;
		}

		/// <summary>
		/// キーワードテーブル
		/// </summary>
		/// <remarks>
		/// [,0]=キーワード、[,1]=完全修飾クラス名
		/// </remarks>
		static public string[,] InstanceTable = new string[,] {
			{ OperatorKeyWord.PI, "Calculators.CalculatorValuePi" },
			{ OperatorKeyWord.E, "Calculators.CalculatorValueE" },

			{ OperatorKeyWord.Open, "Calculators.CalculatorOperatorOpen" },
			{ OperatorKeyWord.Close, "Calculators.CalculatorOperatorClose" },
			{ OperatorKeyWord.Add, "Calculators.CalculatorOperatorAdd" },
			{ OperatorKeyWord.Sub, "Calculators.CalculatorOperatorSub" },
			{ OperatorKeyWord.Mul, "Calculators.CalculatorOperatorMul" },
			{ OperatorKeyWord.Div, "Calculators.CalculatorOperatorDiv" },
			{ OperatorKeyWord.Mod, "Calculators.CalculatorOperatorMod" },
			{ OperatorKeyWord.Max, "Calculators.CalculatorOperatorMax" },
			{ OperatorKeyWord.Min, "Calculators.CalculatorOperatorMin" },
			{ OperatorKeyWord.Pow, "Calculators.CalculatorOperatorPow" },
			{ OperatorKeyWord.Sqrt, "Calculators.CalculatorOperatorSqrt" },
			{ OperatorKeyWord.Floor, "Calculators.CalculatorOperatorFloor" },
			{ OperatorKeyWord.Ceiling, "Calculators.CalculatorOperatorCeiling" },
			{ OperatorKeyWord.Round, "Calculators.CalculatorOperatorRound" },
			{ OperatorKeyWord.Trunc, "Calculators.CalculatorOperatorTrunc" },
			{ OperatorKeyWord.Sign, "Calculators.CalculatorOperatorSign" },
			{ OperatorKeyWord.Abs, "Calculators.CalculatorOperatorAbs" },
			{ OperatorKeyWord.Sin, "Calculators.CalculatorOperatorSin" },
			{ OperatorKeyWord.Cos, "Calculators.CalculatorOperatorCos" },
			{ OperatorKeyWord.Tan, "Calculators.CalculatorOperatorTan" },
			{ OperatorKeyWord.ASin, "Calculators.CalculatorOperatorAsin" },
			{ OperatorKeyWord.ACos, "Calculators.CalculatorOperatorAcos" },
			{ OperatorKeyWord.ATan, "Calculators.CalculatorOperatorAtan" },
			{ OperatorKeyWord.HSin, "Calculators.CalculatorOperatorHsin" },
			{ OperatorKeyWord.HCos, "Calculators.CalculatorOperatorHcos" },
			{ OperatorKeyWord.HTan, "Calculators.CalculatorOperatorHtan" },
			{ OperatorKeyWord.Log, "Calculators.CalculatorOperatorLog" },
			{ OperatorKeyWord.Log10, "Calculators.CalculatorOperatorLog10" },
			{ OperatorKeyWord.Exp, "Calculators.CalculatorOperatorExp" },
			{ OperatorKeyWord.D2R, "Calculators.CalculatorOperatorDegToRad" },
			{ OperatorKeyWord.R2D, "Calculators.CalculatorOperatorRadToDeg" },
		};
		#endregion
	}

	#region OperatorKeyWord
	/// <summary>
	/// 演算子類キーワード定義クラス
	/// </summary>
	static public class OperatorKeyWord
	{
		public const string Open = "(";
		public const string Close = ")";
		public const string Add = "+";
		public const string Sub = "-";
		public const string Mul = "*";
		public const string Div = "/";
		public const string Mod = "%";
		public const string Max = "MAX";
		public const string Min = "MIN";
		public const string Pow = "^";
		public const string Sqrt = "SQRT(";
		public const string Floor = "FLOOR(";
		public const string Ceiling = "CEILING(";
		public const string Round = "ROUND(";
		public const string Trunc = "TRUNC(";
		public const string Sign = "SIGN(";
		public const string Abs = "ABS(";
		public const string Sin = "SIN(";
		public const string Cos = "COS(";
		public const string Tan = "TAN(";
		public const string ASin = "ASIN(";
		public const string ACos = "ACOS(";
		public const string ATan = "ATAN(";
		public const string HSin = "HSIN(";
		public const string HCos = "HCOS(";
		public const string HTan = "HTAN(";
		public const string Log = "LOG(";
		public const string Log10 = "LOG10(";
		public const string Exp = "EXP(";
		public const string R2D = "R2D(";
		public const string D2R = "D2R(";

		public const string PI = "PI";
		public const string E = "E";
	}
	#endregion
}
