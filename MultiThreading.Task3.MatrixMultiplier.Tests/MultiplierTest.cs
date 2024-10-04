using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            // todo: implement a test method to check the size of the matrix which makes parallel multiplication more effective than
            // todo: the regular one
        
            const int maxSize = 1000; // Adjust this for larger sizes
            const int iterations = 10;

            long syncTime = 0;
            long parallelTime = 0;

            for (int size = 100; size <= maxSize; size += 100)
            {
                var m1 = CreateRandomMatrix(size, size);
                var m2 = CreateRandomMatrix(size, size);

                // Measure synchronous multiplication
                var syncMultiplier = new MatricesMultiplier();
                var watchSync = System.Diagnostics.Stopwatch.StartNew();
                for (int i = 0; i < iterations; i++)
                {
                    syncMultiplier.Multiply(m1, m2);
                }
                watchSync.Stop();
                syncTime = watchSync.ElapsedMilliseconds;

                // Measure parallel multiplication
                var parallelMultiplier = new MatricesMultiplierParallel();
                var watchParallel = System.Diagnostics.Stopwatch.StartNew();
                for (int i = 0; i < iterations; i++)
                {
                    parallelMultiplier.Multiply(m1, m2);
                }
                watchParallel.Stop();
                parallelTime = watchParallel.ElapsedMilliseconds;

                Console.WriteLine($"Matrix size: {size} | Sync Time: {syncTime} ms | Parallel Time: {parallelTime} ms");

                // Check if parallel is faster
                if (parallelTime < syncTime)
                {
                    Assert.IsTrue(true, $"Parallel multiplication is faster for matrix size {size}");
                    break;
                }
            }
        }

        private Matrix CreateRandomMatrix(int rows, int cols)
        {
            var matrix = new Matrix(rows, cols);
            var rand = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix.SetElement(i, j, rand.Next(1, 10));
                }
            }
            return matrix;
        }


        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion
    }
}
