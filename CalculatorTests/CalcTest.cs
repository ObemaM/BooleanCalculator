using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooleanMinimizerLibrary;
using System.Collections.Generic;
using System;

namespace CalculatorTests
{
    [TestClass]
    public class BooleanMinimizerTests
    {
        [TestMethod]
        public void GetIndicesByValue_ReturnsCorrectIndices()
        {
            string vector = "0110";
            var indices = BooleanMinimizer.GetIndicesByValue(vector, '1');

            CollectionAssert.AreEqual(new List<int> { 1, 2 }, indices);
        }

        [TestMethod]
        public void GetIndicesByValue_ReturnsрCorrectIndices()
        {
            string vector = "0000";
            var indices = BooleanMinimizer.GetIndicesByValue(vector, '1');

            CollectionAssert.AreEqual(new List<int>(), indices); // Ожидается пустой список
        }

        [TestMethod]
        public void MinimizeSDNF_ConstantZero_ReturnsZero()
        {
            string vector = "0000";
            var result = BooleanMinimizer.MinimizeSDNF(vector);

            Assert.AreEqual("0", result);
        }

        [TestMethod]
        public void MinimizeSDNF_ConstantOne_ReturnsOne()
        {
            string vector = "1111";
            var result = BooleanMinimizer.MinimizeSDNF(vector);

            Assert.AreEqual("1", result);
        }


        [TestMethod]
        public void MinimizeSKNF_ConstantZero_ReturnsZero()
        {
            string vector = "0000";
            var result = BooleanMinimizer.MinimizeSKNF(vector);

            Assert.AreEqual("0", result);
        }

