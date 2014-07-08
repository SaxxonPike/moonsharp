﻿using System;
using MoonSharp.Interpreter.Execution;
using NUnit.Framework;

namespace MoonSharp.Interpreter.Tests
{
	[TestFixture]
	public class ClosureTests
	{
		[Test]
		public void ClosureOnParam()
		{
			string script = @"
				local function g (z)
				  local function f(a)
					return a + z;
				  end
				  return f;
				end

				return (g(3)(2));";

			DynValue res = Script.RunString(script);

			Assert.AreEqual(DataType.Number, res.Type);
			Assert.AreEqual(5, res.Number);
		}



		[Test]
		public void Closures()
		{
			// expected : 201 2001 20001 200001 2000001
			string script = @"
						a = {}
						x = 0

						function container()
							local x = 20

							for i=1,5 do
								local y = 0
								a[i] = function () y=y+1; x = x * 10; return x+y end
							end
						end

						container();

						x = 4000

						return a[1](), a[2](), a[3](), a[4](), a[5]()";


			DynValue res = Script.RunString(script);

			Assert.AreEqual(DataType.Tuple, res.Type);
			Assert.AreEqual(5, res.Tuple.Length);
			Assert.AreEqual(DataType.Number, res.Tuple[0].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[1].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[2].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[3].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[4].Type);
			Assert.AreEqual(201, res.Tuple[0].Number);
			Assert.AreEqual(2001, res.Tuple[1].Number);
			Assert.AreEqual(20001, res.Tuple[2].Number);
			Assert.AreEqual(200001, res.Tuple[3].Number);
			Assert.AreEqual(2000001, res.Tuple[4].Number);
		}

		[Test]
		public void ClosuresNonAnonymousLocal()
		{
			// expected : 201 2001 20001 200001 2000001
			string script = @"
						a = {}
						x = 0

						function container()
							local x = 20

							for i=1,5 do
								local y = 0
								local function zz() y=y+1; x = x * 10; return x+y end
								a[i] = zz;
							end
						end

						container();

						x = 4000

						return a[1](), a[2](), a[3](), a[4](), a[5]()";

			DynValue res = Script.RunString(script);

			Assert.AreEqual(DataType.Tuple, res.Type);
			Assert.AreEqual(5, res.Tuple.Length);
			Assert.AreEqual(DataType.Number, res.Tuple[0].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[1].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[2].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[3].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[4].Type);
			Assert.AreEqual(201, res.Tuple[0].Number);
			Assert.AreEqual(2001, res.Tuple[1].Number);
			Assert.AreEqual(20001, res.Tuple[2].Number);
			Assert.AreEqual(200001, res.Tuple[3].Number);
			Assert.AreEqual(2000001, res.Tuple[4].Number);
		}


		[Test]
		public void ClosuresNonAnonymous()
		{
			// expected : 201 2001 20001 200001 2000001
			string script = @"
						a = {}
						x = 0

						function container()
							local x = 20

							for i=1,5 do
								local y = 0
								function zz() y=y+1; x = x * 10; return x+y end
								a[i] = zz;
							end
						end

						container();

						x = 4000

						return a[1](), a[2](), a[3](), a[4](), a[5]()";

			DynValue res = Script.RunString(script);

			Assert.AreEqual(DataType.Tuple, res.Type);
			Assert.AreEqual(5, res.Tuple.Length);
			Assert.AreEqual(DataType.Number, res.Tuple[0].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[1].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[2].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[3].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[4].Type);
			Assert.AreEqual(201, res.Tuple[0].Number);
			Assert.AreEqual(2001, res.Tuple[1].Number);
			Assert.AreEqual(20001, res.Tuple[2].Number);
			Assert.AreEqual(200001, res.Tuple[3].Number);
			Assert.AreEqual(2000001, res.Tuple[4].Number);
		}

		[Test]
		public void ClosureNoTable()
		{
			string script = @"
				x = 0

				function container()
					local x = 20

					for i=1,5 do
						local y = 0
		
						function zz() y=y+1; x = x * 10; return x+y end
		
						a1 = a2;
						a2 = a3;
						a3 = a4;
						a4 = a5;
						a5 = zz;
					end
				end

				container();

				x = 4000

				return a1(), a2(), a3(), a4(), a5()";

			DynValue res = Script.RunString(script);

			Assert.AreEqual(DataType.Tuple, res.Type);
			Assert.AreEqual(5, res.Tuple.Length);
			Assert.AreEqual(DataType.Number, res.Tuple[0].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[1].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[2].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[3].Type);
			Assert.AreEqual(DataType.Number, res.Tuple[4].Type);
			Assert.AreEqual(201, res.Tuple[0].Number);
			Assert.AreEqual(2001, res.Tuple[1].Number);
			Assert.AreEqual(20001, res.Tuple[2].Number);
			Assert.AreEqual(200001, res.Tuple[3].Number);
			Assert.AreEqual(2000001, res.Tuple[4].Number);
		}


		[Test]
		public void NestedUpvaluesNotQuiteWorking()
		{
			string script = @"
	local y = y;

	local x = x;
	local m = { };

	function m:a()
		self.t = {
			dojob = function() 
				if (x == nil) then return 1; else return 0; end
			end,
		};
	end

	m:a();

	return 10 * m.t.dojob();
								";

			DynValue res = Script.RunString(script);

			Assert.AreEqual(DataType.Number, res.Type);
			Assert.AreEqual(10, res.Number);
		}


	}
}