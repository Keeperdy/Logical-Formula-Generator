using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logical_Formula_Generator
{
    class Program
    {
        //количество и идентфикация операндов 
        static char FormulaVariables(int x, out char[] ChosenVariables)
        {
            string allVariables = "ABCXYZ";
            char[] BaseVariables = new char[x];
            int num = 0;
            do
            {
                BaseVariables[num] = allVariables[num];
                num++;
            }
            while (num < x);
            ChosenVariables = BaseVariables;
            return ChosenVariables[x - 1];
        }

        //произвольный выбор операторов
        static char RandomOperators(Random operators)
        {
            string alloperators = "v\u2194\u2192\u005E";
            int num = operators.Next(alloperators.Length);
            return alloperators[num];
        }

        //произвольный выбор операндов
        static char RandomVariables(char[] RndmVrbls, Random variables)
        {
            int num = variables.Next(RndmVrbls.Length);
            return RndmVrbls[num];
        }

        static void Main(string[] args)
        {
            int quantityVariables = 0;
            int quantityOperations = 0;
            bool bracketsUsage = false;
            Random random = new Random();

            //задаем количество операндов или генерация рандомной формулы
            Console.WriteLine("Enter the quantity of variables (from 2 to 6) or 0 for random formula (5-10 random operations)");
            quantityVariables = Convert.ToInt32(Console.ReadLine());
            if (quantityVariables == 0)
            {
                quantityVariables = random.Next(2, 6);
                quantityOperations = random.Next(5, 10);
                bracketsUsage = Convert.ToBoolean(random.Next(0, 2));
            }
            else
            {
                if (quantityVariables == 0 || quantityVariables < 2 || quantityVariables > 6) //ошибка ввода
                {
                    Console.WriteLine();
                    Console.WriteLine("The quantity of variables must be from 2 to 6.");
                    Console.WriteLine("Please, try again!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Enter the quantity of operations (from 1 to 100):"); //задаем количество выполняемых действий
                quantityOperations = Convert.ToInt32(Console.ReadLine());
                if (quantityOperations == 0 || quantityOperations < 0 || quantityOperations > 100) //ошибка ввода
                {
                    Console.WriteLine();
                    Console.WriteLine("The quantity of operations must be from 1 to 100.");
                    Console.WriteLine("Please, try again!");
                    Console.ReadKey();
                    return;
                }

                if (quantityOperations >= 2) //запрос использования скобок
                {
                    Console.WriteLine("Brackets? Y/N");
                    string bracketsFromUser = Convert.ToString(Console.ReadLine());
                    bracketsUsage = bracketsFromUser == "y" ? true : false;
                }
            }

            //MAIN PART
            string result = "";
            Random operators = new Random();
            Random variables = new Random();
            int bracketsCountOpening = 0;
            int bracketsCountClosing = 0;

            char[] UsedVariables = new char[quantityVariables]; //создание и заполнение массива операндами
            FormulaVariables(quantityVariables, out UsedVariables);
            char mainVariables = RandomVariables(UsedVariables, variables);
            char antirepeat = RandomVariables(UsedVariables, variables);

            for (int i = 0; i < quantityOperations; i++) // заполнение формулы операндами и операторами
            {
                if (mainVariables == antirepeat) // антиповтор одинаковых операндов
                {
                    do mainVariables = RandomVariables(UsedVariables, variables);
                    while (mainVariables == antirepeat);
                    i--;
                }
                else
                {
                    string initial = mainVariables + Convert.ToString(RandomOperators(operators));
                    result += initial;
                    antirepeat = mainVariables;
                    mainVariables = RandomVariables(UsedVariables, variables);
                }
            }

            switch (mainVariables == antirepeat) //последний операнд с антиповтором (закрытие формулы)
            {
                case true:
                    do mainVariables = RandomVariables(UsedVariables, variables);
                    while (mainVariables == antirepeat);
                    result += mainVariables;
                    break;
                case false:
                    result += mainVariables;
                    break;
            }

            while (bracketsCountClosing == 0 && bracketsCountClosing == 0 && bracketsUsage == true) // использование скобок
            {
                bool x, y;
                for (int i = 0, j = 0; i < result.Length; ) // рандомные скобки
                {

                    x = Convert.ToBoolean(random.Next(0, 2));
                    y = Convert.ToBoolean(random.Next(0, 2));

                    if (x == true && i < result.Length - 1)
                    {
                        result = result.Insert(i, "(");
                        bracketsCountOpening++;
                        i += 1;
                        j = i + 2;

                        for (; j <= result.Length; )
                        {
                            if (y == true && j < result.Length)
                            {
                                result = result.Insert(j + 1, ")");
                                bracketsCountClosing++;
                                j += 1;
                                i = j + 2;
                                break;
                            }
                            else
                            {
                                do
                                {
                                    y = Convert.ToBoolean(random.Next(0, 2));
                                    j += 2;
                                }
                                while (y == false && j < result.Length);

                                if (j >= result.Length)
                                {
                                    result = result.Insert(result.Length, ")");
                                    bracketsCountClosing++;
                                    i = j;
                                    break;
                                }

                            }
                        }
                    }
                    else i += 2;
                }
            }

            if (result[0] == '(' && result[result.Length - 1] == ')' && bracketsCountClosing == 1) //исправляем одинарные скобки по краям формулы (АvBvCv)
            {
                result = result.Insert(4, ")");
                result = result.Remove(result.Length - 1);
            }

            for (int i = result.Length - 1; i > 0; i--) //рандомное отрицание
            {
                if (UsedVariables.Contains(result[i]) || result[i] == '(')
                {
                    int negation = random.Next(0, 3);
                    if (negation == 1)
                        result = result.Insert(i, "\u00AC");
                }
            }

            Console.WriteLine(); //финальный результат
            Console.WriteLine("Try to solve: " + result);
            Console.WriteLine("Good luck!");
            Console.ReadKey();
        }
    }
}