        [TestMethod]
        public void MinimizeSKNF_ConstantOne_ReturnsOne()
        {
            string vector = "1111";
            var result = BooleanMinimizer.MinimizeSKNF(vector);

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void MinimizeSKNF_SimpleTwoVariables_ReturnsCorrectExpression()
        {
            string vector = "1010"; // XNOR
            var result = BooleanMinimizer.MinimizeSKNF(vector);

            Assert.AreEqual("(x ∨ ¬y) ∧ (¬x ∨ y)", result);
        }

        [TestMethod]
        public void QuineMcCluskey_IdentifiesPrimeImplicants()
        {
            var minterms = new List<int> { 0, 1, 2, 5 };
            var primeImplicants = BooleanMinimizer.QuineMcCluskey(minterms, 3); // Changed to 3 variables

            var expectedBits = new[] { "00-", "0-0", "-01" }; // Updated expected bits
            CollectionAssert.AreEquivalent(expectedBits, primeImplicants.ConvertAll(i => i.Bits));
        }

        [TestMethod]
        public void FindEssentialPrimeImplicants_FindsCorrectImplicants()
        {
            var implicants = new List<BooleanMinimizer.Implicant>
        {
            new BooleanMinimizer.Implicant("00-", new HashSet<int>{0, 1}),
            new BooleanMinimizer.Implicant("0-0", new HashSet<int>{0, 2}),
            new BooleanMinimizer.Implicant("-11", new HashSet<int>{5, 7}),
            new BooleanMinimizer.Implicant("1-0", new HashSet<int>{6})
        };

            var minterms = new List<int> { 0, 1, 2, 5, 6, 7 };
            var essentials = BooleanMinimizer.FindEssentialPrimeImplicants(implicants, minterms);

            var expectedBits = new[] { "00-", "0-0", "-11", "1-0" };
            CollectionAssert.AreEquivalent(expectedBits, essentials.ConvertAll(i => i.Bits));
        }

        [TestMethod]
        public void BuildExpression_ForPositiveForm_BuildsCorrectDNF()
        {
            var implicants = new List<BooleanMinimizer.Implicant>
        {
            new BooleanMinimizer.Implicant("01", new HashSet<int>{1}),
            new BooleanMinimizer.Implicant("10", new HashSet<int>{2})
        };

            var variables = new List<string> { "x", "y" };
            var result = BooleanMinimizer.BuildExpression(implicants, variables, true);

            Assert.AreEqual("(¬x ∧ y) ∨ (x ∧ ¬y)", result);
        }

        [TestMethod]
        public void BuildExpression_ForNegativeForm_BuildsCorrectCNF()
        {
            var implicants = new List<BooleanMinimizer.Implicant>
        {
            new BooleanMinimizer.Implicant("01", new HashSet<int>{1}),
            new BooleanMinimizer.Implicant("10", new HashSet<int>{2})
        };

            var variables = new List<string> { "x", "y" };
            var result = BooleanMinimizer.BuildExpression(implicants, variables, false);

            Assert.AreEqual("(x ∨ ¬y) ∧ (¬x ∨ y)", result);
        }

        [TestMethod]
        public void GetFullSDNF_ReturnsCorrectExpression()
        {
            string vector = "0110"; // XOR
            var result = BooleanMinimizer.GetFullSDNF(vector);

            Assert.AreEqual("(¬w ∧ x) ∨ (w ∧ ¬x)", result);
        }

        [TestMethod]
        public void GetFullSKNF_ReturnsCorrectExpression()
        {
            string vector = "0110"; // XOR
            var result = BooleanMinimizer.GetFullSKNF(vector);

            Assert.AreEqual("(w ∨ x) ∧ (¬w ∨ ¬x)", result);
        }

        [TestMethod]
        public void GetIndicesByValue_EmptyVector_ReturnsEmptyList()
        {
            string vector = "";
            var indices = BooleanMinimizer.GetIndicesByValue(vector, '1');
            CollectionAssert.AreEqual(new List<int>(), indices);
        }

        [TestMethod]
        public void GetIndicesByValue_NoMatchingValue_ReturnsEmptyList()
        {
            string vector = "0000";
            var indices = BooleanMinimizer.GetIndicesByValue(vector, '2');
            CollectionAssert.AreEqual(new List<int>(), indices);
        }

        [TestMethod]
        public void MinimizeSDNF_ComplexExpression_ReturnsCorrectMinimizedForm()
        {
            string vector = "0110110011000011"; // Complex 4-variable function
            var result = BooleanMinimizer.MinimizeSDNF(vector);
            Assert.IsTrue(result.Contains("∧") || result.Contains("∨")); // Should contain at least one operator
        }

        [TestMethod]
        public void MinimizeSKNF_ComplexExpression_ReturnsCorrectMinimizedForm()
        {
            string vector = "0110110011000011"; // Complex 4-variable function
            var result = BooleanMinimizer.MinimizeSKNF(vector);
            Assert.IsTrue(result.Contains("∧") || result.Contains("∨")); // Should contain at least one operator
        }

        [TestMethod]
        public void QuineMcCluskey_EmptyMinterms_ReturnsEmptyList()
        {
            var minterms = new List<int>();
            var result = BooleanMinimizer.QuineMcCluskey(minterms, 2);
            CollectionAssert.AreEqual(new List<BooleanMinimizer.Implicant>(), result);
        }

        [TestMethod]
        public void GetFullSDNF_ThreeVariables_ReturnsCorrectExpression()
        {
            string vector = "01101100"; // 3-variable function
            var result = BooleanMinimizer.GetFullSDNF(vector);
            Assert.IsTrue(result.Contains("∧") && result.Contains("∨")); // Should contain both operators
        }

        [TestMethod]
        public void GetFullSKNF_ThreeVariables_ReturnsCorrectExpression()
        {
            string vector = "01101100"; // 3-variable function
            var result = BooleanMinimizer.GetFullSKNF(vector);
            Assert.IsTrue(result.Contains("∧") && result.Contains("∨")); // Should contain both operators
        }

        [TestMethod]
        public void BuildExpression_EmptyImplicants_ReturnsEmptyString()
        {
            var implicants = new List<BooleanMinimizer.Implicant>();
            var variables = new List<string> { "x", "y" };
            var result = BooleanMinimizer.BuildExpression(implicants, variables, true);
            Assert.AreEqual("", result);
        }
    }

    [TestClass]
    public class KarnaughMapBuilderTests
    {
        private KarnaughMapBuilder builder;

        [TestInitialize]
        public void Initialize()
        {
            builder = new KarnaughMapBuilder();
        }


        [TestMethod]
        public void BuildForTwoVariables_ReturnsCorrectMap()
        {
            string vector = "0110";
            var variables = new List<string> { "x", "y" };

            var result = builder.BuildForTwoVariables(vector, variables);

            var expected = new List<List<string>>
            {
                new List<string> { "x\\y", "0", "1" },
                new List<string> { "0", "0", "1" },
                new List<string> { "1", "1", "0" }
            };

            CollectionAssert.AreEqual(expected[0], result[0]);
            CollectionAssert.AreEqual(expected[1], result[1]);
            CollectionAssert.AreEqual(expected[2], result[2]);
        }

