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

FOR EXAMPLE

1.基本形
	Calculator calc = new Calculator();
	calc.EntryLine("1 + 2"); ← "2."以降は主にココの記述について
	CalculatorValue ans = calc.GetAnswer();
	WriteLine( ans.Value );

2.基本仕様
	・数値の表現にはdouble型を使用します

	・値や演算子の登録には Calculator.Entry() を使用します

	・計算結果は Calculator.GetAnswer() で得られます

	・Calculator.GetAnswer() を呼ぶまでは、Entry()系メソッドを何度でも呼び出せます

	・三角関数が使用する角度はラジアンです

	・関数(CalculatorOperatorOpenクラスもしくは派生クラス)は、必ず閉じ括弧で括ってください
	  閉じ括弧が足りない場合は式の末尾に閉じ括弧があるものとして計算されます。
	  (ex)"5 * ( 4 / ( 3 + 2" → "5 * ( 4 / ( 3 + 2 ) )"

	・式の正当性は評価しないため、不正な式には注意してください
	  (ex) 以下は、GetAnswer()を呼んだときに初めて例外が投げられます
	  "1 2 + 3"
	  "+ 2"

3.Entry(object)
	・Entry()は値や演算子を１つだけ登録するために使用します
	  calc.Entry(1);
	  calc.Entry("+");
	  calc.Entry(2);

	・objectが値なのか演算子なのかは CalculatorItemBase.Make() が判断します

	・演算子の一覧は OperatorKeyWord を参照のこと

	・一度に複数登録できる Entry(params object[]) もあります
	  calc.Entry(1, "+", 2);

4.EntryLine(string)
	・EntryLine()は値や演算子を一度に複数個登録するために使用します
	  calc.EntryLine("1 + 2");

	・各項目は空白文字で区切ってください

5.Entry(CalculatorItemBase)
	・値や演算子オブジェクトを直接作成・登録する場合に使用します。
	　自動解釈させない分、高速です。

	・x * COS( theta ) - y * SIN( theta )
	  double x = ***;
	  double y = ***;
	  double theta = ***;	// Radian 0～2π

	　calc.Entry( new CalculatorValue( x ) );		// x

	  calc.Entry( new CalculatorOperatorMul() );	// *

	  calc.Entry( new CalculatorOperatorCos() );	// cos( theta )
	　calc.Entry( new CalculatorValue( theta ) );
	　calc.Entry( new CalculatorOperatorClose() );

	  calc.Entry( new CalculatorOperatorSub() );	// -

	  calc.Entry( new CalculatorValue( y ) );		// y

	  calc.Entry( new CalculatorOperatorMul() );	// *

	  calc.Entry( new CalculatorOperatorSin() );	// sin( theta )
	　calc.Entry( new CalculatorValue( theta ) );
	　calc.Entry( new CalculatorOperatorClose() );

6.特殊変数「@」
	・式中で先頭が'@'で始まる文字列は変数とみなされ、GetItemEvent() が呼ばれます
  
		Calculator calc2 = new Calculator();
		calc2.GetItemEventHandler += delegate (string pKeyword) {
			switch (pKeyword) {
				case "@1":
					return new CalculatorValue(10);

				case "@2":
					return new CalculatorValue(20);
			}

			throw new NotImplementedException();
		};

		calc2.EntryLine("@1 * @2");

		ans = calc2.GetAnswer();	// ans.Value is 200

7."D2R"および"R2D"、"PI"、"E"について
	"D2R"はCalculatorOperatorDegToRad、"R2D"はCalculatorOperatorRadToDegインスタンスが作成されます。
	"D2R"はDegree→Radian、"R2D"はRadian→Degree変換を行います。
	(ex) "D2R( 180 )" → 3.1415926…

	"PI"はπ(Math.PI)、"E"はe(Math.E)を表す値が生成されます。

8.独自演算子や関数実装方法
	・三角関数のような「引数１つ」のタイプ
		始まり括弧(CalculatorOperatorOpen)から派生クラスを作ってください。
		→ CalculatorOperatorSqrt(),etc..

	・四則演算のような「引数２つ」のタイプ
		CalculatorOperatorBaseの派生クラスを作ってください。
		→ CalculatorOperatorPow(),CalculatorOperatorMax(),etc..

	・独自演算子はEntry(CalculatorItemBase)で登録してください

	・直接Calculators名前空間に追加を行うのであれば、以下の２つも編集しておくと文字列から各種インスタンスが生成できて便利です
		Calculators.OperatorKeyWord
		Calculators.CalculatorItemBase.InstanceTable

		尚、関数(引数１つ)型をOperatorKeyWordに登録する際は、閉じ括弧が必要とわかるようにするためにキーワードの末尾に'('を付加してください。

9.値の破壊防止方法
	※2016/10/10の修正で、値は破壊されなくなりました。

	<del>Entry()で登録した値オブジェクトのValue値は破壊されます。
	<del>防止するにはコピーを渡してください。
	<del>(ex)
	<del>CalculatorValue valItem = new CalculatorValue( *** );
	<del>Entry( new CalculatorValue( valItem ) );	// Entry Copy-Object

Etc...
	・テストコードはMain()に書いています
		使い方の参考にしてください。

------------------------------------------------------------------------------
[Update History]
2016/10/09	ZZO(MB68B09)	First Release.
2016/10/10	ZZO(MB68B09)	演算時に、スタックに積まれた値要素を破壊しないように変更
