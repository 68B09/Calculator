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
2016/10/10	ZZO(68B09)	値及び演算スタックを専用クラスに置き換え
*/

using System;
using System.Collections.Generic;

namespace Calculators
{
	/// <summary>
	/// 演算クラス
	/// </summary>
	public class Calculator
	{
		#region 定数
		/// <summary>
		/// 要素要求デリゲート
		/// </summary>
		/// <param name="pKeyword">要素名</param>
		/// <returns>CalculatorItemBase</returns>
		public delegate CalculatorItemBase GetItemDelegate(string pKeyword);
		#endregion

		#region フィールド/プロパティー
		/// <summary>
		/// 値スタック
		/// </summary>
		private ValueStack mStackValue = new ValueStack();

		/// <summary>
		/// 演算子スタック
		/// </summary>
		private OperatorStack mStackOperator = new OperatorStack();

		/// <summary>
		/// 要素要求イベント
		/// </summary>
		public event GetItemDelegate GetItemEventHandler;
		#endregion

		#region コンストラクタ
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Calculator()
		{
		}
		#endregion

		#region 公開メソッド
		/// <summary>
		/// クリア
		/// </summary>
		public void Clear()
		{
			this.mStackValue.Clear();
			this.mStackOperator.Clear();
		}

		/// <summary>
		/// 要素登録
		/// </summary>
		/// <param name="pItem">要素</param>
		/// <remarks>
		/// CalculatorItemBaseクラスおよび派生クラスを登録します。
		/// 登録する順番は数式に準じます。
		/// </remarks>
		public void Entry(CalculatorItemBase pItem)
		{
			if (pItem is CalculatorValueBase) {
				this.mStackValue.Push((CalculatorValue)pItem);
				return;
			}

			CalculatorOperatorBase opeThis = (CalculatorOperatorBase)pItem;

			// (
			if (opeThis.OperatorType == CalculatorOperatorBase.EnumOperatorType.Open) {
				this.mStackOperator.Push(opeThis);
				return;
			}

			// )
			if (opeThis.OperatorType == CalculatorOperatorBase.EnumOperatorType.Close) {
				while (true) {
					CalculatorOperatorBase ope = this.mStackOperator.Pop();
					ope.Calculation(this.mStackValue);

					if (ope.OperatorType == CalculatorOperatorBase.EnumOperatorType.Open) {
						break;
					}
				}
				return;
			}

			if (mStackOperator.Count == 0) {
				this.mStackOperator.Push(opeThis);
				return;
			}

			CalculatorOperatorBase opeBefore = mStackOperator.Peek();

			int priority = opeBefore.ComparePriority(opeThis);

			if (priority < 0) {
				this.mStackOperator.Push(opeThis);
				return;
			}

			opeBefore.Calculation(this.mStackValue);
			mStackOperator.Pop();

			this.mStackOperator.Push(opeThis);
		}

		/// <summary>
		/// 複数要素登録
		/// </summary>
		/// <param name="pItems">要素</param>
		public void Entry(params CalculatorItemBase[] pItems)
		{
			foreach (CalculatorItemBase item in pItems) {
				this.Entry(item);
			}
		}

		/// <summary>
		/// 要素登録(自動解釈型)
		/// </summary>
		/// <param name="pObject">要素</param>
		/// <remarks>
		/// 様々な型の要素を判別し、登録します。
		/// 詳しくはCalculatorItemBase.Make()メソッドを参照。
		/// (ex)
		/// Entry(1);
		/// Entry("+");
		/// Entry(2);
		/// </remarks>
		/// <seealso cref="CalculatorItemBase.Make"/>
		public void Entry(object pObject)
		{
			object obj;

			string str = pObject.ToString();
			if ((str.Length > 0) && (str[0] == '@')) {
				obj = this.GetItemEventHandler(str);
			} else {
				obj = CalculatorItemBase.Make(pObject);
			}

			if (obj == null) {
				throw new ArgumentNullException();
			}

			this.Entry((CalculatorItemBase)obj);
		}

		/// <summary>
		/// 複数要素登録(自動解釈型)
		/// </summary>
		/// <param name="pObject">要素</param>
		public void Entry(params object[] pObjects)
		{
			foreach (object obj in pObjects) {
				this.Entry(obj);
			}
		}

		/// <summary>
		/// 複数要素登録(自動解釈型)
		/// </summary>
		/// <param name="pLine">式文字列</param>
		/// <remarks>
		/// 文字列中の各要素をEntry(Object)を使用して登録します。
		/// 各要素は空白文字で区切られている必要があります。
		/// (ex)
		/// EntryLine("1 + 2")
		/// </remarks>
		public void EntryLine(string pLine)
		{
			string[] items = pLine.Split(' ', '　', '\t');

			foreach (string item in items) {
				if (string.IsNullOrWhiteSpace(item) == false) {
					this.Entry(item);
				}
			}
		}

		/// <summary>
		/// 計算結果取得
		/// </summary>
		/// <returns>CalculatorValue</returns>
		/// <remarks>
		/// 計算結果を返します。
		/// </remarks>
		public CalculatorValue GetAnswer()
		{
			while (this.mStackOperator.Count > 0) {
				CalculatorOperatorBase ope = this.mStackOperator.Pop();
				ope.Calculation(this.mStackValue);
			}

			if (this.mStackValue.Count != 1) {
				throw new InvalidArithmeticExpressionException("値がスタックに残っています");
			}

			return this.mStackValue.Pop();
		}
		#endregion
	}

	#region ValueStack
	/// <summary>
	/// 値スタッククラス
	/// </summary>
	public class ValueStack : Stack<CalculatorValue>
	{
	}
	#endregion

	#region OperatorStack
	/// <summary>
	/// 演算子スタッククラス
	/// </summary>
	public class OperatorStack : Stack<CalculatorOperatorBase>
	{
	}
	#endregion
}