        [TestMethod]
        public void BuildForFourVariables_ReturnsCorrectMap()
        {
            string vector = "0110110011000011"; // 16 бит
            var variables = new List<string> { "w", "x", "y", "z" };

            var result = builder.BuildForFourVariables(vector, variables);

            var expected = new List<List<string>>
            {
                new List<string> { "wx\\yz", "00", "01", "11", "10" },
                new List<string> { "00", "0", "1", "1", "0" },
                new List<string> { "01", "1", "1", "0", "0" },
                new List<string> { "11", "0", "0", "1", "1" },
                new List<string> { "10", "1", "1", "0", "0" }
            };

            for (int i = 0; i < expected.Count; i++)
            {
                CollectionAssert.AreEqual(expected[i], result[i]);
            }
        }

        [TestMethod]
        public void FindAllMaximalAreas_FindsLargestGroups()
        {
            var map = new List<List<string>>
            {
                new List<string> { "x\\y", "0", "1" },
                new List<string> { "0", "1", "1" },
                new List<string> { "1", "1", "0" }
            };

            var result = builder.FindAllMaximalAreas(map);

            Assert.AreEqual(2, result.Count); // Changed to expect 2 areas
            Assert.IsTrue(result.Any(a => a.StartRow == 0 && a.StartCol == 0 && a.Height == 1 && a.Width == 2));
            Assert.IsTrue(result.Any(a => a.StartRow == 0 && a.StartCol == 0 && a.Height == 2 && a.Width == 1));
        }

        [TestMethod]
        public void FindAllMaximalZeroAreas_FindsZeroGroups()
        {
            var map = new List<List<string>>
            {
                new List<string> { "x\\y", "0", "1" },
                new List<string> { "0", "0", "1" },
                new List<string> { "1", "1", "0" }
            };

            var result = builder.FindAllMaximalZeroAreas(map);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => a.StartRow == 0 && a.StartCol == 0 && a.Height == 1 && a.Width == 1));
            Assert.IsTrue(result.Any(a => a.StartRow == 1 && a.StartCol == 1 && a.Height == 1 && a.Width == 1));
        }

        [TestMethod]
        public void GetVariablesFromMap_ExtractsVariablesCorrectly()
        {
            var mapWithHeader = new List<List<string>>
            {
                new List<string> { "w\\xy", "00", "01", "11", "10" },
                new List<string> { "0", "0", "1", "1", "0" },
                new List<string> { "1", "1", "1", "0", "0" }
            };

            var result = builder.GetVariablesFromMap(mapWithHeader);

            CollectionAssert.AreEquivalent(new[] { "w", "x", "y" }, result);
        }

        [TestMethod]
        public void GetVariablesFromMap_UsesDefaultNamesIfNoHeader()
        {
            var mapWithoutHeader = new List<List<string>>
            {
                new List<string> { "00", "01", "11", "10" },
                new List<string> { "0", "1", "1", "0" }
            };

            var result = builder.GetVariablesFromMap(mapWithoutHeader);

            CollectionAssert.AreEquivalent(new[] { "x", "y" }, result); // Changed to expect 2 variables
        }

        [TestMethod]
        public void Area_GetCoveredCells_ReturnsExpectedCoordinates()
        {
            var area = new KarnaughMapBuilder.Area(1, 1, 2, 2);

            var covered = area.GetCoveredCells(4, 4).ToList();

            CollectionAssert.AreEquivalent(new[] { (1, 1), (1, 2), (2, 1), (2, 2) }, covered);
        }

        [TestMethod]
        public void BuildSteps_ForTwoVariables_ReturnsMultipleSteps()
        {
            string vector = "0110";
            var rootNode = new Node(NodeType.Vector, vector)
            {
                Variables = new List<string> { "x", "y" }
            };

            var steps = builder.BuildSteps(rootNode);

            Assert.AreEqual(5, steps.Count);

            Assert.AreEqual("Создаем заголовок таблицы", steps[0].Description);
            Assert.AreEqual("Добавляем строку для значения переменной x = 0", steps[1].Description);
            Assert.AreEqual("Заполняем ячейки для комбинаций y: 0, 1 при x=0", steps[2].Description);
            Assert.AreEqual("Добавляем строку для значения переменной x = 1", steps[3].Description);
            Assert.AreEqual("Заполняем ячейки для комбинаций y: 0, 1 при x=1", steps[4].Description);
        }

