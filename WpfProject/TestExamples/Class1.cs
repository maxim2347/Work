using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExamples {
    [TestFixture]
    public class TestFixture1 {
        [Test]
        public void Test1() {
            Assert.AreEqual(13,Helper.ToCenterArrayPath(5,5));
        }
        [Test]
        public void Test2()
        {
            Assert.AreEqual(10, Helper.ToCenterArrayPath(4, 4));
        }
        [Test]
        public void Test3()
        {
            Assert.AreEqual(5, Helper.ToCenterArrayPath(1, 5));
        }
    }

    public static class Helper {
        public enum Direction { Right,Down,Left,Up,Start}
        
        public static int Sum(int x, int y) {
            return x + y;
        }

        public static int ToCenterArrayPath(int rowCount, int columCount ) {
            int arraySize = columCount * rowCount;
            int[,] array = new int[rowCount,columCount];
            int[,] virtualArray = new int[rowCount, columCount];
            int[] pathArray = new int[arraySize];
            int step = 1;
            for(int i=0; i<rowCount;i++) {
                for(int j = 0; j < columCount; j++) {
                    virtualArray[i, j] = 0;
                    array[i, j] = step;
                    step++;
                }
            }
            int pathArrayCurrentElement = 0;
            ToArrayBorder(pathArray, pathArrayCurrentElement, array, virtualArray, 0, 0);
            return pathArray[arraySize-1];
        }

        public static void ToArrayBorder(int[] pathArray,int pathArrayCurrentElement ,int[,] array,int[,] virtualArray, int rowPosition,int columPosition) {
            if (pathArrayCurrentElement == array.Length) return;
            Direction dir = ChoseDirection(virtualArray, rowPosition, columPosition);
            switch (dir)
            {
                case Direction.Start:
                    while (virtualArray[rowPosition, columPosition] == 0)
                    {
                        virtualArray[rowPosition, columPosition] = 1;
                        pathArray[pathArrayCurrentElement] = array[rowPosition, columPosition];
                        if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Start))
                        {
                            columPosition += 1;
                            pathArrayCurrentElement += 1;
                        }
                    }
                    break;
                case Direction.Down:
                    rowPosition += 1;
                    while (virtualArray[rowPosition, columPosition] == 0) {
                        virtualArray[rowPosition, columPosition] = 1;
                        pathArray[pathArrayCurrentElement] = array[rowPosition, columPosition];
                        if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Down))
                        {
                            rowPosition += 1;
                            pathArrayCurrentElement += 1;
                        }  
                    }
                    break;
                case Direction.Up:
                    rowPosition -= 1;
                    while (virtualArray[rowPosition, columPosition] == 0)
                    {
                        virtualArray[rowPosition, columPosition] = 1;
                        pathArray[pathArrayCurrentElement] = array[rowPosition, columPosition];
                        if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Up))
                        {
                            rowPosition -= 1; 
                            pathArrayCurrentElement += 1;
                        }
                    }
                    break;
                case Direction.Right:
                    columPosition += 1;
                    while (virtualArray[rowPosition, columPosition] == 0)
                    {
                        virtualArray[rowPosition, columPosition] = 1;
                        pathArray[pathArrayCurrentElement] = array[rowPosition, columPosition];
                        if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Right))
                        {
                            columPosition += 1;
                            pathArrayCurrentElement += 1;
                        }
                    }
                    break;
                case Direction.Left:
                    columPosition -= 1;
                    while (virtualArray[rowPosition, columPosition] == 0)
                    {
                        virtualArray[rowPosition, columPosition] = 1;
                        pathArray[pathArrayCurrentElement] = array[rowPosition, columPosition];
                        if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Left))
                        {
                            columPosition -= 1;
                            pathArrayCurrentElement += 1;
                        }
                    }
                    break;
            }
            ToArrayBorder(pathArray, pathArrayCurrentElement+1, array, virtualArray, rowPosition, columPosition);

        }


        public static Direction ChoseDirection(int[,] virtualArray , int rowPosition ,int columPosition) {
            Direction direction= Direction.Start;
            if (rowPosition == 0 && columPosition == 0) return direction;
            if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Down)) direction = Direction.Down;
            if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Left)) direction = Direction.Left;
            if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Up)) direction = Direction.Up;
            if (!IsBorderNext(virtualArray, rowPosition, columPosition, Direction.Right)) direction = Direction.Right;
            return direction;
        }

        public static bool IsBorderNext(int[,] virtualArray,int rowPosition, int columPosition, Direction direction) {
            int sourceRows = virtualArray.GetUpperBound(0);
            int sourceColums = virtualArray.GetUpperBound(1);
            bool isBorderNext = true;
            switch (direction)
            {
                case Direction.Start:
                    if (columPosition < sourceColums )
                        if (virtualArray[rowPosition, columPosition + 1] == 0)
                            isBorderNext = false;          
                    break;
                case Direction.Down:
                    rowPosition += 1;
                    if (rowPosition <= sourceRows )
                        if (virtualArray[rowPosition, columPosition] == 0)
                            isBorderNext = false;
                    break;
                case Direction.Up:
                    rowPosition -= 1;
                    if (rowPosition > 0)
                        if (virtualArray[rowPosition, columPosition] == 0)
                            isBorderNext= false;
                    break;
                case Direction.Right:
                    columPosition += 1;
                    if (columPosition <= sourceColums )
                        if (virtualArray[rowPosition, columPosition] == 0)
                            isBorderNext = false;
                    break;
                case Direction.Left:
                    columPosition -= 1;
                    if (columPosition >= 0)
                        if (virtualArray[rowPosition, columPosition] == 0)
                            isBorderNext = false;
                    break;
            }

            return isBorderNext;
        }
    }
}
