using System;
using System.Linq;

namespace GaussAlgorithm;

public class Solver
{
    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        int rows = matrix.Length;
        int cols = matrix[0].Length;

        // Проверка входных данных
        if (rows == 0 || cols == 0 || freeMembers.Length != rows)
            throw new ArgumentException("Invalid input dimensions");

        // Создаем копии матрицы и вектора свободных членов
        double[][] augmentedMatrix = new double[rows][];
        for (int i = 0; i < rows; i++)
        {
            augmentedMatrix[i] = new double[cols + 1];
            Array.Copy(matrix[i], augmentedMatrix[i], cols);
            augmentedMatrix[i][cols] = freeMembers[i];
        }

        // Инициализация pivotColumns для отслеживания строк для каждого ведущего столбца
        int[] pivotColumns = new int[cols];
        for (int i = 0; i < cols; i++)
            pivotColumns[i] = -1; // -1 означает, что столбец еще не использован как ведущий

        // Прямой ход с выбором главного элемента
        int rank = 0; // Текущий ранг (количество обработанных строк)
        for (int col = 0, row = 0; col < cols && row < rows; col++)
        {
            // Найдём максимальный элемент в текущем столбце (по модулю)
            int maxRow = row;
            for (int i = row + 1; i < rows; i++)
            {
                if (Math.Abs(augmentedMatrix[i][col]) > Math.Abs(augmentedMatrix[maxRow][col]))
                    maxRow = i;
            }

            // Если он слишком мал, пропускаем столбец
            if (Math.Abs(augmentedMatrix[maxRow][col]) < 1e-10)
                continue;

            // Переставим строки
            if (row != maxRow)
            {
                var temp = augmentedMatrix[row];
                augmentedMatrix[row] = augmentedMatrix[maxRow];
                augmentedMatrix[maxRow] = temp;
            }

            // Нормализуем текущую строку
            double pivot = augmentedMatrix[row][col];
            for (int j = col; j <= cols; j++)
                augmentedMatrix[row][j] /= pivot;

            // Вычтем текущую строку из всех других
            for (int i = 0; i < rows; i++)
            {
                if (i != row)
                {
                    double factor = augmentedMatrix[i][col];
                    for (int j = col; j <= cols; j++)
                        augmentedMatrix[i][j] -= factor * augmentedMatrix[row][j];
                }
            }

            pivotColumns[col] = row;
            row++;
            rank++;
        }

        // Проверка на наличие решения
        for (int i = rank; i < rows; i++)
        {
            bool isZeroRow = true;
            for (int j = 0; j < cols; j++)
            {
                if (Math.Abs(augmentedMatrix[i][j]) > 1e-10)
                {
                    isZeroRow = false;
                    break;
                }
            }
            if (isZeroRow && Math.Abs(augmentedMatrix[i][cols]) > 1e-10)
                throw new NoSolutionException(matrix, freeMembers, matrix);
        }

        // Обратный ход
        double[] solution = new double[cols];
        for (int col = cols - 1; col >= 0; col--)
        {
            if (pivotColumns[col] != -1)
            {
                int row = pivotColumns[col];
                solution[col] = augmentedMatrix[row][cols];
                for (int j = col + 1; j < cols; j++)
                {
                    solution[col] -= augmentedMatrix[row][j] * solution[j];
                }
            }
            else
            {
                solution[col] = 0; // Свободная переменная
            }
        }

        // Для недопределенных систем заполняем оставшиеся переменные нулями
        if (rows < cols)
        {
            for (int i = 0; i < cols; i++)
            {
                if (pivotColumns[i] == -1)
                    solution[i] = 0;
            }
        }

        return solution;
    }
}