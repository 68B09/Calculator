using Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Pattern[] testPatternTbl = new Pattern[]{
				new Pattern("PI", Math.PI ),
				new Pattern("E", Math.E ),

				new Pattern("1 + 2", 1.0 + 2.0),
				new Pattern("2 - 3", 2.0 - 3.0),
				new Pattern("3 * 4", 3.0 * 4.0),
				new Pattern("4 / 5", 4.0 / 5.0),
				new Pattern("7 % 6", 7 % 6),
				new Pattern("7 MAX 8", 8),
				new Pattern("8 MIN 9", 8),
				new Pattern("9 ^ 2", 9 * 9),
				new Pattern("SQRT( 9 )",3 ),

				new Pattern("FLOOR( 1.5 )", 1),
				new Pattern("FLOOR( -1.5 )", -2),

				new Pattern("CEILING( 1.4 )", 2),
				new Pattern("CEILING( -1.4 )", -1),

				new Pattern("ROUND( 1.4 )", 1),
				new Pattern("ROUND( 1.5 )", 2),

				new Pattern("TRUNC( 1.9 )", 1),
				new Pattern("TRUNC( -1.9 )", -1),

				new Pattern("SIGN( -2 )", -1),
				new Pattern("SIGN( 0 )", 0),
				new Pattern("SIGN( 2 )", 1),

				new Pattern("ABS( 10 )", 10),
				new Pattern("ABS( -10 )", 10),

				new Pattern("D2R( 180 )", Math.PI ),
				new Pattern("R2D( PI )", 180 ),

				new Pattern("SIN( PI / 2 )", 1),
				new Pattern("COS( 0 )", 1),
				new Pattern("TAN( 1 )", Math.Tan(1) ),

				new Pattern("ASIN( 1 )", Math.Asin(1)),
				new Pattern("ACOS( 1 )", Math.Acos(1)),
				new Pattern("ATAN( 1 )", Math.Atan(1)),

				new Pattern("HSIN( 1 )", Math.Sinh(1)),
				new Pattern("HCOS( 1 )", Math.Cosh(1)),
				new Pattern("HTAN( 1 )", Math.Tanh(1)),

				new Pattern("LOG( 100 )", Math.Log(100)),
				new Pattern("LOG10( 100 )", Math.Log10(100)),

				new Pattern("EXP( 0.1 )", Math.Exp(0.1)),

				new Pattern("1 + 2 * 3", 1 + 2 * 3),
				new Pattern("( 1 + 2 ) * 3", ( 1 + 2 ) * 3),
			};

			Calculator calc = new Calculator();
			CalculatorValue ans;

			//----------------------------------------------------------------
			for (int i = 0; i < testPatternTbl.Length; i++) {
				Pattern pattern = testPatternTbl[i];
				Console.Write(string.Format("[{0}]:{1} ", i + 1, pattern.Cmd));

				calc.Clear();
				calc.EntryLine(pattern.Cmd);
				ans = calc.GetAnswer();
				Console.Write(string.Format("= {0} ", ans.Value));

				if (ans.Value != pattern.Ans) {
					Console.WriteLine("NG");
					throw new InvalidProgramException();
				}

				Console.WriteLine("OK");
			}

			//----------------------------------------------------------------
			calc.Clear();

			calc.Entry(new CalculatorOperatorOpen());
			calc.Entry(new CalculatorValue(1.2));
			calc.Entry(new CalculatorOperatorMul());
			calc.Entry(new CalculatorValue(3.4));
			calc.Entry(new CalculatorOperatorClose());

			calc.Entry(OperatorKeyWord.Sub);

			calc.Entry(new CalculatorOperatorOpen());
			calc.Entry(1.2);
			calc.Entry(OperatorKeyWord.Mul);
			calc.Entry(3.4);
			calc.Entry(new CalculatorOperatorClose());

			ans = calc.GetAnswer();
			if (ans.Value != 0) {
				Console.WriteLine("NG");
				throw new InvalidProgramException();
			}

			//----------------------------------------------------------------
			{
				CalculatorItemBase item1 = new CalculatorValue(12);
				CalculatorItemBase item2 = new CalculatorOperatorMod();
				CalculatorItemBase item3 = new CalculatorValue(5);

				calc.Clear();
				calc.Entry(item1, item2, item3);

				ans = calc.GetAnswer();
				if (ans.Value != 2) {
					Console.WriteLine("NG");
					throw new InvalidProgramException();
				}
			}

			//----------------------------------------------------------------
			{
				object item1 = -0.2;
				object item2 = "Max";
				object item3 = -0.1;

				calc.Clear();
				calc.Entry(item1, item2, item3);

				ans = calc.GetAnswer();
				if (ans.Value != -0.1) {
					Console.WriteLine("NG");
					throw new InvalidProgramException();
				}
			}

			//----------------------------------------------------------------
			{
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

				ans = calc2.GetAnswer();
				if (ans.Value != (10 * 20)) {
					Console.WriteLine("NG");
					throw new InvalidProgramException();
				}
			}

			//----------------------------------------------------------------
			Console.WriteLine("complete");
		}
	}

	class Pattern
	{
		public string Cmd { get; set; }
		public double Ans { get; set; }

		public Pattern(string pCmd, double pAns)
		{
			this.Cmd = pCmd;
			this.Ans = pAns;
		}
	}
}
