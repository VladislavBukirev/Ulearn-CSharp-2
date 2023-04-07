using System;
using System.Linq;
namespace GaussAlgorithm;

public class Solver
{
    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        matrix = AddColumn(matrix, freeMembers);

        return GaussSolve(matrix, matrix, freeMembers);
    }

    private static double[] GaussSolve(double[][] matrix, double[][] sourceMatrix,
        double[] sourceFreeMembers)
    {
        var rows = matrix.Length;
        var columns = matrix[0].Length;

        var answer = new double[columns - 1];
        var haveAnswerColumn = new bool[columns - 1];
        ElementaryTransforms(matrix, rows, columns, answer, haveAnswerColumn);

        if (!CheckResolvability(matrix, rows, columns))
            throw new NoSolutionException(sourceMatrix, sourceFreeMembers, matrix);

        CollectAnswer(matrix, columns, haveAnswerColumn, rows, answer);

        return answer;
    }

    private static void ElementaryTransforms(double[][] matrix, int rows, int columns, double[] answer,
        bool[] haveAnswerColumn)
    {
        var usedRows = new bool[rows];

        for (var j = 0; j < columns - 1; j++)
        {
            var mainElementIndex = -1;
            for (var i = 0; i < rows; i++)
            {
                if (matrix[i][j] != 0 && !usedRows[i])
                {
                    usedRows[i] = true;
                    mainElementIndex = i;
                    break;
                }
            }

            if (mainElementIndex == -1)
            {
                answer[j] = 0;
                haveAnswerColumn[j] = true;
                continue;
            }

            for (var i = 0; i < rows; i++)
            {
                if (i == mainElementIndex)
                    continue;

                var coefficient = matrix[i][j] / matrix[mainElementIndex][j];

                for (var k = 0; k < columns; k++)
                    matrix[i][k] -= matrix[mainElementIndex][k] * coefficient;
            }
        }
    }

    private static void CollectAnswer(double[][] matrix, int columns, bool[] haveAnswerColumn, int rows,
        double[] answer)
    {
        for (var j = 0; j < columns - 1; j++)
        {
            if (haveAnswerColumn[j])
                continue;
            var findNotNullIndex = -1;
            for (var i = 0; i < rows; i++)
            {
                if (!(Math.Abs(matrix[i][j]) > 1e-5)) continue;
                findNotNullIndex = i;
                break;
            }

            answer[j] = findNotNullIndex == -1
                ? 0
                : matrix[findNotNullIndex][columns - 1] / matrix[findNotNullIndex][j];
        }
    }

    private static bool CheckResolvability(double[][] matrix, int rows, int columns)
    {
        for (var i = 0; i < rows; i++)
        {
            if (matrix[i][columns - 1] == 0)
                continue;
            var badRow = true;
            for (var j = 0; j < columns - 1; j++)
            {
                if (matrix[i][j] == 0) continue;
                badRow = false;
                break;
            }

            if (badRow)
                return false;
        }

        return true;
    }

    private static double[][] AddColumn(double[][] array, double[] column)
    {
        if (array.Length != column.Length)
            throw new ArgumentOutOfRangeException();
        return array
            .Select((row, i) => row.Concat(new[] { column[i] }).ToArray())
            .ToArray();
    }
}