        [TestMethod]
        public void BuildSteps_ForThreeVariables_ReturnsMultipleSteps()
        {
            string vector = "01101100";
            var rootNode = new Node(NodeType.Vector, vector)
            {
                Variables = new List<string> { "w", "x", "y" }
            };

            var steps = builder.BuildSteps(rootNode);

            Assert.AreEqual(5, steps.Count);
        }

        [TestMethod]
        public void BuildSteps_ForFourVariables_ReturnsMultipleSteps()
        {
            string vector = "0110110011000011";
            var rootNode = new Node(NodeType.Vector, vector)
            {
                Variables = new List<string> { "w", "x", "y", "z" }
            };

            var steps = builder.BuildSteps(rootNode);

            Assert.AreEqual(5, steps.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Build_InvalidVariableCount_ThrowsException()
        {
            var root = new Node(NodeType.Vector, "01") { Variables = new List<string> { "x" } };
            builder.Build(root);
        }

        [TestMethod]
        public void BuildForThreeVariables_ReturnsCorrectMap()
        {
            string vector = "01101100";
            var variables = new List<string> { "x", "y", "z" };

            var result = builder.BuildForThreeVariables(vector, variables);

            Assert.AreEqual(3, result.Count); // Header + 2 rows
            Assert.AreEqual(5, result[0].Count); // Header + 4 columns
            Assert.AreEqual("x \\ yz", result[0][0]);
        }

        [TestMethod]
        public void BuildSteps_ForThreeVariables_ReturnsCorrectSteps()
        {
            string vector = "01101100";
            var rootNode = new Node(NodeType.Vector, vector)
            {
                Variables = new List<string> { "x", "y", "z" }
            };

            var steps = builder.BuildSteps(rootNode);

            Assert.IsTrue(steps.Count > 0);
            Assert.IsTrue(steps[0].Description.Contains("заголовок"));
        }

        [TestMethod]
        public void BuildSteps_ForFourVariables_ReturnsCorrectSteps()
        {
            string vector = "0110110011000011";
            var rootNode = new Node(NodeType.Vector, vector)
            {
                Variables = new List<string> { "w", "x", "y", "z" }
            };

            var steps = builder.BuildSteps(rootNode);

            Assert.IsTrue(steps.Count > 0);
            Assert.IsTrue(steps[0].Description.Contains("заголовок"));
        }
    }

    [TestClass]
    public class FunctionVectorBuilderTests
    {
        private FunctionVectorBuilder builder;

        [TestInitialize]
        public void Setup()
        {
            builder = new FunctionVectorBuilder();
        }

        [TestMethod]
        public void BuildVector_AndExpression_ReturnsCorrectVector()
        {
            // x ∧ y
            var node = new Node(NodeType.And,
                left: new Node(NodeType.Variable, "x"),
                right: new Node(NodeType.Variable, "y"));

            var vector = builder.BuildVector(node);

            Assert.AreEqual("0001", vector); // x ∧ y
        }

        [TestMethod]
        public void BuildVector_ConstantOne_ReturnsSingleOne()
        {
            var node = new Node(NodeType.Constant, "1");

            var vector = builder.BuildVector(node);

            Assert.AreEqual("1", vector);
        }

        [TestMethod]
        public void BuildVector_VectorWithoutVariables_InferVariables()
        {
            var node = new Node(NodeType.Vector, "1010");

            var vector = builder.BuildVector(node);

            Assert.AreEqual("1010", vector);
            CollectionAssert.AreEqual(new List<string> { "x", "y" }, node.Variables);
        }

        [TestMethod]
        public void BuildTruthTable_OrExpression_IsCorrect()
        {
            // x ∨ y
            var node = new Node(NodeType.Or,
                left: new Node(NodeType.Variable, "x"),
                right: new Node(NodeType.Variable, "y"));

            var table = builder.BuildTruthTable(node);

            Assert.AreEqual(4, table.Count);
            Assert.IsFalse(table[0]["F"]); // 00
            Assert.IsTrue(table[1]["F"]);  // 01
            Assert.IsTrue(table[2]["F"]);  // 10
            Assert.IsTrue(table[3]["F"]);  // 11
        }

        [TestMethod]
        public void BuildTruthTable_VectorNode_CorrectMapping()
        {
            var node = new Node(NodeType.Vector, "1001")
            {
                Variables = new List<string> { "x", "y" } // x - старший бит
            };

            var table = builder.BuildTruthTable(node);

            Assert.AreEqual(4, table.Count);
            Assert.AreEqual(true, table[0]["F"]);  // 00 -> '1'
            Assert.AreEqual(false, table[1]["F"]); // 01 -> '0'
            Assert.AreEqual(false, table[2]["F"]); // 10 -> '0'
            Assert.AreEqual(true, table[3]["F"]);  // 11 -> '1'
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Переменные для вектора не определены")]
        public void EvaluateVector_WithoutVariableList_Throws()
        {
            var node = new Node(NodeType.Vector, "1010");
            // Не указаны переменные
            builder.BuildTruthTable(node); // вызовет EvaluateVector => исключение
        }
    }

    [TestClass]
    public class SyntaxAnalyzerTests
    {
        private SyntaxAnalyzer analyzer;

        [TestInitialize]
        public void SetUp()
        {
            analyzer = new SyntaxAnalyzer();
        }

        [TestMethod]
        public void Parse_SingleVariable_ReturnsVariableNode()
        {
            var node = analyzer.Parse("x");

            Assert.AreEqual(NodeType.Variable, node.Type);
            Assert.AreEqual("x", node.Value);
        }

        [TestMethod]
        public void Parse_ConstantOne_ReturnsConstantNode()
        {
            var node = analyzer.Parse("1");

            Assert.AreEqual(NodeType.Constant, node.Type);
            Assert.AreEqual("1", node.Value);
        }

        [TestMethod]
        public void Parse_NotOperator_ReturnsNotNode()
        {
            var node = analyzer.Parse("¬x");

            Assert.AreEqual(NodeType.Not, node.Type);
            Assert.AreEqual(NodeType.Variable, node.Right.Type);
            Assert.AreEqual("x", node.Right.Value);
        }

        [TestMethod]
        public void Parse_AndExpression_ReturnsAndNode()
        {
            var node = analyzer.Parse("x∧y");

            Assert.AreEqual(NodeType.And, node.Type);
            Assert.AreEqual("x", node.Left.Value);
            Assert.AreEqual("y", node.Right.Value);
        }

        [TestMethod]
        public void Parse_ExpressionWithParentheses_ReturnsCorrectTree()
        {
            var node = analyzer.Parse("(x∨y)∧z");

            Assert.AreEqual(NodeType.And, node.Type);
            Assert.AreEqual(NodeType.Or, node.Left.Type);
            Assert.AreEqual("z", node.Right.Value);
        }

        [TestMethod]
        public void Parse_ImpliesExpression_ReturnsImpliesNode()
        {
            var node = analyzer.Parse("x→y");

            Assert.AreEqual(NodeType.Implies, node.Type);
            Assert.AreEqual("x", node.Left.Value);
            Assert.AreEqual("y", node.Right.Value);
        }

        [TestMethod]
        public void Parse_EquivalentExpression_ReturnsEquivalentNode()
        {
            var node = analyzer.Parse("x↔y");

            Assert.AreEqual(NodeType.Equivalent, node.Type);
            Assert.AreEqual("x", node.Left.Value);
            Assert.AreEqual("y", node.Right.Value);
        }

        [TestMethod]
        public void Parse_ValidVector_ReturnsVectorNode()
        {
            var node = analyzer.Parse("0101");

            Assert.AreEqual(NodeType.Vector, node.Type);
            Assert.AreEqual("0101", node.Value);
            CollectionAssert.AreEqual(new List<string> { "w", "x" }, node.Variables);
        }

        [TestMethod]
        public void Parse_VectorInsideExpression_ThrowsException()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => analyzer.Parse("x∧0101"));
            StringAssert.Contains(ex.Message, "Вектор не может находиться внутри выражения");
        }

        [TestMethod]
        public void Parse_EmptyInput_ThrowsException()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => analyzer.Parse(""));
            StringAssert.Contains(ex.Message, "Ввод не может быть пустым");
        }

        [TestMethod]
        public void Parse_InvalidSymbol_ThrowsException()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => analyzer.Parse("x$y"));
            StringAssert.Contains(ex.Message, "Неожиданный символ");
        }

        [TestMethod]
        public void Parse_MissingClosingParenthesis_ThrowsException()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => analyzer.Parse("(x∨y"));
            StringAssert.Contains(ex.Message, "Ожидалось ')'");
        }

        [TestMethod]
        public void Parse_VectorLengthNotPowerOfTwo_ThrowsException()
        {
            var ex = Assert.ThrowsException<ArgumentException>(() => analyzer.Parse("011"));
            StringAssert.Contains(ex.Message, "Вектор должен быть длины степени два");
        }
    }
}